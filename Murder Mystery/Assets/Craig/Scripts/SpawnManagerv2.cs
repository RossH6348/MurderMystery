//Author: Craig Zeki - Zek21003166
//Author: Jon Fieldhouse - original SpawnManager.cs in Jon's folder

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SpawnManagerv2 : MonoBehaviour
{
    [SerializeField] private GameObject listOfSpawnPoints;
    [SerializeField] private List<Material> playerCharacterMaterials = new List<Material>();
    
    private List<Transform> spawnPoints = new List<Transform>();
    
    private int spawnPointIndex = -1;
    private static SpawnManagerv2 instance;

    private int getSpawnPointIndex = 0;

    public static SpawnManagerv2 Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<SpawnManagerv2>();
            }
            return instance;
        }
    }

    void Awake()
    {
        

        foreach (Transform pointTransform in listOfSpawnPoints.GetComponentsInChildren<Transform>())
        {
            spawnPoints.Add(pointTransform);
        }
        if(spawnPoints.Count > 0)
        {
            spawnPoints.RemoveAt(0);
        }
        spawnPoints.Shuffle();

        playerCharacterMaterials.Shuffle();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnAllPlayers(List<GameObject> characterPrefabs, ref List<GameObject> characters)
    {
        bool assassinAssigned = false;
        characters.Clear();
        foreach(GameObject character in characterPrefabs)
        {
            GameObject tempCharacter = Spawn(character);
            if (tempCharacter == null) break;
            tempCharacter.name += " P" + (characters.Count + 1).ToString();

            CharacterScript characterScript;
            AIScript aIScript;
            PlayerColourizer playerColourizer;
            if (tempCharacter.TryGetComponent<CharacterScript>(out characterScript))
            {
                characterScript.turnSystem = GameSystemv2.Instance.TurnSystem;
                characterScript.gridSystem = GameSystemv2.Instance.GridSystem;
                characterScript.characterName = "Player: " + (characters.Count + 1).ToString();
                if(assassinAssigned == false)
                {
                    characterScript.role = Role.Assassin;
                    assassinAssigned = true;
                }
                else
                {
                    characterScript.role = Role.Suspect;
                }
                
            }
            //if character is an AI, initialize - must be done after character script is setup with grid system
            if(tempCharacter.TryGetComponent<AIScript>(out aIScript))
            {
                aIScript.initAI();
            }

            playerColourizer = tempCharacter.GetComponentInChildren<PlayerColourizer>();

            //if(tempCharacter.TryGetComponent<PlayerColourizer>(out playerColourizer))
            if(playerColourizer != null)
            {
                playerColourizer.SetMaterial(playerCharacterMaterials[characters.Count]);
            }
            
            //select the starting item from the deck
            if(tempCharacter.TryGetComponent<PlayerInventory>(out PlayerInventory inventory))
            {
                inventory.ItemCollect(ItemDeck.Instance.DrawFromDeck());
            }

            characters.Add(tempCharacter);

        }

        //set assassains target (assassin is always assigned first)
        if(characters.Count >= 2) characters[0].GetComponent<CharacterScript>().target = characters[Random.Range(1, characters.Count - 1)];
    }

    public GameObject Spawn(GameObject characterPrefab)
    {
        

        if (characterPrefab == null) return null;
        spawnPointIndex++;
        if (spawnPointIndex >= spawnPoints.Count) return null;

        VRPlayerScript temp;
        if(characterPrefab.TryGetComponent<VRPlayerScript>(out temp))
        {
            //need to move the existing player, not spawn - workaround for VIVE headset not allowing spawning of VR device
            characterPrefab.transform.position = spawnPoints[spawnPointIndex].position;
            characterPrefab.transform.rotation = spawnPoints[spawnPointIndex].rotation;
            return characterPrefab;
        }

        GameObject tempGO = Instantiate(characterPrefab, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        //register the room that the player is in
        RoomManager.Instance.RegisterInRoom(tempGO, spawnPoints[spawnPointIndex].GetComponent<SpawnPoint>().Room);

        return tempGO;
       
    }

    //GetRandomSpawnPoint - provides the AI with a way to pick a random space if no other places left to move to
    //helps prevent dead-locking of AI players
    public Transform GetRandomSpawnPoint()
    {
        //spawn points are already randomized, will use an index to track through the list
        //this allows for unique (and still random) referencing and prevents several calls to random
        //if called multiple times - essentially provides psuedo random

        if (spawnPoints.Count <= 0) return null;
        if (getSpawnPointIndex >= spawnPoints.Count) getSpawnPointIndex = 0; //wrap back to start

        return spawnPoints[getSpawnPointIndex++]; //post-increment ready for next call
    }
}
