using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuppetHero
{

    [RequireComponent(typeof(AudioSource))]
    public class StringManager : MonoBehaviour
    {
        [Header("Limb strings")]
        public List<LimbString> limbStrings = new();
        private List<LimbString> remainingStrings = new();

        [Header("Head")]
        [SerializeField] private SpriteRenderer? headSpriteRenderer;
        [SerializeField] private Sprite? deadHeadSprite;
        [SerializeField] private Rigidbody2D? bodyRigidbody;

        [Header("Death Flail")]
        [SerializeField] private float flailDuration = 3.5f;
        [SerializeField] private float flailForceMin = 5f;
        [SerializeField] private float flailForceMax = 30f;
        [SerializeField] private float flailForceInterval = 0.1f;

        [Header("Input")]
        [SerializeField] private ControllerInput? controllerInput;

        [Header("Camera")]
        [SerializeField] private CameraShake? cameraShake;

        private System.Action? onAllStringBroken;
        private System.Action? onHanged;

        private int brokenCount;
        private bool allStringsBroken;

        // Audio

        private AudioSource? audioSource;

        // Audio Clips
        [SerializeField] private AudioClip? ropeSwinging;
        [SerializeField] private AudioClip? neckBreak;
        [SerializeField] private AudioClip? ropeBreak;
        [SerializeField] private AudioClip? choking;

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
                    s.RegisterOnBreak(HandleLimbBreak);
            }
        }

        void OnDisable()
        {
            foreach (var s in limbStrings)
            {
                if (s != null)
                    s.RemoveOnBreak(HandleLimbBreak);
            }
        }

        public void RegisterOnHanged(System.Action callback)
        {
            if (onHanged == null)
                onHanged = new(callback);
            else
                onHanged += callback;
        }

        public void RemoveOnHanged(System.Action callback)
        {
            if (onHanged == null)
                return;

            onHanged -= callback;
        }

        public void RegisterOnAllStringBroken(System.Action callback)
        {
            if (onAllStringBroken == null)
                onAllStringBroken = new(callback);
            else
                onAllStringBroken += callback;
        }

        public void RemoveOnAllStringBroken(System.Action callback)
        {
            if (onAllStringBroken == null)
                return;

            onAllStringBroken -= callback;
        }

        void HandleLimbBreak()
        {
            // SFX
            if (audioSource != null && ropeBreak != null)
                audioSource.PlayOneShot(ropeBreak);

            cameraShake?.ShakeDefault();

            brokenCount++;
            int remaining = limbStrings.Count - brokenCount;

            if (remaining <= 0 && !allStringsBroken)
            {
                allStringsBroken = true;
                if (controllerInput != null)
                    controllerInput.IsEnabled = false;

                onAllStringBroken?.Invoke();

                if (bodyRigidbody != null)
                {
                    if (audioSource != null)
                    {
                        if (ropeSwinging != null)
                            audioSource.PlayOneShot(ropeSwinging);
                        if (choking != null)
                            audioSource.PlayOneShot(choking);
                    }

                    StartCoroutine(FlailCoroutine());
                }
                else
                {
                    FinishHanging();
                }
            }
        }

        public bool BreakRandomString()
        {
            List<LimbString> availableStrings = limbStrings.FindAll(
                stringItem => stringItem != null && !stringItem.isBroken);

            if (availableStrings.Count == 0)
                return false;

            int randomIndex = Random.Range(0, availableStrings.Count);
            LimbString chosen = availableStrings[randomIndex];
            chosen.Break();

            //String.CutByColor(chosen.trackColor);

            return true;
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
                    bodyRigidbody?.AddForce(randomDir * randomForce, ForceMode2D.Impulse);

                    nextForceTime = Time.time + flailForceInterval;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
            if (controllerInput != null)
                controllerInput.IsEnabled = false;

            FinishHanging();
        }

        private void FinishHanging()
        {
            if (audioSource != null)
            {
                audioSource.Stop();
                if (neckBreak != null)
                    audioSource.PlayOneShot(neckBreak);
            }

            if (headSpriteRenderer != null)
                headSpriteRenderer.sprite = deadHeadSprite;

            Debug.Log("StringManager: Invoking OnHanged event - game over!");
            onHanged?.Invoke();
        }
    }
}
