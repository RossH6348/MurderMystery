using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZekstersLab.BehaviourTree;
public class AIScript : CharacterScript
{
    private BehaviourTree myTree;

    private void Awake()
    {
        myTree = ScriptableObject.CreateInstance<BehaviourTree>();

        var log1 = ScriptableObject.CreateInstance<DebugLogNode>();
        log1.message = "log1";
        var log2 = ScriptableObject.CreateInstance<DebugLogNode>();
        log2.message = "log2";
        var log3 = ScriptableObject.CreateInstance<DebugLogNode>();
        log3.message = "log3";
        var log4 = ScriptableObject.CreateInstance<DebugLogNode>();
        log4.message = "log4";

        var wait1 = ScriptableObject.CreateInstance<WaitNode>();
        wait1.duration = 12;

        var wait2 = ScriptableObject.CreateInstance<WaitNode>();
        wait2.duration = 12;

        var sequence1 = ScriptableObject.CreateInstance<SequenceNode>();
        sequence1.children.Add(log1);
        sequence1.children.Add(wait1);
        sequence1.children.Add(log2);

        var inverter1 = ScriptableObject.CreateInstance<InverterNode>();
        inverter1.child = log3;

        var selector1 = ScriptableObject.CreateInstance<SelectorNode>();
        //selector1.children.Add(inverter1);
        selector1.children.Add(log3);
        
        //selector1.children.Add(wait2);
        selector1.children.Add(log4);

        var sequence2 = ScriptableObject.CreateInstance<SequenceNode>();
        sequence2.children.Add(sequence1);
        sequence2.children.Add(selector1);

        myTree.root = sequence2;

    }
    public override void onTurnEnter()
    {
        myTree.Reset();
    }

    public override void onTurnTick()
    {
        
        if(myTree.treeState == NodeState.Success || myTree.treeState == NodeState.Failed)
        {
            turnSystem.takeupAction();
        }
        else
        {
            myTree.Update();
        }
        
    }

    public override void onTurnExit()
    {
        if (myTree.treeState == NodeState.Running)
        {
            myTree.Pause();
        }
    }

}