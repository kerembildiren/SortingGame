using System;
using System.Collections.Generic;
using System.Linq;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;
using Random = System.Random;

namespace SortingGame.Section
{
    /// <summary>
    /// Builds one section from its definition and owns its rules: correct shelf = snap + coins,
    /// wrong shelf = soft return + hint (GDD 7.1, 7.2), sweeping the dirt layer (7.3), finding collectibles (9.3)
    /// and the 100% renovation (5.5). Views report to it, the HUD listens to its events.
    /// </summary>
    public class SectionController : MonoBehaviour
    {
        const float WallHeight = 2.6f;
        const float ShelfGap = 0.12f;
        const float FloorMargin = 0.45f;

        public event Action<ItemView, ShelfView, int> ItemPlaced;   // item, shelf, coins
        public event Action<ItemView, ShelfView> WrongShelf;        // item, correct shelf
        public event Action<ShelfView> ShelfCompleted;
        public event Action SectionCompleted;
        public event Action<SectionProgress> ProgressChanged;
        public event Action<ItemView> ItemRevealed;
        public event Action<ItemView, CollectionBook.FindResult> CollectibleFound;

        public SectionDefinition Definition { get; private set; }
        public SectionProgress Progress { get; private set; }
        public Bounds ViewBounds { get; private set; }
        public bool HasDirt => _dirt != null && _dirt.HasDirt;
        public bool IsComplete => _completed;
        public IReadOnlyList<ShelfView> Shelves => _shelves;
        public IReadOnlyList<ItemView> Items => _items;
        public IReadOnlyList<ContainerView> Containers => _containers;
        public IEnumerable<ItemView> CommonItems => _items.Where(i => i != null && !i.IsCollectible);
        public IEnumerable<ItemView> Collectibles => _items.Where(i => i != null && i.IsCollectible);
        public float DirtCleaned => _dirt == null ? 1f : _dirt.CleanedFraction;

        readonly List<ShelfView> _shelves = new();
        readonly List<ItemView> _items = new();
        readonly List<ContainerView> _containers = new();
        readonly List<ItemView> _buried = new();

        GameDatabase _database;
        SectionVisuals _visuals;
        FeelConfig _feel;
        Wallet _wallet;
        CollectionBook _book;
        PlaceholderFactory _factory;
        Fx _fx;
        Transform _root;
        Vector2 _floorSize;
        float _shelfZoneDepth;
        int _placeStreak;
        float _lastPlaceTime;
        Coroutine _moodTween;
        bool _completed;
        float _mood;

        DirtMask _dirt;
        DirtLayerView _dirtView;
        Material _floorMaterial;
        Material _wallMaterial;

        public void Build(SectionDefinition definition, GameDatabase database, SectionVisuals visuals, Wallet wallet, CollectionBook book, int seed)
        {
            Clear();
            Definition = definition;
            _database = database;
            _visuals = visuals;
            _feel = database.Feel;
            _wallet = wallet;
            _book = book;
            _factory ??= new PlaceholderFactory(visuals);
            _fx ??= new Fx(visuals);
            _root = new GameObject($"Section_{definition.Id}").transform;
            _root.SetParent(transform, false);

            var layout = SectionLayoutGenerator.Generate(definition, c => _database.CommonItemsOf(c).ToList(), seed);
            var random = new Random(seed);

            BuildRoom(definition);
            BuildDirt(definition, seed);
            BuildShelves(definition);
            PlaceContainers(layout, random);
            var taken = _containers.Select(c => new Vector2(c.transform.localPosition.x, c.transform.localPosition.z)).ToList();
            PlaceBuried(layout, random, taken);
            PlaceLoose(layout, random, taken);

            Progress = new SectionProgress(layout.TotalItems, HasDirt, database.Balance.DirtProgressWeight);
            Progress.Changed += OnProgressChanged;

            // Everything the camera needs to see: floor plus the shelves standing on it.
            var tallest = _shelves.Count == 0 ? 1f : _shelves.Max(s => s.Size.y) + 0.35f;
            ViewBounds = new Bounds(new Vector3(0f, tallest / 2f, 0f), new Vector3(_floorSize.x, tallest, _floorSize.y));
            ApplyMood(0f);
            SfxPlayer.Instance?.SetLoop(Sfx.CleanAmbienceLoop, 0f);
        }

