using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Interactable))]
public class Item : MonoBehaviour
{
    private Interactable myInteractable = null;
    private Rigidbody myRB = null;
    private bool previousAttachedToHand = false;

    private void Awake()
    {
        previousAttachedToHand = false;
        myRB = GetComponent<Rigidbody>();
        myInteractable = GetComponent<Interactable>();
        myRB.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if((!previousAttachedToHand) && (myInteractable.attachedToHand == true))
        {
            previousAttachedToHand = true;
            //if(gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            //{
            //    rb.isKinematic = false;
            //}
        }
        else if(previousAttachedToHand && (myInteractable.attachedToHand == false))
        {
            myRB.isKinematic = false;
            transform.SetParent(null);
        }
    }

    
}
