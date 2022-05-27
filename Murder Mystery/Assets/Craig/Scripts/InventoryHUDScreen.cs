using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHUDScreen : MonoBehaviour, IHUDScreen
{
    [SerializeField] Transform itemSpawnPoint;
    public void executeScreenClick(GameObject caller)
    {
        if(caller.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
        {
            
            ItemObject item = playerInventory.FirstItemRequest();
            if (item == null) return;
            GameObject temp = Instantiate(item.prefab3d, itemSpawnPoint);
            if(temp.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }
            
            ItemDeck.Instance.ReturnToDeck(item);
        }
        
    }

    
}
