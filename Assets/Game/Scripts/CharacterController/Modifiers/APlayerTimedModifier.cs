using StateMachine;
using UnityEngine;

namespace CharacterLogic
{
    using CommonLogic;
    [System.Serializable]
    public abstract class APlayerTimedModifier : APlayerModifier
    {
        protected Timer timer;

        enum TimedModifierStackBehaviour
        {
            AddTime,
            RestartTime,
        }

        [SerializeField] TimedModifierStackBehaviour stackBehaviour;
        [SerializeField] float duration;

        public float Duration { get => duration; }

        public override void OnEntry(StateMachine<Player_State, Player_Status> stateMachine)
        {
            base.OnEntry(stateMachine);
            timer = new Timer(duration).AddCallBack(Expire);
        }

        public override void OnUpdate(ref float delta)
        {
            base.OnUpdate(ref delta);
            timer.Update(ref delta);
        }

        private void Expire()
        {
            stateMachine.RemoveModifier(Name);
        }

        public override void OnModifierStack(IModifier<Player_State, Player_Status> newModifier, StateMachine<Player_State, Player_Status> stateMachine)
        {
            base.OnModifierStack(newModifier, stateMachine);
            switch(stackBehaviour)
            {
                case TimedModifierStackBehaviour.AddTime:
                    timer.Duration += ((APlayerTimedModifier)newModifier).Duration;
                    break;

                case TimedModifierStackBehaviour.RestartTime:
                    timer.Reset();
                    break;
            }
        }
    }
}
