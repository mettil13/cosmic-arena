using UnityEngine;
namespace StateMachine
{
    public interface IModifier<T1, T2> 
        where T1 : System.Enum 
        where T2 : System.Enum
    {
        string Name { get; }
        void OnEntry(StateMachine<T1, T2> stateMachine);
        void OnUpdate(ref float delta);
        void OnSecondUpdate(ref float delta);
        void OnLateUpdate();
        void OnFixedUpdate();
        void OnExit();
        void OnModifierStack(IModifier<T1, T2> newModifier, StateMachine<T1, T2> stateMachine);
    }

}
