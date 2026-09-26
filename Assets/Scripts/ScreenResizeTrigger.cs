using System;
using UnityEngine;

namespace PuppetHero
{

    public class ScreenResizeTrigger : MonoBehaviour
    {

        public static event Action? OnScreenResized;

        private void OnRectTransformDimensionsChange()
        {
            OnScreenResized?.Invoke();
        }
    }
}