        public void Clear()
        {
            if (_root != null) Destroy(_root.gameObject);
            Tween.Stop(_moodTween);
            _shelves.Clear();
            _items.Clear();
            _containers.Clear();
            _buried.Clear();
            _dirt = null;
            _dirtView = null;
            _placeStreak = 0;
            _completed = false;
        }

        // ---------- Rules: shelves ----------

        public bool TryPlace(ItemView item, ShelfView shelf, Vector3 nearPoint)
        {
            if (item.Definition.Category == shelf.Category)
            {
                var slot = shelf.NearestFreeSlot(nearPoint);
                if (slot != null)
                {
                    item.FlyToSlot(slot, () => OnLanded(item, shelf));
                    return true;
                }
            }

            item.ReturnToPickup();
            var correct = ShelfFor(item.Definition.Category);
            if (correct != null && correct != shelf) correct.FlashHint(_feel.WrongShelfHintDuration);
            SfxPlayer.Instance?.Play(Sfx.Wrong);
            WrongShelf?.Invoke(item, correct);
            return false;
        }

        public void OpenContainer(ContainerView container)
        {
            if (container == null || container.IsOpened) return;
            container.Open(_feel);
        }

        public ShelfView ShelfFor(CategoryDefinition category) => _shelves.FirstOrDefault(s => s.Category == category);

        public Vector3 ClampToRoom(Vector3 point)
        {
            var hx = _floorSize.x / 2f - 0.15f;
            var hz = _floorSize.y / 2f - 0.15f;
            point.x = Mathf.Clamp(point.x, -hx, hx);
            point.z = Mathf.Clamp(point.z, -hz, hz);
            return point;
        }

        void OnLanded(ItemView item, ShelfView shelf)
        {
            var coins = item.Definition.CoinValue;
            _wallet.Add(coins);

            // Quick chains climb in pitch: small reward for flow.
            _placeStreak = Time.time - _lastPlaceTime < 1.5f ? Mathf.Min(_placeStreak + 1, 8) : 0;
            _lastPlaceTime = Time.time;
            SfxPlayer.Instance?.PlayPitched(SfxPlayer.PlaceSoundFor(shelf.Category.PlaceSound), 1f + _placeStreak * 0.04f);
            SfxPlayer.Instance?.Play(Sfx.Coin, 0.03f, 0.6f);
            Haptics.Light();

            ItemPlaced?.Invoke(item, shelf, coins);

            if (shelf.IsFull)
            {
                shelf.Celebrate();
                SfxPlayer.Instance?.Play(Sfx.ShelfFull, 0f);
                Haptics.Medium();
                ShelfCompleted?.Invoke(shelf);
            }

            Progress.AddPlaced();
        }

        // ---------- Rules: dirt ----------

        /// <summary>Broom stroke at a floor point. Returns true if any dirt was removed (drives dust + sound).</summary>
        public bool Sweep(Vector3 worldPoint, float deltaTime)
        {
            if (_dirt == null || !_dirt.HasDirt || _dirt.CleanedFraction >= 1f) return false;

            var local = _root.InverseTransformPoint(worldPoint);
            var u = local.x / _floorSize.x + 0.5f;
            var v = local.z / _floorSize.y + 0.5f;
            if (u < -0.1f || u > 1.1f || v < -0.1f || v > 1.1f) return false;

            var rect = _dirt.Brush(u, v, _feel.BroomRadius / _floorSize.x, _feel.BroomStrength * deltaTime);
            if (rect == null) return false;
            var r = rect.Value;
            _dirtView.RefreshRect(r.xMin, r.yMin, r.xMax, r.yMax);

            RevealUncovered();

            if (_dirt.CleanedFraction >= _feel.DirtAutoFinish) FinishDirt();
            else Progress.SetDirtCleaned(_dirt.CleanedFraction);
            return true;
        }

        void RevealUncovered()
        {
            for (var i = _buried.Count - 1; i >= 0; i--)
            {
                var item = _buried[i];
                var (u, v) = FloorUv(item.transform.position);
                if (_dirt.Sample(u, v, 0.02f) > _feel.RevealThreshold) continue;
                Reveal(item);
                _buried.RemoveAt(i);
            }
        }

        void Reveal(ItemView item)
        {
            item.Reveal();
            _fx.Burst(item.transform.position + Vector3.up * 0.1f, _visuals.DirtSpeckColor, 10, 1.2f, 0.07f, 0.5f, 0.5f);
            SfxPlayer.Instance?.Play(Sfx.Reveal);
            Haptics.Light();
            ItemRevealed?.Invoke(item);
        }

