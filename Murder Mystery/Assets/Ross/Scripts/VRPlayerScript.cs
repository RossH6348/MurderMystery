using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerScript : CharacterScript
{

    [SerializeField] private Transform pointer;

    private Transform attachmentPoint;

    private GameObject laser;
    private GameObject selectedObject = null;

    // Start is called before the first frame update
    private void Awake()
    {
        name = "Ross";

        laser = GameObject.CreatePrimitive(PrimitiveType.Cube);
        laser.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        laser.transform.position = pointer.position;

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

        if(attachmentPoint.childCount < 1)
        {
            //Allow the VR player to carry out their controls with the laser pointer.
            RaycastHit result;
            if (Physics.Raycast(pointer.position, pointer.forward, out result, 9999.0f))
            {

                if(!laser.activeSelf)
                    laser.SetActive(true);

                //Position and rotate the laser correctly, so show the player what they are pointing at.
                laser.transform.position = (pointer.position + result.point) * 0.5f;
                laser.transform.localScale = new Vector3(0.05f, 0.05f, (pointer.position - result.point).magnitude);
                laser.transform.rotation *= Quaternion.FromToRotation(laser.transform.forward, pointer.transform.forward);

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

        if(Input.GetKeyDown(KeyCode.Space) && selectedObject != null)
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

    }

    public override void MoveTo(Vector3 nodePosition)
    {
        //Get the position from the player's camera down to the floor as start position.
        Vector3 startPos = transform.position;

        List<GameObject> Path = gridSystem.findPath(startPos, nodePosition);

        //Check if they rolled high enough to cover the path.
        if(Path.Count <= maxRoll)
        {
            //Yes they have! They can move this many spaces, start coroutine loop of moving node by node during action phase, while taking up an action.
            StartCoroutine(movePath(Path, transform, 0.5f));

            maxRoll -= Path.Count; //They may have not move all of there spaces in one go, so subtract by path count.

            turnSystem.takeupAction();

        }

    }


    //This will start a coroutine loop of moving an object through a path.
    public IEnumerator movePath(List<GameObject> path, Transform transform, float time)
    {
        status = turnStatus.Action; //Set this player to action status.
        while (path.Count > 0)
        {
            GameObject node = path[0];

            Vector3 currentPos = transform.position;
            float t = 0.0f;
            while(t < 1.0f)
            {
                t += Time.deltaTime / time;
                transform.position = Vector3.Lerp(currentPos, node.transform.position, t);
                yield return new WaitForEndOfFrame();
            }

            path.RemoveAt(0);
        }

        status = turnStatus.Play; //Set this player back to play status.
    }

}
