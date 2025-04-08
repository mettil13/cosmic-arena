namespace GOAP
{
    using CharacterLogic;
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
            Vector3 randomDirecition = Random.insideUnitSphere;
            characterManager.characterInputAdapter.Thrust = 1;
            characterManager.characterInputAdapter.Direction = randomDirecition;
        }

        public void Update(float deltaT)
        {
            Complete = true;
        }

        public void Stop()
        {
            characterManager.characterInputAdapter.Thrust = 0;
            characterManager.characterInputAdapter.Direction = Vector2.zero;
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

        public void Update(float deltaT) => timer.Update(ref deltaT);

    }

    public class WaitForCooldown : IActionStrategy
    {
        public bool CanPerform => true;

        public bool Complete { get; private set; }

        CharacterManager characterManager;
        Player_Status statusToWaitFor;

        public WaitForCooldown(CharacterManager characterManager, Player_Status statusToWaitFor)
        {
            Complete = false;
            this.characterManager = characterManager;
            this.statusToWaitFor = statusToWaitFor;
        }

        public void Update(float deltaT)
        {
            if (characterManager.stateMachine.Status.HasFlag(statusToWaitFor)) 
                Complete = true;
        }

    }

    public class AttackTargetStrategy : IActionStrategy
    {
        public bool CanPerform => true;

        public bool Complete { get; private set; }

        readonly CharacterManager characterManager;
        Belief playerLocationBelief;


        public AttackTargetStrategy(CharacterManager characterManager, Belief targetLocationBelief)
        {
            this.characterManager = characterManager;
            this.playerLocationBelief = targetLocationBelief;
        }

        public void Start()
        {
            characterManager.characterInputAdapter.Attack = 1;

            Vector3 dir = playerLocationBelief.Location - characterManager.transform.position;
            characterManager.characterInputAdapter.Direction = new Vector2(dir.x, dir.z);
        }


        public void Update(float deltaT)
        {
            Complete = true;
        }


        public void Stop()
        {
            characterManager.characterInputAdapter.Attack = 0;
            characterManager.characterInputAdapter.Direction = Vector2.zero;
        }


    }
}
