using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
namespace PuppetHero
{

    public class Gauge : MonoBehaviour
    {

        static private readonly int InitValue = 100;
        public bool PublicHappy = false;
        
        private int value = InitValue;
       
        private int maxValue = InitValue;
        private int cumlativeValue = 0;
        public Slider gaugeSlider;

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
            gaugeSlider.value = value;
            int prevValue = value;
            value = Mathf.Max(0, value - v);
            cumlativeValue = 0;
            PublicHappy = false;
            if (value == 0)
            {
                CutAString();
            }
            else if (prevValue >= InitValue / 4 && value < InitValue / 4)
            {
                maxValue = InitValue / 4;

                CutAString();

            }
            else if (prevValue >= InitValue / 2 && value < InitValue / 2)
            {
                maxValue = InitValue / 2;

                CutAString();

            }
            else if (prevValue >= 3 * InitValue / 4 && value < 3 * InitValue / 4)
            {
                maxValue = 3 * InitValue / 4;

                CutAString();
            }
        }

        public void Increase(int v)
        {
            gaugeSlider.value = value;
            int prevValue = value;
            value = Mathf.Min(maxValue, value + v);
            cumlativeValue ++;
        }
        public void GoodPerformance()
        {
            if (cumlativeValue >= 10)
            {
                PublicHappy = true;
            } // Verifie si le public va pouvoir s'afficher au bout de 10 touches reussit


        }

        private void CutAString()
        {
            // Selecting the random string to cut

            // Cutting the string from the character

            // Cuttin the string for the gameplay
        }
    }
}
