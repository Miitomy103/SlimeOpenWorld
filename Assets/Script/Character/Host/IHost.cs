using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHost 
{
    /// <summary>
    /// 乗っ取りを開始する
    /// </summary>
    void StartHost();
    /// <summary>
    /// 乗っ取りを終了する
    /// </summary>
    void StopHost();

    /// <summary>
    /// ホストの状態を更新する
    /// </summary>
    void UpdateHost();

    /// <summary>
    /// 入力を処理する
    /// </summary>
    void HandleInput(PlayerInput input);
}
