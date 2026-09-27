using Unity.AppUI.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
namespace PuppetHero
{

    public class Gauge : MonoBehaviour
    {

        [SerializeField] private int InitValue = 100;
        public bool PublicHappy { get; private set; } = false;

        [SerializeField] private int value;
        [SerializeField] private int maxValue;

        private int cumlativeValue = 0;
        [SerializeField] private Slider? gaugeSlider;
        [SerializeField] private StringManager? stringManager;

        private void Awake()
        {
            gaugeSlider = GetComponent<Slider>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            value = InitValue;
            maxValue = InitValue;
            gaugeSlider!.maxValue = InitValue;
            gaugeSlider!.value = 0;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Decrease(int v)
        {
            int prevValue = value;
            value = Mathf.Max(0, value - v);
            gaugeSlider!.value = InitValue - value;
            cumlativeValue = 0;
            PublicHappy = false;

            if (value == 0)
            {
                CutAString();
                
            }
            else if (prevValue > InitValue / 4f && value <= InitValue / 4f)
            {
                maxValue = (int)(InitValue / 4f);
                
                CutAString();

            }
            else if (prevValue > InitValue / 2 && value <= InitValue / 2)
            {
                maxValue = (int)(InitValue / 2f);

                CutAString();

            }
            else if (prevValue > 3 * InitValue / 4 && value <= 3 * InitValue / 4)
            {
                maxValue = (int)(3f * InitValue / 4f);

                CutAString();
            }
        }

        public void Increase(int v)
        {
            value = Mathf.Min(maxValue, value + v);
            gaugeSlider!.value = InitValue - value;
            cumlativeValue++;
            Debug.Log("Cumulative Value: " + cumlativeValue);

            if (cumlativeValue >= 10)
                PublicHappy = true;
        }

        private void CutAString()
        {
            if (stringManager != null)
                stringManager.BreakRandomString();
        }
    }
}