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
        Placed     // on a shelf, final
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
        Vector3 _pickupPosition;
        Quaternion _pickupRotation;
        float _restTimer;
        float _physicsTimer;
        Coroutine _motion;

        public bool CanPick => State is ItemState.Resting or ItemState.Physics;

        public static ItemView Create(ItemDefinition definition, PlaceholderFactory factory, FeelConfig feel, Transform parent)
        {
            var go = new GameObject($"Item_{definition.Id}");
            go.transform.SetParent(parent, false);
            var body = go.AddComponent<Rigidbody>();
            var view = go.AddComponent<ItemView>();
            view.Definition = definition;
            view._feel = feel;
            view._body = body;

            if (definition.Prefab != null) Instantiate(definition.Prefab, go.transform, false);
            else factory.CreateShape(definition.Placeholder, go.transform);

            _physicsMaterial ??= new PhysicsMaterial("Item")
            {
                dynamicFriction = 0.6f,
                staticFriction = 0.7f,
                bounciness = 0.15f,
                bounceCombine = PhysicsMaterialCombine.Minimum
            };
            view._colliders = go.GetComponentsInChildren<Collider>();
            foreach (var c in view._colliders) c.sharedMaterial = _physicsMaterial;

            body.mass = 0.3f;
            body.linearDamping = 0.15f;
            body.angularDamping = 0.6f;
            body.interpolation = RigidbodyInterpolation.Interpolate;
            body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
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

        /// <summary>Wrong shelf: soft arc back to where it was picked up (GDD 7.2, no penalty).</summary>
        public void ReturnToPickup()
        {
            StopMotion();
            State = ItemState.Flying;
            var from = transform.position;
            var fromRotation = transform.rotation;
            var fromScale = transform.localScale;
            var to = _pickupPosition;
            _motion = Tween.Run(this, _feel.ReturnDuration, t =>
            {
                transform.position = Vector3.Lerp(from, to, t) + Vector3.up * (Ease.Pulse(t) * 0.35f);
                transform.rotation = Quaternion.Slerp(fromRotation, _pickupRotation, t);
                transform.localScale = Vector3.Lerp(fromScale, Vector3.one, t);
            }, Ease.InOutQuad, () => SetResting(to, _pickupRotation));
        }

        public void FlyToSlot(ShelfSlot slot, Action onLanded)
        {
            StopMotion();
            SetPhysics(false);
            SetCollidersEnabled(false);
            State = ItemState.Flying;
            slot.Reserve();

            var height = Definition.Prefab != null ? 0f : Definition.Placeholder.Size.y * 0.5f;
            var target = slot.Shelf.transform.TransformPoint(slot.LocalBase + Vector3.up * height);
            var targetRotation = slot.Shelf.transform.rotation;
            var from = transform.position;
            var fromRotation = transform.rotation;
            var fromScale = transform.localScale;

            _motion = Tween.Run(this, _feel.PlaceDuration, t =>
            {
                transform.position = Vector3.Lerp(from, target, t);
                transform.rotation = Quaternion.Slerp(fromRotation, targetRotation, t);
                transform.localScale = Vector3.Lerp(fromScale, Vector3.one, t);
            }, Ease.OutCubic, () =>
            {
                transform.SetPositionAndRotation(target, targetRotation);
                transform.SetParent(slot.Shelf.transform, true);
                State = ItemState.Placed;
                slot.Fill(this);
                Punch();
                onLanded?.Invoke();
            });
        }

        void Punch()
        {
            _motion = Tween.Run(this, 0.2f, t =>
                transform.localScale = Vector3.one * (1f + (_feel.PlacePunchScale - 1f) * Ease.Pulse(t)));
        }

        void Update()
        {
            if (State != ItemState.Physics) return;

            // Safety net: anything that escapes the room comes back.
            if (transform.position.y < -2f)
            {
                SetResting(_pickupPosition + Vector3.up * 0.3f, _pickupRotation);
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
