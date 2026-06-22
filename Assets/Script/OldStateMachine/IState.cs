namespace InGame
{
    public interface IState<T>
    {
        void DoStart(T owner);
        void DoUpdate(T owner);
        void DoExit(T owner);
    }
}
