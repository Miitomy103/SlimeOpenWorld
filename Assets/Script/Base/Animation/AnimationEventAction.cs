using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// AnimatorコンポーネントのTriggerから呼び出されるクラス。
/// </summary>
public class AnimationEventAction : MonoBehaviour
{
    Dictionary<string, Action> eventActions = new Dictionary<string, Action>();
    /// <summary>
    /// AnimatorのTriggerから呼び出される関数。
    /// </summary>
    public void AnimationEvent(string key)
    {
        eventActions.TryGetValue(key, out Action action);
        action?.Invoke();
    }
    /// <summary>
    /// AnimatorのTriggerから呼び出される関数を追加する関数。
    /// </summary>
    public void AddEventAction(string key, Action action)
    {
        if (!eventActions.ContainsKey(key))
        {
            eventActions.Add(key, action);
        }
    }
}
