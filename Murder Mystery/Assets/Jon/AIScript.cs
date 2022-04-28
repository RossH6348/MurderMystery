using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : CharacterScript
{

    public override void onTurnTick()
    {
        Debug.Log("Doing nothing!");
        turnSystem.takeupAction();
    }

}