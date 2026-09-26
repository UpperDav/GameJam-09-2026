using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StringManager : MonoBehaviour
{
    [Header("Limb strings")]
    public List<LimbString> limbStrings = new List<LimbString>();

    // Fires with the number of limb strings still intact
    public System.Action<int> OnPhaseChanged;

    // Fires once, when all 4 limb strings have broken
    public System.Action OnHanged;

    private int brokenCount;

    void OnEnable()
    {
        foreach (var s in limbStrings)
            s.OnBreak += HandleLimbBreak;
    }

    void OnDisable()
    {
        foreach (var s in limbStrings)
            s.OnBreak -= HandleLimbBreak;
    }

    void HandleLimbBreak()
    {
        brokenCount++;
        int remaining = limbStrings.Count - brokenCount; // 3, 2, 1, 0
        OnPhaseChanged?.Invoke(remaining);

        if (brokenCount >= limbStrings.Count)
            OnHanged?.Invoke();
    }
}
