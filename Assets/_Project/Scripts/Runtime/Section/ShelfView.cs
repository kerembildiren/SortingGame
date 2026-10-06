using System.Collections.Generic;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Section
{
    /// <summary>One place on a shelf. Reserved while an item flies to it, occupied once it lands.</summary>
    public class ShelfSlot
    {
        public readonly ShelfView Shelf;
        public readonly Vector3 LocalBase; // centre of the slot floor, in shelf space
        public readonly MeshRenderer Ghost;
        public ItemView Occupant { get; private set; }
        public bool IsReserved { get; private set; }

        public ShelfSlot(ShelfView shelf, Vector3 localBase, MeshRenderer ghost)
        {
            Shelf = shelf;
            LocalBase = localBase;
            Ghost = ghost;
        }

        public bool IsFree => Occupant == null && !IsReserved;
        public Vector3 WorldBase => Shelf.transform.TransformPoint(LocalBase);

        public void Reserve() => IsReserved = true;

        public void Fill(ItemView item)
        {
            IsReserved = false;
            Occupant = item;
            if (Ghost != null) Ghost.enabled = false;
        }
    }

    /// <summary>
    /// A bookcase for one category (GDD 6.2): rows of slots with dashed outlines and a label sign on top.
    /// Local origin is the floor centre of the shelf, front faces -Z (towards the camera).
    /// </summary>
    public class ShelfView : MonoBehaviour
    {
        const float Board = 0.04f;
        const float Plinth = 0.08f;
        const float CellPadding = 0.06f;

        public CategoryDefinition Category { get; private set; }
        public readonly List<ShelfSlot> Slots = new();
        public Vector3 Size { get; private set; }

        Transform _sign;
        Material _signMaterial;
        Color _signColor;
        Vector3 _signScale;
        bool _hovered;
        Coroutine _hintRoutine;

        public int FilledCount
        {
            get
            {
                var count = 0;
                foreach (var slot in Slots) if (slot.Occupant != null) count++;
                return count;
            }
        }

        public bool IsFull => FilledCount == Slots.Count;
        public bool HasFreeSlot => Slots.Exists(s => s.IsFree);

        /// <summary>World point just above the sign, where the HUD draws the category label.</summary>
        public Vector3 LabelAnchor => _sign.position;

        public static Vector3 MeasureSize(CategoryDefinition category, int slotCount, out int rows, out int columns)
        {
            // Portrait screen: prefer tall, narrow shelves (12 slots -> 4 rows x 3 columns); big shelves get 5 rows.
            rows = Mathf.Clamp(Mathf.CeilToInt(Mathf.Sqrt(slotCount * 1.3f)), 1, slotCount > 24 ? 5 : 4);
            columns = Mathf.CeilToInt(slotCount / (float)rows);
            var cellWidth = category.SlotSize.x + CellPadding;
            var cellHeight = category.SlotSize.y + CellPadding;
            var depth = Mathf.Max(category.SlotSize.z + 0.1f, 0.34f);
            var width = columns * cellWidth + 2f * Board;
            var height = Plinth + rows * (cellHeight + Board) + Board;
            return new Vector3(width, height, depth);
        }

        public void Build(CategoryDefinition category, int slotCount, PlaceholderFactory factory, SectionVisuals visuals)
        {
            Category = category;
            Size = MeasureSize(category, slotCount, out var rows, out var columns);

            var cellWidth = category.SlotSize.x + CellPadding;
            var cellHeight = category.SlotSize.y + CellPadding;
            var innerWidth = columns * cellWidth;
            var width = Size.x;
            var height = Size.y;
            var depth = Size.z;
            var wood = visuals.WoodColor;
            var darkWood = wood * 0.75f;
            darkWood.a = 1f;

            // Frame
            factory.CreateBox("Back", transform, new Vector3(0f, height / 2f, depth / 2f - Board / 2f), new Vector3(width, height, Board), darkWood);
            factory.CreateBox("SideL", transform, new Vector3(-width / 2f + Board / 2f, height / 2f, 0f), new Vector3(Board, height, depth), wood);
            factory.CreateBox("SideR", transform, new Vector3(width / 2f - Board / 2f, height / 2f, 0f), new Vector3(Board, height, depth), wood);
            factory.CreateBox("Plinth", transform, new Vector3(0f, Plinth / 2f, 0f), new Vector3(width, Plinth, depth), darkWood);
            for (var r = 0; r <= rows; r++)
            {
                var y = Plinth + r * (cellHeight + Board) + Board / 2f;
                factory.CreateBox($"Board{r}", transform, new Vector3(0f, y, 0f), new Vector3(width, Board, depth), wood);
            }

            // Slots, filled bottom-left first in index order (order only matters for tests).
            var slotZ = depth / 2f - Board - category.SlotSize.z / 2f - 0.01f;
            var ghostZ = depth / 2f - Board - 0.004f;
            for (var i = 0; i < slotCount; i++)
            {
                var r = rows - 1 - i / columns; // top row first so a partly filled shelf reads well
                var c = i % columns;
                var x = -innerWidth / 2f + cellWidth * (c + 0.5f);
                var floorY = Plinth + r * (cellHeight + Board) + Board;
                var ghost = factory.CreateSlotGhost(transform, new Vector3(x, floorY + category.SlotSize.y / 2f, ghostZ),
                    new Vector2(category.SlotSize.x, category.SlotSize.y), visuals.SlotGhostColor);
                Slots.Add(new ShelfSlot(this, new Vector3(x, floorY, slotZ), ghost));
            }

            // Label sign on top (text is drawn by the HUD).
            _signColor = visuals.SignColor;
            _signMaterial = factory.UniqueLit(_signColor);
            var sign = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sign.name = "Sign";
            DestroyImmediate(sign.GetComponent<Collider>());
            sign.transform.SetParent(transform, false);
            sign.transform.localPosition = new Vector3(0f, height + 0.11f, -depth * 0.1f);
            _signScale = new Vector3(Mathf.Min(width * 0.75f, 1.0f), 0.2f, 0.05f);
            sign.transform.localScale = _signScale;
            sign.GetComponent<MeshRenderer>().sharedMaterial = _signMaterial;
            _sign = sign.transform;

            // Generous invisible drop zone so the player does not have to hit the exact frame.
            var zone = gameObject.AddComponent<BoxCollider>();
            zone.isTrigger = true;
            zone.center = new Vector3(0f, (height + 0.35f) / 2f, -0.1f);
            zone.size = new Vector3(width + 0.12f, height + 0.35f, depth + 0.4f);
        }

        public ShelfSlot NearestFreeSlot(Vector3 worldPoint)
        {
            ShelfSlot best = null;
            var bestDistance = float.MaxValue;
            foreach (var slot in Slots)
            {
                if (!slot.IsFree) continue;
                var distance = (slot.WorldBase - worldPoint).sqrMagnitude;
                if (distance >= bestDistance) continue;
                bestDistance = distance;
                best = slot;
            }
            return best;
        }

        /// <summary>Neutral highlight while an item is dragged over (does not reveal the right shelf).</summary>
        public void SetHovered(bool hovered)
        {
            if (_hovered == hovered) return;
            _hovered = hovered;
            if (_hintRoutine != null) return;
            _sign.localScale = hovered ? _signScale * 1.12f : _signScale;
            _signMaterial.SetColor("_BaseColor", hovered ? Color.Lerp(_signColor, Color.white, 0.6f) : _signColor);
        }

        /// <summary>GDD 7.2: after a wrong drop, the right shelf glows briefly.</summary>
        public void FlashHint(float duration)
        {
            Tween.Stop(_hintRoutine);
            var hint = new Color(1f, 0.82f, 0.25f);
            _hintRoutine = Tween.Run(this, duration, t =>
            {
                var pulse = Ease.Pulse(Mathf.Repeat(t * 2f, 1f));
                _signMaterial.SetColor("_BaseColor", Color.Lerp(_signColor, hint, pulse));
                _sign.localScale = _signScale * (1f + 0.15f * pulse);
            }, null, () =>
            {
                _hintRoutine = null;
                _signMaterial.SetColor("_BaseColor", _signColor);
                _sign.localScale = _signScale;
            });
        }

        public void Celebrate()
        {
            Tween.Run(this, 0.45f, t =>
            {
                _sign.localScale = _signScale * (1f + 0.3f * Ease.Pulse(t));
                _signMaterial.SetColor("_BaseColor", Color.Lerp(_signColor, new Color(0.6f, 1f, 0.6f), Ease.Pulse(t)));
            });
        }
    }
}
