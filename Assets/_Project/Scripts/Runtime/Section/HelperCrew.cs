using System.Collections.Generic;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Section
{
    /// <summary>
    /// GDD 10.3: the hired helpers of the room the player is in. There is no assignment screen: they appear when a
    /// room is opened (or when one is hired there) and leave with it. Nothing happens while the player is away.
    /// </summary>
    public class HelperCrew : MonoBehaviour
    {
        /// <summary>Items a helper is walking to or carrying, so two helpers never go for the same one.</summary>
        public readonly HashSet<ItemView> Taken = new();
        public IReadOnlyList<HelperView> Helpers => _helpers;
        public Fx Fx { get; private set; }

        readonly List<HelperView> _helpers = new();
        GameContext _ctx;
        SectionController _section;
        PlaceholderFactory _factory;

        public void Init(GameContext context, SectionController section)
        {
            _ctx = context;
            _section = section;
            _factory = new PlaceholderFactory(context.Visuals);
            Fx = new Fx(context.Visuals);
        }

        /// <summary>Makes the room match what is hired. Call after a section is built and after hiring.</summary>
        public void Sync()
        {
            _helpers.RemoveAll(h => h == null); // the old room took its helpers with it
            Taken.RemoveWhere(i => i == null);
            if (_helpers.Count == 0) Taken.Clear();
            if (!_section.IsLoaded) return;

            foreach (var definition in _ctx.Database.Helpers)
            {
                if (definition == null || !_ctx.Helpers.IsHired(definition)) continue;
                if (_helpers.Exists(h => h.Definition == definition)) continue;
                _helpers.Add(HelperView.Create(definition, this, _section, _ctx, _factory, SpawnPoint()));
            }
        }

        /// <summary>In front of the player: the floor under the middle of the screen, a little off to the side.</summary>
        Vector3 SpawnPoint()
        {
            var camera = Camera.main;
            if (camera == null) return _section.RandomFloorPoint();
            var ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.4f, 0f));
            if (!new Plane(Vector3.up, Vector3.zero).Raycast(ray, out var enter)) return _section.RandomFloorPoint();
            var offset = Random.insideUnitCircle * 0.6f;
            return _section.ClampToRoom(ray.GetPoint(enter) + new Vector3(offset.x, 0f, offset.y));
        }

        public HelperView Of(HelperDefinition definition) => _helpers.Find(h => h != null && h.Definition == definition);
    }
}
