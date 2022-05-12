using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZekstersLab.BehaviourTree;
public class AI_Test_Die : ActionNode
{
    private int stepsToTake = 0;
    public override void Pause()
    {
        //Do Nothing
    }

    protected override void OnStart()
    {
        stepsToTake = (int)myTree.GetData("DiceRoll");
    }

    protected override void OnStop()
    {
        //Do Nothing
    }

    protected override NodeState OnUpdate()
    {
        if(stepsToTake > 0)
        {
            stepsToTake--;
            Debug.Log("Step 1 taken, " + stepsToTake.ToString() + " left");
        }

        return stepsToTake > 0 ? NodeState.Running : NodeState.Success;
        
    }
}
