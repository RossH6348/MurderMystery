using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZekstersLab.BehaviourTree;
public class AI_PickTask : ActionNode
{
    private List<Task> tasks = new List<Task>();
    private int index;

    public override void Pause()
    {
        //Do nothing
    }

    protected override void OnStart()
    {
        tasks = (List<Task>)myTree.GetData("TaskList");
    }

    protected override void OnStop()
    {
        //Do nothing
    }

    protected override NodeState OnUpdate()
    {
        if (tasks.Count <= 0) return NodeState.Failed;

        index = Random.Range((int)0, tasks.Count);
        myTree.SetData("TaskIndex", index);
        myTree.SetData("TargetPosition", tasks[index].GetPosition());

        return NodeState.Success;
    }

}
