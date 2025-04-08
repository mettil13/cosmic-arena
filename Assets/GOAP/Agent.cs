using CharacterLogic;
using Mono.Cecil.Cil;
using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
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
        CharacterHealth characterHealth;


        IGoapPlanner gPlanner;

        private void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            characterHealth = GetComponent<CharacterHealth>();
            gPlanner = new GoapPlanner();
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
            factory.AddBelief("AgentIsLowHealth", () => characterHealth.HP < characterHealth.maxHP / 10f);
            factory.AddBelief("AgentCanAttack", () => StateMachine.Status.HasFlag(Player_Status.CanMeleeAttack));
            factory.AddBelief("AgentCanThrust", () => StateMachine.Status.HasFlag(Player_Status.CanThrust));
            factory.AddBelief("AgentIsCloseToTarget", () => attackSensor.IsTargetInRange);
            factory.AddBelief("AgentHasTargetTarget", () => chaseSensor.IsTargetInRange);

        }
        private void SetupActions()
        {
            actions = new();
            actions.Add(new AgentAction.Builder("Relax")
                .WithStrategy(new IdleStrategy(5))
                .AddEffect(beliefs["Nothing"])
                .AddEffect(beliefs["AgentCanThrust"])
                .AddEffect(beliefs["AgentCanAttack"])
                .Build());

            actions.Add(new AgentAction.Builder("WanderAround")
                .AddPrencondition(beliefs["AgentCanThrust"])
                .WithStrategy(new WanderStrategy(characterManager, 10))
                .AddEffect(beliefs["AgentMoving"])
                .Build());
            
            actions.Add(new AgentAction.Builder("AttackTarget")
                .AddPrencondition(beliefs["AgentCanAttack"])
                .AddPrencondition(beliefs["AgentIsCloseToTarget"])
                .WithStrategy(new AttackTargetStrategy(characterManager, beliefs["AgentIsCloseToTarget"]))
                .AddEffect(beliefs["AgentMeleeAttack"])
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
            
            goals.Add(new AgentGoal.Builder("Attack")
                .WithPriority(2)
                .WithDesiredEffect(beliefs["AgentMeleeAttack"])
                .Build());

            
        }


        private void OnEnable()
        {
            chaseSensor.OnTargetChanged += HandleTargetChanged;
            attackSensor.OnTargetChanged += HandleTargetIsClose;
        }

        private void OnDisable()
        {
            chaseSensor.OnTargetChanged -= HandleTargetChanged;
            attackSensor.OnTargetChanged -= HandleTargetIsClose;
        }

        private void HandleTargetIsClose()
        {

        }

        void HandleTargetChanged()
        {
            Debug.Log("Target changed, clearing current action and goal");

            currentGoal = null;
            currentAction = null;
        }

        private void Update()
        {
            if(currentAction == null)
            {
                Debug.Log("Calculating any potential plan");
                CalculatePlan();

                if (actionPlan != null && actionPlan.Actions.Count > 0)
                {
                    currentGoal = actionPlan.Goal;
                    Debug.Log($"Goal: {currentGoal.Name} with {actionPlan.Actions.Count} actions in plan");
                    currentAction = actionPlan.Actions.Pop();
                    Debug.Log($"Popped action: {currentAction.Name}");
                    currentAction.Start();


                }
            }

            if(actionPlan != null && currentAction != null)
            {
                currentAction.Update(Time.deltaTime);

                if (currentAction.Complete)
                {
                    Debug.Log($"{currentAction.Name} complete");
                    currentAction.Stop();

                    if(actionPlan.Actions.Count == 0)
                    {
                        Debug.Log("PlanComplete");
                        lastGoal = currentGoal;
                        currentGoal = null;
                    }
                }
            }
        }

        private void CalculatePlan()
        {
            var priorityLevel = currentGoal?.Prioity ?? 0;

            HashSet<AgentGoal> goalsToCheck = goals;

            if(currentGoal != null)
            {
                Debug.Log("Current goal exists, checking goals with higher priority");
                goalsToCheck = new HashSet<AgentGoal>(goals.Where(g => g.Prioity > priorityLevel));

            }

            var potentialPlan = gPlanner.Plan(this, goalsToCheck, lastGoal);

            if(potentialPlan != null)
            {
                actionPlan = potentialPlan;
            }
        }
    }

}
