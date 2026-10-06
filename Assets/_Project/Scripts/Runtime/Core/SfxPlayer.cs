using System;
using System.Collections.Generic;
using SortingGame.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SortingGame.Core
{
    public enum Sfx
    {
        PlacePaper,
        PlacePlastic,
        PlaceMetal,
        Pickup,
        Wrong,
        Coin,
        Tip,
        ShelfFull,
        SectionComplete,
        Reveal,
        RareShimmer,
        RareFanfare,
        DuplicateSold,
        BookStamp,
        SweepLoop,
        CleanAmbienceLoop,
        Magnet,
        Mastery,
        Purchase
    }

    /// <summary>
    /// Placeholder sound effects synthesised at startup, so feedback exists from day one (GDD 7.4)
    /// without any audio files. Swap for real clips later by replacing <see cref="_clips"/> entries.
    /// </summary>
    public class SfxPlayer : MonoBehaviour
    {
        const int SampleRate = 44100;
        const int Voices = 8;

        readonly Dictionary<Sfx, AudioClip> _clips = new();
        readonly List<AudioSource> _sources = new();
        readonly Dictionary<Sfx, AudioSource> _loops = new();
        int _next;

        public static SfxPlayer Instance { get; private set; }

        public float Volume = 0.8f;

        void Awake()
        {
            Instance = this;
            for (var i = 0; i < Voices; i++)
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.spatialBlend = 0f;
                _sources.Add(source);
            }
            Build();
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public static Sfx PlaceSoundFor(PlaceSoundType type) => type switch
        {
            PlaceSoundType.Paper => Sfx.PlacePaper,
            PlaceSoundType.Metal => Sfx.PlaceMetal,
            _ => Sfx.PlacePlastic
        };

        public void Play(Sfx sfx, float pitchVariance = 0.06f, float volume = 1f)
        {
            if (!_clips.TryGetValue(sfx, out var clip)) return;
            var source = _sources[_next];
            _next = (_next + 1) % _sources.Count;
            source.pitch = 1f + Random.Range(-pitchVariance, pitchVariance);
            source.PlayOneShot(clip, Volume * volume);
        }

        /// <summary>Looping sound (sweeping, ambience). Volume 0 keeps it running silently.</summary>
        public void SetLoop(Sfx sfx, float volume, float pitch = 1f)
        {
            if (!_loops.TryGetValue(sfx, out var source))
            {
                if (!_clips.TryGetValue(sfx, out var clip)) return;
                source = gameObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.loop = true;
                source.playOnAwake = false;
                source.volume = 0f;
                _loops[sfx] = source;
            }
            source.volume = Volume * volume;
            source.pitch = pitch;
            if (volume > 0f && !source.isPlaying) source.Play();
            else if (volume <= 0f && source.isPlaying) source.Stop();
        }

        /// <summary>Rising pitch for chains of quick placements feels rewarding.</summary>
        public void PlayPitched(Sfx sfx, float pitch, float volume = 1f)
        {
            if (!_clips.TryGetValue(sfx, out var clip)) return;
            var source = _sources[_next];
            _next = (_next + 1) % _sources.Count;
            source.pitch = pitch;
            source.PlayOneShot(clip, Volume * volume);
        }

        void Build()
        {
            var noise = new System.Random(7);
            float Noise() => (float)(noise.NextDouble() * 2.0 - 1.0);

            // Paper: soft filtered noise "fff" + low thump.
            _clips[Sfx.PlacePaper] = Make("place_paper", 0.14f, (t, i) =>
            {
                var env = Mathf.Exp(-t * 32f);
                return (Noise() * 0.35f + Mathf.Sin(2 * Mathf.PI * 140f * t) * 0.6f) * env;
            }, lowPass: 0.25f);

            // Plastic: two short clicks.
            _clips[Sfx.PlacePlastic] = Make("place_plastic", 0.09f, (t, i) =>
            {
                var env = Mathf.Exp(-t * 60f);
                return (Mathf.Sin(2 * Mathf.PI * 1250f * t) * 0.5f + Mathf.Sin(2 * Mathf.PI * 2600f * t) * 0.25f + Noise() * 0.1f) * env;
            });

            // Metal: inharmonic partials with a longer ring.
            _clips[Sfx.PlaceMetal] = Make("place_metal", 0.4f, (t, i) =>
            {
                var env = Mathf.Exp(-t * 11f);
                return (Mathf.Sin(2 * Mathf.PI * 880f * t) * 0.35f + Mathf.Sin(2 * Mathf.PI * 2390f * t) * 0.25f + Mathf.Sin(2 * Mathf.PI * 3710f * t) * 0.15f) * env;
            });

            _clips[Sfx.Pickup] = Make("pickup", 0.06f, (t, i) =>
                Mathf.Sin(2 * Mathf.PI * Mathf.Lerp(500f, 900f, t / 0.06f) * t) * Mathf.Exp(-t * 50f) * 0.4f);

            _clips[Sfx.Wrong] = Make("wrong", 0.18f, (t, i) =>
                Mathf.Sin(2 * Mathf.PI * Mathf.Lerp(320f, 190f, t / 0.18f) * t) * Mathf.Exp(-t * 14f) * 0.5f);

            _clips[Sfx.Coin] = Make("coin", 0.22f, (t, i) =>
            {
                var f = t < 0.06f ? 1320f : 1760f;
                return Mathf.Sin(2 * Mathf.PI * f * t) * Mathf.Exp(-(t < 0.06f ? t : t - 0.06f) * 18f) * 0.3f;
            });

            // Box tip: low rumble + random clatter clicks.
            _clips[Sfx.Tip] = Make("tip", 0.45f, (t, i) =>
            {
                var rumble = Noise() * Mathf.Exp(-t * 7f) * 0.5f;
                var clatter = (i % 1800 < 60 && t > 0.08f) ? Noise() * 0.6f : 0f;
                return rumble + clatter * Mathf.Exp(-t * 4f);
            }, lowPass: 0.15f);

            _clips[Sfx.Reveal] = Make("reveal", 0.16f, (t, i) =>
                Mathf.Sin(2 * Mathf.PI * Mathf.Lerp(380f, 760f, t / 0.16f) * t) * Mathf.Exp(-t * 18f) * 0.45f);

            // High twinkle that loops softly near a glowing rare item.
            _clips[Sfx.RareShimmer] = Make("rare_shimmer", 0.6f, (t, i) =>
            {
                var sum = 0f;
                float[] notes = { 2093f, 2637f, 3136f };
                for (var n = 0; n < notes.Length; n++)
                {
                    var start = n * 0.12f;
                    if (t < start) continue;
                    sum += Mathf.Sin(2 * Mathf.PI * notes[n] * (t - start)) * Mathf.Exp(-(t - start) * 9f);
                }
                return sum * 0.12f;
            });

            _clips[Sfx.RareFanfare] = Arpeggio("rare_fanfare", new[] { 523f, 659f, 784f, 1047f, 1319f, 1568f, 2093f }, 0.07f);
            _clips[Sfx.DuplicateSold] = Arpeggio("duplicate_sold", new[] { 1319f, 1568f, 1760f, 2093f }, 0.05f);

            _clips[Sfx.BookStamp] = Make("book_stamp", 0.2f, (t, i) =>
                (Mathf.Sin(2 * Mathf.PI * 110f * t) * 0.7f + Noise() * 0.3f) * Mathf.Exp(-t * 25f));

            // Seamless brushing loop: band-limited noise with a slow swish.
            _clips[Sfx.SweepLoop] = Make("sweep_loop", 1.0f, (t, i) =>
                Noise() * (0.5f + 0.25f * Mathf.Sin(2 * Mathf.PI * 4f * t)) * 0.5f, lowPass: 0.35f, fadeEdges: false);

            // Warm pad for clean rooms (GDD 14: "temizlenince sıcak bir müzik katmanı").
            _clips[Sfx.CleanAmbienceLoop] = Make("clean_ambience", 4.0f, (t, i) =>
            {
                float[] chord = { 261.75f, 329.5f, 392f, 494f }; // whole cycles in 4 s, so the loop has no click
                var sum = 0f;
                foreach (var f in chord) sum += Mathf.Sin(2 * Mathf.PI * f * t) + 0.25f * Mathf.Sin(4 * Mathf.PI * f * t);
                var swell = 0.75f + 0.25f * Mathf.Sin(2 * Mathf.PI * 0.25f * t);
                return sum * swell * 0.05f;
            }, fadeEdges: false);

            // Rising "whoop" when followers snap onto the carried item.
            _clips[Sfx.Magnet] = Make("magnet", 0.25f, (t, i) =>
                Mathf.Sin(2 * Mathf.PI * Mathf.Lerp(220f, 880f, t / 0.25f) * t) * Mathf.Exp(-t * 6f) * 0.35f);

            // Bigger than a shelf: category mastered (GDD 10.2 celebration).
            _clips[Sfx.Mastery] = Arpeggio("mastery", new[] { 392f, 523f, 659f, 784f, 1047f, 784f, 1047f, 1319f }, 0.08f);

            _clips[Sfx.Purchase] = Make("purchase", 0.35f, (t, i) =>
            {
                var bell = Mathf.Sin(2 * Mathf.PI * 1568f * t) * Mathf.Exp(-t * 10f);
                var second = t > 0.08f ? Mathf.Sin(2 * Mathf.PI * 2093f * (t - 0.08f)) * Mathf.Exp(-(t - 0.08f) * 10f) : 0f;
                return (bell + second) * 0.25f;
            });

            _clips[Sfx.ShelfFull] = Arpeggio("shelf_full", new[] { 784f, 988f, 1175f }, 0.08f);
            _clips[Sfx.SectionComplete] = Arpeggio("section_complete", new[] { 523f, 659f, 784f, 1047f, 1319f }, 0.11f);
        }

        static AudioClip Arpeggio(string name, float[] notes, float step)
        {
            var length = step * notes.Length + 0.5f;
            return Make(name, length, (t, i) =>
            {
                var sum = 0f;
                for (var n = 0; n < notes.Length; n++)
                {
                    var start = n * step;
                    if (t < start) break;
                    var local = t - start;
                    sum += (Mathf.Sin(2 * Mathf.PI * notes[n] * local) + 0.3f * Mathf.Sin(4 * Mathf.PI * notes[n] * local)) * Mathf.Exp(-local * 6f);
                }
                return sum * 0.2f;
            });
        }

        static AudioClip Make(string name, float seconds, Func<float, int, float> sample, float lowPass = 1f, bool fadeEdges = true)
        {
            var count = Mathf.CeilToInt(seconds * SampleRate);
            var data = new float[count];
            var previous = 0f;
            for (var i = 0; i < count; i++)
            {
                var value = sample((float)i / SampleRate, i);
                previous += (value - previous) * lowPass; // one-pole low-pass, 1 = off
                // Short fade-in avoids clicks. Loops skip it (frequencies are chosen to wrap cleanly).
                var fadeIn = fadeEdges ? Mathf.Clamp01(i / 64f) : 1f;
                data[i] = Mathf.Clamp(previous * fadeIn, -1f, 1f);
            }
            var clip = AudioClip.Create(name, count, 1, SampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }
    }
}
