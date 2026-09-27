using System;
using UnityEngine;

namespace PuppetHero
{

    public class Note : MonoBehaviour
    {

        static internal float speed = 0f;
        static private Vector3 dir = Vector3.down;
        static private Gauge? gauge;
        static private NoteScroller? noteScroller;
        static private CameraShake? cameraShake;

        [Header("Miss shake")]
        [SerializeField] private float missShakeDuration = 0.1f;
        [SerializeField] private float missShakeMagnitude = 0.1f;

        private bool canBeHit;

        [SerializeField] private ControllerInput? controller;
        [SerializeField] private string controlName = "";
        [SerializeField] private int value = 0;

        internal String? parent;

        internal bool setToDie = false;

        private void Awake()
        {
            getGauge();
            getNoteScroller();
            getCameraShake();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            canBeHit = false;
        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<Transform>().position += speed * Time.deltaTime * dir;

            if (canBeHit && controller!.IsPressed(controlName))
            {
                (gauge ?? getGauge()).Increase(value);
                parent!.NoteLeftZone();
                parent!.KillNote(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            canBeHit = true;
            parent!.NoteEnteredZone();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (setToDie)
                return;

            (gauge ?? getGauge()).Decrease(value);
            (noteScroller ?? getNoteScroller())?.PlayMissSound();
            (cameraShake ?? getCameraShake())?.Shake(missShakeDuration, missShakeMagnitude);
            parent!.NoteLeftZone();
            parent!.KillNote(gameObject);
        }

        static private Gauge getGauge()
        {
            if (gauge == null)
                gauge = GameObject.Find("Gauge").GetComponent<Gauge>();

            return gauge;
        }

        static private NoteScroller? getNoteScroller()
        {
            if (noteScroller == null)
                noteScroller = UnityEngine.Object.FindFirstObjectByType<NoteScroller>();

            return noteScroller;
        }

        static private CameraShake? getCameraShake()
        {
            if (cameraShake == null)
                cameraShake = UnityEngine.Object.FindFirstObjectByType<CameraShake>();

            return cameraShake;
        }
    }
}