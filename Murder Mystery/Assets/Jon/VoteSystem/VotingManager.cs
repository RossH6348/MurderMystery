using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VotingManager : MonoBehaviour
{
    private int maxVotes = 3;
    private int votesRemaining;
    private bool voteCast = false;
    private bool buttonPressed = false;
    [SerializeField] private GameObject verticalLayout;
    [SerializeField] private GameObject characternamePrefab;

    private static VotingManager instance;

    public static VotingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<VotingManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {

    }

    // Start is called before the first frame update
    public void initializeVotingPanel()
    {
        if (characternamePrefab == null) return;
        if (verticalLayout == null) return;

        foreach (GameObject character in GameSystemv2.Instance.Characters)
        {
            GameObject temp = Instantiate(characternamePrefab, verticalLayout.transform);
            if (character.TryGetComponent<CharacterScript>(out CharacterScript characterScript))
            {
                Text tempText = temp.GetComponentInChildren<Text>();
                if(tempText != null)
                {
                    tempText.text = characterScript.characterName;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
