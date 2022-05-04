using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : CharacterScript
{

    private void Awake()
    {
        name = "Jon";
    }

    public override void onTurnTick()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Doing nothing!");
            turnSystem.takeupAction();
        }
    }

}