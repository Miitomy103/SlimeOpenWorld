using Unity.VisualScripting;
using UnityEngine;
using InGame;

[System.Serializable]
public class SlimeIdle : IState<SlimeHost>
{
    public void DoExit(SlimeHost owner)
    {

    }

    public void DoStart(SlimeHost owner)
    {

    }

    public void DoUpdate(SlimeHost owner)
    {
        if (owner.Input.Move.magnitude>0.1f)
        {
            owner.stateMachine.ChangeState(owner.SlimeMove);
            return;
        }
        if(owner.Input.Action1.onDown)
        {
            owner.stateMachine.ChangeState(owner.SlimeAim);
        }
        if (owner.Input.Action0.onDown)
        {
            owner.stateMachine.ChangeState(owner.SlimeAttack);
        }
    }
}
