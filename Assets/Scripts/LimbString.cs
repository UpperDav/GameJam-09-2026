using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TargetJoint2D))]
public class LimbString : MonoBehaviour
{
    [Header("String target")]
    public Transform stringAnchor;

    [Header("Pull feel")]
    public float pulledFrequency = 8f;
    public float pulledDampingRatio = 0.7f;
    public float pulledMaxForce = 1000f;

    [Header("Breaking")]
    public bool isBroken = false;
    public bool IsPulled { get; private set; }

    public System.Action OnBreak;

    private TargetJoint2D targetJoint;
    private bool hasBeenBroken = false;

    [Header("Input")]
    public ControllerInput controllerInput;
    public string actionName = "Blue";

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
        if (isBroken && !hasBeenBroken)
        {
            Break();
            hasBeenBroken = true;
            return;
        }

        if (isBroken)
            return;

        if (controllerInput == null || !controllerInput.IsEnabled)
            return;

        if (controllerInput.IsPressed(actionName))
        {
            Pull();
        }

        if (controllerInput.IsReleased(actionName))
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
        targetJoint.enabled = false;
        targetJoint.maxForce = 0f;

        OnBreak?.Invoke();
    }
}