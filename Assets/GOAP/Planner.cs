using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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


        public GOAPAction[] Plan(BlackBoard beliefs, BlackBoard goal)
        {


            return plan;
        }

        public delegate float Heuristic<T> (T a, T b);
        public GOAPAction[] AStar(BlackBoard startState, BlackBoard goal, Heuristic<BlackBoard> heuristic)
        {
            List<Node> openList = new List<Node>() { new Node(startState, null, 0, heuristic(startState, goal), null) };
            List<Node> closedList = new List<Node>();

            while (openList.Count > 0)
            {
                openList.Sort((Node a, Node b) => { return (a.f < b.f) ? -1 : a.f > b.f ? 1 : 0; });
                Node node = openList[0];
                closedList.Add(node);

                foreach(GOAPAction action in actions)
                {
                    BlackBoard newState = action.CalculateEffect(node.state);
                    Node newNode = new Node(newState, action, node.g + action.weight, heuristic(newState, goal), node);
                    //check better path
                    openList.Add(newNode);
                    
                }

            }


            return new GOAPAction[0];

            
        }
        GOAPAction[] ReconstructPath(List<Node> closedList)
        {
            List<GOAPAction> path = new List<GOAPAction>();
            foreach (Node node in closedList)
            {
                path.Add(node.doneAction);
            }
            return path.ToArray();
        }


        public class Node
        {
            public BlackBoard state;
            public GOAPAction doneAction;
            public float g;
            public float h;
            public float f => g + h;
            Node parent;

            public Node(BlackBoard state, GOAPAction doneAction, float g, float h, Node parent)
            {
                this.state = state;
                this.doneAction = doneAction;
                this.g = g;
                this.h = h;
                this.parent = parent;
            }
        }
    }

}
