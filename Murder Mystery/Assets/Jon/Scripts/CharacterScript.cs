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
    public string characterName = "Matey";

    //Assassin's target.
    public GameObject target;

    //ROLE AND STATUS OF THIS PARTICULAR CHARACTER.
    public Role role = Role.Suspect;
    public turnStatus status = turnStatus.Wait;

    public TurnSystem turnSystem;

    public GridSystem gridSystem; //So the player and AI can make use of the pathfinding system.

    public int maxRoll = 0; //How far can they move?

    //This stores the dice granted, just so we have a way of removing it if their turn ends and they didn't roll the dice.
    protected GameObject playerDice;

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

        if (playerDice != null)
            Destroy(playerDice);

    }

    public virtual void onTurnTick()
    {

    }

    public virtual void MoveTo(Vector3 nodePosition)
    {
        
    }

    public virtual void grantDice(GameObject dice)
    {

        dice.GetComponent<DiceScript>().roller = this; //Automatically set the roller to this script.

        playerDice = dice;

    }


    


}