namespace GOAP
{
    using CommonLogic;
    using UnityEngine;

    public interface IActionStrategy
    {
        bool CanPerform { get; }

        bool Complete { get; }

        void Start() 
        { 
            
        }

        void Update(float deltaT)
        {

        }

        void Stop()
        {

        }
    }


    public class WanderStrategy : IActionStrategy
    {
        public bool CanPerform => true;

        public bool Complete { get; private set; }

        float wanderRadius;
        readonly CharacterManager characterManager;

        public WanderStrategy(CharacterManager characterManager, float wanderRadius)
        {
            this.characterManager = characterManager;
            this.wanderRadius = wanderRadius;
        }

        public void Start()
        {
            for(int i = 0; i < 5; i++)
            {
                Vector3 randomDirecition = (Random.insideUnitCircle * wanderRadius);

                
            }
        }


    }

    public class IdleStrategy : IActionStrategy
    {
        public bool CanPerform => true;

        public bool Complete { get; private set; }

        readonly Timer timer;

        public IdleStrategy(float duration)
        {
            timer = new Timer(duration);
            timer.OnStart += () => Complete = false;
            timer.OnEnd += () => Complete = true;
        }

        public void Start()
        {
            
        }

        public void Update(float deltaT) => timer.Update(ref deltaT);

    }
}
