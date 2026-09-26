using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace PuppetHero
{

    public class String : MonoBehaviour
    {
        public enum Color
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
                bNote = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/BlueNote.prefab", typeof(GameObject)) as GameObject;
            if (gNote == null)
                gNote = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GreenNote.prefab", typeof(GameObject)) as GameObject;
            if (pNote == null)
                pNote = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/PurpleNote.prefab", typeof(GameObject)) as GameObject;
            if (rNote == null)
                rNote = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/RedNote.prefab", typeof(GameObject)) as GameObject;
            if (yNote == null)
                yNote = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/YellowNote.prefab", typeof(GameObject)) as GameObject;
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

        private void CreateNodeImpl(Color c)
        {
            GameObject? note = c switch
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
            note.GetComponent<Transform>().parent = GetComponent<Transform>();
            note.GetComponent<Transform>().localPosition = spawnPoint;

            notes.Add(note);
        }

        public void CreateNote()
        {
            CreateNodeImpl(color);
        }

        public void CreateNote(Color c)
        {
            CreateNodeImpl(c);
        }

        public void KillNote(GameObject note)
        {
            notes.Remove(note);
            Destroy(note);
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
