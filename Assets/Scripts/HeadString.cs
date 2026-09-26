using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class HeadString : MonoBehaviour
{
    public Key key = Key.Space;

    [Header("Pulse")]
    public float pulseForce = 6f;
    public Vector2 pulseDirection = Vector2.up;

    public System.Action OnPulse;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var keyControl = Keyboard.current[key];

        if (keyControl.wasPressedThisFrame)
            Pulse();
    }

    private void Pulse()
    {
        rb.AddForce(pulseDirection.normalized * pulseForce, ForceMode2D.Impulse);
        OnPulse?.Invoke();
        Debug.Log("Pulse");
    }

    public void RegisterHit() { }

    public void RegisterMiss() { }
}