        /// <summary>The last few specks dissolve by themselves (FeelConfig.DirtAutoFinish).</summary>
        void FinishDirt()
        {
            _dirt.ClearAll();
            _dirtView.FadeOut(0.6f);
            foreach (var item in _buried) Reveal(item);
            _buried.Clear();
            Progress.SetDirtCleaned(1f);
        }

        (float u, float v) FloorUv(Vector3 world)
        {
            var local = _root.InverseTransformPoint(world);
            return (local.x / _floorSize.x + 0.5f, local.z / _floorSize.y + 0.5f);
        }

        // ---------- Rules: collectibles ----------

        /// <summary>Player tapped a glowing collectible (GDD 9.3). First copy -> book, duplicates -> coins.</summary>
        public void FindCollectible(ItemView item)
        {
            if (item == null || !item.CanTapToFind || item.Definition is not CollectibleDefinition collectible) return;
            item.MarkFound();
            _items.Remove(item);
            var result = _book.Register(collectible);
            if (!result.IsNew) _wallet.Add(result.DuplicateCoins);
            CollectibleFound?.Invoke(item, result);
        }

        // ---------- Progress, mood, renovation ----------

        void OnProgressChanged(SectionProgress progress)
        {
            ProgressChanged?.Invoke(progress);
            if (_completed) return;
            if (progress.IsComplete)
            {
                _completed = true;
                PlayRenovation();
                SectionCompleted?.Invoke();
                return;
            }
            // Principle 1: every action makes the place visibly nicer, a little.
            ApplyMood(progress.Fraction * _feel.ProgressMoodShare);
        }

        /// <summary>GDD 5.5: dust gone, lights up, colours come alive, warm music layer fades in.</summary>
        void PlayRenovation()
        {
            SfxPlayer.Instance?.Play(Sfx.SectionComplete, 0f);
            Haptics.Strong();
            if (_dirtView != null) _dirtView.FadeOut(0.8f);

            var startMood = _mood;
            var camera = Camera.main;
            var startBackground = camera != null ? camera.backgroundColor : _visuals.BackgroundColor;
            _moodTween = Tween.Run(this, _feel.RenovationDuration, t =>
            {
                ApplyMood(Mathf.Lerp(startMood, 1f, t));
                if (camera != null) camera.backgroundColor = Color.Lerp(startBackground, _visuals.BackgroundColorClean, t);
                SfxPlayer.Instance?.SetLoop(Sfx.CleanAmbienceLoop, t * 0.6f);
            }, Ease.InOutQuad);

            // Sparkle wave from the back wall to the front.
            const int waves = 6;
            for (var i = 0; i < waves; i++)
            {
                var z = Mathf.Lerp(_floorSize.y / 2f - 0.5f, -_floorSize.y / 2f + 0.5f, i / (waves - 1f));
                Tween.Delay(this, i * 0.18f, () =>
                {
                    for (var k = 0; k < 4; k++)
                    {
                        var x = Mathf.Lerp(-_floorSize.x / 2f + 0.5f, _floorSize.x / 2f - 0.5f, (k + 0.5f) / 4f);
                        _fx.Burst(_root.TransformPoint(new Vector3(x, 0.3f, z)), new Color(1f, 0.9f, 0.55f), 8, 0.8f, 0.09f, 0.9f, -0.1f);
                    }
                });
            }
        }

        /// <summary>0 = dusty and dim, 1 = warm and bright (GDD 12.1).</summary>
        void ApplyMood(float clean)
        {
            _mood = clean;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = Color.Lerp(_visuals.AmbientDirty, _visuals.AmbientClean, clean);
            var sun = RenderSettings.sun;
            if (sun != null)
            {
                sun.color = _visuals.LightColor;
                sun.intensity = Mathf.Lerp(_visuals.LightIntensityDirty, _visuals.LightIntensityClean, clean);
            }
            if (_floorMaterial != null) _floorMaterial.SetColor("_BaseColor", Color.Lerp(_visuals.FloorColor, _visuals.FloorColorClean, clean));
            if (_wallMaterial != null) _wallMaterial.SetColor("_BaseColor", Color.Lerp(_visuals.WallColor, _visuals.WallColorClean, clean));
        }

        // ---------- Construction ----------

