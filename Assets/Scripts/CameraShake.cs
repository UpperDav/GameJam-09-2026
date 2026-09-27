using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PuppetHero
{

    public class CameraShake : MonoBehaviour
    {
        public float duration = 0.15f;
        public float magniteude = 0.2f;

        private Vector3 originalLocalPos;
        private Coroutine? activateShake;

        private void Awake()
        {
            originalLocalPos = transform.localPosition;
        }

        public void ShakeDefault()
        {
            Shake(duration, magniteude);
        }

        public void Shake(float duration, float magnitude)
        {
            if (activateShake != null)
            {
                StopCoroutine(activateShake);
                transform.localPosition = originalLocalPos;
            }

            activateShake = StartCoroutine(ShakeRoutine(duration, magnitude));
        }

        private IEnumerator ShakeRoutine(float duration, float magnitude)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float damper = 1f - (elapsed / duration);
                Vector2 offset = Random.insideUnitCircle * magnitude * damper;
                transform.localPosition = originalLocalPos + new Vector3(offset.x, offset.y, 0f);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originalLocalPos;
            activateShake = null;
        }
    }
}
