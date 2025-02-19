using UnityEngine;

namespace StateMachine
{
    public abstract class AStateSO : ScriptableObject, IState
    {
        public abstract bool CheckChangeTo();
        public abstract void OnEntry();
        public abstract void OnExit();
        public abstract void OnLateUpdate();
        public abstract void OnSecondUpdate(ref float delta);
        public abstract void OnUpdate(ref float delta);

        public abstract void OnFixedUpdate();

    }
}