using System;
using UnityEngine;

public class ScreenResizeTrigger : MonoBehaviour
{

    public static event Action? OnScreenResized;

    private void OnRectTransformDimensionsChange()
    {
        OnScreenResized?.Invoke();
    }
}
