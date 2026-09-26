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

    [Header("Input")]
    public ControllerInput controllerInput;

    [Header("Death Flail")]
    public float flailDuration = 3.5f;
    public float flailForceMin = 5f;
    public float flailForceMax = 30f;
    public float flailForceInterval = 0.1f;

    public System.Action OnAllStringBroken;

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
        int remaining = limbStrings.Count - brokenCount;

        if (remaining <= 0)
        {
            OnAllStringBroken?.Invoke();

            // Disable input
            if (controllerInput != null)
                controllerInput.IsEnabled = false;

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
        headSpriteRenderer.sprite = deadHeadSprite;
    }
}
