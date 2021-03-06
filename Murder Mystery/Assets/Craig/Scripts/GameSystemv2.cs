//Author: Craig Zeki - Zek21003166
//Author: Jon Fieldhouse - original GameSystem.cs found in Jon's folder

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystemv2 : MonoBehaviour
{

    public enum WinReason : int
    {
        AllTasksComplete = 0,
        TargetKilled,
        TargetAccused,
        //Insert new reasons above this line
        NumOfWinReasons
    }

    private static GameSystemv2 instance;

    private GameStates currentState = GameStates.Menu;
    private GameStates nextState;

    [SerializeField] private TurnSystem turnSystem;
    [SerializeField] private GridSystem gridSystem;

    [SerializeField] private GameObject vrPlayer;
    [SerializeField] private GameObject aiPlayerPrefab;
    [SerializeField] private int numberOfAIPlayers = 3;

    [SerializeField] private GameObject tasklistParent;

    [SerializeField] private Canvas controlsCanvas;
    [SerializeField] private Canvas winCanvas;
    [SerializeField] private Text winPlayerText;
    [SerializeField] private Text winReasonText;

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
    public GameObject TasklistParent { get => tasklistParent; }
    public GameObject VrPlayer { get => vrPlayer;  }
    public Canvas ControlsCanvas { get => controlsCanvas; set => controlsCanvas = value; }



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
            case GameStates.Lose:
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
                    if (winCanvas != null) winCanvas.enabled = false;
                    //spawn characters
                    spawnPlayers();
                    VotingManager.Instance.initializeVotingPanel();
                    if (controlsCanvas != null) controlsCanvas.enabled = true;
                    currentState = NextState;
                }
                else if (nextState == GameStates.Quit)
                {
                    currentState = NextState;
                }
                else { nextState = currentState; }
                break;
            case GameStates.Initializing:
                if (nextState == GameStates.GamePlay)
                {
                    TurnSystem.Instance.StartTurnSystem();
                    currentState = NextState;
                }
                break;
            case GameStates.GamePlay:
                if(nextState == GameStates.Win)
                {
                    Debug.Log("YOU WIN! :-)");
                    currentState = nextState;
                }
                else if(nextState == GameStates.Lose)
                {
                    Debug.Log("YOU LOST! :-(");
                    currentState = nextState;
                }
                else { nextState = currentState; }
                break;
            case GameStates.Pause:
                break;
            case GameStates.Win:
                break;
            case GameStates.Quit:
                break;
            case GameStates.NumOfStates:
                break;
            case GameStates.Lose:
                break;
            default:
                break;
        }
    }

    private void showWinCanvas(CharacterScript winner, WinReason reason)
    {
        if (winner == null) return;
        if (reason >= WinReason.NumOfWinReasons) return;

        switch (reason)
        {
            case WinReason.AllTasksComplete:
                winReasonText.text = "Completed All Tasks";
                winPlayerText.text = winner.characterName;
                if (winner.characterName == vrPlayer.GetComponent<CharacterScript>().characterName) winPlayerText.text = winPlayerText.text + " (You!)";
                winPlayerText.text = winPlayerText.text + " Won!";
                winCanvas.enabled = true;
                break;
            case WinReason.TargetKilled:
                break;
            case WinReason.TargetAccused:
                break;
            case WinReason.NumOfWinReasons:
            default:
                break;
        }
        winCanvas.enabled = true;
    }

    private void spawnPlayers()
    {
        characterPrefabs.Add(vrPlayer);
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


    public void DeclareWin(CharacterScript player, WinReason reason)
    {

        Debug.Log("Player: " + player.characterName + " is the winner! : " + reason.ToString());

        //switch statement to do special thing depending on win reason - does nothing for now - could also be moved to Update - Win State
        switch (reason)
        {
            case WinReason.AllTasksComplete:
                break;
            case WinReason.TargetKilled:
                break;
            case WinReason.TargetAccused:
                break;

            case WinReason.NumOfWinReasons:
            default:
                //Do nothing
                break;
        }

        showWinCanvas(player, reason);

        VRPlayerScript vrPlayer;
        if (player.gameObject.TryGetComponent<VRPlayerScript>(out vrPlayer))
        {
            NextState = GameStates.Win;
        }
        else
        {
            NextState = GameStates.Lose;
        }
        
    }
}
