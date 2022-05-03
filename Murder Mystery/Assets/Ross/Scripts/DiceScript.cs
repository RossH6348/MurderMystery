using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class DiceScript : MonoBehaviour
{

    private Rigidbody rigid;
    private Interactable interact;
    private List<Transform> faces = new List<Transform>();

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        interact = GetComponent<Interactable>();

        for (int i = transform.childCount - 1; i > -1; i--)
            faces.Add(transform.GetChild(i));
    }

    public void onEnterRoll()
    {
        //Disable the interact script, to stop it from being picked up mid roll.
        interact.enabled = false;
    }

}
