using System;
using UnityEngine;

namespace PuppetHero
{

    public class Note : MonoBehaviour
    {

        static internal float speed = 0f;
        static private Vector3 dir = Vector3.down;
        static private Gauge? gauge;

        private bool canBeHit;

        [SerializeField] private ControllerInput? controller;
        [SerializeField] private string controlName = "";
        [SerializeField] private int value = 0;

        internal String? parent;

        internal bool setToDie = false;

        private void Awake()
        {
            getGauge();
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
                Debug.Log($"Calling Gauge.Increase() from instance #{GetHashCode()}");
                (gauge ?? getGauge()).Increase(value);
                parent!.KillNote(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            canBeHit = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (setToDie)
                return;

            Debug.Log($"Calling Gauge.Decrease() from instance #{GetHashCode()}");
            (gauge ?? getGauge()).Decrease(value);
            parent!.KillNote(gameObject);
        }

        static private Gauge getGauge()
        {
            if (gauge == null)
                gauge = GameObject.Find("Gauge").GetComponent<Gauge>();

            return gauge;
        }
    }
}
