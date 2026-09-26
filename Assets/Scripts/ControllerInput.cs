using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public class ControllerInput : MonoBehaviour
{

    [Serializable] class Input
    {
        public string name;
        public Key key;
        public GamepadButton gamepad;
    };

    private Keyboard keyboard;
    private Gamepad gamepad;
    private Mouse mouse;

    [SerializeField] private List<Input> inputs;

    public Vector2 leftStick => gamepad.leftStick.value;
    public Vector2 rightStick => gamepad.rightStick.value;

    public float leftTrigger => gamepad.leftTrigger.value;
    public float rightTrigger => gamepad.rightTrigger.value;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyboard = Keyboard.current;
        gamepad = Gamepad.current;
        mouse = Mouse.current;

        // TODO: Read config file for input settings
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetInput(string name)
    {
        Input input = inputs.Find(x => x.name == name);
        if (input == null)
            return false;

        return keyboard[input.key].isPressed || gamepad[input.gamepad].isPressed;
    }

    public Vector2 mousePos => mouse.position.value;

    public bool this[string name] => GetInput(name);
}
