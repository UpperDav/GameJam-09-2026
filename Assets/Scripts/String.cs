using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace PuppetHero
{

    public class String : MonoBehaviour
    {
        public enum StringColor
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

        [SerializeField] private StringColor color;
        [SerializeField] private Vector3 spawnPoint;

        static private List<String> instances = new();

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

            instances.Add(this);
        }

        private void OnDestroy()
        {
            instances.Remove(this);
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

        private void CreateNodeImpl(StringColor c)
        {
            GameObject? note = c switch
            {
                StringColor.blue => Instantiate(bNote),
                StringColor.green => Instantiate(gNote),
                StringColor.purple => Instantiate(pNote),
                StringColor.red => Instantiate(rNote),
                StringColor.yellow => Instantiate(yNote),
                _ => throw new UnexpectedEnumValueException<StringColor>(color)
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

        public void CreateNote(StringColor c)
        {
            CreateNodeImpl(c);
        }

        public void KillNote(GameObject note)
        {
            notes.Remove(note);
            Destroy(note);
        }

        static public void CutRandom()
        {
            List<String> availableStrings = instances.FindAll(
                inst => inst != null && !inst.isCut);

            if (availableStrings.Count == 0)
                return;

            availableStrings[Random.Range(0, availableStrings.Count)].Cut();
        }

        public void Cut()
        {
            isCut = true;

            GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }

        public void Repair()
        {
            // TODO: Implement repair animation

            isCut = false;
        }

        private string colorName => color switch
        {
            StringColor.blue => "blue",
            StringColor.green => "green",
            StringColor.purple => "purple",
            StringColor.red => "red",
            StringColor.yellow => "yellow",
            _ => throw new UnexpectedEnumValueException<StringColor>(color)
        };
    }
}
