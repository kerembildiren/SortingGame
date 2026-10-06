using System.Collections.Generic;
using SortingGame.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace SortingGame.EditorTools
{
    /// <summary>
    /// Creates the placeholder content used by M1 (garage test section: Comics / Toys / Tools).
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

            var box = Container("cardboard_box", 10, overwrite);

            // ---- Section + venue ----
            var section = Load<SectionDefinition>(StartSectionPath, out var isNew);
            if (isNew || overwrite)
            {
                section.Id = "garage";
                section.DisplayNameKey = "section.garage";
                section.FloorSize = new Vector2(4.3f, 6.4f);
                section.Shelves = new List<SectionDefinition.ShelfEntry>
                {
                    new() { Category = comics, SlotCount = 12 },
                    new() { Category = toys, SlotCount = 12 },
                    new() { Category = tools, SlotCount = 12 },
                };
                section.Containers = new List<SectionDefinition.ContainerEntry> { new() { Container = box, Count = 3 } };
                section.LooseItemRatio = 0.25f;
                section.DirtCoverage = 0f; // dirt arrives in M2
                section.Seed = 1;
                EditorUtility.SetDirty(section);
            }

            var venue = Load<VenueDefinition>(ContentFolder + "/Venue_Garage.asset", out isNew);
            if (isNew || overwrite)
            {
                venue.Id = "garage";
                venue.DisplayNameKey = "venue.garage";
                venue.ThemeId = "garage";
                venue.Sections = new List<SectionDefinition> { section };
                EditorUtility.SetDirty(venue);
            }

            database.Categories = new List<CategoryDefinition> { comics, toys, tools };
            database.Items = items;
            database.Containers = new List<ContainerDefinition> { box };
            database.Venues = new List<VenueDefinition> { venue };
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
            category.MasteryThreshold = 100;
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
