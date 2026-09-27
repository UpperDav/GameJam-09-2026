using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StringManager : MonoBehaviour
{
    [Header("Limb strings")]
    public List<LimbString> limbStrings = new List<LimbString>();
    private List<LimbString> remainingStrings = new List<LimbString>();

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

    [Header("Camera")]
    public CameraShake cameraShake;

    public System.Action OnAllStringBroken;
    public System.Action OnHanged;

    private int brokenCount;
    private bool allStringsBroken;

    // Audio

    private AudioSource audioSource;

    // Audio Clips
    public AudioClip ropeSwinging;
    public AudioClip neckBreak;
    public AudioClip ropeBreak;
    public AudioClip choking;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
        // SFX
        audioSource.PlayOneShot(ropeBreak);
        cameraShake.ShakeDefault();

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
                audioSource.PlayOneShot(ropeSwinging);
                audioSource.PlayOneShot(choking);
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
        audioSource.Stop();
        audioSource.PlayOneShot(neckBreak);
        headSpriteRenderer.sprite = deadHeadSprite;
        OnHanged?.Invoke();
    }
}
