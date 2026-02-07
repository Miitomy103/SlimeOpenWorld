using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationEventAction : MonoBehaviour
{
    Dictionary<string, Action> eventActions = new Dictionary<string, Action>();
    public void AnimationEvent(string key)
    {
        eventActions.TryGetValue(key, out Action action);
        action?.Invoke();
    }
    public void AddEventAction(string key, Action action)
    {
        if (!eventActions.ContainsKey(key))
        {
            eventActions.Add(key, action);
        }
    }
}
