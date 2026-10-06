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
    /// wrong shelf = soft return + hint (GDD 7.1, 7.2), sweeping the dirt layer (7.3), finding collectibles (9.3),
    /// the Auto Sort boost (10.2) and the 100% renovation (5.5). Views report to it, the HUD listens to its events.
    /// </summary>
    public class SectionController : MonoBehaviour
    {
        const float WallHeight = 2.6f;
        const float ShelfGap = 0.12f;
        const float FloorMargin = 0.45f;
        const float AutoSortInterval = 0.4f;

        public event Action<ItemView, ShelfView, int> ItemPlaced;   // item, shelf, coins
        public event Action<ItemView, ShelfView> WrongShelf;        // item, correct shelf
        public event Action<ShelfView> ShelfCompleted;
        public event Action SectionCompleted;
        public event Action<SectionProgress> ProgressChanged;
        public event Action<ItemView> ItemRevealed;
        public event Action<ItemView> CollectibleFound;
        /// <summary>The Auto Sort boost was switched on for this category (GDD 10.2).</summary>
        public event Action<CategoryDefinition> AutoSortStarted;
        /// <summary>Everything is sorted and swept but a collectible is still waiting to be picked up.</summary>
        public event Action OnlyCollectiblesLeft;
        /// <summary>Something worth saving happened (placement, sweep, box opened, find...).</summary>
        public event Action StateChanged;

        public SectionDefinition Definition { get; private set; }
        public SectionProgress Progress { get; private set; }
        public Bounds ViewBounds { get; private set; }
        public bool HasDirt => _dirt != null && _dirt.HasDirt;
        public bool IsComplete => _completed;
        public IReadOnlyList<ShelfView> Shelves => _shelves;
        public IReadOnlyList<ItemView> Items => _items;
        public IReadOnlyList<ContainerView> Containers => _containers;
        /// <summary>Everything that belongs on a shelf: common and rare items.</summary>
        public IEnumerable<ItemView> SortableItems => _items.Where(i => i != null && !i.IsCollectible);
        public IEnumerable<ItemView> Collectibles => _items.Where(i => i != null && i.IsCollectible);
        public float DirtCleaned => _dirt == null ? 1f : _dirt.CleanedFraction;

        /// <summary>Category that sorts itself in this room until it is finished; null while the boost is unused.</summary>
        public CategoryDefinition AutoSortCategory { get; private set; }
        /// <summary>One shelf per room (GDD 10.2).</summary>
        public bool CanStartAutoSort => IsLoaded && AutoSortCategory == null && !_completed;

        readonly List<ShelfView> _shelves = new();
        readonly List<ItemView> _items = new();
        readonly List<ContainerView> _containers = new();
        readonly List<ItemView> _buried = new();

        GameContext _ctx;
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
        int _seed;
        bool _shinyHintShown;
        float _autoSortTimer;
        readonly Dictionary<ItemView, MeshRenderer> _lensMarkers = new();

        DirtMask _dirt;
        DirtLayerView _dirtView;
        Material _floorMaterial;
        Material _wallMaterial;

        /// <param name="save">Saved state of this section; null = generate a fresh layout from the seed.</param>
        public void Build(SectionDefinition definition, GameContext context, int seed, SectionSave save = null)
        {
            Clear();
            Definition = definition;
            _ctx = context;
            _database = context.Database;
            _visuals = context.Visuals;
            _feel = context.Feel;
            _wallet = context.Wallet;
            _book = context.Book;
            _seed = seed;
            _factory ??= new PlaceholderFactory(_visuals);
            _fx ??= new Fx(_visuals);
            _root = new GameObject($"Section_{definition.Id}").transform;
            _root.SetParent(transform, false);

            BuildShelves(definition);
            BuildRoom(definition);

            int totalItems;
            if (save != null && save.SectionId == definition.Id)
            {
                totalItems = Restore(save);
            }
            else
            {
                // A collectible already in the book never shows up again (user decision 2026-10-06).
                var layout = SectionLayoutGenerator.Generate(definition, c => _database.CommonItemsOf(c).ToList(), seed, c => !_book.Has(c));
                var random = new Random(seed);
                BuildDirt(definition, seed);
                PlaceContainers(layout, random);
                var taken = _containers.Select(c => new Vector2(c.transform.localPosition.x, c.transform.localPosition.z)).ToList();
                PlaceBuried(layout, random, taken);
                PlaceLoose(layout, random, taken);
                totalItems = layout.TotalItems;
            }

            Progress = new SectionProgress(totalItems, HasDirt, _database.Balance.DirtProgressWeight);
            Progress.Restore(_shelves.Sum(s => s.FilledCount), DirtCleaned);
            Progress.SetCollectiblesRemaining(CountCollectiblesLeft());
            Progress.Changed += OnProgressChanged;
            _completed = Progress.IsComplete;

            // Everything the camera needs to see: floor plus the shelves standing on it.
            var tallest = _shelves.Count == 0 ? 1f : _shelves.Max(s => s.Size.y) + 0.35f;
            ViewBounds = new Bounds(new Vector3(0f, tallest / 2f, 0f), new Vector3(_floorSize.x, tallest, _floorSize.y));

            ApplyMood(_completed ? 1f : Progress.Fraction * _feel.ProgressMoodShare);
            var camera = Camera.main;
            if (camera != null) camera.backgroundColor = _completed ? _visuals.BackgroundColorClean : _visuals.BackgroundColor;
            SfxPlayer.Instance?.SetLoop(Sfx.CleanAmbienceLoop, _completed ? 0.6f : 0f);

            // Loaded with the boost already on: give the player a moment to see the room before items fly.
            _autoSortTimer = 0.8f;
        }

        /// <summary>Leaving the section view: free everything (only the active section is loaded, GDD 15.3).</summary>
        public void Unload()
        {
            Clear();
            Definition = null;
            SfxPlayer.Instance?.SetLoop(Sfx.CleanAmbienceLoop, 0f);
        }

        public bool IsLoaded => Definition != null && _root != null;
        /// <summary>Parent of everything in the room; what hangs here leaves with the room.</summary>
        public Transform Root => _root;

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
            _lensMarkers.Clear();
            _placeStreak = 0;
            _completed = false;
            _shinyHintShown = false;
            AutoSortCategory = null;
        }

        // ---------- Rules: shelves ----------

        public bool TryPlace(ItemView item, ShelfView shelf, Vector3 nearPoint)
        {
            if (item.Definition.Category == shelf.Category)
            {
                var slot = shelf.NearestFreeSlot(nearPoint);
                if (slot != null)
                {
                    item.FlyToSlot(slot, () => OnLanded(item, shelf), PlaceDuration);
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

        float PlaceDuration => _feel.PlaceDuration;

        public void OpenContainer(ContainerView container)
        {
            if (container == null || container.IsOpened) return;
            container.Open(_feel);
            StateChanged?.Invoke();
        }

        // ---------- Carrying several items (Hand capacity, Magnet) ----------

        /// <summary>
        /// Closest pickable common item within <paramref name="radius"/> of a floor point, optionally only of one category.
        /// Used while dragging: Hand collects anything it passes over, Magnet pulls its own category.
        /// </summary>
        public ItemView FindAttachable(Vector3 floorPoint, float radius, CategoryDefinition onlyCategory, ICollection<ItemView> exclude)
        {
            ItemView best = null;
            var bestDistance = radius;
            foreach (var item in _items)
            {
                if (item == null || item.IsCollectible || !item.CanPick || exclude.Contains(item)) continue;
                if (onlyCategory != null && item.Definition.Category != onlyCategory) continue;
                var distance = FlatDistance(item.transform.position, floorPoint);
                if (distance > bestDistance) continue;
                best = item;
                bestDistance = distance;
            }
            return best;
        }

        /// <summary>
        /// Puts every carried item that belongs on <paramref name="shelf"/> into its slots, one after another.
        /// Returns the delivered items. When <paramref name="releasing"/>, the rest go back where they were picked up
        /// (no penalty, GDD 7.2); otherwise they stay in hand for the next shelf.
        /// </summary>
        public List<ItemView> DeliverStack(IReadOnlyList<ItemView> stack, ShelfView shelf, Vector3 nearPoint, bool releasing)
        {
            var delivered = new List<ItemView>();
            foreach (var item in stack)
            {
                if (item.Definition.Category != shelf.Category) continue;
                var slot = shelf.NearestFreeSlot(nearPoint);
                if (slot == null) break;
                var landing = item;
                landing.FlyToSlot(slot, () => OnLanded(landing, shelf), PlaceDuration, delivered.Count == 0 ? 0f : 0.15f, 0.07f * delivered.Count);
                delivered.Add(item);
            }

            if (!releasing) return delivered;

            var misses = stack.Where(i => !delivered.Contains(i)).ToList();
            foreach (var item in misses) item.ReturnToPickup();
            if (delivered.Count == 0 && misses.Count > 0)
            {
                var correct = ShelfFor(misses[0].Definition.Category);
                if (correct != null && correct != shelf) correct.FlashHint(_feel.WrongShelfHintDuration);
                SfxPlayer.Instance?.Play(Sfx.Wrong);
                WrongShelf?.Invoke(misses[0], correct);
            }
            return delivered;
        }

        static float FlatDistance(Vector3 a, Vector3 b) => Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));

        // ---------- Auto Sort boost (GDD 10.2) ----------

        /// <summary>
        /// Switches the boost on for this shelf's category: its common items fly to the shelf by themselves until
        /// the room is finished, whether they lie on the floor now, spill from a box or are swept free later.
        /// Rare items and collectibles stay for the player (principle 6). The caller has paid (ad or charge).
        /// </summary>
        public bool StartAutoSort(ShelfView shelf)
        {
            if (!CanStartAutoSort || shelf == null || !shelf.HasFreeSlot) return false;
            AutoSortCategory = shelf.Category;
            _autoSortTimer = AutoSortInterval;
            SfxPlayer.Instance?.Play(Sfx.Mastery, 0f);
            Haptics.Strong();
            AutoSortStarted?.Invoke(shelf.Category);
            AutoSortLoose(0.35f, 0.08f);
            StateChanged?.Invoke();
            return true;
        }

        bool IsAutoSorted(ItemDefinition definition) =>
            AutoSortCategory != null && definition.Rarity == ItemRarity.Common && definition.Category == AutoSortCategory;

        /// <summary>A box spilled an item: the boosted category flies straight to its shelf, the rest tumble.</summary>
        void SpillFromContainer(ItemDefinition definition, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angular)
        {
            var view = SpawnItem(definition, position, rotation);
            if (IsAutoSorted(definition) && AutoSort(view, 0f)) return;
            view.Launch(velocity, angular);
        }

        bool AutoSort(ItemView item, float delay)
        {
            var shelf = ShelfFor(item.Definition.Category);
            var slot = shelf != null ? shelf.NearestFreeSlot(item.transform.position) : null;
            if (slot == null) return false;
            item.FlyToSlot(slot, () => OnLanded(item, shelf), 0.55f, 0.9f, delay);
            return true;
        }

        /// <summary>Every boosted item that lies free right now takes off, one after another.</summary>
        void AutoSortLoose(float firstDelay, float stagger)
        {
            var loose = _items
                .Where(i => i != null && IsAutoSorted(i.Definition) && i.State is ItemState.Resting or ItemState.Physics)
                .ToList();
            for (var i = 0; i < loose.Count; i++) AutoSort(loose[i], firstDelay + i * stagger);
        }

        /// <summary>
        /// Safety net behind the direct hooks (spill, reveal): whatever ends up loose later, e.g. an item the player
        /// dropped or one that came back from a wrong shelf, is picked up on the next tick.
        /// </summary>
        void Update()
        {
            if (AutoSortCategory == null || _root == null) return;
            _autoSortTimer -= Time.deltaTime;
            if (_autoSortTimer > 0f) return;
            _autoSortTimer = AutoSortInterval;
            AutoSortLoose(0f, 0.08f);
        }

        // ---------- Helpers (GDD 10.3) ----------

        /// <summary>
        /// Nearest loose common item a helper may take: lying still, not rare, not already flying by itself
        /// (Auto Sort) and not claimed by another helper. Scanned fresh every time, so items that turn up later
        /// (a box opened, dirt swept, something dropped) are found like any other.
        /// </summary>
        public ItemView FindHelperTarget(Vector3 from, float maxDistance, ICollection<ItemView> taken)
        {
            ItemView best = null;
            var bestDistance = maxDistance;
            foreach (var item in _items)
            {
                if (item == null || item.State != ItemState.Resting || item.Definition.Rarity != ItemRarity.Common) continue;
                if (IsAutoSorted(item.Definition) || taken.Contains(item)) continue;
                var shelf = ShelfFor(item.Definition.Category);
                if (shelf == null || !shelf.HasFreeSlot) continue;
                var distance = FlatDistance(item.transform.position, from);
                if (distance >= bestDistance) continue;
                best = item;
                bestDistance = distance;
            }
            return best;
        }

        /// <summary>Floor point in front of the shelf slot the item will go to.</summary>
        public bool HelperStandPoint(ItemView item, Vector3 from, out Vector3 point)
        {
            point = default;
            var shelf = ShelfFor(item.Definition.Category);
            var slot = shelf != null ? shelf.NearestFreeSlot(from) : null;
            if (slot == null) return false;
            point = new Vector3(slot.WorldBase.x, 0f, shelf.transform.position.z - shelf.Size.z / 2f - 0.3f);
            return true;
        }

        /// <summary>A helper puts the item it carries on its shelf. Same coins as the player (GDD 10.3), quieter feedback.</summary>
        public bool HelperPlace(ItemView item)
        {
            if (item == null || item.State != ItemState.Dragging) return false;
            var shelf = ShelfFor(item.Definition.Category);
            var slot = shelf != null ? shelf.NearestFreeSlot(item.transform.position) : null;
            if (slot == null) return false;
            item.FlyToSlot(slot, () => OnLanded(item, shelf, true), 0.4f, 0.35f);
            return true;
        }

        /// <summary>Somewhere on the free floor, for strolling.</summary>
        public Vector3 RandomFloorPoint()
        {
            var free = FreeFloor();
            var local = new Vector3(UnityEngine.Random.Range(free.xMin, free.xMax), 0f, UnityEngine.Random.Range(free.yMin, free.yMax));
            return _root.TransformPoint(local);
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

        /// <param name="byHelper">Placed by a helper: quieter, no haptics, and it leaves the player's chain alone.</param>
        void OnLanded(ItemView item, ShelfView shelf, bool byHelper = false)
        {
            var coins = item.Definition.CoinValue;
            _wallet.Add(coins);

            if (byHelper)
            {
                SfxPlayer.Instance?.PlayPitched(SfxPlayer.PlaceSoundFor(shelf.Category.PlaceSound), 1.15f, 0.45f);
                SfxPlayer.Instance?.Play(Sfx.Coin, 0.03f, 0.25f);
            }
            else
            {
                // Quick chains climb in pitch: small reward for flow.
                _placeStreak = Time.time - _lastPlaceTime < 1.5f ? Mathf.Min(_placeStreak + 1, 8) : 0;
                _lastPlaceTime = Time.time;
                SfxPlayer.Instance?.PlayPitched(SfxPlayer.PlaceSoundFor(shelf.Category.PlaceSound), 1f + _placeStreak * 0.04f);
                SfxPlayer.Instance?.Play(Sfx.Coin, 0.03f, 0.6f);
                Haptics.Light();
            }

            if (item.IsRare)
            {
                // GDD 9.4: a rare item on its shelf gets a small celebration of its own.
                var sparkle = _visuals.RareItemGlowColor;
                sparkle.a = 1f;
                _fx.Burst(item.transform.position, sparkle, 18, 1.4f, 0.08f, 0.7f);
                SfxPlayer.Instance?.Play(Sfx.RareItemPlaced, 0f);
                Haptics.Medium();
            }

            ItemPlaced?.Invoke(item, shelf, coins);

            if (shelf.IsFull)
            {
                shelf.Celebrate();
                SfxPlayer.Instance?.Play(Sfx.ShelfFull, 0f);
                Haptics.Medium();
                ShelfCompleted?.Invoke(shelf);
            }

            Progress.AddPlaced();
            StateChanged?.Invoke();
        }

        // ---------- Rules: dirt ----------

        /// <summary>Broom stroke at a floor point. Returns true if any dirt was removed (drives dust + sound).</summary>
        public bool Sweep(Vector3 worldPoint, float deltaTime) => Sweep(worldPoint, deltaTime, _ctx.ToolStats(ToolType.Broom).Primary);

        public bool Sweep(Vector3 worldPoint, float deltaTime, float radius)
        {
            if (_dirt == null || !_dirt.HasDirt || _dirt.CleanedFraction >= 1f) return false;

            var local = _root.InverseTransformPoint(worldPoint);
            var u = local.x / _floorSize.x + 0.5f;
            var v = local.z / _floorSize.y + 0.5f;
            if (u < -0.1f || u > 1.1f || v < -0.1f || v > 1.1f) return false;

            var rect = _dirt.Brush(u, v, radius / _floorSize.x, _feel.BroomStrength * deltaTime);
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
            if (_lensMarkers.TryGetValue(item, out var marker))
            {
                if (marker != null) Destroy(marker.gameObject);
                _lensMarkers.Remove(item);
            }
            _fx.Burst(item.transform.position + Vector3.up * 0.1f, _visuals.DirtSpeckColor, 10, 1.2f, 0.07f, 0.5f, 0.5f);
            SfxPlayer.Instance?.Play(Sfx.Reveal);
            Haptics.Light();
            ItemRevealed?.Invoke(item);
            // Swept free while its category is boosted: straight to the shelf once it has popped out.
            if (IsAutoSorted(item.Definition)) AutoSort(item, 0.4f);
            StateChanged?.Invoke();
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

        /// <summary>Player tapped a glowing Chubby (GDD 9.3): it goes into the book and never spawns again.</summary>
        public void FindCollectible(ItemView item)
        {
            if (item == null || !item.CanTapToFind || item.Definition is not CollectibleDefinition collectible) return;
            item.MarkFound();
            _items.Remove(item);
            _book.Register(collectible);
            CollectibleFound?.Invoke(item);
            Progress.SetCollectiblesRemaining(CountCollectiblesLeft());
            StateChanged?.Invoke();
        }

        /// <summary>Collectibles on the floor, under the dirt or still inside closed boxes.</summary>
        int CountCollectiblesLeft()
        {
            var count = _items.Count(i => i != null && i.IsCollectible && i.State != ItemState.Found);
            foreach (var container in _containers)
                if (container != null) count += container.Contents.Count(c => c.IsCollectible);
            return count;
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
            if (progress.OnlyCollectiblesLeft && !_shinyHintShown)
            {
                _shinyHintShown = true;
                OnlyCollectiblesLeft?.Invoke();
            }
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

        // ---------- Save / load (GDD 15.4) ----------

        public SectionSave Capture()
        {
            var save = new SectionSave
            {
                SectionId = Definition.Id,
                Seed = _seed,
                Completed = _completed,
                PlacedItems = Progress.PlacedItems,
                TotalItems = Progress.TotalItems,
                Fraction = Progress.Fraction,
                AutoSortCategoryId = AutoSortCategory != null ? AutoSortCategory.Id : ""
            };

            for (var s = 0; s < _shelves.Count; s++)
            for (var k = 0; k < _shelves[s].Slots.Count; k++)
            {
                var occupant = _shelves[s].Slots[k].Occupant;
                if (occupant == null) continue;
                save.Items.Add(new ItemSave { Id = occupant.Definition.Id, State = ItemSaveState.Placed, Shelf = s, Slot = k });
            }

            foreach (var item in _items)
            {
                if (item == null || item.State is ItemState.Placed or ItemState.Found) continue;
                var (position, rotation) = item.SavePose;
                save.Items.Add(new ItemSave
                {
                    Id = item.Definition.Id,
                    State = item.State == ItemState.Buried ? ItemSaveState.Buried : ItemSaveState.Floor,
                    Position = position,
                    Rotation = rotation
                });
            }

            foreach (var container in _containers)
            {
                if (container == null || container.IsOpened) continue;
                var entry = new ContainerSave
                {
                    Id = container.Definition.Id,
                    Position = container.transform.localPosition,
                    Yaw = container.transform.localEulerAngles.y
                };
                entry.Contents.AddRange(container.Contents.Select(c => c.Id));
                save.Containers.Add(entry);
            }

            if (_dirt != null)
            {
                save.DirtWidth = _dirt.Width;
                save.DirtHeight = _dirt.Height;
                save.DirtInitialTotal = _dirt.InitialTotal;
                save.DirtData = SaveSystem.Pack(_dirt.Export());
            }
            return save;
        }

        /// <summary>Rebuilds items, boxes and dirt from a save. Returns the number of shelf items (for the %).</summary>
        int Restore(SectionSave save)
        {
            _seed = save.Seed;
            AutoSortCategory = string.IsNullOrEmpty(save.AutoSortCategoryId) ? null : _database.CategoryById(save.AutoSortCategoryId);
            if (!string.IsNullOrEmpty(save.DirtData) && save.DirtWidth > 0)
            {
                _dirt = new DirtMask(save.DirtWidth, save.DirtHeight);
                _dirt.Import(SaveSystem.Unpack(save.DirtData), save.DirtInitialTotal);
                CreateDirtView();
            }

            var total = 0;
            foreach (var entry in save.Containers)
            {
                var definition = _database.ContainerById(entry.Id);
                if (definition == null) continue;
                var contents = entry.Contents.Select(_database.ItemById).Where(i => i != null).ToList();
                total += contents.Count(i => !i.IsCollectible);
                CreateContainer(definition, contents, entry.Position, entry.Yaw);
            }

            foreach (var entry in save.Items)
            {
                var definition = _database.ItemById(entry.Id);
                if (definition == null) continue;
                if (!definition.IsCollectible) total++;
                var view = SpawnItem(definition, entry.Position, entry.Rotation);
                var placeable = entry.State == ItemSaveState.Placed && entry.Shelf < _shelves.Count
                                && entry.Slot < _shelves[entry.Shelf].Slots.Count && _shelves[entry.Shelf].Slots[entry.Slot].IsFree;
                if (placeable)
                {
                    view.PlaceInstant(_shelves[entry.Shelf].Slots[entry.Slot]);
                }
                else if (entry.State == ItemSaveState.Buried && _dirt != null)
                {
                    view.Bury(entry.Position, entry.Rotation);
                    _buried.Add(view);
                }
                else
                {
                    view.SetResting(entry.Position, entry.Rotation);
                    if (entry.Position.y > 0.6f) view.Launch(Vector3.zero, Vector3.zero); // saved mid-air
                }
            }
            return total;
        }

        // ---------- Construction ----------

        void BuildRoom(SectionDefinition definition)
        {
            var w = _floorSize.x;
            var d = _floorSize.y;
            // Tall bookcases need a taller back wall.
            var wallHeight = Mathf.Max(WallHeight, (_shelves.Count == 0 ? 0f : _shelves.Max(s => s.Size.y)) + 0.5f);

            _floorMaterial = _factory.UniqueLit(_visuals.FloorColor);
            _wallMaterial = _factory.UniqueLit(_visuals.WallColor);

            SetMaterial(_factory.CreateBox("Floor", _root, new Vector3(0f, -0.05f, 0f), new Vector3(w + 0.4f, 0.1f, d + 0.4f), _visuals.FloorColor), _floorMaterial);
            SetMaterial(_factory.CreateBox("BackWall", _root, new Vector3(0f, wallHeight / 2f, d / 2f + 0.05f), new Vector3(w + 0.4f, wallHeight, 0.1f), _visuals.WallColor), _wallMaterial);
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
            CreateDirtView();
        }

        void CreateDirtView()
        {
            var go = new GameObject("DirtLayer");
            go.transform.SetParent(_root, false);
            _dirtView = go.AddComponent<DirtLayerView>();
            _dirtView.Build(_dirt, _floorSize, _visuals, _seed);
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
            _floorSize = definition.FloorSize;
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

            // Spilled items must not land on shelf boards: they would look sorted without being sorted.
            // Drags and slot flights ignore physics, so this only stops tumbling items.
            if (_shelves.Count > 0)
                AddInvisibleWall("ShelfGuard", new Vector3(0f, 1.5f, _floorSize.y / 2f - _shelfZoneDepth - 0.06f), new Vector3(_floorSize.x + 0.4f, 3f, 0.1f));
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

                // Tip towards the open middle of the room and slightly towards the camera, so the spill is visible.
                var toCentre = new Vector3(free.center.x - spot.x, 0f, free.center.y - spot.y - 0.6f);
                var yaw = Mathf.Atan2(toCentre.x, toCentre.z) * Mathf.Rad2Deg + Range(random, -25f, 25f);

                // Collectibles hide among the commons inside the box.
                var contents = new List<ItemDefinition>(content.Items);
                foreach (var collectible in content.Collectibles)
                    contents.Insert(random.Next(contents.Count + 1), collectible);

                CreateContainer(content.Container, contents, new Vector3(spot.x, 0f, spot.y), yaw);
            }
        }

        void CreateContainer(ContainerDefinition definition, List<ItemDefinition> contents, Vector3 localPosition, float yaw)
        {
            var go = new GameObject($"Container_{definition.Id}");
            go.transform.SetParent(_root, false);
            go.transform.localPosition = localPosition;
            go.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
            var view = go.AddComponent<ContainerView>();
            view.Build(definition, contents, _factory);
            view.Spill = SpillFromContainer;
            view.Emptied += _ => StateChanged?.Invoke();
            _containers.Add(view);
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
            var glow = definition.IsCollectible ? _visuals.RareGlowColor : _visuals.RareItemGlowColor;
            var view = ItemView.Create(definition, _factory, _feel, _root, _fx, glow);
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
