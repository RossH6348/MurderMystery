using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Item : MonoBehaviour
{
    private Interactable myInteractable = null;
    bool hasBeenInHand = false;

    private void Awake()
    {
        hasBeenInHand = false;
        myInteractable = GetComponent<Interactable>();
    }

    private void FixedUpdate()
    {
        if((!hasBeenInHand) && (myInteractable.attachedToHand == true))
        {
            hasBeenInHand = true;
            if(gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = false;
            }
        }
    }
}
