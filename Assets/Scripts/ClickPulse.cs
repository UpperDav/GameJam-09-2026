using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ClickPulse : MonoBehaviour
{
    [Header("Pulse feel")]
    public float pulseDuration = 0.15f;
    public float scaleMultiplier = 1.3f;
    public Color flashColor = Color.white;

    private SpriteRenderer sr;
    private Vector3 originalScale;
    private Color originalColor;
    private Coroutine activePulse;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        originalColor = sr.color;
    }

    public void Pulse()
    {
        if (activePulse != null)
            StopCoroutine(activePulse);

        activePulse = StartCoroutine(PulseRoutine());
    }

    private IEnumerator PulseRoutine()
    {
        float elapsed = 0f;

        while (elapsed < pulseDuration)
        {
            // 0 -> 1 -> 0 over the duration, so it grows then settles
            // back instead of snapping.
            float t = elapsed / pulseDuration;
            float wave = Mathf.Sin(t * Mathf.PI);

            transform.localScale = Vector3.Lerp(originalScale, originalScale * scaleMultiplier, wave);
            sr.color = Color.Lerp(originalColor, flashColor, wave);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        sr.color = originalColor;
        activePulse = null;
    }
}