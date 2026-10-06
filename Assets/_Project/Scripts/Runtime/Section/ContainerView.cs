using System;
using System.Collections;
using System.Collections.Generic;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SortingGame.Section
{
    /// <summary>
    /// GDD 7.1: tap and it tips over where it stands, spilling its contents onto the floor.
    /// Local origin is the floor centre; it tips towards its local +Z.
    /// </summary>
    public class ContainerView : MonoBehaviour
    {
        const float Wall = 0.025f;

        public ContainerDefinition Definition { get; private set; }
        public readonly List<ItemDefinition> Contents = new();
        public bool IsOpened { get; private set; }

        BoxCollider _collider;
        Vector3 _size;

        /// <summary>Called for every spilled item; the section creates the ItemView.</summary>
        public Func<ItemDefinition, Vector3, Quaternion, ItemView> SpawnItem;
        public event Action<ContainerView> Emptied;

        public void Build(ContainerDefinition definition, IEnumerable<ItemDefinition> contents, PlaceholderFactory factory)
        {
            Definition = definition;
            Contents.AddRange(contents);
            _size = definition.Placeholder.Size;
            var color = definition.Placeholder.Color;
            var dark = color * 0.8f;
            dark.a = 1f;

            if (definition.Prefab != null)
            {
                Instantiate(definition.Prefab, transform, false);
            }
            else
            {
                // Open-top box made of five panels, plus closed flaps that fly open on tip.
                var w = _size.x; var h = _size.y; var d = _size.z;
                factory.CreateBox("Bottom", transform, new Vector3(0f, Wall / 2f, 0f), new Vector3(w, Wall, d), dark, false);
                factory.CreateBox("Front", transform, new Vector3(0f, h / 2f, d / 2f - Wall / 2f), new Vector3(w, h, Wall), color, false);
                factory.CreateBox("Back", transform, new Vector3(0f, h / 2f, -d / 2f + Wall / 2f), new Vector3(w, h, Wall), color, false);
                factory.CreateBox("Left", transform, new Vector3(-w / 2f + Wall / 2f, h / 2f, 0f), new Vector3(Wall, h, d), color, false);
                factory.CreateBox("Right", transform, new Vector3(w / 2f - Wall / 2f, h / 2f, 0f), new Vector3(Wall, h, d), color, false);
                factory.CreateBox("Lid", transform, new Vector3(0f, h - Wall / 2f, 0f), new Vector3(w, Wall, d), dark, false);
                // A tape stripe makes the "closed box" read at a glance.
                factory.CreateBox("Tape", transform, new Vector3(0f, h + 0.002f, 0f), new Vector3(0.08f, 0.004f, d * 1.01f), new Color(0.85f, 0.75f, 0.55f), false);
            }

            _collider = gameObject.AddComponent<BoxCollider>();
            _collider.center = new Vector3(0f, _size.y / 2f, 0f);
            _collider.size = _size;
        }

        public void Open(FeelConfig feel)
        {
            if (IsOpened) return;
            IsOpened = true;
            StartCoroutine(TipAndSpill(feel));
        }

        IEnumerator TipAndSpill(FeelConfig feel)
        {
            // Lid pops off first.
            var lid = transform.Find("Lid");
            var tape = transform.Find("Tape");
            if (lid != null) Destroy(lid.gameObject);
            if (tape != null) Destroy(tape.gameObject);

            _collider.enabled = false;
            SfxPlayer.Instance?.Play(Sfx.Tip);
            Haptics.Medium();

            // Tip around the front-bottom edge: fall past 90 degrees, then settle back with a small bounce.
            var pivot = transform.TransformPoint(new Vector3(0f, 0f, _size.z / 2f));
            var axis = transform.right;
            var applied = 0f;
            var time = 0f;
            while (time < feel.TipDuration)
            {
                time += Time.deltaTime;
                var t = Mathf.Clamp01(time / feel.TipDuration);
                var angle = t < 0.75f
                    ? Mathf.Lerp(0f, 102f, Ease.InQuad(t / 0.75f))
                    : Mathf.Lerp(102f, 90f, Ease.OutQuad((t - 0.75f) / 0.25f));
                transform.RotateAround(pivot, axis, angle - applied);
                applied = angle;
                yield return null;
            }

            // Mouth of the box now faces outwards along the box's local up.
            var mouth = transform.TransformPoint(new Vector3(0f, _size.y + 0.08f, 0f));
            var outward = transform.up;
            var side = Vector3.Cross(Vector3.up, outward).normalized;

            foreach (var item in Contents)
            {
                var spawn = mouth
                            + side * Random.Range(-_size.x * 0.35f, _size.x * 0.35f)
                            + Vector3.up * Random.Range(0.02f, _size.z * 0.4f);
                var view = SpawnItem?.Invoke(item, spawn, Random.rotation);
                if (view != null)
                {
                    var velocity = outward * Random.Range(feel.SpillForwardSpeed.x, feel.SpillForwardSpeed.y)
                                   + Vector3.up * Random.Range(feel.SpillUpSpeed.x, feel.SpillUpSpeed.y)
                                   + side * Random.Range(-feel.SpillSideSpeed, feel.SpillSideSpeed);
                    view.Launch(velocity, Random.insideUnitSphere * 8f);
                }
                yield return new WaitForSeconds(feel.SpillInterval);
            }
            Contents.Clear();
            Emptied?.Invoke(this);

            // Kaos -> düzen: the empty box does not stay as clutter.
            yield return new WaitForSeconds(feel.EmptyContainerVanishDelay);
            var startScale = transform.localScale;
            Tween.Run(this, 0.3f, t => transform.localScale = Vector3.LerpUnclamped(startScale, Vector3.zero, t), Ease.InQuad,
                () => Destroy(gameObject));
        }
    }
}
