using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSystemv2 : MonoBehaviour
{
    private static GameSystemv2 instance;

    private GameStates currentState = GameStates.Menu;
    private GameStates nextState;

    [SerializeField] private TurnSystem turnSystem;
    [SerializeField] private GridSystem gridSystem;

    [SerializeField] private GameObject vrPlayerPrefab;
    [SerializeField] private GameObject aiPlayerPrefab;
    [SerializeField] private int numberOfAIPlayers = 3;

    private List<GameObject> characterPrefabs = new List<GameObject>();
    private List<GameObject> characters = new List<GameObject>();

    public GameStates NextState
    {
        get => nextState; set
        {
            nextState = value;
            DoTransition();
        }
    }

    public static GameSystemv2 Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameSystemv2>();
            return instance;
        }
    }

    public TurnSystem TurnSystem { get => turnSystem;  }
    public GridSystem GridSystem { get => gridSystem;  }
    public List<GameObject> Characters { get => characters; }
    public GameStates CurrentState { get => currentState; }



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
                NextState = GameStates.GamePlay;
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
                if(nextState == GameStates.GamePlay)
                {
                    TurnSystem.Instance.StartTurnSystem();
                    currentState = NextState;
                }
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
        characterPrefabs.Add(vrPlayerPrefab);
        for(int i = 0; i < numberOfAIPlayers; i++)
        {
            characterPrefabs.Add(aiPlayerPrefab);
        }
        characterPrefabs.Shuffle();

        SpawnManagerv2.Instance.SpawnAllPlayers(characterPrefabs, ref characters);

        //foreach(GameObject characterPrefab in characterPrefabs)
        //{
        //    characters.Add(SpawnManager.Instance.Spawn(characterPrefab));
        //}
    }
}
