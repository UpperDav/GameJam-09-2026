using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TargetJoint2D))]
public class LimbString : MonoBehaviour
{
    [Header("Input")]
    public Key key = Key.A;

    [Header("String target")]
    public Transform stringAnchor;

    [Header("Pull feel")]
    public float pulledFrequency = 8f;
    public float pulledDampingRatio = 0.7f;
    public float pulledMaxForce = 1000f;

    [Header("Breaking")]
    public int maxMisses = 3;
    public bool IsBroken { get; private set; }
    public bool IsPulled { get; private set; }

    public System.Action OnBreak;

    private TargetJoint2D targetJoint;
    private Rigidbody2D rb;
    private int misses;

    [SerializeField] private ControllerInput controllerInput;

    void Awake()
    {
        targetJoint = GetComponent<TargetJoint2D>();
        rb = GetComponent<Rigidbody2D>();

        targetJoint.autoConfigureTarget = false;
        targetJoint.frequency = pulledFrequency;
        targetJoint.dampingRatio = pulledDampingRatio;
        targetJoint.maxForce = pulledMaxForce;
        targetJoint.enabled = false;
    }

    void Update()
    {
        if (IsBroken)
            return;

        var keyControl = Keyboard.current[key];

        if (keyControl.wasPressedThisFrame)
            Pull();

        if (keyControl.wasReleasedThisFrame)
            Release();

        if (targetJoint.enabled && stringAnchor != null)
            targetJoint.target = stringAnchor.position;
    }

    void Pull()
    {
        targetJoint.enabled = true;
        IsPulled = true;
    }

    void Release()
    {
        targetJoint.enabled = false;
        IsPulled = false;
    }

    public void RegisterHit()
    {
       
    }

    void Break()
    {
        IsBroken = true;
        targetJoint.enabled = false;
        targetJoint.maxForce = 0f;

        OnBreak?.Invoke();
    }
}