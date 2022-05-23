using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZekstersLab.BehaviourTree;
public class AI_DoTask : ActionNode
{
    List<Task> taskList = new List<Task>();
    int index = 0;
    public override void Pause()
    {
        //Do nothing
    }

    protected override void OnStart()
    {
        taskList.AddRange((List<Task>)myTree.GetData("TaskList"));
        index = (int)myTree.GetData("TaskIndex");
    }

    protected override void OnStop()
    {
        //do nothing
    }

    protected override NodeState OnUpdate()
    {

        if(taskList[index].DoTask())
        {
            taskList.RemoveAt(index);
            Debug.Log("Task Completed");
            return NodeState.Success;
        }
        Debug.Log("Task Failed");
        return NodeState.Failed; 
    }
}
