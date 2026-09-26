using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PuppetHero
{

    public class String : MonoBehaviour
    {
        enum Color
        {
            blue,
            green,
            purple,
            red,
            yellow
        };

        static private GameObject? bNote;
        static private GameObject? gNote;
        static private GameObject? pNote;
        static private GameObject? rNote;
        static private GameObject? yNote;

        private List<GameObject> notes = new();

        [SerializeField] private Color color;
        [SerializeField] private Vector3 spawnPoint;

        public bool isCut { get; private set; }

        private void Awake()
        {
            if (bNote == null)
                bNote = GameObject.Find("bNote");
            if (gNote == null)
                gNote = GameObject.Find("gNote");
            if (pNote == null)
                pNote = GameObject.Find("pNote");
            if (rNote == null)
                rNote = GameObject.Find("rNote");
            if (yNote == null)
                yNote = GameObject.Find("yNote");
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            isCut = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CreateNote()
        {
            GameObject? note = color switch
            {
                Color.blue => Instantiate(bNote),
                Color.green => Instantiate(gNote),
                Color.purple => Instantiate(pNote),
                Color.red => Instantiate(rNote),
                Color.yellow => Instantiate(yNote),
                _ => throw new UnexpectedEnumValueException<Color>(color)
            };

            if (note == null)
                throw new MissingReferenceException($"Note of color {colorName} should not be null");

            note.GetComponent<Note>().parent = this;
            note.GetComponent<Transform>().position = spawnPoint;

            notes.Add(note);
        }

        public void KillNote(GameObject note)
        {
            notes.Remove(note);
        }

        public void Cut()
        {
            isCut = true;

            // TODO: Implement cutting animation
        }

        public void Repair()
        {
            // TODO: Implement repair animation

            isCut = false;
        }

        private string colorName => color switch
        {
            Color.blue => "blue",
            Color.green => "green",
            Color.purple => "purple",
            Color.red => "red",
            Color.yellow => "yellow",
            _ => throw new UnexpectedEnumValueException<Color>(color)
        };
    }
}
