using UnityEngine;
using System.Collections.Generic;
namespace GOAP
{
    [System.Serializable]
    public class GOAPAction
    {
        public float weight;
        public WorldState[] requirements;
        public WorldState[] effects;


        public BlackBoard CalculateEffect(BlackBoard current)
        {
            BlackBoard newBlackboard = new BlackBoard(current);
            foreach(var effect in effects)
            {
                if (newBlackboard.worldStates.ContainsKey(effect.name))
                {
                    newBlackboard.worldStates[effect.name].ApplyEffect(effect);
                }
                else
                {
                    newBlackboard.worldStates.Add(effect.name, effect);
                }
             
            }

            return newBlackboard;
        }
    }


    public class AgentActionSO : ScriptableObject
    {

        [SerializeField] public GOAPAction action;

    }

}
