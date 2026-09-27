using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TargetJoint2D))]
public class LimbString : MonoBehaviour
{
    [Header("String target")]
    [SerializeField] private Transform? stringAnchor;

    [Header("Pull feel")]
    [SerializeField] private float pulledFrequency = 8f;
    [SerializeField] private float pulledDampingRatio = 0.7f;
    [SerializeField] private float pulledMaxForce = 1000f;

    [Header("Breaking")]
    public bool isBroken { get; private set; } = false;
    public bool IsPulled { get; private set; }

    public Action? onBreak;

    private TargetJoint2D? targetJoint;
    private bool hasBeenBroken = false;

    [Header("Input")]
    [SerializeField] private ControllerInput? controllerInput;
    [SerializeField] private string actionName = "Blue";

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

        if (targetJoint!.enabled && stringAnchor != null)
            targetJoint.target = stringAnchor.position;
    }

    void Pull()
    {
        targetJoint!.enabled = true;
        IsPulled = true;
    }

    void Release()
    {
        targetJoint!.enabled = false;
        IsPulled = false;
    }

    public void RegisterOnBreak(Action callback)
    {
        if (onBreak == null)
            onBreak = new(callback);
        else
            onBreak += callback;
    }

    public void RemoveOnBreak(Action callback)
    {
        if (onBreak == null)
            return;

        onBreak -= callback;
    }

    public void Break()
    {
        if (hasBeenBroken)
            return;

        isBroken = true;
        hasBeenBroken = true;
        targetJoint!.enabled = false;
        targetJoint!.maxForce = 0f;

        onBreak?.Invoke();
    }
}