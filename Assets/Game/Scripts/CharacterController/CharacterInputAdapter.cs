using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInputAdapter
{
    private Vector2 direction;
    private float brake;
    private float thrust;
    InputSystem_Actions inputs;


    public Vector2 Direction 
    { 
        get => direction; 
        set => direction = value; 
    }
    public float Brake
    {
        get => brake;
        set => brake = value;
    }
    public float Thrust
    {
        get => thrust;
        set => thrust = value;
    }

    public CharacterInputAdapter()
    {
        inputs = new InputSystem_Actions();
        inputs.Enable();
        inputs.Player.Enable();

        inputs.Player.Direction.started += OnDirectionInput;
        inputs.Player.Direction.performed += OnDirectionInput;
        inputs.Player.Direction.canceled += OnDirectionInput;

        inputs.Player.Thrust.started += OnThrustInput;
        inputs.Player.Thrust.performed += OnThrustInput;
        inputs.Player.Thrust.canceled += OnThrustInput;

        inputs.Player.Brake.started += OnBrakeInput;
        inputs.Player.Brake.performed += OnBrakeInput;
        inputs.Player.Brake.canceled += OnBrakeInput;

    }

    private void OnBrakeInput(InputAction.CallbackContext context)
    {
        brake = context.ReadValue<bool>() ? 1f : 0f;
    }

    private void OnThrustInput(InputAction.CallbackContext context)
    {


        thrust = context.ReadValueAsButton() ? 1f : 0f;
    }

    private void OnDirectionInput(InputAction.CallbackContext context)
    {
        direction = context.action.ReadValue<Vector2>();
    }


}
