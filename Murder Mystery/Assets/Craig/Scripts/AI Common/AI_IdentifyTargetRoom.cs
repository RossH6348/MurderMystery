using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZekstersLab.BehaviourTree;

public class AI_IdentifyTargetRoom : ActionNode
{
    GameObject target = null;
    public override void Pause()
    {
        //Do nothing
    }

    protected override void OnStart()
    {
        target = (GameObject)myTree.GetData("AssassinTarget");
    }

    protected override void OnStop()
    {
        //Do nothing
    }

    protected override NodeState OnUpdate()
    {
        if (target == null) return NodeState.Failed;


        myTree.SetData("TargetPosition", RoomManager.Instance.GetRoomPointForPlayer(target));
        Debug.Log("Target's Room Point: " + RoomManager.Instance.GetRoomPointForPlayer(target).ToString());

        return NodeState.Success;
    }
}
