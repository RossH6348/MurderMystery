using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZekstersLab.BehaviourTree;

public class AI_RepeatUntillAtTarget : DecoratorNode
{
    public float threshold = 0.1f;
    NodeState childState = NodeState.Ready;

    public int stuckInPlaceThreshold = 2;

    private bool stuckInPlace = false;
    private int stuckInPlaceCount = 0;
    public override void Pause()
    {
        //do nothing
    }

    protected override void OnStart()
    {
        
        myTree.SetData("StuckInPlace", stuckInPlace);
        childState = NodeState.Ready;
    }

    protected override void OnStop()
    {
        if (state == NodeState.Success)
        {
            stuckInPlaceCount = 0;
            stuckInPlace = false;
        }
        Debug.Log("RepeatUntilAtTarget: child state=" + childState.ToString() + "    returning=" + state.ToString() + "    stuckInPlaceCount=" + stuckInPlaceCount.ToString());
    }

    protected override NodeState OnUpdate()
    {
        Vector3 previousPos;
        Vector3 currentPos;
        Vector3 targetPos;

        targetPos = (Vector3)myTree.GetData("TargetPosition");
        previousPos = (Vector3)myTree.GetData("CurrentPosition");
        //Debug.Log("RepeatUntilAtTarget: TargetPosition: " + targetPos.ToString());
        if (targetPos == null) return NodeState.Failed;
        if (previousPos == null) return NodeState.Failed;

        childState = child.Update();
        if (childState == NodeState.Running) return NodeState.Running;

        currentPos = (Vector3)myTree.GetData("Position");
        //Debug.Log("RepeatUntilAtTarget: Position: " + currentPos.ToString());
        if (currentPos == null) return NodeState.Failed;

        if (childState == NodeState.Failed)
        {
            if((currentPos == previousPos) && (++stuckInPlaceCount >= stuckInPlaceThreshold))
            {
                //stuck in place
                Debug.Log("Stuck in place");
                stuckInPlace = true;
                myTree.SetData("StuckInPlace", stuckInPlace);
            }
            return NodeState.Failed;
        }
        

        
        
        myTree.SetData("TakeUpAction", (bool)true);
        
        return (Vector3.Distance(currentPos, targetPos) <= threshold) ? NodeState.Success : NodeState.Running;
        


    }
}
