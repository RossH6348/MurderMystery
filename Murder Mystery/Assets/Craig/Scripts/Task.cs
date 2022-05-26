using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
public class Task : MonoBehaviour
{
    [SerializeField] bool requiresItem = false;
    [SerializeField] ItemObject item;

    public bool DoTask(ItemObject itemPresented, GameObject item)
    {
        if(itemPresented.name == item.name)
        {
            Destroy(item);
        }
        if (requiresItem) return false;
        return Random.Range((int)0, (int)2) == 0 ? false : true;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        Interactable interactible;
        ItemObject itemObject;

        if (!other.gameObject.TryGetComponent<ItemObject>(out itemObject)) return;

        if(other.gameObject.TryGetComponent<Interactable>(out interactible))
        {
            //object is interactible
            //test to see if it is still in hand
            if(!interactible.attachedToHand)
            {
                //not attached to the hand anymore - player has dropped it
                DoTask(itemObject, other.gameObject);
            }
        }
        
    }
}
