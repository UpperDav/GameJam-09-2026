using UnityEngine;
using UnityEngine.Rendering;

namespace PuppetHero
{

    public class Gauge : MonoBehaviour
    {

        static private readonly int InitValue = 100;
        public bool PublicHappy = false;
        private int value = InitValue;
        private int maxValue = InitValue;
        private int cumlativeValue = 0;

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
            cumlativeValue = 0;
            PublicHappy = false;
            if (value == 0)
            {
                // TODO: Cut the last string => Game Over
            }
            else if (prevValue >= 25 && value < 25)
            {
                maxValue = 25;

                // TODO: Cut the third string
            }
            else if (prevValue >= 50 && value < 50)
            {
                maxValue = 50;

                // TODO: Cut the second string
            }
            else if (prevValue >= 75 && value < 75)
            {
                maxValue = 75;

                // TODO: Cut the first string
            }
        }

        public void Increase(int v)
        {
            int prevValue = value;
            value = Mathf.Min(maxValue, value + v);
            cumlativeValue += v;
        }
        public void GoodPerformance()
        {
            if (cumlativeValue >= 10)
            {
                PublicHappy = true;
            } // Verifie si le public va pouvoir s'afficher au bout de 10 touches reussit
        }
    }
}
