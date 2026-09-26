using UnityEngine;

namespace PuppetHero
{

    public class Gauge : MonoBehaviour
    {

        static private readonly int InitValue = 100;

        private int value = InitValue;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Decrease(int v)
        {
            int prevValue = value;
            value = Mathf.Max(0, value - v);

            if (value == 0)
            {
                // TODO: Cut the last string => Game Over
            }
            else if (prevValue >= 25 && value < 25)
            {
                // TODO: Cut the third string
            }
            else if (prevValue >= 50 && value < 50)
            {
                // TODO: Cut the second string
            }
            else if (prevValue >= 75 && value < 75)
            {
                // TODO: Cut the first string
            }
        }

        public void Increase(int v)
        {
            int prevValue = value;
            value = Mathf.Min(100, value + v);

            if (prevValue < 75 && value >= 75)
            {
                // TODO: Fix the first string
            }
            else if (prevValue < 50 && value >= 50)
            {
                // TODO: Fix the second string
            }
            else if (prevValue < 25 && value >= 25)
            {
                // TODO: Fix the third string
            }
        }
    }
}
