using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZekstersLab.BehaviourTree;

public class AI_StuckInPlaceConditionTrue : DecoratorNode
{
    public override void Pause()
    {
        //Do Nothing
    }

    protected override void OnStart()
    {
        Debug.Log("Stuck in place condition check: " + ((bool)myTree.GetData("StuckInPlace")).ToString());
    }

    protected override void OnStop()
    {
        
    }

    protected override NodeState OnUpdate()
    {
        if (!(bool)myTree.GetData("StuckInPlace")) return NodeState.Failed;

        return child.Update();
        
        
    }
}
