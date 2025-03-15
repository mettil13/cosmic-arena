using System;
using System.Collections.Generic;
using System.Linq;

namespace GOAP
{
    public interface IGoapPlanner
    {
        ActionPlan Plan(Agent agent, HashSet<AgentGoal> goals, AgentGoal mostRecentGoal = null);
    }


    public class GoapPlanner : IGoapPlanner
    {
        public ActionPlan Plan(Agent agent, HashSet<AgentGoal> goals, AgentGoal mostRecentGoal = null)
        {
            List<AgentGoal> orderedGoals = goals
                .Where(g => g.DesiredEffects.Any(b => !b.Evaluate()))
                .ToList();

            foreach(var goal in orderedGoals)
            {
                Node goalNode = new Node(null, null, goal.DesiredEffects, 0);



                if(FindPath(goalNode, agent.actions))
                {
                    if (goalNode.IsLeafDead) continue;

                    Stack<AgentAction> actionStack = new();
                    while(goalNode.Leaves.Count > 0)
                    {
                        var cheapestLeafNode = goalNode.Leaves.OrderBy(leaf => leaf.Cost);
                    }
                }
            }

            return null;
        }

        private bool FindPath(Node parent, HashSet<AgentAction> actions)
        {
            foreach(var action in actions)
            {
                var requiredEffects = parent.RequiredEffects;
                requiredEffects.RemoveWhere(b => b.Evaluate());


                if (requiredEffects.Count == 0) 
                    return true;

                if (action.Effects.Any(requiredEffects.Contains))
                {
                    var newRequiredEffects = new HashSet<Belief>(requiredEffects);

                    newRequiredEffects.ExceptWith(action.Effects);
                    newRequiredEffects.UnionWith(action.Preconditions);

                    var newAvailableActions = new HashSet<AgentAction>(actions);
                    newAvailableActions.Remove(action);

                    var newNode = new Node(parent, action, newRequiredEffects, parent.Cost + action.Cost);

                    if(FindPath(newNode, newAvailableActions))
                    {
                        parent.Leaves.Add(newNode);
                        newRequiredEffects.ExceptWith(newNode.Action.Preconditions);
                    }

                    if(newRequiredEffects.Count == 0)
                    {
                        return true;
                    }
                }


            }
            return false;
        }
    }


    public class Node
    {


        public Node Parent { get; }
        public AgentAction Action { get; }
        public HashSet<Belief> RequiredEffects { get; }
        public List<Node> Leaves { get; }
        public float Cost { get; }

        public bool IsLeafDead => Leaves.Count == 0 && Action == null;

        public Node(Node parent, AgentAction action, HashSet<Belief> effects, float cost)
        {
            Parent = parent;
            Action = action;
            RequiredEffects = new HashSet<Belief>(effects);
            Leaves = new List<Node>();
            Cost = cost;
        }
    }

    public class ActionPlan
    {
        public ActionPlan(AgentGoal goal, Stack<AgentAction> actions, float totalCost)
        {
            Goal = goal;
            Actions = actions;
            TotalCost = totalCost;
        }

        public AgentGoal Goal { get; }
        public Stack<AgentAction> Actions { get; }

        public float TotalCost { get; set; }

        

    }
}