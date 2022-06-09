using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class DiceScript : MonoBehaviour
{

    private Rigidbody rigid;
    private List<Transform> faces = new List<Transform>();

    public CharacterScript roller;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        //Make the dice unable to be affected by physics immediately.
        rigid.constraints = RigidbodyConstraints.FreezeAll;

        for (int i = transform.childCount - 1; i > -1; i--)
            faces.Add(transform.GetChild(i));
    }
    
    public void onPickup()
    {
        GameSystemv2.Instance.ControlsCanvas.enabled = false;
        //Add an ignore hovering component, which effectively makes it so the player can't pick up the dice again.
        gameObject.AddComponent<IgnoreHovering>();
    }

    public void onEnterRoll()
    {

        //The player on the monitor may have picked up the dice, so check one more time.
        if(gameObject.GetComponent<IgnoreHovering>() == null)
            gameObject.AddComponent<IgnoreHovering>();

        rigid.constraints = RigidbodyConstraints.None; //Unfreeze the dice.

        //And begin checking when the dice stops moving, and to prevent wasting time for their turn trying to roll, we will at least put them in an action state.
        //Without taking the dice roll as an actual action.
        StartCoroutine(RollingDice());
    }

    IEnumerator RollingDice()
    {

        roller.status = turnStatus.Action;
        //Wait until the dice stop moving and not attached.
        while(!rigid.IsSleeping())
        {
            yield return new WaitForEndOfFrame();
        }

        //Set back to play action, and add whatever they rolled to their maxroll variable, by finding which face is closest to the up vector.
        Transform closestFace = null;
        foreach(Transform face in faces)
        {

            if (closestFace == null || Vector3.Dot(face.forward,Vector3.up) > Vector3.Dot(closestFace.forward,Vector3.up))
                closestFace = face;

        }

        roller.maxRoll += int.Parse(closestFace.name);
        roller.status = turnStatus.Play;

        //Wait two more seconds, before disabling the dice.
        yield return new WaitForSeconds(2.0f);

        Destroy(gameObject);

    }

}
