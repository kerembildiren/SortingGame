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
    /// wrong shelf = soft return + hint (GDD 7.1, 7.2). Views report to it, the HUD listens to its events.
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

        public SectionDefinition Definition { get; private set; }
        public SectionProgress Progress { get; private set; }
        public Bounds ViewBounds { get; private set; }
        public IReadOnlyList<ShelfView> Shelves => _shelves;
        public IReadOnlyList<ItemView> Items => _items;
        public IReadOnlyList<ContainerView> Containers => _containers;

        readonly List<ShelfView> _shelves = new();
        readonly List<ItemView> _items = new();
        readonly List<ContainerView> _containers = new();

        GameDatabase _database;
        SectionVisuals _visuals;
        FeelConfig _feel;
        Wallet _wallet;
        PlaceholderFactory _factory;
        Transform _root;
        Vector2 _floorSize;
        float _shelfZoneDepth;
        int _placeStreak;
        float _lastPlaceTime;
        Coroutine _moodTween;

        public void Build(SectionDefinition definition, GameDatabase database, SectionVisuals visuals, Wallet wallet, int seed)
        {
            Clear();
            Definition = definition;
            _database = database;
            _visuals = visuals;
            _feel = database.Feel;
            _wallet = wallet;
            _factory ??= new PlaceholderFactory(visuals);
            _root = new GameObject($"Section_{definition.Id}").transform;
            _root.SetParent(transform, false);

            var layout = SectionLayoutGenerator.Generate(definition, c => _database.CommonItemsOf(c).ToList(), seed);
            Progress = new SectionProgress(layout.TotalItems, false, database.Balance.DirtProgressWeight);
            Progress.Changed += p => ProgressChanged?.Invoke(p);

            BuildRoom(definition);
            BuildShelves(definition);
            var random = new Random(seed);
            PlaceContainers(layout, random);
            PlaceLooseItems(layout, random);

            // Everything the camera needs to see: floor plus the shelves standing on it.
            var tallest = _shelves.Count == 0 ? 1f : _shelves.Max(s => s.Size.y) + 0.35f;
            ViewBounds = new Bounds(new Vector3(0f, tallest / 2f, 0f), new Vector3(_floorSize.x, tallest, _floorSize.y));
            ApplyMood(0f);
        }

        public void Clear()
        {
            if (_root != null) Destroy(_root.gameObject);
            Tween.Stop(_moodTween);
            _shelves.Clear();
            _items.Clear();
            _containers.Clear();
            _placeStreak = 0;
        }

        // ---------- Rules ----------

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
            Progress.AddPlaced();

            if (shelf.IsFull)
            {
                shelf.Celebrate();
                SfxPlayer.Instance?.Play(Sfx.ShelfFull, 0f);
                Haptics.Medium();
                ShelfCompleted?.Invoke(shelf);
            }

            if (Progress.IsComplete)
            {
                SfxPlayer.Instance?.Play(Sfx.SectionComplete, 0f);
                Haptics.Strong();
                _moodTween = Tween.Run(this, 1.6f, ApplyMood, Ease.InOutQuad);
                SectionCompleted?.Invoke();
            }
        }

        /// <summary>0 = dusty and dim, 1 = warm and bright (GDD 12.1). M2 adds the full before/after.</summary>
        void ApplyMood(float clean)
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = Color.Lerp(_visuals.AmbientDirty, _visuals.AmbientClean, clean);
            var sun = RenderSettings.sun;
            if (sun != null)
            {
                sun.color = _visuals.LightColor;
                sun.intensity = Mathf.Lerp(_visuals.LightIntensityDirty, _visuals.LightIntensityClean, clean);
            }
        }

        // ---------- Construction ----------

        void BuildRoom(SectionDefinition definition)
        {
            _floorSize = definition.FloorSize;
            var w = _floorSize.x;
            var d = _floorSize.y;

            _factory.CreateBox("Floor", _root, new Vector3(0f, -0.05f, 0f), new Vector3(w + 0.4f, 0.1f, d + 0.4f), _visuals.FloorColor);
            _factory.CreateBox("BackWall", _root, new Vector3(0f, WallHeight / 2f, d / 2f + 0.05f), new Vector3(w + 0.4f, WallHeight, 0.1f), _visuals.WallColor);
            // Low side walls read as a room without blocking the 3/4 view.
            _factory.CreateBox("WallL", _root, new Vector3(-w / 2f - 0.1f, 0.2f, 0f), new Vector3(0.2f, 0.4f, d + 0.4f), _visuals.WallColor);
            _factory.CreateBox("WallR", _root, new Vector3(w / 2f + 0.1f, 0.2f, 0f), new Vector3(0.2f, 0.4f, d + 0.4f), _visuals.WallColor);

            // Invisible walls keep tumbling items inside.
            AddInvisibleWall("BlockL", new Vector3(-w / 2f - 0.1f, 1.5f, 0f), new Vector3(0.2f, 3f, d + 0.4f));
            AddInvisibleWall("BlockR", new Vector3(w / 2f + 0.1f, 1.5f, 0f), new Vector3(0.2f, 3f, d + 0.4f));
            AddInvisibleWall("BlockFront", new Vector3(0f, 1.5f, -d / 2f - 0.1f), new Vector3(w + 0.4f, 3f, 0.2f));
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
                var spot = FindSpot(free, placed, 1.1f, random);
                placed.Add(spot);

                var go = new GameObject($"Container_{content.Container.Id}");
                go.transform.SetParent(_root, false);
                go.transform.localPosition = new Vector3(spot.x, 0f, spot.y);

                // Tip towards the open middle of the room and slightly towards the camera, so the spill is visible.
                var toCentre = new Vector3(free.center.x - spot.x, 0f, free.center.y - spot.y - 0.6f);
                var yaw = Mathf.Atan2(toCentre.x, toCentre.z) * Mathf.Rad2Deg + Range(random, -25f, 25f);
                go.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);

                var view = go.AddComponent<ContainerView>();
                view.Build(content.Container, content.Items, _factory);
                view.SpawnItem = SpawnItem;
                _containers.Add(view);
            }
        }

        void PlaceLooseItems(SectionLayout layout, Random random)
        {
            var free = FreeFloor();
            // Keep loose items apart from boxes and from each other so nothing intersects at rest.
            var taken = _containers.Select(c => new Vector2(c.transform.localPosition.x, c.transform.localPosition.z)).ToList();
            foreach (var definition in layout.LooseItems)
            {
                var spot = FindSpot(free, taken, 0.45f, random);
                taken.Add(spot);
                var (rest, height) = PlaceholderFactory.RestPose(definition.Placeholder);
                var rotation = Quaternion.Euler(0f, Range(random, 0f, 360f), 0f) * rest;
                var view = SpawnItem(definition, new Vector3(spot.x, height + 0.002f, spot.y), rotation);
                view.SetResting(view.transform.position, rotation);
            }
        }

        ItemView SpawnItem(ItemDefinition definition, Vector3 position, Quaternion rotation)
        {
            var view = ItemView.Create(definition, _factory, _feel, _root);
            view.transform.SetPositionAndRotation(position, rotation);
            _items.Add(view);
            return view;
        }

        static Vector2 FindSpot(Rect area, List<Vector2> avoid, float minDistance, Random random)
        {
            var best = area.center;
            var bestClearance = -1f;
            for (var attempt = 0; attempt < 40; attempt++)
            {
                var candidate = new Vector2(Range(random, area.xMin, area.xMax), Range(random, area.yMin, area.yMax));
                var clearance = float.MaxValue;
                foreach (var other in avoid) clearance = Mathf.Min(clearance, Vector2.Distance(candidate, other));
                if (clearance >= minDistance) return candidate;
                if (clearance > bestClearance)
                {
                    bestClearance = clearance;
                    best = candidate;
                }
            }
            return best;
        }

        static float Range(Random random, float min, float max) => (float)(min + random.NextDouble() * (max - min));
    }
}
