using System.Collections.Generic;
using UnityEngine;

namespace PuppetHero
{

    public class NoteScroller : MonoBehaviour
    {
        [SerializeField] private List<GameObject> strings = new();

        private float elapsedTime;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            elapsedTime = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            elapsedTime += Time.deltaTime;

            if (timeForNewNotes())
            {
                // TODO: Create new notes
            }
        }

        private bool timeForNewNotes()
        {
            // TODO: Check if it's time for new notes to appear

            return false;
        }
    }
}
