using System.Collections.Generic;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Section
{
    /// <summary>
    /// GDD 10.3: one helper in the room. It hops to a loose common item, picks it up, carries it to its shelf and
    /// puts it there; with a higher level it collects a few items per trip. It never opens boxes, sweeps, or
    /// touches rare items and Chubby. With nothing to do (or in a finished room) it wanders, and it likes being petted.
    /// It looks for work by scanning what is loose right now, so items that appear later are never missed.
    /// </summary>
    public class HelperView : MonoBehaviour
    {
        public enum Activity
        {
            Idle,       // deciding what to do next
            ToItem,
            Picking,
            ToShelf,
            Placing,
            Wandering
        }

        const float ArriveDistance = 0.14f;
        const float AvoidRadius = 0.7f;
        const float HappyDuration = 0.9f;
        const float LookUpTilt = -20f; // the face tilts towards the camera above

        public HelperDefinition Definition { get; private set; }
        public Activity Current { get; private set; } = Activity.Idle;
        public int CarriedCount => _carried.Count;
        public bool IsHappy => _happyTime < HappyDuration;
        public int PetCount { get; private set; }
        public int Capacity => Mathf.Max(1, _ctx.Helpers.Stats(Definition).Capacity);
        float Speed => Mathf.Max(0.1f, _ctx.Helpers.Stats(Definition).Speed);

        readonly List<ItemView> _carried = new();
        HelperCrew _crew;
        SectionController _section;
        GameContext _ctx;
        FeelConfig _feel;
        Transform _model;
        float _size;
        ItemView _target;
        Vector3 _goal;
        float _stateTime;
        float _pause;
        float _hopPhase;
        float _happyTime = HappyDuration;
        float _spawnTime;

        public static HelperView Create(HelperDefinition definition, HelperCrew crew, SectionController section, GameContext context, PlaceholderFactory factory, Vector3 position)
        {
            var go = new GameObject($"Helper_{definition.Id}");
            go.transform.SetParent(section.Root, false);
            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            var helper = go.AddComponent<HelperView>();
            helper.Definition = definition;
            helper._crew = crew;
            helper._section = section;
            helper._ctx = context;
            helper._feel = context.Feel;
            helper._size = context.Feel.HelperSize;
            helper.BuildModel(factory);

            // Trigger only: it must not push tumbling items around. Taps find it with a trigger raycast.
            var touch = go.AddComponent<SphereCollider>();
            touch.isTrigger = true;
            touch.center = new Vector3(0f, helper._size * 0.5f, 0f);
            touch.radius = helper._size * 0.9f;
            return helper;
        }

        /// <summary>Round, soft and small: body, big eyes, rosy cheeks, two feet and a little sprout on top.</summary>
        void BuildModel(PlaceholderFactory factory)
        {
            _model = new GameObject("Model").transform;
            _model.SetParent(transform, false);
            var s = _size;
            var white = new Color(0.98f, 0.98f, 0.98f);
            var black = new Color(0.08f, 0.08f, 0.1f);
            var blush = new Color(1f, 0.6f, 0.65f);

            Part(factory, "Body", PlaceholderShape.Sphere, Definition.BodyColor, new Vector3(0f, s * 0.48f, 0f), new Vector3(s, s * 0.92f, s));
            for (var side = -1; side <= 1; side += 2)
            {
                // Big eyes set high: the camera looks down on the room, the face still has to read.
                Part(factory, "Eye", PlaceholderShape.Sphere, white, new Vector3(side * s * 0.2f, s * 0.64f, s * 0.36f), new Vector3(s * 0.3f, s * 0.34f, s * 0.16f));
                Part(factory, "Pupil", PlaceholderShape.Sphere, black, new Vector3(side * s * 0.2f, s * 0.66f, s * 0.43f), new Vector3(s * 0.15f, s * 0.19f, s * 0.08f));
                Part(factory, "Cheek", PlaceholderShape.Sphere, blush, new Vector3(side * s * 0.36f, s * 0.46f, s * 0.36f), new Vector3(s * 0.15f, s * 0.1f, s * 0.07f));
                Part(factory, "Foot", PlaceholderShape.Sphere, Definition.AccentColor, new Vector3(side * s * 0.2f, s * 0.05f, s * 0.08f), new Vector3(s * 0.26f, s * 0.12f, s * 0.34f));
            }
            Part(factory, "Stem", PlaceholderShape.Rod, Definition.AccentColor, new Vector3(0f, s * 1.0f, 0f), new Vector3(s * 0.05f, s * 0.2f, s * 0.05f));
            Part(factory, "Sprout", PlaceholderShape.Sphere, Definition.AccentColor, new Vector3(0f, s * 1.14f, 0f), new Vector3(s * 0.2f, s * 0.14f, s * 0.2f));
        }

        void Part(PlaceholderFactory factory, string partName, PlaceholderShape shape, Color color, Vector3 position, Vector3 size)
        {
            var part = factory.CreateShape(new PlaceholderVisual(shape, color, size), _model, false);
            part.name = partName;
            part.transform.localPosition = position;
        }

        // ---------- Petting ----------

        /// <summary>Tapped by the player: a happy jump, hearts and a chirp (GDD 10.3).</summary>
        public void Pet()
        {
            _happyTime = 0f;
            PetCount++;
            _crew.Fx.Hearts(transform.position + Vector3.up * (_size * 1.2f));
            SfxPlayer.Instance?.Play(Sfx.HelperChirp, 0.12f);
            Haptics.Light();
        }

        // ---------- Brain ----------

        void Update()
        {
            var dt = Time.deltaTime;
            _spawnTime += dt;
            if (IsHappy)
            {
                // Enjoying the moment: work waits.
                _happyTime += dt;
                Animate(0f, dt);
                Carry(dt);
                return;
            }

            _stateTime += dt;
            var moved = 0f;
            switch (Current)
            {
                case Activity.Idle: Decide(); break;
                case Activity.ToItem: moved = WalkToItem(dt); break;
                case Activity.Picking: PickUp(); break;
                case Activity.ToShelf: moved = WalkToShelf(dt); break;
                case Activity.Placing: Place(); break;
                case Activity.Wandering: moved = Wander(dt); break;
            }
            Animate(moved, dt);
            Carry(dt);
        }

        void Enter(Activity next)
        {
            Current = next;
            _stateTime = 0f;
        }

        bool RoomNeedsWork => _section.IsLoaded && !_section.IsComplete;

        void Decide()
        {
            if (_carried.Count > 0)
            {
                HeadForShelf();
                return;
            }
            if (RoomNeedsWork && Claim(float.MaxValue)) return;
            StartWandering();
        }

        /// <summary>Reserves the nearest free item within <paramref name="maxDistance"/> and walks to it.</summary>
        bool Claim(float maxDistance)
        {
            var item = _section.FindHelperTarget(transform.position, maxDistance, _crew.Taken);
            if (item == null) return false;
            _target = item;
            _crew.Taken.Add(item);
            Enter(Activity.ToItem);
            return true;
        }

        void Release()
        {
            if (_target != null) _crew.Taken.Remove(_target);
            _target = null;
        }

        /// <summary>The player (or the Auto Sort boost) may take the item first; then the helper thinks again.</summary>
        bool TargetStillThere => _target != null && _target.State == ItemState.Resting;

        float WalkToItem(float dt)
        {
            if (!TargetStillThere)
            {
                Release();
                Enter(Activity.Idle);
                return 0f;
            }
            var moved = Step(_target.transform.position, Speed, dt, out var arrived);
            if (arrived) Enter(Activity.Picking);
            return moved;
        }

        void PickUp()
        {
            if (!TargetStillThere)
            {
                Release();
                Enter(Activity.Idle);
                return;
            }
            if (_stateTime < _feel.HelperPickupPause) return;

            var item = _target;
            _target = null; // stays in Taken while carried
            item.BeginCarry();
            _carried.Add(item);
            SfxPlayer.Instance?.Play(Sfx.Pickup, 0.1f, 0.35f);

            // Room in its arms: one more nearby item, otherwise off to the shelf.
            if (_carried.Count < Capacity && Claim(_feel.HelperChainRadius)) return;
            HeadForShelf();
        }

        void HeadForShelf()
        {
            if (_carried.Count == 0)
            {
                Enter(Activity.Idle);
                return;
            }
            if (_section.HelperStandPoint(_carried[0], transform.position, out _goal))
            {
                Enter(Activity.ToShelf);
                return;
            }
            DropEverything(); // no free slot (should not happen: one slot per item)
            Enter(Activity.Idle);
        }

        float WalkToShelf(float dt)
        {
            var moved = Step(_goal, Speed, dt, out var arrived);
            if (arrived) Enter(Activity.Placing);
            return moved;
        }

        /// <summary>One item per pause; everything in its arms that belongs on this shelf, then the next shelf.</summary>
        void Place()
        {
            if (_stateTime < _feel.HelperPlacePause) return;
            _stateTime = 0f;

            var category = _carried.Count > 0 ? _carried[0].Definition.Category : null;
            var item = _carried.Count > 0 ? _carried[0] : null;
            if (item != null)
            {
                _carried.RemoveAt(0);
                _crew.Taken.Remove(item);
                if (item.State == ItemState.Dragging && !_section.HelperPlace(item)) item.Drop();
            }
            if (_carried.Exists(i => i.Definition.Category == category))
            {
                // Bring the next one of this shelf to the front.
                var index = _carried.FindIndex(i => i.Definition.Category == category);
                var next = _carried[index];
                _carried.RemoveAt(index);
                _carried.Insert(0, next);
                return;
            }
            if (_carried.Count > 0) HeadForShelf();
            else Enter(Activity.Idle);
        }

        void StartWandering()
        {
            _goal = _section.RandomFloorPoint();
            _pause = Random.Range(_feel.HelperWanderPause.x, _feel.HelperWanderPause.y);
            Enter(Activity.Wandering);
        }

        float Wander(float dt)
        {
            // Work may turn up at any time: a box opened, dirt swept away, an item dropped.
            if (RoomNeedsWork && _stateTime >= _feel.HelperIdleRescan)
            {
                _stateTime = 0f;
                if (Claim(float.MaxValue)) return 0f;
            }
            if (_pause > 0f)
            {
                _pause -= dt;
                return 0f;
            }
            var moved = Step(_goal, _feel.HelperWanderSpeed, dt, out var arrived);
            if (arrived) StartWandering();
            return moved;
        }

        void DropEverything()
        {
            foreach (var item in _carried)
            {
                if (item == null) continue;
                _crew.Taken.Remove(item);
                item.Drop();
            }
            _carried.Clear();
        }

        void OnDestroy()
        {
            // Hired helpers leave with the room; whatever they held is saved at its floor position (ItemView.SavePose).
            if (_crew == null) return;
            Release();
            foreach (var item in _carried) _crew.Taken.Remove(item);
        }

        // ---------- Movement and looks ----------

        /// <summary>One step along the floor towards <paramref name="goal"/>, around closed boxes. Returns the distance moved.</summary>
        float Step(Vector3 goal, float speed, float dt, out bool arrived)
        {
            var position = transform.position;
            var to = new Vector3(goal.x - position.x, 0f, goal.z - position.z);
            var distance = to.magnitude;
            arrived = distance <= ArriveDistance;
            if (arrived) return 0f;

            var direction = to / distance;
            if (distance > AvoidRadius) direction = AroundBoxes(position, direction);

            var move = Mathf.Min(speed * dt, distance);
            position += direction * move;
            position.y = 0f;
            transform.position = _section.ClampToRoom(position);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), 1f - Mathf.Exp(-10f * dt));
            return move;
        }

        Vector3 AroundBoxes(Vector3 position, Vector3 direction)
        {
            var steer = direction;
            foreach (var container in _section.Containers)
            {
                if (container == null) continue;
                var away = position - container.transform.position;
                away.y = 0f;
                var distance = away.magnitude;
                if (distance >= AvoidRadius || distance < 0.001f || Vector3.Dot(-away, direction) <= 0f) continue;
                var push = 1f - distance / AvoidRadius;
                var side = Vector3.Cross(Vector3.up, direction);
                if (Vector3.Dot(side, away) < 0f) side = -side;
                steer += side * (push * 2f) + away / distance * push;
            }
            return steer.sqrMagnitude < 0.0001f ? direction : steer.normalized;
        }

        /// <summary>Hops while moving, breathes while standing, jumps and spins while happy. All on the model, the root stays on the floor.</summary>
        void Animate(float moved, float dt)
        {
            float height, stretch;
            var spin = 0f;
            if (IsHappy)
            {
                var t = _happyTime / HappyDuration;
                height = Ease.Pulse(t) * _size * 1.4f;
                stretch = Mathf.Sin(t * Mathf.PI * 3f) * 0.22f;
                spin = t * 360f;
            }
            else if (moved > 0f)
            {
                _hopPhase += moved * _feel.HelperHopsPerMetre * Mathf.PI;
                var hop = Mathf.Abs(Mathf.Sin(_hopPhase));
                height = hop * _feel.HelperHopHeight;
                stretch = Mathf.Lerp(-0.14f, 0.12f, hop);
            }
            else
            {
                _hopPhase = 0f;
                height = 0f;
                stretch = Mathf.Sin(Time.time * 3f + _size * 10f) * 0.035f;
            }

            // Standing around or being petted: turn to the player.
            if (moved <= 0f && (IsHappy || Current == Activity.Wandering))
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back, Vector3.up), 1f - Mathf.Exp(-6f * dt));

            var grow = Mathf.Min(1f, Ease.OutBack(Mathf.Clamp01(_spawnTime / 0.4f)));
            _model.localPosition = new Vector3(0f, height, 0f);
            _model.localRotation = Quaternion.Euler(LookUpTilt, spin, 0f);
            _model.localScale = new Vector3(1f - stretch * 0.6f, 1f + stretch, 1f - stretch * 0.6f) * Mathf.Max(0.01f, grow);
        }

        /// <summary>Carried items ride above its head, stacked.</summary>
        void Carry(float dt)
        {
            for (var i = _carried.Count - 1; i >= 0; i--)
            {
                var item = _carried[i];
                // Gone, or no longer in its arms (something else moved it on): let go without touching it.
                if (item == null || item.State != ItemState.Dragging)
                {
                    if (item != null) _crew.Taken.Remove(item);
                    _carried.RemoveAt(i);
                    continue;
                }
                var seat = _model.position + Vector3.up * (_size * 1.25f + item.VisualSize * 0.5f + i * 0.14f);
                item.DragTowards(seat, dt);
            }
        }
    }
}
