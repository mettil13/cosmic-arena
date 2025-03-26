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
    //public int playerNumber;
    //public PlayerManager manager;


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

    protected Vector2 lastValidDirection = Vector2.zero;
    public Vector2 LastValidDirection {
        get => lastValidDirection;
        set => lastValidDirection = value.normalized;
    }


    //public characterinputadapter()
    //{
    //    inputs = new inputsystem_actions();
    //    inputs.enable();
    //    inputs.player.enable();

    //    inputs.player.direction.started += ondirectioninput;
    //    inputs.player.direction.performed += ondirectioninput;
    //    inputs.player.direction.canceled += ondirectioninput;

    //    inputs.player.thrust.started += onthrustinput;
    //    inputs.player.thrust.performed += onthrustinput;
    //    inputs.player.thrust.canceled += onthrustinput;

    //    inputs.player.brake.started += onbrakeinput;
    //    inputs.player.brake.performed += onbrakeinput;
    //    inputs.player.brake.canceled += onbrakeinput;

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
        if(direction != Vector2.zero) {
            lastValidDirection = direction.normalized;
        }
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
