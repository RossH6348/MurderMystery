using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZekstersLab.BehaviourTree;

public class AI_RepeatUntillAtTarget : DecoratorNode
{
    public float threshold = 0.1f;
    NodeState childState = NodeState.Ready;
    public override void Pause()
    {
        //do nothing
    }

    protected override void OnStart()
    {
        childState = NodeState.Ready;
    }

    protected override void OnStop()
    {
        Debug.Log("RepeatUntilAtTarget: child state=" + childState.ToString() + "    returning=" + state.ToString());
    }

    protected override NodeState OnUpdate()
    {
        Vector3 currentPos;
        Vector3 targetPos;

        targetPos = (Vector3)myTree.GetData("TargetPosition");
        //Debug.Log("RepeatUntilAtTarget: TargetPosition: " + targetPos.ToString());
        if (targetPos == null) return NodeState.Failed;

        childState = child.Update();

        if (childState == NodeState.Failed) return NodeState.Failed;
        if (childState == NodeState.Running) return NodeState.Running;

        currentPos = (Vector3)myTree.GetData("Position");
        //Debug.Log("RepeatUntilAtTarget: Position: " + currentPos.ToString());
        if (currentPos == null) return NodeState.Failed;

        return (Vector3.Distance(currentPos, targetPos) <= threshold) ? NodeState.Success : NodeState.Running;
        


    }
}
