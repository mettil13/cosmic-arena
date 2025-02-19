using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInputAdapter : MonoBehaviour
{
    private Vector2 direction;
    private float brake;
    private float thrust;
    private float attack;
    private float specialAbility;

    //InputSystem_Actions inputs;


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
    public float Attack 
    { 
        get => attack; 
        set => attack = value; 
    }
    public float SpecialAbility 
    { 
        get => specialAbility; 
        set => specialAbility = value; 
    }

    //public CharacterInputAdapter()
    //{
    //    inputs = new InputSystem_Actions();
    //    inputs.Enable();
    //    inputs.Player.Enable();

    //    inputs.Player.Direction.started += OnDirectionInput;
    //    inputs.Player.Direction.performed += OnDirectionInput;
    //    inputs.Player.Direction.canceled += OnDirectionInput;

    //    inputs.Player.Thrust.started += OnThrustInput;
    //    inputs.Player.Thrust.performed += OnThrustInput;
    //    inputs.Player.Thrust.canceled += OnThrustInput;

    //    inputs.Player.Brake.started += OnBrakeInput;
    //    inputs.Player.Brake.performed += OnBrakeInput;
    //    inputs.Player.Brake.canceled += OnBrakeInput;

    //}

    public void OnBrakeInput(InputAction.CallbackContext context)
    {
        brake = context.ReadValue<bool>() ? 1f : 0f;
    }

    public void OnThrustInput(InputAction.CallbackContext context)
    {
        thrust = context.ReadValueAsButton() ? 1f : 0f;
    }

    public void OnDirectionInput(InputAction.CallbackContext context)
    {
        direction = context.action.ReadValue<Vector2>();
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        attack = context.action.ReadValue<float>();
    }
    public void OnSpecialAbility(InputAction.CallbackContext context)
    {
        attack = context.action.ReadValue<float>();
    }


}
