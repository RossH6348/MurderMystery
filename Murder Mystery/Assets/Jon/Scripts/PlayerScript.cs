using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : CharacterScript
{

    public override void onTurnTick()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Doing nothing!");
            turnSystem.takeupAction();
        }
    }

}