
using System.Collections.Generic;

namespace GOAP
{
    public class AgentGoal
    {
        public string Name { get; }
        public float Prioity { get; private set; }

        public HashSet<Belief> DesiredEffects { get; } = new();

        AgentGoal(string name)
        {
            Name = name;
        }


        public class Builder
        {
            readonly AgentGoal goal;

            public Builder(string name)
            {
                goal = new AgentGoal(name);
            }

            public Builder WithPriority(float priority)
            {
                goal.Prioity = priority;
                return this;
            }

            public Builder WithDesiredEffect(Belief effect)
            {
                goal.DesiredEffects.Add(effect);
                return this;
            }

            public AgentGoal Build()
            {
                return goal;
            }


        }
    }
}
