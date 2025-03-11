using System;
using UnityEngine;
namespace GOAP
{
    [System.Serializable]
    public struct WorldState
    {
        public string name;
        public bool isActive;
        public float value;
        public float valueThreshold;
        public Vector3 position;
        public float positionSqrThreshold;

        internal void ApplyEffect(WorldState effect)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Uses the thresholds of a
        /// </summary>
        public static bool operator == (WorldState a, WorldState b)
        {
            return  (a.isActive && b.isActive) && 
                    ((a.position - b.position).sqrMagnitude < a.positionSqrThreshold) && 
                    (MathF.Abs(a.value - b.value) < a.valueThreshold);
        }
        /// <summary>
        /// Uses the thresholds of a
        /// </summary>
        public static bool operator != (WorldState a, WorldState b)
        {
            return !(a == b);
        }
    }

}
