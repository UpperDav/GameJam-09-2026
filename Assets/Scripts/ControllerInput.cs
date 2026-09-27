using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(fileName = "ControllerInput", menuName = "Scriptable Objects/ControllerInput")]
public class ControllerInput : ScriptableObject
{

    [Serializable] class Input
    {
        public string? name;
        public Key key;
        public GamepadButton gamepad;
    };

    static private ControllerInput? instance;

    private Keyboard? keyboard;
    private Gamepad? gamepad;
    private Mouse? mouse;

    [SerializeField] private List<Input> inputs = new();

    public Vector2 leftStick => gamepad?.leftStick.value ?? new Vector2(0, 0);
    public Vector2 rightStick => gamepad?.rightStick.value ?? new Vector2(0, 0);

    public float leftTrigger => gamepad?.leftTrigger.value ?? 0f;
    public float rightTrigger => gamepad?.rightTrigger.value ?? 0f;

    public bool IsEnabled { get; set; } = true;

    private void OnEnable()
    {
        InitializeInputDevices();
    }

    private void InitializeInputDevices()
    {
        keyboard = Keyboard.current;
        gamepad = Gamepad.current;
        mouse = Mouse.current;

        // TODO: Read config file for input settings, maybe?
    }

    private void EnsureInitialized()
    {
        if (keyboard == null || gamepad == null || mouse == null)
        {
            InitializeInputDevices();
        }
    }

    public bool IsPressed(string name)
    {
        EnsureInitialized();
        Input input = inputs.Find(x => x.name == name);
        if (input == null)
            return false;

        bool k = (keyboard?[input.key].wasPressedThisFrame ?? false);
        bool g = (gamepad?[input.gamepad].wasPressedThisFrame ?? false);
        bool ret = k || g;
        return ret;
    }

    public bool IsHeld(string name)
    {
        EnsureInitialized();
        Input input = inputs.Find(x => x.name == name);
        if (input == null)
            return false;

        return (keyboard?[input.key].isPressed ?? false) || (gamepad?[input.gamepad].isPressed ?? false);
    }
    public bool IsReleased(string name)
    {
        EnsureInitialized();
        Input input = inputs.Find(x => x.name == name);
        if (input == null)
            return false;

        return (keyboard?[input.key].wasReleasedThisFrame ?? false) || (gamepad?[input.gamepad].wasReleasedThisFrame ?? false);
    }

    public ButtonControl this[int b] => mouse == null ? new ButtonControl() : b switch
    {
        0 => mouse.leftButton,
        1 => mouse.rightButton,
        2 => mouse.middleButton,
        3 => mouse.backButton,
        4 => mouse.forwardButton,
        _ => throw new ArgumentOutOfRangeException("button", b, "Unsupported Mouse button ID")
    };

    public bool GetMouse(int button)
    {
        EnsureInitialized();
        ButtonControl input = this[button];
        return input.isPressed;
    }

    public Vector2 mousePos => mouse?.position.value ?? new Vector2(0, 0);

    public float mouseScroll => mouse?.scroll.value.y ?? 0f;
}
