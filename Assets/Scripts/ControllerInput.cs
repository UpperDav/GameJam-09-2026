using System;
using System.Collections;
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

    private Keyboard keyboard;
    private Gamepad gamepad;
    private Mouse mouse;

    [SerializeField] private List<Input> inputs = new();

    static public ControllerInput Instance()
    {
        if (instance == null)
            instance = new();

        return instance;
    }

    public Vector2 leftStick => gamepad.leftStick.value;
    public Vector2 rightStick => gamepad.rightStick.value;

    public float leftTrigger => gamepad.leftTrigger.value;
    public float rightTrigger => gamepad.rightTrigger.value;

    private void Awake()
    {
        keyboard = Keyboard.current;
        gamepad = Gamepad.current;
        mouse = Mouse.current;

        // TODO: Read config file for input settings
    }

    public bool GetInput(string name)
    {
        Input input = inputs.Find(x => x.name == name);
        if (input == null)
            return false;

        return keyboard[input.key].isPressed || gamepad[input.gamepad].isPressed;
    }

    private ButtonControl this[int b] => b switch
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
        ButtonControl input = this[button];
        return input.isPressed;
    }

    public Vector2 mousePos => mouse.position.value;

    public float mouseScroll => mouse.scroll.value.y;

    public bool this[string name] => GetInput(name);
}
