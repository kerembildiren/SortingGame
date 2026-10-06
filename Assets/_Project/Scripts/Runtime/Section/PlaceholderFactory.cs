using System.Collections.Generic;
using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Section
{
    /// <summary>Builds primitive stand-ins for items, props and room parts. One shared material per colour.</summary>
    public class PlaceholderFactory
    {
        readonly Material _lit;
        readonly Material _ghost;
        readonly Dictionary<Color, Material> _litCache = new();
        Texture2D _dashedTexture;

        public PlaceholderFactory(SectionVisuals visuals)
        {
            _lit = visuals.LitMaterial;
            _ghost = visuals.GhostMaterial;
        }

        public Material LitFor(Color color)
        {
            if (_litCache.TryGetValue(color, out var material)) return material;
            material = new Material(_lit) { name = $"Lit_{ColorUtility.ToHtmlStringRGB(color)}" };
            material.SetColor("_BaseColor", color);
            _litCache[color] = material;
            return material;
        }

        /// <summary>Own material instance, for things that change colour at runtime (highlights).</summary>
        public Material UniqueLit(Color color)
        {
            var material = new Material(_lit);
            material.SetColor("_BaseColor", color);
            return material;
        }

        /// <summary>
        /// Item-shaped visual as a child of <paramref name="parent"/>, centred on it.
        /// Size is the upright size: x = width, y = height, z = depth (thickness for books).
        /// </summary>
        public GameObject CreateShape(PlaceholderVisual visual, Transform parent, bool withCollider = true)
        {
            var type = visual.Shape switch
            {
                PlaceholderShape.Sphere => PrimitiveType.Sphere,
                PlaceholderShape.Capsule => PrimitiveType.Capsule,
                PlaceholderShape.Cylinder or PlaceholderShape.Rod => PrimitiveType.Cylinder,
                _ => PrimitiveType.Cube
            };

            var go = GameObject.CreatePrimitive(type);
            go.name = "Visual";
            go.transform.SetParent(parent, false);

            var size = visual.Size;
            go.transform.localScale = type is PrimitiveType.Capsule or PrimitiveType.Cylinder
                ? new Vector3(size.x, size.y * 0.5f, size.z) // these primitives are 2 units tall
                : size;

            go.GetComponent<MeshRenderer>().sharedMaterial = LitFor(visual.Color);
            if (!withCollider) Object.DestroyImmediate(go.GetComponent<Collider>());
            return go;
        }

        public GameObject CreateBox(string name, Transform parent, Vector3 localCenter, Vector3 size, Color color, bool withCollider = true)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localCenter;
            go.transform.localScale = size;
            go.GetComponent<MeshRenderer>().sharedMaterial = LitFor(color);
            if (!withCollider) Object.DestroyImmediate(go.GetComponent<Collider>());
            return go;
        }

        /// <summary>The look of any item: real prefab if set, else mascot figure or primitive. Centred on the parent.</summary>
        public void CreateItemVisual(ItemDefinition definition, Transform parent, bool withCollider = true)
        {
            if (definition.Prefab != null)
            {
                var instance = Object.Instantiate(definition.Prefab, parent, false);
                if (!withCollider)
                    foreach (var c in instance.GetComponentsInChildren<Collider>()) Object.DestroyImmediate(c);
            }
            else if (definition.Rarity == ItemRarity.Mascot && definition is CollectibleDefinition mascot)
            {
                var body = CreateMascot(definition.Placeholder, mascot.CostumeColor, parent);
                if (!withCollider) Object.DestroyImmediate(body.GetComponent<Collider>());
            }
            else
            {
                CreateShape(definition.Placeholder, parent, withCollider);
            }
        }

        /// <summary>
        /// Placeholder Chubby (GDD 9.2): round body, big eyes, costume mask + cape in <paramref name="costume"/> colour.
        /// Only the body has a collider.
        /// </summary>
        public GameObject CreateMascot(PlaceholderVisual visual, Color costume, Transform parent)
        {
            var size = visual.Size;
            var body = CreateShape(new PlaceholderVisual(PlaceholderShape.Sphere, visual.Color, size), parent);
            body.name = "Body";

            var white = new Color(0.98f, 0.98f, 0.98f);
            var black = new Color(0.08f, 0.08f, 0.1f);
            var eye = size.x * 0.26f;
            for (var side = -1; side <= 1; side += 2)
            {
                var eyeBall = CreateBox("Eye", parent, Vector3.zero, Vector3.one, white, false);
                ReplaceMesh(eyeBall, PrimitiveType.Sphere);
                eyeBall.transform.localPosition = new Vector3(side * size.x * 0.17f, size.y * 0.12f, -size.z * 0.42f);
                eyeBall.transform.localScale = new Vector3(eye, eye * 1.15f, eye * 0.5f);

                var pupil = CreateBox("Pupil", parent, Vector3.zero, Vector3.one, black, false);
                ReplaceMesh(pupil, PrimitiveType.Sphere);
                pupil.transform.localPosition = new Vector3(side * size.x * 0.17f, size.y * 0.12f, -size.z * 0.48f);
                pupil.transform.localScale = new Vector3(eye * 0.5f, eye * 0.6f, eye * 0.25f);
            }

            // Hero mask band across the eyes and a small cape behind.
            CreateBox("Mask", parent, new Vector3(0f, size.y * 0.12f, -size.z * 0.36f), new Vector3(size.x * 0.75f, size.y * 0.16f, size.z * 0.12f), costume, false);
            var cape = CreateBox("Cape", parent, new Vector3(0f, -size.y * 0.05f, size.z * 0.45f), new Vector3(size.x * 0.8f, size.y * 0.75f, size.z * 0.05f), costume, false);
            cape.transform.localRotation = Quaternion.Euler(-12f, 0f, 0f);
            return body;
        }

        static void ReplaceMesh(GameObject target, PrimitiveType type)
        {
            var temp = GameObject.CreatePrimitive(type);
            target.GetComponent<MeshFilter>().sharedMesh = temp.GetComponent<MeshFilter>().sharedMesh;
            Object.DestroyImmediate(temp);
        }

        /// <summary>Camera-facing soft sprite on a quad (halos, rays, dimmer, broom cursor).</summary>
        public MeshRenderer CreateSoftQuad(string name, Transform parent, Color color, Texture texture)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.name = name;
            Object.DestroyImmediate(go.GetComponent<Collider>());
            go.transform.SetParent(parent, false);
            var renderer = go.GetComponent<MeshRenderer>();
            var material = new Material(_ghost);
            if (texture != null) material.SetTexture("_BaseMap", texture);
            material.SetColor("_BaseColor", color);
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            return renderer;
        }

        /// <summary>Dashed outline for an empty shelf slot (concept 02_section_garage).</summary>
        public MeshRenderer CreateSlotGhost(Transform parent, Vector3 localCenter, Vector2 size, Color color)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.name = "SlotGhost";
            Object.DestroyImmediate(go.GetComponent<Collider>());
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localCenter;
            go.transform.localScale = new Vector3(size.x, size.y, 1f);

            var renderer = go.GetComponent<MeshRenderer>();
            var material = new Material(_ghost);
            material.SetTexture("_BaseMap", DashedTexture);
            material.SetColor("_BaseColor", color);
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            return renderer;
        }

        Texture2D DashedTexture
        {
            get
            {
                if (_dashedTexture != null) return _dashedTexture;
                const int size = 128, border = 6, dash = 14, inset = 4;
                var texture = new Texture2D(size, size, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp, name = "DashedSlot" };
                var pixels = new Color32[size * size];
                for (var y = 0; y < size; y++)
                for (var x = 0; x < size; x++)
                {
                    var inBand = (x >= inset && x < inset + border) || (x < size - inset && x >= size - inset - border) ||
                                 (y >= inset && y < inset + border) || (y < size - inset && y >= size - inset - border);
                    var inside = x >= inset && x < size - inset && y >= inset && y < size - inset;
                    var horizontalEdge = y < inset + border || y >= size - inset - border;
                    var along = horizontalEdge ? x : y;
                    var on = inBand && inside && (along / dash) % 2 == 0;
                    var fill = inside ? (byte)28 : (byte)0; // faint fill so the slot reads as a slot
                    pixels[y * size + x] = on ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, fill);
                }
                texture.SetPixels32(pixels);
                texture.Apply();
                _dashedTexture = texture;
                return texture;
            }
        }

        /// <summary>
        /// How an item lies on the floor: flat for books, on its side for long things.
        /// Returns the rotation (before random yaw) and the height of its centre above the floor.
        /// </summary>
        public static (Quaternion rotation, float centreHeight) RestPose(PlaceholderVisual visual)
        {
            var s = visual.Size;
            switch (visual.Shape)
            {
                case PlaceholderShape.Book:
                    return (Quaternion.Euler(90f, 0f, 0f), s.z * 0.5f);
                case PlaceholderShape.Rod:
                case PlaceholderShape.Capsule:
                    return (Quaternion.Euler(0f, 0f, 90f), s.x * 0.5f);
                case PlaceholderShape.Cylinder:
                    return s.y > s.x * 1.4f ? (Quaternion.Euler(0f, 0f, 90f), s.x * 0.5f) : (Quaternion.identity, s.y * 0.5f);
                default:
                    return (Quaternion.identity, s.y * 0.5f);
            }
        }
    }
}
