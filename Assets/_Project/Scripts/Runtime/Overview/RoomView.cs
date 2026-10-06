using System.Linq;
using SortingGame.Core;
using SortingGame.Data;
using SortingGame.Section;
using UnityEngine;

namespace SortingGame.Overview
{
    /// <summary>
    /// GDD 6.1: one section seen from the isometric overview. Not the real items (GDD 15.3): a stand-in whose
    /// clutter and colours follow the section's progress. Back and left walls are tall, front and right are cut away.
    /// </summary>
    public class RoomView : MonoBehaviour
    {
        const float WallHeight = 1.6f;
        const float Wall = 0.12f;

        public SectionDefinition Section { get; private set; }
        public VenueProgress.SectionStatus Status { get; private set; }
        public Vector3 Size { get; private set; }

        /// <summary>World point above the room where the HUD draws its name + %.</summary>
        public Vector3 LabelAnchor => transform.TransformPoint(new Vector3(0f, WallHeight + 0.4f, 0f));
        public Vector3 Centre => transform.position;

        /// <summary>Size drawn on the overview: long sections are shortened so the building stays readable (GDD 15.3).</summary>
        public static Vector2 DrawnSize(SectionDefinition section) =>
            new(Mathf.Min(section.FloorSize.x, 7f), Mathf.Min(section.FloorSize.y, 6.4f));

        public void Build(SectionDefinition section, VenueProgress.SectionStatus status, GameContext ctx, PlaceholderFactory factory)
        {
            Section = section;
            Status = status;
            var visuals = ctx.Visuals;
            var drawn = DrawnSize(section);
            var w = drawn.x;
            var d = drawn.y;
            Size = new Vector3(w, WallHeight, d);

            var clean = status.Completed ? 1f : status.Fraction;
            Color Tint(Color dirty, Color tidy) => status.Unlocked ? Color.Lerp(dirty, tidy, clean) : dirty * 0.35f;
            var floor = Tint(visuals.FloorColor, visuals.FloorColorClean);
            var wall = Tint(visuals.WallColor, visuals.WallColorClean);
            floor.a = wall.a = 1f;

            factory.CreateBox("Floor", transform, new Vector3(0f, -0.05f, 0f), new Vector3(w, 0.1f, d), floor, false);
            factory.CreateBox("WallBack", transform, new Vector3(0f, WallHeight / 2f, d / 2f - Wall / 2f), new Vector3(w, WallHeight, Wall), wall, false);
            factory.CreateBox("WallLeft", transform, new Vector3(-w / 2f + Wall / 2f, WallHeight / 2f, 0f), new Vector3(Wall, WallHeight, d), wall, false);
            factory.CreateBox("WallFront", transform, new Vector3(0f, 0.15f, -d / 2f + Wall / 2f), new Vector3(w, 0.3f, Wall), wall, false);
            factory.CreateBox("WallRight", transform, new Vector3(w / 2f - Wall / 2f, 0.15f, 0f), new Vector3(Wall, 0.3f, d), wall, false);

            var random = new System.Random(section.Id.GetHashCode());
            BuildShelves(section, w, d, ctx, factory, clean, status.Unlocked, random);
            if (status.Unlocked) BuildClutter(w, d, clean, visuals, factory, random);
            else BuildLock(factory, visuals);
            if (status.Completed) BuildPlant(factory, w, d);

            var collider = gameObject.AddComponent<BoxCollider>();
            collider.center = new Vector3(0f, WallHeight / 2f, 0f);
            collider.size = new Vector3(w, WallHeight, d);
        }

