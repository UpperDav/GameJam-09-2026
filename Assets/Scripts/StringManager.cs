using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StringManager : MonoBehaviour
{
    [Header("Limb strings")]
    public List<LimbString> limbStrings = new List<LimbString>();

    [Header("Head")]
    public SpriteRenderer headSpriteRenderer;
    public Sprite deadHeadSprite;
    public Rigidbody2D bodyRigidbody;

    [Header("Death Flail")]
    public float flailDuration = 3.5f;
    public float flailForceMin = 5f;
    public float flailForceMax = 30f;
    public float flailForceInterval = 0.1f;

    [Header("Input")]
    public ControllerInput controllerInput;

    public System.Action OnAllStringBroken;

    private int brokenCount;
    private bool allStringsBroken;

    void OnEnable()
    {
        brokenCount = 0;
        allStringsBroken = false;

        if (controllerInput != null)
            controllerInput.IsEnabled = true;

        foreach (var s in limbStrings)
        {
            if (s != null)
                s.OnBreak += HandleLimbBreak;
        }
    }

    void OnDisable()
    {
        foreach (var s in limbStrings)
        {
            if (s != null)
                s.OnBreak -= HandleLimbBreak;
        }
    }

    void HandleLimbBreak()
    {
        brokenCount++;
        int remaining = limbStrings.Count - brokenCount;

        if (remaining <= 0 && !allStringsBroken)
        {
            allStringsBroken = true;
            if (controllerInput != null)
                controllerInput.IsEnabled = false;

            OnAllStringBroken?.Invoke();

            if (bodyRigidbody != null)
            {
                StartCoroutine(FlailCoroutine());
            }
        }
    }

    IEnumerator FlailCoroutine()
    {
        float elapsed = 0f;
        float nextForceTime = 0f;

        while (elapsed < flailDuration)
        {
            if (Time.time >= nextForceTime)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                float randomForce = Random.Range(flailForceMin, flailForceMax);
                bodyRigidbody.AddForce(randomDir * randomForce, ForceMode2D.Impulse);

                nextForceTime = Time.time + flailForceInterval;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
        if (controllerInput != null)
            controllerInput.IsEnabled = false;
        headSpriteRenderer.sprite = deadHeadSprite;

    }
}
