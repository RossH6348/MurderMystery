//Author: Craig Zeki - Zek21003166


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ZekstersLab.BehaviourTree;

[RequireComponent(typeof(PlayerInventory))]
public class AIScript : CharacterScript
{
    private BehaviourTree myTree;
    private Vector3 targetPos = Vector3.zero;

    [SerializeField] private Transform targetTransform;

    private List<Task> tasks = new List<Task>();
    private bool takeUpAction = false;
    private PlayerInventory inventory = null;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        if (inventory == null) Debug.LogError("Error: AI Requires a Player Inventory");
    }

    public void initAI()
    {
        tasks.AddRange(GameSystemv2.Instance.TasklistParent.GetComponentsInChildren<Task>());

        myTree = ScriptableObject.CreateInstance<BehaviourTree>();


        myTree.SetData("GridSystem", gridSystem);
        myTree.SetData("TaskList", tasks);
        myTree.SetData("Inventory", inventory);
        myTree.SetData("Role", role);
        

        var assassinPickRoom = ScriptableObject.CreateInstance<AI_IdentifyTargetRoom>();
        var assassinRoleDice = ScriptableObject.CreateInstance<AI_Roll_Die>();
        var assassinMoveToTarget = ScriptableObject.CreateInstance<AI_MoveToTarget>();
        assassinMoveToTarget.moveSpeed = 0.5f;

        var assassinSequence = ScriptableObject.CreateInstance<SequenceNode>();
        assassinSequence.children.Add(assassinPickRoom);
        assassinSequence.children.Add(assassinRoleDice);
        assassinSequence.children.Add(assassinMoveToTarget);

        var assassinConditionCheck = ScriptableObject.CreateInstance<AI_AssassinModeConditionCheck>();
        assassinConditionCheck.child = assassinSequence;

        var diceRoll = ScriptableObject.CreateInstance<AI_Roll_Die>();
        var moveToTarget = ScriptableObject.CreateInstance<AI_MoveToTarget>();
        var taskPicker = ScriptableObject.CreateInstance<AI_PickTask>();

        moveToTarget.moveSpeed = 0.5f;

        var moveSequence = ScriptableObject.CreateInstance<SequenceNode>();
        moveSequence.children.Add(diceRoll);
        moveSequence.children.Add(moveToTarget);

        var repeatUntilAtTarget = ScriptableObject.CreateInstance<AI_RepeatUntillAtTarget>();
        repeatUntilAtTarget.child = moveSequence;

        var doTask = ScriptableObject.CreateInstance<AI_DoTask>();

        var gotoAndDoNextTask = ScriptableObject.CreateInstance<SequenceNode>();
        gotoAndDoNextTask.children.Add(taskPicker);
        gotoAndDoNextTask.children.Add(repeatUntilAtTarget);
        gotoAndDoNextTask.children.Add(doTask);

        var getRandomSpawnPoint = ScriptableObject.CreateInstance<AI_SetTargetToRandomSpawnPoint>();
        var moveToSpawnPoint = ScriptableObject.CreateInstance<AI_MoveToTarget>();
        moveToSpawnPoint.moveSpeed = 0.5f;

        var moveToSpawnPointSequence = ScriptableObject.CreateInstance<SequenceNode>();
        moveToSpawnPointSequence.children.Add(getRandomSpawnPoint);
        moveToSpawnPointSequence.children.Add(moveToSpawnPoint);

        var stuckInPlaceCondition = ScriptableObject.CreateInstance<AI_StuckInPlaceConditionTrue>();
        stuckInPlaceCondition.child = moveToSpawnPointSequence;

        var taskGroupSelector = ScriptableObject.CreateInstance<SelectorNode>();
        taskGroupSelector.children.Add(gotoAndDoNextTask);
        taskGroupSelector.children.Add(stuckInPlaceCondition);

        var aiRootSelector = ScriptableObject.CreateInstance<SelectorNode>();
        aiRootSelector.children.Add(assassinConditionCheck);
        aiRootSelector.children.Add(taskGroupSelector);

        myTree.SetRoot(aiRootSelector);

        //var log1 = ScriptableObject.CreateInstance<DebugLogNode>();
        //var log2 = ScriptableObject.CreateInstance<DebugLogNode>();
        //var log3 = ScriptableObject.CreateInstance<DebugLogNode>();
        //var log4 = ScriptableObject.CreateInstance<DebugLogNode>();
        //log1.message = "Hello from Behaviour Tree - Log 1";
        //log2.message = "Hello from Behaviour Tree - Log 2";
        //log3.message = "Hello from Behaviour Tree - Log 3";
        //log4.message = "Hello from Behaviour Tree - Log 4";

        //var invertNode = ScriptableObject.CreateInstance<InverterNode>();
        //invertNode.child = log3;

        //var waitNode = ScriptableObject.CreateInstance<WaitNode>();
        //waitNode.duration = 2;

        //var sequence1 = ScriptableObject.CreateInstance<SequenceNode>();
        //sequence1.children.Add(log1);
        //sequence1.children.Add(waitNode);
        //sequence1.children.Add(log2);

        //var selector1 = ScriptableObject.CreateInstance<SelectorNode>();
        //selector1.children.Add(log3);
        //selector1.children.Add(log4);

        //var sequence2 = ScriptableObject.CreateInstance<SequenceNode>();
        //sequence2.children.Add(sequence1);
        //sequence2.children.Add(selector1);

        //myTree.SetRoot(sequence2);
    }

    public override void onTurnEnter()
    {
        targetTransform = transform;
        myTree.SetData("TurnOver", (bool)false);
        myTree.Reset();
    }

    public override void onTurnTick()
    {

        if (myTree.treeState == NodeState.Success || myTree.treeState == NodeState.Failed)
        {
            Debug.Log("Tree State: " + myTree.treeState.ToString());
            turnSystem.takeupAction();
            myTree.Reset();
        }
        else
        {
            //myTree.SetData("TakeUpAction", (bool)false);
            takeUpAction = false;
            targetPos = targetTransform.position;
            myTree.SetData("CurrentTransform", transform);
            myTree.SetData("CurrentPosition", transform.position);
            myTree.SetData("TakeUpAction", takeUpAction);
            myTree.SetData("AssassinTarget", target);

            //myTree.SetData("TargetPosition", targetPos);


            if (myTree.Update() == NodeState.Running)
            {
                if((bool)myTree.GetData("TakeUpAction")) turnSystem.takeupAction();
            }
            //transform.position = (Vector3)myTree.GetData("CurrentPosition");
        }

        if (tasks.Count == 0) GameSystemv2.Instance.DeclareWin(this, GameSystemv2.WinReason.AllTasksComplete);


    }

    public override void onTurnExit()
    {
        if (myTree.treeState == NodeState.Running)
        {
            myTree.SetData("TurnOver", (bool)true);
            myTree.Pause();
        }
    }

}