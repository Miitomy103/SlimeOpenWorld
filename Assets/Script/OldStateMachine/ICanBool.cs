namespace InGame
{
    /// <summary>
    /// 実装したステートに遷移できるかどうかを示す。trueでなければStateMachine.ChangeStateで遷移しない。
    /// </summary>
    public interface ICanBool
    {
        bool CanBool { get; }
    }
}
