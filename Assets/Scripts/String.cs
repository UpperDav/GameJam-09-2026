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

        [Header("Empty-click detection")]
        [SerializeField] private ControllerInput? controllerInput;
        [SerializeField] private string actionName = "";

        [Header("Click visual feedback")]
        [SerializeField] private ClickPulse? clickPulse;

        private int activeNoteCount = 0;

        static private NoteScroller? noteScroller;

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

            if (instances.Exists(inst => inst != null && inst.color == color))
                Debug.LogWarning($"String: another instance already uses color {color} " +
                    $"({gameObject.name} is a duplicate) -- CutByColor will not be able to " +
                    "tell them apart.");

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
            FindFirstObjectByType<StringManager>().RegisterOnAllStringBroken(ClearAllNotes);
        }

        // Update is called once per frame
        void Update()
        {
            if (isCut || controllerInput == null)
                return;

            if (controllerInput.IsPressed(actionName))
            {
                clickPulse?.Pulse();

                if (activeNoteCount <= 0)
                    (noteScroller ?? getNoteScroller())?.PlayEmptyClickSound();
            }
        }

        // Called by Note.cs when a note enters this track's hit zone.
        public void NoteEnteredZone()
        {
            activeNoteCount++;
        }

        // Called by Note.cs when a note leaves this track's hit zone,
        // whether by a successful hit or by missing (exiting uncaught).
        public void NoteLeftZone()
        {
            activeNoteCount = Mathf.Max(0, activeNoteCount - 1);
        }

        static private NoteScroller? getNoteScroller()
        {
            if (noteScroller == null)
                noteScroller = UnityEngine.Object.FindFirstObjectByType<NoteScroller>();

            return noteScroller;
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
            note.GetComponent<Note>().setToDie = true;
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

        static public void CutByColor(StringColor color)
        {
            List<String> matches = instances.FindAll(
                inst => inst != null && inst.color == color);

            if (matches.Count == 0)
            {
                Debug.LogWarning($"String.CutByColor: no String instance found with color {color}. " +
                    "Check that a note-track GameObject actually has this color assigned.");
                return;
            }

            if (matches.Count > 1)
            {
                Debug.LogWarning($"String.CutByColor: {matches.Count} String instances share color {color}. " +
                    "Each color should be unique across your 5 note tracks -- fix the duplicate in the scene.");
            }

            String uncut = matches.Find(inst => !inst.isCut);
            if (uncut == null)
            {
                Debug.LogWarning($"String.CutByColor: the {color} track is already cut -- nothing to do. " +
                    "If this fires more than once per limb, two LimbStrings likely share the same trackColor.");
                return;
            }

            uncut.Cut();
        }

        public void Cut()
        {
            isCut = true;

            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f);
            GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f);

            ClearAllNotes();
        }

        public void ClearAllNotes()
        {
            // Iterate a copy since KillNote modifies the notes list while we loop.
            List<GameObject> notesToClear = new(notes);
            foreach (GameObject note in notesToClear)
            {
                if (note != null)
                    KillNote(note);
            }
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