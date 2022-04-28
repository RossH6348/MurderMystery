using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerScript : CharacterScript
{

    [SerializeField] private Transform rightHand;
    private int maxRoll = 5;

    [SerializeField] private GameObject laser;

    // Start is called before the first frame update
    private void Awake()
    {
        name = "Ross";
    }

    public override void onTurnEnter()
    {

        //When their turn begins, turn on their laser or something.
        laser.SetActive(true);

    }

    public override void onTurnTick()
    {

        //Allow the VR player to carry out their controls.
        RaycastHit result;
        if(Physics.Raycast(rightHand.position,rightHand.forward,out result,9999.0f))
        {
            laser.SetActive(true);
            laser.transform.position = (rightHand.position + result.point) * 0.5f;
            laser.transform.rotation.SetLookRotation(rightHand.forward);
            laser.transform.localScale = new Vector3(0.1f, 0.1f, (rightHand.position - result.point).magnitude);
        }
        else
        {
            laser.SetActive(false);
        }

    }

    public override void onTurnExit()
    {

        //Their turn is over, disable their laser.
        laser.SetActive(false);

    }
}
