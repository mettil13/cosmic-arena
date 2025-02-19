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
        public Vector3 position;

        internal void ApplyEffect(WorldState effect)
        {
            throw new NotImplementedException();
        }
    }

}
