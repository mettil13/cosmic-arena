namespace StateMachine
{
    public interface IState
    {
        bool CheckChangeTo();
        void OnEntry();
        void OnUpdate(ref float delta);
        void OnSecondUpdate(ref float delta);
        void OnLateUpdate();
        void OnFixedUpdate();
        void OnExit();

    }

}
