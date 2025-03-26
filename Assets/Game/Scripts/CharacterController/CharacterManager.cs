using UnityEngine;
using StateMachine;
using CharacterLogic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using physics;

public class CharacterManager : SerializedMonoBehaviour
{
    [OdinSerialize, NonSerialized] public StateMachine<Player_State, Player_Status> stateMachine;
    [SerializeField] public CharacterInputAdapter characterInputAdapter;
    [SerializeField] public Dictionary<GameObject, float> modifiers;
    [SerializeField] public CharacterMovementEstetic characterMovementAesthetic;
    [SerializeField] public Transform cursor;
    [SerializeField] public CharacterSO characterStats;

    public new Rigidbody rigidbody;

    public int playerNumber;
    public PlayerManager manager;


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
        cursor.eulerAngles = new Vector3(0, -Vector2.SignedAngle(Vector2.right, characterInputAdapter.LastValidDirection), 0);
        characterMovementAesthetic.transform.localPosition = Vector3.zero;
        characterMovementAesthetic.ApplyDirectionIfControlled(characterInputAdapter.LastValidDirection);

        stateMachine.Execute(Time.deltaTime);
        //characterPhysics.SetDirection(characterInputAdapter.Direction);
    }

    private void LateUpdate()
    {
        stateMachine.LateExecute();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedExecute();
    }

    internal void Die()
    {
        Ranking.Instance.AddToRanking((int)GameManager.Instance.gameTimer, characterStats.health , gameObject.name);
        DynamicCamera.Instance.CurrentTargets.Remove(transform);
        stateMachine.ChangeState(Player_State.Dead);
    }

    public void GenerateMovementModifier(GameObject applier, float modifier)
    {
        if (modifiers.ContainsKey(applier)) modifiers[applier] = modifier;
        else modifiers.Add(applier, modifier);
    }
    public void ClearMovementModifier(GameObject applier)
    {
        modifiers.Remove(applier);
    }
}
