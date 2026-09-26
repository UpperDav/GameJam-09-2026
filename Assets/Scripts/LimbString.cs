using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TargetJoint2D))]
public class LimbString : MonoBehaviour
{
    [Header("Input")]
    public KeyCode key = KeyCode.A;

    [Header("String target")]

    public Transform stringAnchor;

    [Header("Pull feel (TargetJoint2D)")]
    public float pulledFrequency = 8f;
    public float pulledDampingRatio = 0.7f;
    public float pulledMaxForce = 1000f;

    [Header("Breaking")]
    public int maxMisses = 3;
    public bool IsBroken { get; private set; }

    public System.Action OnBreak;
    public System.Action<bool> OnPull; // true = string pulled, false = released

    private TargetJoint2D targetJoint;

    void Awake()
    {
        targetJoint = GetComponent<TargetJoint2D>();

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

        if (Input.GetKeyDown(key))
            Pull();

        if (Input.GetKeyUp(key))
            Release();

        if (targetJoint.enabled && stringAnchor != null)
            targetJoint.target = stringAnchor.position;
    }

    void Pull()
    {
        targetJoint.enabled = true;
        OnPull?.Invoke(true);
    }

    void Release()
    {
        targetJoint.enabled = false;
        OnPull?.Invoke(false);
    }

    void Break()
    {
        IsBroken = true;
        targetJoint.enabled = false;
        targetJoint.maxForce = 0f;

        OnBreak?.Invoke();
    }
}