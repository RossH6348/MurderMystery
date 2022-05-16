using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

public class PlayerScript : CharacterScript
{


    //Access to each of the huds.
    [SerializeField] private GameObject GlobalScreen;
    [SerializeField] private GameObject ActionsScreen;

    //Random text elements.
    [SerializeField] private Transform turnStart;

    //Text element for timer.
    [SerializeField] private Text timerText;

    [SerializeField] private Camera playerCamera;

    private GameObject selectedObject = null;

    private bool hasRolled = false;
    private bool playerMove = false;

    private void Awake()
    {
        playerCamera = transform.Find("PlayerCamera").GetComponent<Camera>();
    }

    public override void onTurnEnter()
    {

        playerMove = false;

        hasRolled = false;

        //Open up the global screen. (Mostly the turn timer.)
        GlobalScreen.SetActive(true);

        //While we are at it, play a little intro stating it's now their turn.
        StartCoroutine(TurnStartIntro());

    }

    public override void onTurnTick()
    {
        
        //Update the timer text.
        timerText.text = "Time left: " + Mathf.FloorToInt(turnSystem.turnTime + 1.0f).ToString();

        if(playerDice != null)
        {
            //They haven't rolled their dice yet, or they have but haven't stopped rolling about.

            if (Input.GetMouseButtonDown(0))
            {
                //Convert their position on the screen to ray direction.
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit result;

                //Cast a ray.
                if(Physics.Raycast(ray, out result, 9999.0f))
                {

                    //If it hits a collider that have a diceScript attached to it, we know it is a dice.
                    DiceScript diceScript = result.collider.GetComponent<DiceScript>();
                    if(diceScript != null && result.collider.GetComponent<IgnoreHovering>() == null) //Also check if it doesn't have the VR ignorehovering on it. (Hence they already rolled it.)
                    {

                        //Apply random velocities to the dice and make the dice enter its roll state.
                        Rigidbody diceRigid = diceScript.transform.GetComponent<Rigidbody>();
                        diceScript.onEnterRoll();

                        float rollAngle = Random.Range(-3.141592f, 3.141592f);
                        diceRigid.velocity = new Vector3(0.0f, Random.Range(2.5f, 5.0f), 0.0f) + (new Vector3(Mathf.Sin(rollAngle), 0.0f, Mathf.Cos(rollAngle)) * Random.Range(2.5f, 5.0f));
                        diceRigid.angularVelocity = new Vector3(Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f));
                    }
                }
            }
        }
        else
        {

            if (!hasRolled)
            {
                hasRolled = true;
                ActionsScreen.SetActive(true);
            }

            //They have rolled their dice, do logic based on whether they are wanting to move or not.
            if (playerMove)
            {
                //They are wanting to move, do raycast to node stuff.

                GameObject laserSelected = null;

                //Convert their position on the screen to ray direction.
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit result;

                //Cast a ray.
                if (Physics.Raycast(ray, out result, 9999.0f))
                    laserSelected = result.collider.gameObject;


                if (laserSelected != selectedObject)
                {
                    //Okay, a diffferent object have been selected, we need to do the appropriate onLaserSelected and onLaserDeselected.
                    if (selectedObject != null)
                    {
                        LaserInput input = selectedObject.GetComponent<LaserInput>();
                        if (input != null)
                            input.onLaserDeselected(this); //We will pass in our own CharacterScript so the object selected knows who have deselected.
                        selectedObject = null;
                    }

                    if (laserSelected != null)
                    {
                        selectedObject = laserSelected;
                        LaserInput input = selectedObject.GetComponent<LaserInput>();
                        if (input != null)
                            input.onLaserSelected(this); //We will pass in our own CharacterScript so the object selected knows who have selected.
                    }
                }

                if (selectedObject != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        LaserInput input = selectedObject.GetComponent<LaserInput>();
                        if (input != null)
                            input.onLaserClick(this, true);
                    }else if(Input.GetMouseButtonUp(0)){
                        LaserInput input = selectedObject.GetComponent<LaserInput>();
                        if (input != null)
                            input.onLaserClick(this, false);
                    }
                }
            }
            else
            {
                //Do other stuff, such as whether they want to equip and stuff.


            }


            //This key will toggle between whether the player wants to start moving, or do something else.
            if (Input.GetKeyDown(KeyCode.Space))
            {

                //Toggle between actions to movement and vice-versa.
                ActionsScreen.SetActive(!ActionsScreen.activeSelf);
                playerMove = !playerMove;

                //There turn is over, make sure to deselect everything.
                if (selectedObject != null)
                {
                    LaserInput input = selectedObject.GetComponent<LaserInput>();
                    if (input != null)
                        input.onLaserDeselected(this); //We will pass in our own CharacterScript so the object selected knows who have deselected.
                }

                selectedObject = null;

            }


        }

    }

    public override void onTurnExit()
    {
        //Their turn have ended! Automatically close down everything, while cleaning up stuff.
        GlobalScreen.SetActive(false);
        ActionsScreen.SetActive(false);

        //There turn is over, make sure to deselect everything.
        if (selectedObject != null)
        {
            LaserInput input = selectedObject.GetComponent<LaserInput>();
            if (input != null)
                input.onLaserDeselected(this); //We will pass in our own CharacterScript so the object selected knows who have deselected.
        }

        selectedObject = null;

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
            status = turnStatus.Action;
            //Yes they have! They can move this many spaces, start coroutine loop of moving node by node during action phase, while taking up an action.
            StartCoroutine(Helpers.movePath(Path, transform, 0.5f, () => { status = turnStatus.Play; }));

            maxRoll -= Path.Count; //They may have not move all of there spaces in one go, so subtract by path count.

            turnSystem.takeupAction();

        }

    }

    public override void grantDice(GameObject dice)
    {

        //We need to position this dice below the player.
        dice.transform.position = transform.Find("DicePan").position + new Vector3(0.0f, 1.0f, 0.0f);

        base.grantDice(dice); //We need to return to the base function, which will automatically set the roller of the dice for us.

    }

    IEnumerator TurnStartIntro()
    {
        timerText.enabled = false;
        turnStart.localScale = Vector3.zero;

        status = turnStatus.Action; //Just to stop their timer from going down during their intro animation.

        while(turnStart.localScale.x < 1.0f)
        {
            turnStart.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * 3.0f;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.0f);

        while(turnStart.localScale.y > 0.0f)
        {
            turnStart.localScale -= new Vector3(0.0f, Time.deltaTime, 0.0f) * 3.0f;
            yield return new WaitForEndOfFrame();
        }

        status = turnStatus.Play; //Okay they can now make a play.

        //At this point, display the turn timer.
        timerText.enabled = true;

    }

}