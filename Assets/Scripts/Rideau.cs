using Unity.VisualScripting;
using UnityEngine;

public class Rideau : MonoBehaviour
{

    [SerializeField] private GameObject left, right;

    private void UpdateFrame()
    {
        Camera mainCam = Camera.main;
        if (mainCam == null || left == null || right == null)
            return;

        float heightUnits = mainCam.orthographicSize * 2f;
        float widthUnits = heightUnits * mainCam.aspect;

        float leftWidth = left.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float leftHeight = left.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        float leftRatio = heightUnits / leftHeight;
        leftWidth *= leftRatio;

        left.GetComponent<Transform>().localScale = new(leftRatio, leftRatio, 1f);

        float rightWidth = right.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float rightHeight = right.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        float rightRatio = heightUnits / rightHeight;
        rightWidth *= rightRatio;

        right.GetComponent<Transform>().localScale = new(rightRatio, rightRatio, 1f); ;

        float leftEdge = -(widthUnits / 2f) + (leftWidth / 2f);
        float rightEdge = (widthUnits / 2f) - (rightWidth / 2f);

        left.GetComponent<Transform>().position = new(leftEdge, 0f, 0f);
        right.GetComponent<Transform>().position = new(rightEdge, 0f, 0f);
    }

    private void Awake()
    {
        UpdateFrame();
    }

    private void OnEnable()
    {
        ScreenResizeTrigger.OnScreenResized += UpdateFrame;
    }

    private void OnDisable()
    {
        ScreenResizeTrigger.OnScreenResized -= UpdateFrame;
    }
}
