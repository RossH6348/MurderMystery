using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();

    private int currentTurn = 0;

    public float turnTime = 12f;

    private int remainingActions = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentTurn = 0;
        startTurn();
    }

    // Update is called once per frame
    void Update()
    {

        CharacterScript character = players[currentTurn].GetComponent<CharacterScript>();
        if (turnTime > 0.0f && remainingActions > 0)
        {
            //If the character still got actions left to do while timer is there, keep counting down.
            if (character.status != turnStatus.Action)
                turnTime -= Time.deltaTime;
            else
                turnTime = 12.0f;
        }
        else
        {
            //The character have ran out of time or no more actions to make, set their turn as finished and move onto next character.
            if(character.status == turnStatus.Play)
            {
                character.status = turnStatus.Wait;
                character.onTurnExit();
                nextPlayer();
            }
        }
    }

    private void startTurn()
    {
        //Start timer and give amount of actions they can perform.
        turnTime = 12.0f;
        //Set the character's turn status to play.
        GameObject activeCharacter = players[currentTurn];

        CharacterScript character = activeCharacter.GetComponent<CharacterScript>();

        //Check if this character's role happened to be the assassin, if so give 2 turns instead of 1.
        if (character.role == Role.Assassin)
            remainingActions = 2;
        else
            remainingActions = 1;

        //Update status to play, so their controls can be activated.
        character.status = turnStatus.Play;

        character.onTurnEnter();

        Debug.Log("It is currently: " + character.characterName + "'s turn!");

    }
    private void nextPlayer()
    {
        currentTurn++;
        if (currentTurn >= players.Count)
            currentTurn = 0;
        startTurn();
    }

    public void takeupAction()
    {
        remainingActions--;
    }

    public int getRemainingActions()
    {
        return remainingActions;
    }

    //This will return the transform of a character requested.
    //Allowing AI to follow other players for instance.
    public Transform getPlayerTransform(string name)
    {
        for (int i = 0; i < players.Count; i++)
            if (players[i].GetComponent<CharacterScript>().characterName.Equals(name))
                return players[i].transform;
        return null;
    }
}