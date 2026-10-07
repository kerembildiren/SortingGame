using System;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Section
{
    public enum ItemState
    {
        Resting,   // lying still on the floor, physics off
        Physics,   // tumbling after a spill or a drop
        Dragging,
        Flying,    // animating to a slot or back to the floor
        Placed,    // on a shelf, final
        Buried,    // hidden under the dirt layer until swept free
        Found      // collectible taken by the player
    }

    /// <summary>A single sortable item in the section.</summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ItemView : MonoBehaviour
    {
        static PhysicsMaterial _physicsMaterial;

        public ItemDefinition Definition { get; private set; }
        public ItemState State { get; private set; }

        FeelConfig _feel;
        Rigidbody _body;
        Collider[] _colliders;
        Renderer[] _renderers;
        RareGlow _glow;
        Vector3 _pickupPosition;
        Quaternion _pickupRotation;
        float _restTimer;
        float _physicsTimer;
        Coroutine _motion;

        /// <summary>Set by the room: the nearest spot on the open floor for a world position. Used when an item gets lost.</summary>
        public Func<Vector3, Vector3> FloorPointFor;
        /// <summary>Raised when a tumbling item has come to rest.</summary>
        public Action<ItemView> Settled;

        public bool IsCollectible => Definition.IsCollectible;
        public bool IsRare => Definition.IsRare;

        /// <summary>Common and rare items are dragged; collectibles are tapped (GDD 7.1).</summary>
        public bool CanPick => !IsCollectible && State is ItemState.Resting or ItemState.Physics;
        public bool CanTapToFind => IsCollectible && State is ItemState.Resting or ItemState.Physics;

        /// <summary>Largest placeholder dimension, used to scale the rare-find presentation.</summary>
        public float VisualSize
        {
            get
            {
                var s = Definition.Placeholder.Size;
                return Mathf.Max(s.x, Mathf.Max(s.y, s.z));
            }
        }

        public static ItemView Create(ItemDefinition definition, PlaceholderFactory factory, FeelConfig feel, Transform parent, Fx fx = null, Color glowColor = default)
        {
            var go = new GameObject($"Item_{definition.Id}");
            go.transform.SetParent(parent, false);
            var body = go.AddComponent<Rigidbody>();
            var view = go.AddComponent<ItemView>();
            view.Definition = definition;
            view._feel = feel;
            view._body = body;

            factory.CreateItemVisual(definition, go.transform);

            // Unity null check on purpose: the static outlives a Play session, the material does not.
            if (_physicsMaterial == null)
                _physicsMaterial = new PhysicsMaterial("Item")
                {
                    dynamicFriction = 0.6f,
                    staticFriction = 0.7f,
                    bounciness = 0.15f,
                    bounceCombine = PhysicsMaterialCombine.Minimum
                };
            view._colliders = go.GetComponentsInChildren<Collider>();
            view._renderers = go.GetComponentsInChildren<Renderer>();
            if ((definition.IsCollectible || definition.IsRare) && fx != null)
                view._glow = RareGlow.Attach(view, factory, fx, glowColor, definition.IsCollectible);
            foreach (var c in view._colliders) c.sharedMaterial = _physicsMaterial;

            body.mass = 0.3f;
            body.linearDamping = 0.15f;
            body.angularDamping = 0.6f;
            body.interpolation = RigidbodyInterpolation.Interpolate;
            body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            // Items spill out of a box on top of each other; without a cap the solver shoots them across the room.
            body.maxDepenetrationVelocity = 2f;
            return view;
        }

        public void SetResting(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
            SetPhysics(false);
            SetCollidersEnabled(true);
            State = ItemState.Resting;
            RememberPose();
        }

        void RememberPose()
        {
            _pickupPosition = transform.position;
            _pickupRotation = transform.rotation;
        }

        public void Launch(Vector3 velocity, Vector3 angularVelocity)
        {
            // Physics starts from where the item is shown, not from where the body last was. An item that was just
            // created or moved by its transform would otherwise jump back there (new items: the room's centre).
            _body.position = transform.position;
            _body.rotation = transform.rotation;
            SetCollidersEnabled(true);
            SetPhysics(true);
            _body.linearVelocity = velocity;
            _body.angularVelocity = angularVelocity;
            EnterPhysics();
        }

        public void BeginDrag()
        {
            StopMotion();
            if (State == ItemState.Resting || State == ItemState.Physics) RememberPose();
            SetPhysics(false);
            SetCollidersEnabled(false);
            State = ItemState.Dragging;

            // Lift: grow a little and turn to face the camera so the item is easy to read.
            var startScale = transform.localScale;
            var startRotation = transform.rotation;
            _motion = Tween.Run(this, 0.12f, t =>
            {
                transform.localScale = Vector3.LerpUnclamped(startScale, Vector3.one * _feel.DragScale, t);
                transform.rotation = Quaternion.Slerp(startRotation, Quaternion.identity, t);
            }, Ease.OutBack);
        }

        /// <summary>A helper takes the item: off the floor and out of the player's reach, without the lift animation.</summary>
        public void BeginCarry()
        {
            StopMotion();
            if (State == ItemState.Resting || State == ItemState.Physics) RememberPose();
            SetPhysics(false);
            SetCollidersEnabled(false);
            State = ItemState.Dragging;
        }

        public void DragTowards(Vector3 target, float deltaTime)
        {
            var blend = 1f - Mathf.Exp(-_feel.DragFollowSharpness * deltaTime);
            transform.position = Vector3.Lerp(transform.position, target, blend);
        }

        /// <summary>Released over the floor: let it fall naturally.</summary>
        public void Drop()
        {
            StopMotion();
            transform.localScale = Vector3.one;
            Launch(Vector3.zero, UnityEngine.Random.insideUnitSphere * 2f);
        }

        /// <param name="duration">Defaults to FeelConfig.PlaceDuration.</param>
        /// <param name="arcHeight">Height of the flight curve; Auto Sort and helpers use a visible arc.</param>
        /// <param name="delay">Wait before taking off (staggered chains); the slot is reserved immediately.</param>
        public void FlyToSlot(ShelfSlot slot, Action onLanded, float duration = -1f, float arcHeight = 0f, float delay = 0f)
        {
            StopMotion();
            SetPhysics(false);
            SetCollidersEnabled(false);
            State = ItemState.Flying;
            slot.Reserve();

            void TakeOff()
            {
                var (target, targetRotation) = SlotPose(slot);
                var from = transform.position;
                var fromRotation = transform.rotation;
                var fromScale = transform.localScale;

                _motion = Tween.Run(this, duration > 0f ? duration : _feel.PlaceDuration, t =>
                {
                    transform.position = Vector3.Lerp(from, target, t) + Vector3.up * (Ease.Pulse(t) * arcHeight);
                    transform.rotation = Quaternion.Slerp(fromRotation, targetRotation, t);
                    transform.localScale = Vector3.Lerp(fromScale, Vector3.one, t);
                }, Ease.OutCubic, () =>
                {
                    transform.SetPositionAndRotation(target, targetRotation);
                    transform.SetParent(slot.Shelf.transform, true);
                    State = ItemState.Placed;
                    slot.Fill(this);
                    if (_glow != null) _glow.SetGlowing(false); // a shelved rare item has done its job
                    Punch();
                    onLanded?.Invoke();
                });
            }

            if (delay > 0f) _motion = Tween.Delay(this, delay, TakeOff);
            else TakeOff();
        }

        /// <summary>Loading a save: straight onto the shelf, no animation, no events.</summary>
        public void PlaceInstant(ShelfSlot slot)
        {
            StopMotion();
            SetPhysics(false);
            SetCollidersEnabled(false);
            var (target, rotation) = SlotPose(slot);
            transform.SetPositionAndRotation(target, rotation);
            transform.SetParent(slot.Shelf.transform, true);
            transform.localScale = Vector3.one;
            State = ItemState.Placed;
            slot.Fill(this);
            if (_glow != null) _glow.SetGlowing(false);
        }

        (Vector3 position, Quaternion rotation) SlotPose(ShelfSlot slot)
        {
            var height = Definition.Prefab != null ? 0f : Definition.Placeholder.Size.y * 0.5f;
            return (slot.Shelf.transform.TransformPoint(slot.LocalBase + Vector3.up * height), slot.Shelf.transform.rotation);
        }

        /// <summary>Pose to store in a save: where it rests, not where an animation happens to be.</summary>
        public (Vector3 position, Quaternion rotation) SavePose =>
            State is ItemState.Dragging or ItemState.Flying ? (_pickupPosition, _pickupRotation) : (transform.position, transform.rotation);

        void Punch()
        {
            _motion = Tween.Run(this, 0.2f, t =>
                transform.localScale = Vector3.one * (1f + (_feel.PlacePunchScale - 1f) * Ease.Pulse(t)));
        }

        /// <summary>Hidden under the dirt: invisible and not touchable until revealed.</summary>
        public void Bury(Vector3 position, Quaternion rotation)
        {
            SetResting(position, rotation);
            SetVisible(false);
            SetCollidersEnabled(false);
            State = ItemState.Buried;
        }

        /// <summary>Swept free: pops out of the dust.</summary>
        public void Reveal()
        {
            if (State != ItemState.Buried) return;
            SetVisible(true);
            SetCollidersEnabled(true);
            State = ItemState.Resting;
            var restPosition = transform.position;
            _motion = Tween.Run(this, 0.35f, t =>
            {
                transform.localScale = Vector3.one * Ease.OutBack(t);
                transform.position = restPosition + Vector3.up * (Ease.Pulse(t) * 0.12f);
            }, null, () => transform.position = restPosition);
        }

        /// <summary>Collectible picked up by the player; the presenter now owns its motion.</summary>
        public void MarkFound()
        {
            StopMotion();
            SetPhysics(false);
            SetCollidersEnabled(false);
            State = ItemState.Found;
            if (_glow != null) _glow.SetGlowing(false);
        }

        void SetVisible(bool visible)
        {
            foreach (var r in _renderers) if (r != null) r.enabled = visible;
            if (_glow != null) _glow.SetGlowing(visible);
        }

        void Update()
        {
            if (State != ItemState.Physics) return;

            // Safety net: anything that escapes the room comes back onto the floor right where it left,
            // never somewhere else (it used to reappear in the middle of the room and pile up there).
            if (transform.position.y < -1f)
            {
                var back = FloorPointFor != null ? FloorPointFor(transform.position) : new Vector3(transform.position.x, 0f, transform.position.z);
                // Through SetResting: moving the transform of a live, interpolated body does not stick.
                SetResting(back + Vector3.up * 0.3f, transform.rotation);
                Launch(Vector3.zero, Vector3.zero);
                return;
            }

            _physicsTimer += Time.deltaTime;
            var still = _body.linearVelocity.sqrMagnitude < 0.004f && _body.angularVelocity.sqrMagnitude < 0.02f;
            _restTimer = still ? _restTimer + Time.deltaTime : 0f;
            if (_restTimer >= _feel.SettleTime || _physicsTimer >= _feel.MaxPhysicsTime)
            {
                SetPhysics(false);
                State = ItemState.Resting;
                RememberPose();
                Settled?.Invoke(this);
            }
        }

        void EnterPhysics()
        {
            State = ItemState.Physics;
            _restTimer = 0f;
            _physicsTimer = 0f;
        }

        void SetPhysics(bool on)
        {
            if (!on && !_body.isKinematic)
            {
                _body.linearVelocity = Vector3.zero;
                _body.angularVelocity = Vector3.zero;
            }
            _body.isKinematic = !on;
            // Interpolation would keep pulling the rendered pose towards the old physics pose while
            // tweens/parenting move the transform (seen as the rare item drifting behind the light rays).
            _body.interpolation = on ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
        }

        void SetCollidersEnabled(bool on)
        {
            foreach (var c in _colliders) c.enabled = on;
        }

        void StopMotion()
        {
            Tween.Stop(_motion);
            _motion = null;
        }
    }
}
