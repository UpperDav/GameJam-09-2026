using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class HeadString : MonoBehaviour
{
    [Header("Input")]
    public ControllerInput controllerInput;

    [Header("Pulse")]
    public float pulseForce = 6f;
    public Vector2 pulseDirection = Vector2.up;

    public System.Action OnPulse;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (controllerInput.IsPressed("Head"))
            Pulse();
    }

    void Pulse()
    {
        rb.AddForce(pulseDirection.normalized * pulseForce, ForceMode2D.Impulse);
        OnPulse?.Invoke();
    }

    public void RegisterHit() { }
    public void RegisterMiss() { }
}