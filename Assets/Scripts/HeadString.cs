using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HeadString : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private ControllerInput? controllerInput;

    [Header("Pulse")]
    [SerializeField] private float pulseForce = 6f;
    [SerializeField] private Vector2 pulseDirection = Vector2.up;

    private Rigidbody2D? rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (controllerInput == null || !controllerInput.IsEnabled)
            return;

        if (controllerInput.IsPressed("Head"))
            Pulse();
    }

    void Pulse()
    {
        rb?.AddForce(pulseDirection.normalized * pulseForce, ForceMode2D.Impulse);
    }

    public void RegisterHit() { }
    public void RegisterMiss() { }
}