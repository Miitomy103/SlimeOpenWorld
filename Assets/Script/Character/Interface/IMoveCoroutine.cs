using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IMoveCoroutine
{
    UniTask MoveCoroutineAsync();
}