        void BuildRoom(SectionDefinition definition)
        {
            _floorSize = definition.FloorSize;
            var w = _floorSize.x;
            var d = _floorSize.y;

            _floorMaterial = _factory.UniqueLit(_visuals.FloorColor);
            _wallMaterial = _factory.UniqueLit(_visuals.WallColor);

            SetMaterial(_factory.CreateBox("Floor", _root, new Vector3(0f, -0.05f, 0f), new Vector3(w + 0.4f, 0.1f, d + 0.4f), _visuals.FloorColor), _floorMaterial);
            SetMaterial(_factory.CreateBox("BackWall", _root, new Vector3(0f, WallHeight / 2f, d / 2f + 0.05f), new Vector3(w + 0.4f, WallHeight, 0.1f), _visuals.WallColor), _wallMaterial);
            // Low side walls read as a room without blocking the 3/4 view.
            SetMaterial(_factory.CreateBox("WallL", _root, new Vector3(-w / 2f - 0.1f, 0.2f, 0f), new Vector3(0.2f, 0.4f, d + 0.4f), _visuals.WallColor), _wallMaterial);
            SetMaterial(_factory.CreateBox("WallR", _root, new Vector3(w / 2f + 0.1f, 0.2f, 0f), new Vector3(0.2f, 0.4f, d + 0.4f), _visuals.WallColor), _wallMaterial);

            // Invisible walls keep tumbling items inside.
            AddInvisibleWall("BlockL", new Vector3(-w / 2f - 0.1f, 1.5f, 0f), new Vector3(0.2f, 3f, d + 0.4f));
            AddInvisibleWall("BlockR", new Vector3(w / 2f + 0.1f, 1.5f, 0f), new Vector3(0.2f, 3f, d + 0.4f));
            AddInvisibleWall("BlockFront", new Vector3(0f, 1.5f, -d / 2f - 0.1f), new Vector3(w + 0.4f, 3f, 0.2f));
        }

        static void SetMaterial(GameObject go, Material material) => go.GetComponent<MeshRenderer>().sharedMaterial = material;

        void BuildDirt(SectionDefinition definition, int seed)
        {
            if (definition.DirtCoverage <= 0f) return;
            var width = Mathf.Max(16, _feel.DirtResolution);
            var height = Mathf.Max(16, Mathf.RoundToInt(width * _floorSize.y / _floorSize.x));
            _dirt = new DirtMask(width, height);
            _dirt.Generate(definition.DirtCoverage, seed);

            var go = new GameObject("DirtLayer");
            go.transform.SetParent(_root, false);
            _dirtView = go.AddComponent<DirtLayerView>();
            _dirtView.Build(_dirt, _floorSize, _visuals, seed);
        }

        void AddInvisibleWall(string name, Vector3 center, Vector3 size)
        {
            var go = new GameObject(name);
            go.transform.SetParent(_root, false);
            go.transform.localPosition = center;
            go.AddComponent<BoxCollider>().size = size;
        }

        void BuildShelves(SectionDefinition definition)
        {
            var sizes = definition.Shelves.Select(s => ShelfView.MeasureSize(s.Category, s.SlotCount, out _, out _)).ToList();
            var totalWidth = sizes.Sum(s => s.x) + ShelfGap * Mathf.Max(0, sizes.Count - 1);
            if (totalWidth > _floorSize.x)
                Debug.LogWarning($"[Section] Shelves ({totalWidth:0.00}m) are wider than the floor ({_floorSize.x:0.00}m). Increase FloorSize.");

            _shelfZoneDepth = sizes.Count == 0 ? 0f : sizes.Max(s => s.z);
            var x = -totalWidth / 2f;
            for (var i = 0; i < definition.Shelves.Count; i++)
            {
                var entry = definition.Shelves[i];
                var go = new GameObject($"Shelf_{entry.Category.Id}");
                go.transform.SetParent(_root, false);
                go.transform.localPosition = new Vector3(x + sizes[i].x / 2f, 0f, _floorSize.y / 2f - sizes[i].z / 2f);
                var shelf = go.AddComponent<ShelfView>();
                shelf.Build(entry.Category, entry.SlotCount, _factory, _visuals);
                _shelves.Add(shelf);
                x += sizes[i].x + ShelfGap;
            }
        }

        /// <summary>Free floor: everything in front of the shelves, minus a margin.</summary>
        Rect FreeFloor()
        {
            var minX = -_floorSize.x / 2f + FloorMargin;
            var maxX = _floorSize.x / 2f - FloorMargin;
            var minZ = -_floorSize.y / 2f + FloorMargin;
            var maxZ = _floorSize.y / 2f - _shelfZoneDepth - 0.5f;
            return Rect.MinMaxRect(minX, minZ, maxX, Mathf.Max(minZ + 0.5f, maxZ));
        }

