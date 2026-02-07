using System;
using UnityEngine;

public abstract class QuestTransitionBase : MonoBehaviour, IQuestTransition
{
    public string TargetQuestID => throw new NotImplementedException();

    protected abstract void TransitionQuest();
}
