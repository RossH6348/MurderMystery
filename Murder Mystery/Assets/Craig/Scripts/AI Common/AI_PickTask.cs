using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZekstersLab.BehaviourTree;
public class AI_PickTask : ActionNode
{
    private List<Task> tasks = new List<Task>();
    private int index;
    private PlayerInventory inventory;
    private List<Task> freeTasks = new List<Task>();
    private bool firstRun = false;


    public override void Pause()
    {
        //Do nothing
    }

    private void Init()
    {
        foreach(Task task in tasks)
        {
            if (!task.RequiresItem) freeTasks.Add(task);
        }
        firstRun = true;
    }

    protected override void OnStart()
    {
        tasks = (List<Task>)myTree.GetData("TaskList");
        inventory = (PlayerInventory)myTree.GetData("Inventory");


        if (!firstRun) Init();
    }

    protected override void OnStop()
    {
        //Do nothing
    }

    protected override NodeState OnUpdate()
    {
        index = tasks.Count; //set index to a non-plausible number to check for failure to find task at end
        List<int> freeUseIndex = new List<int>();
        int i = 0;

        if (tasks.Count <= 0) return NodeState.Failed;

        //first check inventory to see if we have an item
        //if yes, find the task which requires this item and select that task
        if(inventory.inventory.Bag.Count > 0)
        {
            //there is an item in the inventory
            for(i = 0; i < tasks.Count; i++)
            {
                if (!tasks[i].RequiresItem)
                {
                    freeUseIndex.Add(i);
                }
                else if (tasks[i].Item.itemName == inventory.inventory.Bag[0].item.itemName)
                {
                    //item matches task
                    index = i;
                    break;
                }

                
            }
        }
       
        //if no, or finding a matching task failed, head to the free use task
        if((inventory.inventory.Bag.Count == 0) || (index == tasks.Count))
        {
            //continue to use i from its previous value (as we have already populated freeUseIndex to this point;
            for(; i < tasks.Count; i++)
            {
                if (!tasks[i].RequiresItem) freeUseIndex.Add(i);
            }


            if(freeUseIndex.Count > 0) index = freeUseIndex[Random.Range((int)0, (int)freeUseIndex.Count)];

        }

        //if failed to find a task which matched the item, and already cleared the free task, re-add a free task and select that
        if((index >= tasks.Count) && (freeTasks.Count > 0))
        {
            tasks.Add(freeTasks[Random.Range((int)0, (int)freeTasks.Count)]);
            index = tasks.Count - 1;
        }

        if (index >= tasks.Count) return NodeState.Failed; //could not find a task to go to

        //index = Random.Range((int)0, tasks.Count); //used during testing only
        myTree.SetData("TaskIndex", index);
        myTree.SetData("TargetPosition", tasks[index].GetPosition());
        Debug.Log("AI Picked Task: " + tasks[index].ToString());
        Debug.Log("Task Location: " + tasks[index].GetPosition().ToString());
        
        return NodeState.Success;
    }

}
