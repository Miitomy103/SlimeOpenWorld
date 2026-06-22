using Cysharp.Threading.Tasks;

/// <summary>
/// 非同期コルーチンによる移動処理を表すインターフェース。
/// </summary>
public interface IMoveCoroutine
{
    UniTask MoveCoroutineAsync();
}
