using System;
using System.Collections.Generic;
using SortingGame.Core;
using SortingGame.Data;
using SortingGame.Section;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SortingGame.Overview
{
    /// <summary>
    /// GDD 6.1 OverviewView: the whole venue as an isometric cutaway. Shows scale and progress; no sorting here.
    /// Tapping a room zooms the camera into it (GDD 6.1 zoom transition). Lives far away from the section world.
    /// </summary>
    public class OverviewController : MonoBehaviour
    {
        const float RoomGap = 0.3f;
        const float TapMaxMovePixels = 30f;
        public static readonly Vector3 WorldOffset = new(500f, 0f, 0f);

        public event Action<RoomView> RoomTapped;

        public IReadOnlyList<RoomView> Rooms => _rooms;
        public bool InputEnabled { get; set; }
        public Bounds BuildingBounds { get; private set; }

        readonly List<RoomView> _rooms = new();
        GameContext _ctx;
        PlaceholderFactory _factory;
        Camera _camera;
        FeelConfig _feel;
        Func<Vector2, bool> _isOverUi;
        Transform _root;
        Vector2 _pressPosition;
        bool _pressValid;
        bool _active;
        bool _zooming;
        Vector2Int _lastScreen;

        /// <summary>The overview camera is in charge (not the section view).</summary>
        public bool IsActive => _active;

        static readonly Quaternion ViewRotation = Quaternion.Euler(50f, -32f, 0f);

        public void Init(GameContext context, Camera cam, Func<Vector2, bool> isOverUi)
        {
            _ctx = context;
            _camera = cam;
            _feel = context.Feel;
            _isOverUi = isOverUi;
            _factory = new PlaceholderFactory(context.Visuals);
            transform.position = WorldOffset;
        }

        public void Build(VenueDefinition venue, SaveData data)
        {
            Clear();
            _root = new GameObject($"Overview_{venue.Id}").transform;
            _root.SetParent(transform, false);

            // Grid of rooms, two columns for multi-room venues, cell size = largest room.
            var columns = venue.Sections.Count > 1 ? 2 : 1;
            var cellW = 0f;
            var cellD = 0f;
            foreach (var s in venue.Sections)
            {
                cellW = Mathf.Max(cellW, s.FloorSize.x);
                cellD = Mathf.Max(cellD, s.FloorSize.y);
            }
            var rows = Mathf.CeilToInt(venue.Sections.Count / (float)columns);
            var totalW = columns * cellW + (columns - 1) * RoomGap;
            var totalD = rows * cellD + (rows - 1) * RoomGap;

            for (var i = 0; i < venue.Sections.Count; i++)
            {
                var section = venue.Sections[i];
                var column = i % columns;
                var row = i / columns;
                var go = new GameObject($"Room_{section.Id}");
                go.transform.SetParent(_root, false);
                go.transform.localPosition = new Vector3(-totalW / 2f + cellW * (column + 0.5f) + column * RoomGap, 0f,
                                                         totalD / 2f - cellD * (row + 0.5f) - row * RoomGap);
                var room = go.AddComponent<RoomView>();
                room.Build(section, VenueProgress.Section(section, data), _ctx, _factory);
                _rooms.Add(room);
            }

            // Ground plate around the building.
            _factory.CreateBox("Ground", _root, new Vector3(0f, -0.12f, 0f), new Vector3(totalW + 3f, 0.1f, totalD + 3f), new Color(0.36f, 0.48f, 0.3f), false);
            BuildingBounds = new Bounds(_root.position + Vector3.up * 0.8f, new Vector3(totalW, 1.6f, totalD));
        }

        public void Clear()
        {
            if (_root != null) Destroy(_root.gameObject);
            _rooms.Clear();
            _active = false;
        }

        public RoomView RoomOf(SectionDefinition section) => _rooms.Find(r => r.Section == section);

        // ---------- Camera ----------

        /// <summary>Overview lighting and camera framing for the whole building.</summary>
        public void ApplyView()
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = _ctx.Visuals.AmbientClean;
            if (RenderSettings.sun != null)
            {
                RenderSettings.sun.color = _ctx.Visuals.LightColor;
                RenderSettings.sun.intensity = 1.25f;
            }
            _camera.backgroundColor = new Color(0.55f, 0.72f, 0.85f);
            _active = true;
            Refit();
        }

        /// <summary>Frame the whole building for the current screen shape.</summary>
        public void Refit()
        {
            _lastScreen = new Vector2Int(Screen.width, Screen.height);
            _camera.orthographic = true;
            var (position, size) = FrameFor(BuildingBounds, 1.1f);
            _camera.transform.SetPositionAndRotation(position, ViewRotation);
            _camera.orthographicSize = size;
        }

        void LateUpdate()
        {
            // Rotating a tablet or resizing the editor's game view: keep the whole building in frame.
            if (!_active || _zooming) return;
            if (_lastScreen != new Vector2Int(Screen.width, Screen.height)) Refit();
        }

        /// <summary>Camera pose that fits <paramref name="bounds"/> between the HUD bars.</summary>
        (Vector3 position, float size) FrameFor(Bounds bounds, float padding)
        {
            var right = ViewRotation * Vector3.right;
            var up = ViewRotation * Vector3.up;
            var forward = ViewRotation * Vector3.forward;
            float minX = float.MaxValue, maxX = float.MinValue, minY = float.MaxValue, maxY = float.MinValue;
            var e = bounds.extents;
            for (var i = 0; i < 8; i++)
            {
                var corner = bounds.center + new Vector3((i & 1) == 0 ? -e.x : e.x, (i & 2) == 0 ? -e.y : e.y, (i & 4) == 0 ? -e.z : e.z) - bounds.center;
                var x = Vector3.Dot(corner, right);
                var y = Vector3.Dot(corner, up);
                minX = Mathf.Min(minX, x); maxX = Mathf.Max(maxX, x);
                minY = Mathf.Min(minY, y); maxY = Mathf.Max(maxY, y);
            }
            var usableHeight = 1f - _feel.TopSafeArea - _feel.BottomSafeArea;
            var halfHeight = Mathf.Max((maxY - minY) / 2f / usableHeight, (maxX - minX) / 2f / Mathf.Max(0.1f, _camera.aspect)) * padding;
            // Shift so the content sits in the middle of the usable band, not the full screen.
            var bandOffset = (_feel.TopSafeArea - _feel.BottomSafeArea) * halfHeight;
            var centre = bounds.center + right * ((minX + maxX) / 2f) + up * ((minY + maxY) / 2f + bandOffset);
            return (centre - forward * 60f, halfHeight);
        }

        /// <summary>Zoom from the whole building into one room (or back out when <paramref name="reverse"/>).</summary>
        public void Zoom(RoomView room, bool reverse, float duration, Action done)
        {
            var (fromPos, fromSize) = FrameFor(BuildingBounds, 1.1f);
            var roomBounds = new Bounds(room.transform.position + Vector3.up * 0.8f, room.Size);
            var (toPos, toSize) = FrameFor(roomBounds, 0.9f);
            if (reverse)
            {
                (fromPos, toPos) = (toPos, fromPos);
                (fromSize, toSize) = (toSize, fromSize);
            }
            _camera.orthographic = true;
            _camera.transform.rotation = ViewRotation;
            _zooming = true;
            Tween.Run(this, duration, t =>
            {
                _camera.transform.position = Vector3.Lerp(fromPos, toPos, t);
                _camera.orthographicSize = Mathf.Lerp(fromSize, toSize, t);
            }, Ease.InOutQuad, () =>
            {
                _zooming = false;
                done?.Invoke();
            });
        }

        // ---------- Input ----------

        void Update()
        {
            if (!InputEnabled) return;
            var pointer = Pointer.current;
            if (pointer == null) return;
            var position = pointer.position.ReadValue();
            if (pointer.press.wasPressedThisFrame)
            {
                _pressValid = _isOverUi == null || !_isOverUi(position);
                _pressPosition = position;
            }
            if (!pointer.press.wasReleasedThisFrame || !_pressValid) return;
            _pressValid = false;
            if ((position - _pressPosition).magnitude > TapMaxMovePixels) return;

            if (!Physics.Raycast(_camera.ScreenPointToRay(position), out var hit, 500f)) return;
            var room = hit.collider.GetComponentInParent<RoomView>();
            if (room != null) RoomTapped?.Invoke(room);
        }
    }
}
