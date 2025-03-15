using CharacterLogic;
using StateMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    [RequireComponent(typeof(CharacterManager))]
    public class Agent : MonoBehaviour
    {
        [Header("Sensor")]
        [SerializeField] Sensor chaseSensor;
        [SerializeField] Sensor attackSensor;


        AgentGoal lastGoal;
        public AgentGoal currentGoal;
        public ActionPlan actionPlan;
        public AgentAction currentAction;

        public Dictionary<string, Belief> beliefs;
        public HashSet<AgentAction> actions;
        public HashSet<AgentGoal> goals;

        CharacterManager characterManager;

        private void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        StateMachine<Player_State, Player_Status> StateMachine => characterManager.stateMachine;

        private void Start()
        {
            SetupTimers();
            SetupBeliefs();
            SetupActions();
            SetupGoals();
        }



        private void SetupTimers()
        {

        }

        private void SetupBeliefs()
        {
            beliefs = new Dictionary<string, Belief>();
            BeliefFactory factory = new(this, beliefs);

            factory.AddBelief("Nothing",() => false);


            factory.AddBelief("AgentIdle",() => StateMachine.IsInState(Player_State.Idle));
            factory.AddBelief("AgentMoving",() => StateMachine.IsInState(Player_State.Thrusting));
            factory.AddBelief("AgentMeleeAttack",() => StateMachine.IsInState(Player_State.MeleeAttack));


        }
        private void SetupActions()
        {
            actions = new();
            actions.Add(new AgentAction.Builder("Relax")
                .WithStrategy(new IdleStrategy(5))
                .AddEffect(beliefs["Nothing"])
                .Build());

            actions.Add(new AgentAction.Builder("WanderAround")
                .WithStrategy(new WanderStrategy(characterManager, 10))
                .AddEffect(beliefs["AgentMoving"])
                .Build());
        }

        private void SetupGoals()
        {
            goals = new();
            goals.Add(new AgentGoal.Builder("ChillOut")
                .WithPriority(1)
                .WithDesiredEffect(beliefs["Nothing"])
                .Build());

            goals.Add(new AgentGoal.Builder("Wander")
                .WithPriority(1)
                .WithDesiredEffect(beliefs["AgentMoving"])
                .Build());
        }


        private void OnEnable()
        {
            chaseSensor.OnTargetChanged += HandleTargetChanged;
        }

        private void OnDisable()
        {
            chaseSensor.OnTargetChanged -= HandleTargetChanged;
        }

        void HandleTargetChanged()
        {
            Debug.Log("Target changed, clearing current action and goal");

            currentGoal = null;
            currentAction = null;
        }


    }

}
