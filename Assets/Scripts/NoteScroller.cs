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
        [SerializeField] private TextAsset? trackFile;
        [SerializeField] private float noteSpeed = 4f;

        private float elapsedTime;

        private List<List<String.StringColor>> track = new();
        private float step = 1f;
        private int lastIndex = -1;

        private String.StringColor GetColor(char c)
        {
            return c switch
            {
                'b' => String.StringColor.blue,
                'g' => String.StringColor.green,
                'p' => String.StringColor.purple,
                'r' => String.StringColor.red,
                'y' => String.StringColor.yellow,
                _ => throw new ArgumentException($"Cannot convert character '{c}' to a valid String.Color value")
            };
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            elapsedTime = 0f;

            string[] lines = trackFile!.text.Split('\n');
            int beat = int.Parse(lines[0]);
            step = 60f / beat;

            foreach (string line in lines.Skip(1))
            {
                List<String.StringColor> colors = new();
                foreach (char c in line)
                    colors.Add(GetColor(c));
                track.Add(colors);
            }

            Note.speed = noteSpeed;
        }

        // Update is called once per frame
        void Update()
        {
            elapsedTime += Time.deltaTime;

            int index = GetIndex();
            if (index != lastIndex)
            {
                List<String.StringColor> colors = track[index];
                foreach (String.StringColor color in track[index])
                {
                    String s = strings[(int)color].GetComponent<String>();
                    while (s.isCut)
                    {
                        s = strings[UnityEngine.Random.Range(0, strings.Count)].GetComponent<String>();
                    }

                    s.CreateNote(color);
                }

                lastIndex = index;
            }
            else if (lastIndex == track.Count - 1)
            {
                // TODO: Handle end of track
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
    }
}
