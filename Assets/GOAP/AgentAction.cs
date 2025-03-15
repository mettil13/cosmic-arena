using System.Collections.Generic;
namespace GOAP
{
    [System.Serializable]
    public class AgentAction
    {
        public string Name { get; }

        public float Cost { get; private set; }

        public HashSet<Belief> Preconditions { get; } = new();
        public HashSet<Belief> Effects { get; } = new();


        IActionStrategy strategy;

        AgentAction(string name)
        {
            Name = name;
        } 

        public bool Complete => strategy.Complete;
        public void Start() => strategy.Start();

        public void Update(float deltaT)
        {
            if (strategy.CanPerform)
            {
                strategy.Update(deltaT);
            }

            if (strategy.Complete) return;

            foreach(var effect in Effects)
            {
                effect.Evaluate();
            }

        }

        public void Stop() => strategy.Stop();

        public class Builder
        {
            readonly AgentAction action;

            public Builder(string name)
            {
                action = new AgentAction(name)
                {
                    Cost = 1
                };
            }

            public Builder WithCost(float cost)
            {
                action.Cost = cost;
                return this;
            }
            public Builder WithStrategy(IActionStrategy strategy)
            {
                action.strategy = strategy;
                return this;
            }
            public Builder AddPrencondition(Belief precondition)
            {
                action.Preconditions.Add(precondition);
                return this;
            }

            public Builder AddEffect(Belief effect)
            {
                action.Effects.Add(effect);
                return this;
            }

            public AgentAction Build()
            {
                return action;
            }
        }
    }

}
