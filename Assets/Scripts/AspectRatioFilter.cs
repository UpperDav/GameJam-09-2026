using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AdaptiveCamera : MonoBehaviour
{
    [SerializeField] private float referenceAspect = 16f / 9f;
    [SerializeField] private float referenceSize = 5f;

    private Camera? cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect < referenceAspect)
        {
            // Narrower screen: zoom out so the gameplay area isn't cropped.
            cam!.orthographicSize =
                referenceSize * (referenceAspect / currentAspect);
        }
        else
        {
            // 16:9 or wider: use the original size.
            cam!.orthographicSize = referenceSize;
        }
    }
}