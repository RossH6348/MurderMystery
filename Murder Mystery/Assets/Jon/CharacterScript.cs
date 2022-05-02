using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Role
{
    Suspect = 1,
    Assassin = 2
}

public enum turnStatus
{
    Play = 1,
    Wait = 2,
    Action = 4
}


public class CharacterScript : MonoBehaviour
{

    //THE NAME OF THIS CHARACTER.
    public string NAME = "Matey";

    //Assassin's target.
    public string target = "";

    //ROLE AND STATUS OF THIS PARTICULAR CHARACTER.
    public Role role = Role.Suspect;
    public turnStatus status = turnStatus.Wait;

    public TurnSystem turnSystem;

    public GridSystem gridSystem; //So the player and AI can make use of the pathfinding system.

    protected int maxRoll = 10; //How far can they move?

    private void Update()
    {
        if(status == turnStatus.Play)
            onTurnTick();
    }

    public virtual void onTurnEnter()
    {

    }

    public virtual void onTurnExit()
    {

    }

    public virtual void onTurnTick()
    {

    }

    public virtual void MoveTo(Vector3 nodePosition)
    {
        
    }

}