//Author: Craig Zeki - Zek21003166

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZekstersLab.BehaviourTree;

public class AI_SetTargetToRandomSpawnPoint : ActionNode
{
    Vector3 target;
    public override void Pause()
    {
        //do nothing
    }

    protected override void OnStart()
    {
        //do nothing
    }

    protected override void OnStop()
    {
        //do nothing
    }

    protected override NodeState OnUpdate()
    {
        target = SpawnManagerv2.Instance.GetRandomSpawnPoint().position;
        if (target == null) return NodeState.Failed;


        myTree.SetData("TargetPosition", target);
        Debug.Log("Picked random spawn location to move to: " + target.ToString());
        return NodeState.Success;
    }
}