        /// <summary>Shelves along the back wall, filled with coloured blocks in proportion to progress.</summary>
        void BuildShelves(SectionDefinition section, float w, float d, GameContext ctx, PlaceholderFactory factory, float clean, bool unlocked, System.Random random)
        {
            var count = section.Shelves.Count;
            if (count == 0) return;
            var usable = w - 0.6f;
            var width = usable / count - 0.1f;
            var wood = unlocked ? ctx.Visuals.WoodColor : ctx.Visuals.WoodColor * 0.35f;
            wood.a = 1f;
            for (var i = 0; i < count; i++)
            {
                var x = -usable / 2f + (i + 0.5f) * (usable / count);
                var z = d / 2f - 0.35f;
                factory.CreateBox($"Shelf{i}", transform, new Vector3(x, 0.6f, z), new Vector3(width, 1.2f, 0.4f), wood, false);
                if (!unlocked) continue;

                var category = section.Shelves[i].Category;
                var colour = ctx.Database.CommonItemsOf(category).Select(item => item.Placeholder.Color).FirstOrDefault();
                const int perRow = 4;
                var filled = Mathf.RoundToInt(clean * perRow * 3);
                for (var k = 0; k < filled; k++)
                {
                    var bx = x - width / 2f + (k % perRow + 0.5f) * (width / perRow);
                    var by = 0.25f + (k / perRow) * 0.35f;
                    var block = colour * (0.85f + (float)random.NextDouble() * 0.3f);
                    block.a = 1f;
                    factory.CreateBox("Item", transform, new Vector3(bx, by, z - 0.22f), new Vector3(width / perRow * 0.7f, 0.22f, 0.08f), block, false);
                }
            }
        }

        /// <summary>Boxes and debris: lots when dirty, none when done (GDD 12.1 kaos -> düzen).</summary>
        void BuildClutter(float w, float d, float clean, SectionVisuals visuals, PlaceholderFactory factory, System.Random random)
        {
            var boxes = Mathf.RoundToInt((1f - clean) * 7f);
            var debris = Mathf.RoundToInt((1f - clean) * 18f);
            float R(float min, float max) => (float)(min + random.NextDouble() * (max - min));
            for (var i = 0; i < boxes; i++)
            {
                var size = R(0.35f, 0.6f);
                var box = factory.CreateBox("Box", transform, new Vector3(R(-w / 2f + 0.5f, w / 2f - 0.5f), size / 2f, R(-d / 2f + 0.5f, d / 2f - 1.0f)),
                    new Vector3(size, size * 0.8f, size), visuals.CardboardColor * R(0.8f, 1.05f), false);
                box.transform.localRotation = Quaternion.Euler(0f, R(0f, 90f), 0f);
            }
            Color[] colours = { new(0.86f, 0.24f, 0.22f), new(0.22f, 0.45f, 0.86f), new(0.98f, 0.8f, 0.2f), new(0.6f, 0.62f, 0.66f), new(0.4f, 0.85f, 0.35f) };
            for (var i = 0; i < debris; i++)
            {
                factory.CreateBox("Debris", transform, new Vector3(R(-w / 2f + 0.3f, w / 2f - 0.3f), 0.05f, R(-d / 2f + 0.3f, d / 2f - 0.8f)),
                    new Vector3(R(0.12f, 0.25f), 0.08f, R(0.12f, 0.25f)), colours[random.Next(colours.Length)], false)
                    .transform.localRotation = Quaternion.Euler(0f, R(0f, 180f), 0f);
            }
        }

        void BuildLock(PlaceholderFactory factory, SectionVisuals visuals)
        {
            var gold = new Color(0.95f, 0.75f, 0.25f);
            var body = factory.CreateBox("LockBody", transform, new Vector3(0f, 1.0f, 0f), new Vector3(0.6f, 0.5f, 0.2f), gold, false);
            var shackle = factory.CreateShape(new PlaceholderVisual(PlaceholderShape.Cylinder, gold * 0.8f, new Vector3(0.42f, 0.08f, 0.42f)), transform, false);
            shackle.transform.localPosition = new Vector3(0f, 1.35f, 0f);
            shackle.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            body.name = "Lock";
        }

        void BuildPlant(PlaceholderFactory factory, float w, float d)
        {
            var pot = factory.CreateShape(new PlaceholderVisual(PlaceholderShape.Cylinder, new Color(0.75f, 0.4f, 0.25f), new Vector3(0.3f, 0.3f, 0.3f)), transform, false);
            pot.transform.localPosition = new Vector3(-w / 2f + 0.5f, 0.15f, -d / 2f + 0.5f);
            var leaves = factory.CreateShape(new PlaceholderVisual(PlaceholderShape.Sphere, new Color(0.35f, 0.7f, 0.35f), new Vector3(0.5f, 0.5f, 0.5f)), transform, false);
            leaves.transform.localPosition = new Vector3(-w / 2f + 0.5f, 0.55f, -d / 2f + 0.5f);
        }
    }
}
