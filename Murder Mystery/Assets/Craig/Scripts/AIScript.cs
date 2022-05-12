using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ZekstersLab.BehaviourTree;
public class AIScript : CharacterScript
{
    private BehaviourTree myTree;

    private void Awake()
    {
        

        myTree = ScriptableObject.CreateInstance<BehaviourTree>();

        var diceRoll = ScriptableObject.CreateInstance<AI_Roll_Die>();
        var testDie = ScriptableObject.CreateInstance<AI_Test_Die>();

        var moveSequence = ScriptableObject.CreateInstance<SequenceNode>();
        moveSequence.children.Add(diceRoll);
        moveSequence.children.Add(testDie);

        myTree.SetRoot(moveSequence);

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