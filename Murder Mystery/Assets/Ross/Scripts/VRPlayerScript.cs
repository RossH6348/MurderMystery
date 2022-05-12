using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VRPlayerScript : CharacterScript
{

    [SerializeField] private Transform pointer;

    private Transform attachmentPoint;

    private GameObject laser;
    private GameObject selectedObject = null;


    //VR Control scheme.
    public SteamVR_Action_Boolean triggerAction;


    // Start is called before the first frame update
    private void Awake()
    {
        laser = GameObject.CreatePrimitive(PrimitiveType.Cube);
        laser.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
        laser.transform.position = pointer.position;
        laser.transform.SetParent(pointer);

        BoxCollider collider = laser.GetComponent<BoxCollider>();
        if (collider != null)
            Destroy(collider);

        laser.SetActive(false);

        //This will be used to check if the player is holding something, so we don't do the laser thing.
        attachmentPoint = pointer.Find("ObjectAttachmentPoint");
    }

    public override void onTurnEnter()
    {

    }

    public override void onTurnTick()
    {

        GameObject laserSelected = null;

        if(GetComponentInChildren<Interactable>() == null)
        {
            //Allow the VR player to carry out their controls with the laser pointer.
            RaycastHit result;
            if (Physics.Raycast(pointer.position, pointer.forward, out result, 9999.0f))
            {

                if(!laser.activeSelf)
                    laser.SetActive(true);

                //Position and rotate the laser correctly, so show the player what they are pointing at.
                laser.transform.localPosition = new Vector3(0.0f, 0.0f, (pointer.position - result.point).magnitude * 0.5f);
                laser.transform.localScale = new Vector3(0.025f, 0.025f, (pointer.position - result.point).magnitude);

                laserSelected = result.collider.gameObject;

            }
            else if(laser.activeSelf)
            {
                laser.SetActive(false);
            }
        }
        else if (laser.activeSelf)
        {
            laser.SetActive(false);
        }

        if(laserSelected != selectedObject)
        {
            //Okay, a diffferent object have been selected, we need to do the appropriate onLaserSelected and onLaserDeselected.
            if (selectedObject != null)
            {
                LaserInput input = selectedObject.GetComponent<LaserInput>();
                if(input != null)
                    input.onLaserDeselected(this); //We will pass in our own CharacterScript so the object selected knows who have deselected.
            }

            if(laserSelected != null)
            {
                selectedObject = laserSelected;
                LaserInput input = selectedObject.GetComponent<LaserInput>();
                if (input != null)
                    input.onLaserSelected(this); //We will pass in our own CharacterScript so the object selected knows who have selected.
            }
        }

        if (triggerAction.GetStateDown(SteamVR_Input_Sources.RightHand) && selectedObject != null)
        {
            LaserInput input = selectedObject.GetComponent<LaserInput>();
            if (input != null)
                input.onLaserClick(this);
        }
    }

    public override void onTurnExit()
    {

        //There turn is over, make sure to deselect everything.
        if (selectedObject != null)
        {
            LaserInput input = selectedObject.GetComponent<LaserInput>();
            if (input != null)
                input.onLaserDeselected(this); //We will pass in our own CharacterScript so the object selected knows who have deselected.
        }

        selectedObject = null;

        //Turn off their laser as well.
        laser.SetActive(false);

        base.onTurnExit(); //Go back to the main, which will handle removing unrolled dice for us.

    }

    public override void MoveTo(Vector3 nodePosition)
    {
        //Get the position from the player's camera down to the floor as start position.
        Vector3 startPos = transform.position;

        List<GameObject> Path = gridSystem.findPath(startPos, nodePosition);

        //Check if they rolled high enough to cover the path.
        if (Path != null && Path.Count <= maxRoll)
        {
            //Yes they have! They can move this many spaces, start coroutine loop of moving node by node during action phase, while taking up an action.
            StartCoroutine(movePath(Path, transform, 0.5f));

            maxRoll -= Path.Count; //They may have not move all of there spaces in one go, so subtract by path count.

            turnSystem.takeupAction();

        }

    }

    public override void grantDice(GameObject dice)
    {

        //We need to position this dice infront of the player.
        dice.transform.position = transform.position + transform.forward + new Vector3(0.0f, 1.0f, 0.0f);

        base.grantDice(dice); //We need to return to the base function, which will automatically set the roller of the dice for us.

    }

}
