using UnityEngine;
using UnityEngine.Rendering;

namespace PuppetHero
{

    public class Gauge : MonoBehaviour
    {

        [SerializeField] private int InitValue = 100;

        public bool PublicHappy { get; private set; }

        private int value;
        private int maxValue;
        private int cumlativeValue = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            value = InitValue;
            maxValue = InitValue;
            PublicHappy = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Decrease(int v)
        {
            int prevValue = value;
            value = Mathf.Max(0, value - v);
            cumlativeValue = 0;
            PublicHappy = false;
            if (value == 0)
            {
                // TODO: Cut the last string => Game Over
            }
            else if (prevValue >= InitValue / 4 && value < InitValue / 4)
            {
                maxValue = InitValue / 4;

                // TODO: Cut the third string
            }
            else if (prevValue >= InitValue / 2 && value < InitValue / 2)
            {
                maxValue = InitValue / 2;

                // TODO: Cut the second string
            }
            else if (prevValue >= 3 * InitValue / 4 && value < 3 * InitValue / 4)
            {
                maxValue = 3 * InitValue / 4;

                // TODO: Cut the first string
            }
        }

        public void Increase(int v)
        {
            int prevValue = value;
            value = Mathf.Min(maxValue, value + v);
            ++cumlativeValue;

            if (cumlativeValue >= 10)
            {
                PublicHappy = true;
            }
        }
    }
}
