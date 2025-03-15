using GOAP;
using Sirenix.OdinInspector.Editor.Drawers;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{ 

    public class BeliefFactory
    {
        readonly Agent agent;
        readonly Dictionary<string, Belief> beliefs;

        public BeliefFactory(Agent agent, Dictionary<string, Belief> beliefs)
        {
            this.agent = agent;
            this.beliefs = beliefs;

        }

        public void AddBelief(string key, Func<bool> condition)
        {
            beliefs.Add(key, new Belief.Builder(key)
                .WithCondition(condition)
                .Build());
        }

        public void AddSensorBelief(string key, Sensor sensor)
        {
            beliefs.Add(key, new Belief.Builder(key)
                .WithCondition(() => sensor.IsTargetInRange)
                .WithLocation(() => sensor.TargetPosition)
                .Build());
        }

        public void AddLocationBelief(string key, float distance, Transform locationCondition)
        {
            AddLocationBelief(key, distance, locationCondition.position);
        }

        public void AddLocationBelief(string key, float distance, Vector3 locationCondition)
        {
            beliefs.Add(key, new Belief.Builder(key)
                .WithCondition(() => InRangeOf(locationCondition, distance))
                .WithLocation(() => locationCondition)
                .Build());
        }



        bool InRangeOf(Vector3 pos, float range) => (agent.transform.position - pos).sqrMagnitude < range * range;

    }


    public class Belief
    {
        public string Name { get; }

        Func<bool> condition = () => false;
        Func<Vector3> observedLocation = () => Vector3.zero;

        public Vector3 Location => observedLocation();

        Belief(string Name)
        {
            this.Name = Name;
        }


        public bool Evaluate() => condition();
        

        public class Builder
        {
            readonly Belief belief;

            public Builder(string name)
            {
                this.belief = new Belief(name);
            }

            public Builder WithCondition(Func<bool> condition)
            {
                belief.condition = condition;
                return this;
            }

            public Builder WithLocation(Func<Vector3> observedLocation)
            {
                belief.observedLocation = observedLocation;
                return this;
            }

            public Belief Build()
            {
                return belief;
            }
        }

    }
}