        void PlaceContainers(SectionLayout layout, Random random)
        {
            var free = FreeFloor();
            var placed = new List<Vector2>();
            foreach (var content in layout.Containers)
            {
                var spot = FindSpot(free, placed, 1.1f, random, null);
                placed.Add(spot);

                var go = new GameObject($"Container_{content.Container.Id}");
                go.transform.SetParent(_root, false);
                go.transform.localPosition = new Vector3(spot.x, 0f, spot.y);

                // Tip towards the open middle of the room and slightly towards the camera, so the spill is visible.
                var toCentre = new Vector3(free.center.x - spot.x, 0f, free.center.y - spot.y - 0.6f);
                var yaw = Mathf.Atan2(toCentre.x, toCentre.z) * Mathf.Rad2Deg + Range(random, -25f, 25f);
                go.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);

                // Collectibles hide among the commons inside the box.
                var contents = new List<ItemDefinition>(content.Items);
                foreach (var collectible in content.Collectibles)
                    contents.Insert(random.Next(contents.Count + 1), collectible);

                var view = go.AddComponent<ContainerView>();
                view.Build(content.Container, contents, _factory);
                view.SpawnItem = SpawnItem;
                _containers.Add(view);
            }
        }

        void PlaceBuried(SectionLayout layout, Random random, List<Vector2> taken)
        {
            if (_dirt == null) return;
            var free = FreeFloor();
            // Only spots with thick dirt, so the item is really hidden until swept.
            bool Dirty(Vector2 p)
            {
                var (u, v) = FloorUv(_root.TransformPoint(new Vector3(p.x, 0f, p.y)));
                return _dirt.Sample(u, v, 0.02f) > 0.85f;
            }

            foreach (var definition in layout.BuriedItems.Concat(layout.BuriedCollectibles))
            {
                var spot = FindSpot(free, taken, 0.45f, random, Dirty);
                taken.Add(spot);
                var (rest, height) = PlaceholderFactory.RestPose(definition.Placeholder);
                var rotation = Quaternion.Euler(0f, Range(random, 0f, 360f), 0f) * rest;
                var view = SpawnItem(definition, _root.TransformPoint(new Vector3(spot.x, height + 0.002f, spot.y)), rotation);
                view.Bury(view.transform.position, rotation);
                _buried.Add(view);
            }
        }

        void PlaceLoose(SectionLayout layout, Random random, List<Vector2> taken)
        {
            var free = FreeFloor();
            // Keep loose items apart from boxes and from each other so nothing intersects at rest.
            foreach (var definition in layout.LooseItems.Concat(layout.LooseCollectibles))
            {
                var spot = FindSpot(free, taken, 0.45f, random, null);
                taken.Add(spot);
                var (rest, height) = PlaceholderFactory.RestPose(definition.Placeholder);
                var rotation = Quaternion.Euler(0f, Range(random, 0f, 360f), 0f) * rest;
                var view = SpawnItem(definition, _root.TransformPoint(new Vector3(spot.x, height + 0.002f, spot.y)), rotation);
                view.SetResting(view.transform.position, rotation);
            }
        }

        ItemView SpawnItem(ItemDefinition definition, Vector3 position, Quaternion rotation)
        {
            var view = ItemView.Create(definition, _factory, _feel, _root, _fx, _visuals.RareGlowColor);
            view.transform.SetPositionAndRotation(position, rotation);
            _items.Add(view);
            return view;
        }

        static Vector2 FindSpot(Rect area, List<Vector2> avoid, float minDistance, Random random, Func<Vector2, bool> accept)
        {
            var best = area.center;
            var bestScore = float.MinValue;
            for (var attempt = 0; attempt < 60; attempt++)
            {
                var candidate = new Vector2(Range(random, area.xMin, area.xMax), Range(random, area.yMin, area.yMax));
                var clearance = float.MaxValue;
                foreach (var other in avoid) clearance = Mathf.Min(clearance, Vector2.Distance(candidate, other));
                var accepted = accept == null || accept(candidate);
                if (clearance >= minDistance && accepted) return candidate;
                var score = Mathf.Min(clearance, minDistance) + (accepted ? 10f : 0f);
                if (score > bestScore)
                {
                    bestScore = score;
                    best = candidate;
                }
            }
            return best;
        }

        static float Range(Random random, float min, float max) => (float)(min + random.NextDouble() * (max - min));
    }
}
