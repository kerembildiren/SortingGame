using System.Collections.Generic;
using SortingGame.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace SortingGame.EditorTools
{
    /// <summary>
    /// Creates the placeholder content: 3 venues (Comic Box, Garage, Warehouse with 4 sections), 7 categories,
    /// collectibles with one costumed mascot per venue, tools.
    /// "Create" only fills in what is missing, so inspector edits survive.
    /// "Rebuild" overwrites the sample assets with the values below.
    /// </summary>
    public static class ContentBuilder
    {
        const string Root = "Assets/_Project";
        const string ContentFolder = Root + "/Data/Content";
        const string MaterialsFolder = Root + "/Materials";
        const string UiFolder = Root + "/UI";

        public const string VisualsPath = Root + "/Data/SectionVisuals.asset";
        public const string FeelPath = Root + "/Data/FeelConfig.asset";
        public const string PanelSettingsPath = UiFolder + "/HudPanelSettings.asset";
        public const string HudStylePath = UiFolder + "/SectionHud.uss";
        public const string StartSectionPath = ContentFolder + "/Section_Garage.asset";

        [MenuItem("Sorting Game/Setup/Create Missing Sample Content")]
        public static void CreateMissing() => Build(false);

        [MenuItem("Sorting Game/Setup/Rebuild Sample Content (overwrite)")]
        public static void Rebuild() => Build(true);

        static void Build(bool overwrite)
        {
            EnsureFolder(ContentFolder);
            EnsureFolder(MaterialsFolder);

            var database = ProjectSetup.LoadOrCreate<GameDatabase>(ProjectSetup.DatabasePath);
            database.Balance = ProjectSetup.LoadOrCreate<BalanceConfig>(ProjectSetup.BalancePath);
            database.Feel = ProjectSetup.LoadOrCreate<FeelConfig>(FeelPath);

            var lit = LitMaterial();
            var ghost = GhostMaterial();
            var visuals = ProjectSetup.LoadOrCreate<SectionVisuals>(VisualsPath);
            visuals.LitMaterial = lit;
            visuals.GhostMaterial = ghost;
            visuals.DirtMaterial = DirtMaterial();
            visuals.ParticleMaterial = ParticleMaterial();
            EditorUtility.SetDirty(visuals);

            // ---- Categories (GDD 8.2: recognisable at a glance) ----
            // Comics = flat colourful slabs, Toys = bright rounded shapes, Tools = grey/steel rods, cans and boxes.
            // Sizes are tuned so items stay readable and touchable on a phone (about 1 cm on screen).
            var comics = Category("comics", 1, new Vector3(0.31f, 0.42f, 0.08f), PlaceSoundType.Paper, overwrite);
            var toys = Category("toys", 2, new Vector3(0.36f, 0.42f, 0.34f), PlaceSoundType.Plastic, overwrite);
            var tools = Category("tools", 2, new Vector3(0.36f, 0.44f, 0.31f), PlaceSoundType.Metal, overwrite);

            var items = new List<ItemDefinition>();
            var book = new Vector3(0.29f, 0.39f, 0.045f);
            items.Add(Item("comic_red", comics, PlaceholderShape.Book, new Color(0.86f, 0.24f, 0.22f), book, overwrite));
            items.Add(Item("comic_blue", comics, PlaceholderShape.Book, new Color(0.22f, 0.45f, 0.86f), book, overwrite));
            items.Add(Item("comic_yellow", comics, PlaceholderShape.Book, new Color(0.98f, 0.80f, 0.20f), book, overwrite));
            items.Add(Item("comic_green", comics, PlaceholderShape.Book, new Color(0.30f, 0.72f, 0.36f), book, overwrite));
            items.Add(Item("comic_purple", comics, PlaceholderShape.Book, new Color(0.58f, 0.36f, 0.80f), book * 0.95f, overwrite));
            items.Add(Item("comic_orange", comics, PlaceholderShape.Book, new Color(0.98f, 0.55f, 0.20f), book * 1.05f, overwrite));

            items.Add(Item("toy_ball", toys, PlaceholderShape.Sphere, new Color(0.95f, 0.30f, 0.35f), new Vector3(0.26f, 0.26f, 0.26f), overwrite));
            items.Add(Item("toy_robot", toys, PlaceholderShape.Capsule, new Color(0.35f, 0.60f, 0.95f), new Vector3(0.18f, 0.39f, 0.18f), overwrite));
            items.Add(Item("toy_dino", toys, PlaceholderShape.Capsule, new Color(0.40f, 0.85f, 0.35f), new Vector3(0.21f, 0.31f, 0.21f), overwrite));
            items.Add(Item("toy_teddy", toys, PlaceholderShape.Sphere, new Color(0.72f, 0.48f, 0.28f), new Vector3(0.31f, 0.36f, 0.29f), overwrite));
            items.Add(Item("toy_duckling", toys, PlaceholderShape.Sphere, new Color(1.00f, 0.86f, 0.25f), new Vector3(0.23f, 0.21f, 0.26f), overwrite));
            items.Add(Item("toy_bunny", toys, PlaceholderShape.Capsule, new Color(1.00f, 0.62f, 0.82f), new Vector3(0.20f, 0.34f, 0.20f), overwrite));

            // Tools: light steel tones so they stand out from the dusty floor.
            items.Add(Item("tool_wrench", tools, PlaceholderShape.Rod, new Color(0.80f, 0.82f, 0.86f), new Vector3(0.08f, 0.40f, 0.08f), overwrite));
            items.Add(Item("tool_screwdriver", tools, PlaceholderShape.Rod, new Color(0.58f, 0.64f, 0.72f), new Vector3(0.07f, 0.34f, 0.07f), overwrite));
            items.Add(Item("tool_hammer", tools, PlaceholderShape.Rod, new Color(0.66f, 0.48f, 0.32f), new Vector3(0.09f, 0.40f, 0.09f), overwrite));
            items.Add(Item("tool_can", tools, PlaceholderShape.Cylinder, new Color(0.64f, 0.70f, 0.76f), new Vector3(0.21f, 0.26f, 0.21f), overwrite));
            items.Add(Item("tool_box", tools, PlaceholderShape.Cube, new Color(0.42f, 0.46f, 0.52f), new Vector3(0.34f, 0.21f, 0.21f), overwrite));
            items.Add(Item("tool_oiler", tools, PlaceholderShape.Cylinder, new Color(0.86f, 0.88f, 0.92f), new Vector3(0.16f, 0.36f, 0.16f), overwrite));

            // M4 categories for the warehouse (GDD 8.2: every new venue brings new ones).
            // Stationery = thin pastel sticks + small blocks, Mugs = short creamy cylinders,
            // Tyres = flat dark discs, Bottles = tall glass-coloured cylinders.
            var stationery = Category("stationery", 2, new Vector3(0.3f, 0.42f, 0.2f), PlaceSoundType.Plastic, overwrite);
            var mugs = Category("mugs", 2, new Vector3(0.3f, 0.34f, 0.3f), PlaceSoundType.Plastic, overwrite);
            var tyres = Category("tyres", 3, new Vector3(0.42f, 0.42f, 0.4f), PlaceSoundType.Paper, overwrite);
            var bottles = Category("bottles", 2, new Vector3(0.24f, 0.44f, 0.24f), PlaceSoundType.Metal, overwrite);

            items.Add(Item("pen_blue", stationery, PlaceholderShape.Rod, new Color(0.55f, 0.70f, 1.00f), new Vector3(0.05f, 0.34f, 0.05f), overwrite));
            items.Add(Item("pen_pink", stationery, PlaceholderShape.Rod, new Color(1.00f, 0.60f, 0.80f), new Vector3(0.05f, 0.30f, 0.05f), overwrite));
            items.Add(Item("stapler", stationery, PlaceholderShape.Cube, new Color(0.50f, 0.90f, 0.75f), new Vector3(0.26f, 0.12f, 0.10f), overwrite));
            items.Add(Item("ruler", stationery, PlaceholderShape.Cube, new Color(0.75f, 0.65f, 1.00f), new Vector3(0.06f, 0.38f, 0.03f), overwrite));

            items.Add(Item("mug_cream", mugs, PlaceholderShape.Cylinder, new Color(0.96f, 0.92f, 0.84f), new Vector3(0.20f, 0.22f, 0.20f), overwrite));
            items.Add(Item("mug_mint", mugs, PlaceholderShape.Cylinder, new Color(0.70f, 0.92f, 0.82f), new Vector3(0.20f, 0.22f, 0.20f), overwrite));
            items.Add(Item("mug_peach", mugs, PlaceholderShape.Cylinder, new Color(1.00f, 0.80f, 0.68f), new Vector3(0.22f, 0.20f, 0.22f), overwrite));
            items.Add(Item("mug_tall", mugs, PlaceholderShape.Cylinder, new Color(0.82f, 0.88f, 1.00f), new Vector3(0.18f, 0.28f, 0.18f), overwrite));

            items.Add(Item("tyre_big", tyres, PlaceholderShape.Cylinder, new Color(0.14f, 0.14f, 0.16f), new Vector3(0.40f, 0.14f, 0.40f), overwrite));
            items.Add(Item("tyre_small", tyres, PlaceholderShape.Cylinder, new Color(0.20f, 0.20f, 0.22f), new Vector3(0.32f, 0.12f, 0.32f), overwrite));
            items.Add(Item("hubcap", tyres, PlaceholderShape.Cylinder, new Color(0.78f, 0.80f, 0.84f), new Vector3(0.34f, 0.06f, 0.34f), overwrite));
            items.Add(Item("tyre_white", tyres, PlaceholderShape.Cylinder, new Color(0.25f, 0.24f, 0.24f), new Vector3(0.36f, 0.16f, 0.36f), overwrite));

            items.Add(Item("bottle_green", bottles, PlaceholderShape.Cylinder, new Color(0.30f, 0.60f, 0.35f), new Vector3(0.12f, 0.38f, 0.12f), overwrite));
            items.Add(Item("bottle_brown", bottles, PlaceholderShape.Cylinder, new Color(0.50f, 0.32f, 0.18f), new Vector3(0.12f, 0.36f, 0.12f), overwrite));
            items.Add(Item("bottle_blue", bottles, PlaceholderShape.Cylinder, new Color(0.30f, 0.45f, 0.75f), new Vector3(0.11f, 0.40f, 0.11f), overwrite));
            items.Add(Item("bottle_clear", bottles, PlaceholderShape.Cylinder, new Color(0.80f, 0.90f, 0.92f), new Vector3(0.13f, 0.34f, 0.13f), overwrite));

            // More variants for the warehouse categories: variety grows along the ladder (M4.2).
            items.Add(Item("marker_green", stationery, PlaceholderShape.Rod, new Color(0.55f, 0.90f, 0.55f), new Vector3(0.06f, 0.30f, 0.06f), overwrite));
            items.Add(Item("eraser", stationery, PlaceholderShape.Cube, new Color(1.00f, 0.90f, 0.55f), new Vector3(0.14f, 0.07f, 0.09f), overwrite));
            items.Add(Item("mug_navy", mugs, PlaceholderShape.Cylinder, new Color(0.45f, 0.55f, 0.85f), new Vector3(0.20f, 0.22f, 0.20f), overwrite));
            items.Add(Item("mug_lemon", mugs, PlaceholderShape.Cylinder, new Color(1.00f, 0.93f, 0.55f), new Vector3(0.19f, 0.24f, 0.19f), overwrite));
            items.Add(Item("tyre_bike", tyres, PlaceholderShape.Cylinder, new Color(0.18f, 0.18f, 0.20f), new Vector3(0.38f, 0.07f, 0.38f), overwrite));
            items.Add(Item("bottle_amber", bottles, PlaceholderShape.Cylinder, new Color(0.80f, 0.55f, 0.20f), new Vector3(0.12f, 0.32f, 0.12f), overwrite));
            items.Add(Item("bottle_rose", bottles, PlaceholderShape.Cylinder, new Color(0.85f, 0.50f, 0.60f), new Vector3(0.11f, 0.38f, 0.11f), overwrite));

            var box = Container("cardboard_box", 15, overwrite);

            // ---- Collectibles (GDD 9): one costumed Chubby per venue + rare items ----
            var chubbyBlue = new Color(0.30f, 0.50f, 0.95f);
            var mascotSize = new Vector3(0.30f, 0.30f, 0.28f);
            var captain = Collectible("captain_chubby", ItemRarity.Mascot, PlaceholderShape.Sphere, chubbyBlue, mascotSize, 150, overwrite, new Color(0.90f, 0.22f, 0.22f));
            var firstIssue = Collectible("first_issue", ItemRarity.Rare, PlaceholderShape.Book, new Color(0.98f, 0.78f, 0.35f), new Vector3(0.29f, 0.39f, 0.05f), 60, overwrite);
            var mechanic = Collectible("mechanic_chubby", ItemRarity.Mascot, PlaceholderShape.Sphere, chubbyBlue, mascotSize, 200, overwrite, new Color(1.00f, 0.55f, 0.10f));
            var robot = Collectible("golden_robot", ItemRarity.Rare, PlaceholderShape.Capsule, new Color(1.00f, 0.80f, 0.25f), new Vector3(0.18f, 0.36f, 0.18f), 60, overwrite);
            var luckyWrench = Collectible("lucky_wrench", ItemRarity.Rare, PlaceholderShape.Rod, new Color(1.00f, 0.84f, 0.30f), new Vector3(0.09f, 0.40f, 0.09f), 60, overwrite);
            var guard = Collectible("night_guard_chubby", ItemRarity.Mascot, PlaceholderShape.Sphere, chubbyBlue, mascotSize, 300, overwrite, new Color(0.15f, 0.20f, 0.42f));
            var stapler = Collectible("brass_stapler", ItemRarity.Rare, PlaceholderShape.Cube, new Color(0.95f, 0.75f, 0.30f), new Vector3(0.26f, 0.12f, 0.10f), 90, overwrite);
            var hubcap = Collectible("chrome_hubcap", ItemRarity.Rare, PlaceholderShape.Cylinder, new Color(1.00f, 0.88f, 0.45f), new Vector3(0.34f, 0.06f, 0.34f), 90, overwrite);
            var bottle = Collectible("message_bottle", ItemRarity.Rare, PlaceholderShape.Cylinder, new Color(1.00f, 0.85f, 0.40f), new Vector3(0.13f, 0.38f, 0.13f), 90, overwrite);
            items.AddRange(new ItemDefinition[] { captain, firstIssue, mechanic, robot, luckyWrench, guard, stapler, hubcap, bottle });

            // ---- Sections (M4.2 "big" sizes: ~60 / ~200 / ~300 per Warehouse room; more categories further up the ladder).
            // Floor width follows the bookcases; wide rooms pan sideways. GDD 5.3 numbers are [VARSAYILAN].
            var comicBoxSection = Section("comic_box", 5.6f, overwrite, 0.35f, 0.1f,
                new[] { (comics, 40), (toys, 20) }, 1, new[] { captain, firstIssue });
            var garageSection = Section("garage", 6.4f, overwrite, 0.55f, 0.15f,
                new[] { (comics, 50), (toys, 50), (tools, 50), (tyres, 50) }, 1, new[] { mechanic, robot, luckyWrench });
            var office = Section("wh_office", 6.4f, overwrite, 0.5f, 0.15f,
                new[] { (stationery, 60), (mugs, 60), (comics, 60), (bottles, 60), (tools, 60) }, 2, new[] { stapler });
            var dock = Section("wh_dock", 6.4f, overwrite, 0.6f, 0.15f,
                new[] { (tools, 60), (tyres, 60), (bottles, 60), (toys, 60), (mugs, 60) }, 3, new[] { hubcap });
            var aisle = Section("wh_aisle", 6.4f, overwrite, 0.5f, 0.15f,
                new[] { (toys, 50), (comics, 50), (stationery, 50), (mugs, 50), (tyres, 50), (bottles, 50) }, 4, new[] { guard });
            var basement = Section("wh_basement", 6.4f, overwrite, 0.7f, 0.2f,
                new[] { (bottles, 50), (stationery, 50), (tools, 50), (tyres, 50), (comics, 50), (toys, 50) }, 5, new[] { bottle });
            if (overwrite || basement.UnlockCoinCost == 0)
            {
                // GDD 5.4: opens when the other rooms average 60%, or right away for coins.
                basement.StartsLocked = true;
                basement.UnlockCoinCost = 150;
                basement.UnlockAtVenuePercent = 60;
                EditorUtility.SetDirty(basement);
            }

            // ---- Venues: GDD 5.3 ladder. First is free; each sale pays for the next. ----
            var comicBox = Venue("comic_box", 0, 150, overwrite, new[] { comicBoxSection }, new[] { captain, firstIssue });
            var garage = Venue("garage", 100, 350, overwrite, new[] { garageSection }, new[] { mechanic, robot, luckyWrench });
            var warehouse = Venue("warehouse", 300, 1500, overwrite, new[] { office, dock, aisle, basement }, new[] { guard, stapler, hubcap, bottle });

            database.Categories = new List<CategoryDefinition> { comics, toys, tools, stationery, mugs, tyres, bottles };
            database.Items = items;
            database.Containers = new List<ContainerDefinition> { box };

            // ---- Tools (GDD 10.1). Level 1 cost = unlock price. Values: see ToolDefinition. ----
            database.Tools = new List<ToolDefinition>
            {
                // Hand: carry capacity / pick-up radius. Magnet: pull radius / extra items pulled.
                Tool("hand", ToolType.Hand, overwrite, (0, 1f, 0.10f), (60, 2f, 0.11f), (180, 3f, 0.12f)),
                Tool("broom", ToolType.Broom, overwrite, (0, 0.38f, 0f), (50, 0.5f, 0f), (150, 0.65f, 0f)),
                Tool("magnet", ToolType.Magnet, overwrite, (40, 0.35f, 2f), (120, 0.45f, 4f), (300, 0.55f, 6f)),
            };
            database.Venues = new List<VenueDefinition> { comicBox, garage, warehouse };
            EditorUtility.SetDirty(database);

            CreatePanelSettings();
            AssetDatabase.SaveAssets();
            Debug.Log($"[ContentBuilder] Sample content ready ({(overwrite ? "rebuilt" : "missing assets created")}).");
        }

        // ---------- Assets ----------

        static CategoryDefinition Category(string id, int coins, Vector3 slotSize, PlaceSoundType sound, bool overwrite)
        {
            var category = Load<CategoryDefinition>($"{ContentFolder}/Category_{id}.asset", out var isNew);
            if (!isNew && !overwrite) return category;
            category.Id = id;
            category.DisplayNameKey = $"category.{id}";
            category.BaseCoinValue = coins;
            category.SlotSize = slotSize;
            category.PlaceSound = sound;
            category.MasteryThreshold = 50; // prototype value; rooms hold 40-60 per category (M4.2)
            EditorUtility.SetDirty(category);
            return category;
        }

        static ItemDefinition Item(string id, CategoryDefinition category, PlaceholderShape shape, Color color, Vector3 size, bool overwrite)
        {
            var item = Load<ItemDefinition>($"{ContentFolder}/Item_{id}.asset", out var isNew);
            if (!isNew && !overwrite) return item;
            item.Id = id;
            item.DisplayNameKey = $"item.{id}";
            item.Category = category;
            item.Rarity = ItemRarity.Common;
            item.Placeholder = new PlaceholderVisual(shape, color, size);
            EditorUtility.SetDirty(item);
            return item;
        }

        static CollectibleDefinition Collectible(string id, ItemRarity rarity, PlaceholderShape shape, Color color, Vector3 size, int duplicateValue, bool overwrite, Color? costume = null)
        {
            var collectible = Load<CollectibleDefinition>($"{ContentFolder}/Collectible_{id}.asset", out var isNew);
            if (!isNew && !overwrite) return collectible;
            collectible.Id = id;
            collectible.DisplayNameKey = $"collectible.{id}";
            collectible.DescriptionKey = $"collectible.{id}.desc";
            collectible.Rarity = rarity;
            collectible.Category = null;
            collectible.DuplicateSellValue = duplicateValue;
            collectible.Placeholder = new PlaceholderVisual(shape, color, size);
            collectible.CostumeColor = costume ?? new Color(0.90f, 0.22f, 0.22f);
            EditorUtility.SetDirty(collectible);
            return collectible;
        }

        static ToolDefinition Tool(string id, ToolType type, bool overwrite, params (int cost, float primary, float secondary)[] levels)
        {
            var tool = Load<ToolDefinition>($"{ContentFolder}/Tool_{id}.asset", out var isNew);
            if (!isNew && !overwrite) return tool;
            tool.Id = id;
            tool.Type = type;
            tool.DisplayNameKey = $"tool.{id}";
            tool.EffectKey = $"tool.{id}.effect";
            tool.Levels = new List<ToolDefinition.Level>();
            foreach (var (cost, primary, secondary) in levels)
                tool.Levels.Add(new ToolDefinition.Level { Cost = cost, Primary = primary, Secondary = secondary });
            EditorUtility.SetDirty(tool);
            return tool;
        }

        const float LooseShare = 0.25f;

        static SectionDefinition Section(string id, float depth, bool overwrite, float dirt, float buried,
            (CategoryDefinition category, int slots)[] shelves, int seed, CollectibleDefinition[] collectibles)
        {
            var path = id == "garage" ? StartSectionPath : $"{ContentFolder}/Section_{id}.asset";
            var section = Load<SectionDefinition>(path, out var isNew);
            if (!isNew && !overwrite) return section;
            section.Id = id;
            section.DisplayNameKey = $"section.{id}";
            section.Shelves = new List<SectionDefinition.ShelfEntry>();
            var shelfWidth = 0f;
            var total = 0;
            foreach (var (category, slots) in shelves)
            {
                section.Shelves.Add(new SectionDefinition.ShelfEntry { Category = category, SlotCount = slots });
                shelfWidth += SortingGame.Section.ShelfView.MeasureSize(category, slots, out _, out _).x + 0.12f;
                total += slots;
            }
            // Floor: bookcases side by side plus a margin; at least one screen wide.
            section.FloorSize = new Vector2(Mathf.Max(4.3f, shelfWidth + 0.6f), depth);

            // Enough boxes for everything that is neither loose nor buried.
            var box = AssetDatabase.LoadAssetAtPath<ContainerDefinition>($"{ContentFolder}/Container_cardboard_box.asset");
            var inBoxes = total * (1f - LooseShare - buried);
            var boxes = Mathf.Max(1, Mathf.CeilToInt(inBoxes / Mathf.Max(1, box.Capacity)));
            section.Containers = new List<SectionDefinition.ContainerEntry> { new() { Container = box, Count = boxes } };
            section.LooseItemRatio = LooseShare;
            section.DirtCoverage = dirt;
            section.BuriedItemRatio = buried;
            section.Collectibles = new List<CollectibleDefinition>(collectibles);
            section.Seed = seed;
            section.StartsLocked = false;
            section.UnlockCoinCost = 0;
            section.UnlockAtVenuePercent = 0;
            EditorUtility.SetDirty(section);
            return section;
        }

        static VenueDefinition Venue(string id, int price, int sellValue, bool overwrite, SectionDefinition[] sections, CollectibleDefinition[] page)
        {
            var venue = Load<VenueDefinition>($"{ContentFolder}/Venue_{Capitalise(id)}.asset", out var isNew);
            if (!isNew && !overwrite) return venue;
            venue.Id = id;
            venue.DisplayNameKey = $"venue.{id}";
            venue.ThemeId = id;
            venue.PurchasePrice = price;
            venue.SellValue = sellValue;
            venue.Sections = new List<SectionDefinition>(sections);
            venue.CollectionPage = new List<CollectibleDefinition>(page);
            EditorUtility.SetDirty(venue);
            return venue;
        }

        static string Capitalise(string id)
        {
            var parts = id.Split('_');
            for (var i = 0; i < parts.Length; i++) parts[i] = char.ToUpperInvariant(parts[i][0]) + parts[i].Substring(1);
            return string.Join("", parts);
        }

        static ContainerDefinition Container(string id, int capacity, bool overwrite)
        {
            var container = Load<ContainerDefinition>($"{ContentFolder}/Container_{id}.asset", out var isNew);
            if (!isNew && !overwrite) return container;
            container.Id = id;
            container.DisplayNameKey = $"container.{id}";
            container.Capacity = capacity;
            container.Placeholder = new PlaceholderVisual(PlaceholderShape.Cube, new Color(0.74f, 0.56f, 0.36f), new Vector3(0.75f, 0.55f, 0.62f));
            EditorUtility.SetDirty(container);
            return container;
        }

        static Material LitMaterial()
        {
            var path = MaterialsFolder + "/Placeholder_Lit.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null) return material;
            material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            material.SetFloat("_Smoothness", 0.25f);
            material.enableInstancing = true;
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        static Material GhostMaterial()
        {
            var path = MaterialsFolder + "/Placeholder_Ghost.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null) return material;
            material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            // URP transparent setup.
            material.SetFloat("_Surface", 1f);
            material.SetFloat("_Blend", 0f);
            material.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
            material.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
            material.SetFloat("_SrcBlendAlpha", (float)BlendMode.One);
            material.SetFloat("_DstBlendAlpha", (float)BlendMode.OneMinusSrcAlpha);
            material.SetFloat("_ZWrite", 0f);
            material.SetOverrideTag("RenderType", "Transparent");
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.renderQueue = (int)RenderQueue.Transparent;
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        static Material DirtMaterial()
        {
            var path = MaterialsFolder + "/Placeholder_Dirt.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null) return material;
            material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            MakeTransparent(material);
            material.SetFloat("_Smoothness", 0f);
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        static Material ParticleMaterial()
        {
            var path = MaterialsFolder + "/Placeholder_Particle.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null) return material;
            material = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
            MakeTransparent(material);
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        /// <summary>URP alpha-blended transparency, same property set for Lit, Unlit and Particles shaders.</summary>
        static void MakeTransparent(Material material)
        {
            material.SetFloat("_Surface", 1f);
            material.SetFloat("_Blend", 0f);
            material.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
            material.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
            material.SetFloat("_SrcBlendAlpha", (float)BlendMode.One);
            material.SetFloat("_DstBlendAlpha", (float)BlendMode.OneMinusSrcAlpha);
            material.SetFloat("_ZWrite", 0f);
            material.SetOverrideTag("RenderType", "Transparent");
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.renderQueue = (int)RenderQueue.Transparent;
        }

        static void CreatePanelSettings()
        {
            var themePath = UiFolder + "/RuntimeTheme.tss";
            AssetDatabase.ImportAsset(themePath);
            var theme = AssetDatabase.LoadAssetAtPath<ThemeStyleSheet>(themePath);

            var settings = AssetDatabase.LoadAssetAtPath<PanelSettings>(PanelSettingsPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<PanelSettings>();
                AssetDatabase.CreateAsset(settings, PanelSettingsPath);
            }
            settings.themeStyleSheet = theme;
            settings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
            settings.referenceResolution = new Vector2Int(1080, 1920);
            settings.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
            settings.match = 0f; // match width: portrait layout stays consistent across tall phones
            EditorUtility.SetDirty(settings);
        }

        static T Load<T>(string path, out bool isNew) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            isNew = asset == null;
            return isNew ? ProjectSetup.LoadOrCreate<T>(path) : asset;
        }

        static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            var parent = System.IO.Path.GetDirectoryName(path)!.Replace('\\', '/');
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, System.IO.Path.GetFileName(path));
        }
    }
}
