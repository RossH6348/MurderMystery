//Author: Craig Zeki - Zek21003166


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZekstersLab.BehaviourTree;

public class AI_AssassinModeConditionCheck : DecoratorNode
{
    private Role role;
    private GameObject assassinTarget = null;
    private PlayerInventory inventory = null;
    public override void Pause()
    {
        //Do nothing
    }

    protected override void OnStart()
    {
        role = (Role)myTree.GetData("Role");
        assassinTarget = (GameObject)myTree.GetData("AssassinTarget");
        inventory = (PlayerInventory)myTree.GetData("Inventory");
    }

    protected override void OnStop()
    {
        //Do nothing
    }

    protected override NodeState OnUpdate()
    {
        if (inventory.inventory.Bag.Count <= 0) { Debug.Log("Bag was empty"); return NodeState.Failed; }
        if( (inventory.inventory.Bag[0].item.type == ItemID.Weapon) &&
            (role == Role.Assassin) &&
            (assassinTarget != null) )
        {
            Debug.Log("*************Assassin Mode Active***************");
            return child.Update();
        }

        return NodeState.Failed;
    }
}
