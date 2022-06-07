using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class VoteButton : UIElement
{
    private string characterName;
    [SerializeField] private Text characterText;
    [SerializeField] private CharacterScript myCharacterScript;

    public string CharacterName
    {
        get => characterName; set
        {
            characterName = value;
            if (characterText != null) characterText.text = characterName;
        }
    }

    public CharacterScript MyCharacterScript { get => myCharacterScript; set => myCharacterScript = value; }

    public void clearSelection()
    {
        //Update image on button to hide X (change button state somehow?)
    }

    protected override void OnButtonClick()
    {
        if (myCharacterScript == null) return;
        base.OnButtonClick();

        VotingManager.Instance.makeSelection(characterName, myCharacterScript.characterName);
        //call function in the VotingManager to inform of current selection
        //Update image on button to show X
    }




}
