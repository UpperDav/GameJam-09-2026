using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PuppetHero
{
    public class NoteScroller : MonoBehaviour
    {
        [SerializeField] private List<GameObject> strings = new();
        [SerializeField] private float delay = 1f;
        [SerializeField] private List<TextAsset> trackFiles = new();
        [SerializeField] private float noteSpeed = 4f;
        [SerializeField] private StringManager? stringManager;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource musicSource;

        [Header("Hit feedback SFX")]
        [SerializeField] private AudioClip? missClip;
        [SerializeField] private AudioClip? emptyClip;

        [Header("Music")]
        [SerializeField] private AudioClip? musicClip;

        private float elapsedTime;

        private List<List<String.StringColor>> track = new();
        private float step = 1f;
        private int lastIndex = -1;

        private int currentTrackIndex = 0;

        private void Start()
        {
            elapsedTime = 0f;

            if (stringManager == null)
            {
                stringManager = FindFirstObjectByType<StringManager>();

                if (stringManager == null)
                {
                    Debug.LogError("NoteScroller: StringManager not found!", this);
                    return;
                }
            }

            if (trackFiles.Count == 0)
            {
                Debug.LogError("NoteScroller: No track files assigned!", this);
                return;
            }

            if (musicClip == null)
            {
                Debug.LogError("NoteScroller: No music clip assigned!", this);
                return;
            }

            LoadTrack(currentTrackIndex);

            Note.speed = noteSpeed;

            stringManager.RegisterOnAllStringBroken(StopSpawning);

            //PlayMusic();
        }

        private void OnDestroy()
        {
        }

        private void LoadTrack(int index)
        {
            if (trackFiles.Count == 0)
                return;

            currentTrackIndex = index % trackFiles.Count;

            TextAsset trackFile = trackFiles[currentTrackIndex];

            if (trackFile == null)
            {
                Debug.LogError(
                    $"NoteScroller: Track file at index {currentTrackIndex} is null!",
                    this
                );

                track.Clear();
                return;
            }

            track.Clear();
            lastIndex = -1;
            elapsedTime = delay;

            string[] lines = trackFile.text.Split('\n');

            if (lines.Length == 0)
            {
                Debug.LogError(
                    $"NoteScroller: Track file '{trackFile.name}' is empty!",
                    this
                );

                return;
            }

            if (!int.TryParse(lines[0].Trim(), out int beat))
            {
                Debug.LogError(
                    $"NoteScroller: Could not read BPM from first line of '{trackFile.name}'.",
                    this
                );

                return;
            }

            step = 60f / beat;

            foreach (string line in lines.Skip(1))
            {
                List<String.StringColor> colors = new();

                foreach (char c in line)
                {
                    if (char.IsWhiteSpace(c))
                        continue;

                    colors.Add(GetColor(c));
                }

                track.Add(colors);
            }

            Debug.Log($"Loaded track file: {trackFile.name}");
        }

        private String.StringColor GetColor(char c)
        {
            return c switch
            {
                'b' => String.StringColor.blue,
                'g' => String.StringColor.green,
                'p' => String.StringColor.purple,
                'r' => String.StringColor.red,
                'y' => String.StringColor.yellow,

                _ => throw new ArgumentException(
                    $"Cannot convert character '{c}' to a valid String.StringColor value"
                )
            };
        }

        private void StopSpawning()
        {
            enabled = false;

            if (musicSource != null)
                musicSource.Stop();

            foreach (GameObject s in strings)
            {
                String str = s.GetComponent<String>();

                if (str != null)
                    str.ClearAllNotes();
            }
        }

        public void PlayMissSound()
        {
            if (audioSource != null && missClip != null)
                audioSource.PlayOneShot(missClip, 0.3f);
        }

        public void PlayEmptyClickSound()
        {
            if (audioSource != null && emptyClip != null)
                audioSource.PlayOneShot(emptyClip, 0.1f);
        }

        private void Update()
        {
            if (track.Count == 0 || strings.Count == 0)
                return;

            elapsedTime += Time.deltaTime;

            int index = GetIndex();

            if (index != lastIndex && index >= 0)
            {
                List<String.StringColor> colors = track[index];

                foreach (String.StringColor color in colors)
                {
                    String? s = null;

                    // Try to use the preferred string for this color.
                    if ((int)color < strings.Count)
                    {
                        String preferredString =
                            strings[(int)color].GetComponent<String>();

                        if (preferredString != null && !preferredString.isCut)
                            s = preferredString;
                    }

                    // If the preferred string is unavailable,
                    // choose another available string.
                    if (s == null)
                    {
                        List<String> availableStrings = strings
                            .FindAll(item =>
                                item != null &&
                                item.GetComponent<String>() != null &&
                                !item.GetComponent<String>().isCut)
                            .ConvertAll(item => item.GetComponent<String>());

                        if (availableStrings.Count == 0)
                            continue;

                        s = availableStrings[
                            UnityEngine.Random.Range(0, availableStrings.Count)
                        ];
                    }

                    s.CreateNote();
                }

                lastIndex = index;
            }

            // The current track file has finished.
            if (lastIndex == track.Count - 1)
            {
                StartNextTrack();
            }
        }

        private int GetIndex()
        {
            float time = elapsedTime - delay;

            int ret = -1;

            while (time >= step)
            {
                time -= step;
                ++ret;
            }

            return Mathf.Min(ret, track.Count - 1);
        }

        private void StartNextTrack()
        {
            if (trackFiles.Count == 0)
                return;

            int nextIndex = currentTrackIndex + 1;

            // Loop back to the first track file.
            if (nextIndex >= trackFiles.Count)
                nextIndex = 0;

            LoadTrack(nextIndex);
        }

        internal void PlayMusic()
        {
            if (musicClip == null)
            {
                Debug.LogError("NoteScroller: Missing music clip!", this);
                return;
            }

            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}