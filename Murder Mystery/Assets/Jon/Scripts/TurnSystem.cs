using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();

    private int currentTurn = 0;

    [SerializeField] private float startingTurnTime = 60.0f;
    public float turnTime = 0.0f;

    private int remainingActions = 0;

    [SerializeField] private GameObject dicePrefab; //The dice prefab to give to other players.

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
        turnTime = startingTurnTime; ;
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

        if(activeCharacter.GetComponent<AIScript>() == null)
        {
            //This is a player or VR player, we can give them a physical dice to use.
            character.grantDice(Instantiate(dicePrefab));
        }
        else
        {
            //Automatically just give the AI it's movement amount.
            character.maxRoll += Random.Range(1, 6);
        }

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