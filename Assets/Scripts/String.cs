using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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

        [Header("Note Prefabs")]
        [SerializeField] private GameObject? bNote;
        [SerializeField] private GameObject? gNote;
        [SerializeField] private GameObject? pNote;
        [SerializeField] private GameObject? rNote;
        [SerializeField] private GameObject? yNote;

        private List<GameObject> notes = new();

        [SerializeField] private StringColor color;
        [SerializeField] private Vector3 spawnPoint;

        [Header("Empty-click detection")]
        [SerializeField] private ControllerInput? controllerInput;
        [SerializeField] private string actionName = "";

        [Header("Click visual feedback")]
        [SerializeField] private ClickPulse? clickPulse;

        [Header("Gauge")]
        [SerializeField] private Gauge? gauge;

        private int activeNoteCount = 0;

        static private NoteScroller? noteScroller;

        static private List<String> instances = new();

        public bool isCut { get; private set; }

        private void Awake()
        {
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

        void Start()
        {
            isCut = false;
            FindFirstObjectByType<StringManager>().RegisterOnAllStringBroken(ClearAllNotes);
        }

        void Update()
        {
            if (isCut || controllerInput == null)
                return;

            if (controllerInput.IsPressed(actionName))
            {
                clickPulse?.Pulse();

                if (activeNoteCount <= 0)
                {
                    Debug.Log("Player clicked on empty string track: " + colorName);
                    gauge?.Decrease(gauge.emptyNoteDecrease);
                    (noteScroller ?? getNoteScroller())?.PlayEmptyClickSound();
                }
            }
        }

        public void NoteEnteredZone()
        {
            activeNoteCount++;
        }

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
                throw new MissingReferenceException(
                    $"Note of color {colorName} should not be null"
                );

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

            transform.GetChild(0).GetComponent<SpriteRenderer>().color =
                new Color(0.2f, 0.2f, 0.2f);

            GetComponent<SpriteRenderer>().color =
                new Color(0.2f, 0.2f, 0.2f);

            ClearAllNotes();
        }

        public void ClearAllNotes()
        {
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