using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZekstersLab.BehaviourTree;

public class AI_Roll_Die : ActionNode
{
    public int numOfSides = 6;
    public override void Pause()
    {
        //Do nothing
    }

    protected override void OnStart()
    {
        //Do nothing
    }

    protected override void OnStop()
    {
        //Do nothing
    }

    protected override NodeState OnUpdate()
    {
        int diceRoll = Random.Range((int)1, numOfSides+1);
        myTree.SetData("DiceRoll", diceRoll);
        Debug.Log("AI rolled a: " + diceRoll.ToString());
        return NodeState.Success;
    }
}
