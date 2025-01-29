using UnityEngine;
using StateMachine;
using CharacterLogic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;

public class CharacterManager : SerializedMonoBehaviour
{
    [OdinSerialize, NonSerialized]public StateMachine<Player_State, Player_Status> stateMachine;
    [SerializeField] public CharacterInputAdapter characterInputAdapter;

    public new Rigidbody rigidbody;

    private void Awake()
    {
        Dictionary<Player_State, IState> newStates = new Dictionary<Player_State, IState>();
        foreach (var statePair in stateMachine.states)
        {
            APlayerState newState = Instantiate((APlayerState)statePair.Value);
            newStates.Add(statePair.Key, newState);
            newState.Init(this);
        }
        stateMachine.states = newStates;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        stateMachine.Init();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Execute(Time.deltaTime);
    }

    private void LateUpdate()
    {
        stateMachine.LateExecute();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedExecute();
    }


}
