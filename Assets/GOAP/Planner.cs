using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace GOAP
{ 
    public class Planner
    {
        GOAPAction[] actions;

        GOAPAction[] plan;

        
        
        void Init()
        {
            actions.Sort((GOAPAction x, GOAPAction y) => 
            {
                if (x.weight < y.weight) 
                    return -1;
                else if (x.weight > y.weight)
                    return 1;
                return 0;
            });
        }


        public List<GOAPAction> Plan(BlackBoard beliefs, BlackBoard goal)
        {
            List<GOAPAction> plan = AStar(beliefs, goal, Node.Heuristic);

            return plan;
        }

        public delegate float Heuristic<T> (T a, T b);
        public List<GOAPAction> AStar(BlackBoard startState, BlackBoard goal, Heuristic<BlackBoard> heuristic)
        {
            List<Node> openList = new List<Node>() { new Node(startState, null, 0, heuristic(startState, goal), null) };
            List<Node> closedList = new List<Node>();

            while (openList.Count > 0)
            {
                openList.Sort((Node a, Node b) => { return (a.f < b.f) ? -1 : a.f > b.f ? 1 : 0; });
                Node node = openList[0];
                if (node.h == 0)
                {
                    return ReconstructPath(node);
                }
                closedList.Add(node);

                foreach (GOAPAction action in actions)
                {
                    BlackBoard newState = action.CalculateEffect(node.state);

                    Node newNode = new Node(newState, action, node.g + action.weight, heuristic(newState, goal), node);
                    //check better path
                    openList.Add(newNode);


                    
                }

            }


            return null;

            
        }
        List<GOAPAction> ReconstructPath(Node current)
        {
            List<GOAPAction> path = new List<GOAPAction>() {current.doneAction};

            while (current.parent != null)
            {
                current = current.parent;
                path.Add(current.doneAction);
            }

            path.Reverse();
            return path;

        }

        public class Node
        {
            public BlackBoard state;
            public GOAPAction doneAction;
            public float g;
            public float h;
            public float f => g + h;
            public Node parent;

            public Node(BlackBoard state, GOAPAction doneAction, float g, float h, Node parent)
            {
                this.state = state;
                this.doneAction = doneAction;
                this.g = g;
                this.h = h;
                this.parent = parent;
            }

            public static float Heuristic(BlackBoard from, BlackBoard to) 
            {
                float h = 0;
                foreach(WorldState objective in to.worldStates.Values) 
                {
                    if (from.worldStates.TryGetValue(objective.name, out WorldState state) && objective == state)
                    {

                    }
                    else
                    {
                        h += 1;
                    }
                }


                return h;
            }
        }
    }

}
