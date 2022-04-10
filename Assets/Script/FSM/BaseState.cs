namespace FSM
{
    public abstract class BaseState<T>
    {
        public abstract void OnEnter(T role);
        public abstract void OnUpdate(T role);
        public abstract void OnExit(T role);
    }
}