//Author: Craig Zeki - Zek21003166

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZekstersLab.BehaviourTree;
public class AI_DoTask : ActionNode
{
    private enum doTaskState : int
    {
        PresentItem = 0,
        ShowingItem,
        DoTask,
        WaitForReward,
        CollectReward,
        //Add new states above this line
        NumOfStates
    }

    private float showDuration = 1.5f;
    private float syncBufferTime = 0.5f;
    private float elapsedCoroutineTime = 0;

    private List<Task> taskList = new List<Task>();
    private int index = 0;
    private PlayerInventory inventory;
    private doTaskState currentState;
    private float elapsedTime = 0f;

    private ItemObject item = null;
    private GameObject temp = null;

    private Coroutine showItemCoroutine = null;

    private doTaskState previousState = doTaskState.PresentItem;

    public override void Pause()
    {
        //Do nothing
    }

    protected override void OnStart()
    {
        Debug.Log("AI_DoTask - OnStart()");
        elapsedCoroutineTime = 0;
        showItemCoroutine = null;
        item = null;
        temp = null;
        currentState = doTaskState.PresentItem;
        //taskList.AddRange((List<Task>)myTree.GetData("TaskList"));
        taskList= (List<Task>)myTree.GetData("TaskList");
        index = (int)myTree.GetData("TaskIndex");
        inventory = (PlayerInventory)myTree.GetData("Inventory");
        elapsedTime = 0f;
    }

    protected override void OnStop()
    {
        Debug.Log("AI_DoTask OnStop");
    }

    

    protected override NodeState OnUpdate()
    {
        
        switch (currentState)
        {
            case doTaskState.PresentItem:
                if(taskList[index].RequiresItem)
                {
                    Debug.Log("Task" + taskList[index].name + " Started");
                    //get the item from the inventory
                    //spawn it at the correct  task point
                    //manually call the DoTask function as the collider is looking for the item to be in a 'hand'
                    if (inventory.inventory.Bag.Count <= 0) return NodeState.Failed; //arrived at a task which required an item and we don't have one
                    if (inventory.inventory.Bag[0].item.itemName != taskList[index].Item.itemName) return NodeState.Failed; //arrived at a task which we don't have an item for

                    item = inventory.FirstItemRequest();
                    Vector3 tempOffset = Vector3.Scale(taskList[index].PosOffset, new Vector3(1, 0.5f, 1));
                    temp = Instantiate(item.prefab3d, taskList[index].TaskItemPoint.position + tempOffset, taskList[index].TaskItemPoint.rotation);
                    if(temp == null)
                    {
                        Debug.Log("AI_DoTask - Could not spawn item from inventory - temp = null");
                        return NodeState.Failed;
                    }
                    //show the item and then move to next state
                    showItemCoroutine = Helpers.Instance.StartCoroutine(Helpers.Instance.showPresentedItem(showDuration, () => { previousState = currentState; currentState = doTaskState.DoTask; }));
                    previousState = currentState;
                    currentState = doTaskState.ShowingItem;
                }
                else
                {
                    //does not require an item to be spwaned
                    //if we have a current item, we should abandon it now
                    ItemDeck.Instance.ReturnToDeck(inventory.FirstItemRequest()); //item is abandoned, return it to the deck;
                    Debug.Log("Task" + taskList[index].name + " In Progress...");
                    taskList[index].DoTask(null, null);
                    previousState = currentState;
                    currentState = doTaskState.WaitForReward;
                }
                break;

            case doTaskState.ShowingItem:
                //waiting for the coroutine to finish, perform a check to make sure it does not lock up
                elapsedCoroutineTime += Time.deltaTime;
                if ((showItemCoroutine == null) || (elapsedCoroutineTime > (showDuration+syncBufferTime)))
                {
                    if (showItemCoroutine != null) Helpers.Instance.StopCoroutine(showItemCoroutine);

                    Debug.LogError("AI_DoTask: ShowPresentedItem Coroutine timed out");
                    return NodeState.Failed;
                }
                break;

            case doTaskState.DoTask:
                if(temp == null)
                {
                    Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! AI_DoTask: currentState = DoTask and temp = Null - should not be possible !!!!!!!!!!!!!!!!!!!!!!!!!");
                    //any spawned item will return to deck automatically
                    return NodeState.Failed;
                    
                }
                Debug.Log("Task" + taskList[index].name + " In Progress...");
                taskList[index].DoTask(item, temp);
                
                previousState = currentState;
                currentState = doTaskState.WaitForReward;
                break;

            case doTaskState.WaitForReward:
                elapsedTime += Time.deltaTime;
                if(elapsedTime > taskList[index].TaskCompletionDelay)
                {
                    if(taskList[index].TaskReward != null)
                    {
                        previousState = currentState;
                        currentState = doTaskState.CollectReward;
                    }

                    //allow re-try up to 1s incase of slight timing issue with coroutine
                    if(elapsedTime > taskList[index].TaskCompletionDelay + 1)
                    {
                        Debug.Log("Task" + taskList[index].name + " Timed Out");
                        return NodeState.Failed;
                    }

                }
                
                break;
            case doTaskState.CollectReward:
                //gather the item into the inventory
                inventory.ItemCollect(taskList[index].TaskReward);
                taskList.RemoveAt(index);
                Debug.Log("Task Completed");
                string tempStr = "Remaining tasks: ";
                foreach(Task task in taskList)
                {
                    tempStr = tempStr + task.name + "     ";
                }
                Debug.Log(tempStr);
                previousState = currentState;
                currentState = doTaskState.PresentItem;
                myTree.SetData("TakeUpAction", (bool)true);
                return NodeState.Success;
                //break; not needed due to previous return

            case doTaskState.NumOfStates:
            default:
                break;
        }

        return NodeState.Running;

        //Testing code below here
        //WeaponObject temp = new WeaponObject();
        //GameObject temp1 = new GameObject();
        //if(taskList[index].DoTask(temp, temp1))
        //{
        //    taskList.RemoveAt(index);
        //    Debug.Log("Task Completed");
            
        //    return NodeState.Success;
        //}
        //Debug.Log("Task Failed");
        //return NodeState.Failed;
    }
}
