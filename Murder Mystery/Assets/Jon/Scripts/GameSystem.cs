using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates
{
    Menu = 0,
    Initializing,
    GamePlay,
    Pause,
    Win,
    Quit,
    Lose,
    //Add new states
    NumOfStates
}
public class GameSystem : MonoBehaviour
{
    private GameStates currentState = GameStates.Menu;
    private GameStates nextState;

    [SerializeField] private List<GameObject> characterPrefabs = new List<GameObject>();
    private List<GameObject> characters = new List<GameObject>();

    public GameStates NextState
    {
        get => nextState; set
        {
            nextState = value;
            DoTransition();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        NextState = currentState;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameStates.Menu:
                NextState = GameStates.Initializing;
                break;
            case GameStates.Initializing:
                break;
            case GameStates.GamePlay:
                break;
            case GameStates.Pause:
                break;
            case GameStates.Win:
                break;
            case GameStates.Quit:
                break;
            case GameStates.NumOfStates:
                break;
            default:
                break;
        }
    }

    private void DoTransition()
    {
        switch (currentState)
        {
            case GameStates.Menu:
                if (nextState == GameStates.Initializing)
                {
                    //spawn characters
                    spawnPlayers();
                    currentState = NextState;
                }
                else if (nextState == GameStates.Quit)
                {
                    currentState = NextState;
                }
                else { nextState = currentState; }
                break;
            case GameStates.Initializing:
                break;
            case GameStates.GamePlay:
                break;
            case GameStates.Pause:
                break;
            case GameStates.Win:
                break;
            case GameStates.Quit:
                break;
            case GameStates.NumOfStates:
                break;
            default:
                break;
        }
    }

    private void spawnPlayers()
    {
        foreach(GameObject characterPrefab in characterPrefabs)
        {
            characters.Add(SpawnManager.Instance.Spawn(characterPrefab));
        }
    }
}
