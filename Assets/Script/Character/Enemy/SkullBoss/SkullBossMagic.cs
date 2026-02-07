using UnityEngine;
using StateMachine;
using System.Collections.Generic;

public class SkullBossMagic : StateBase<string>
{
    public const string key = "SkullBossMagic";
    public override string Key => key;



    protected override List<ITransitionCondition<string>> CreateCondition()
    {
        throw new System.NotImplementedException();
    }

}
