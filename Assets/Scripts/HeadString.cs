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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
