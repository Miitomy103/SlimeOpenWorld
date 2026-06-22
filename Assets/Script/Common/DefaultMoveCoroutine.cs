using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 指定時間で対象のTransformを目標位置まで等速直線移動させるIMoveCoroutine実装。
/// </summary>
public class DefaultMoveCoroutine : IMoveCoroutine
{
    Transform _transform;
    Vector3 _endPos;
    float _time;
    Action _endAction;
    public DefaultMoveCoroutine(Transform transform,Vector3 endPos,float time,Action endAction=null)
    {
        _transform = transform;
        _endPos = endPos;
        _time = time;
        _endAction = endAction;
    }
    public async UniTask MoveCoroutineAsync()
    {
        Vector3 startPos = _transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < _time)
        {
            _transform.position = Vector3.Lerp(startPos, _endPos, elapsedTime / _time);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
        _transform.position = _endPos;

        _endAction?.Invoke();
    }
}
