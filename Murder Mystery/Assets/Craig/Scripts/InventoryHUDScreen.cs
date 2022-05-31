using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DisplayInventory))]
public class InventoryHUDScreen : MonoBehaviour, IHUDScreen
{
    [SerializeField] Transform itemSpawnPoint;
    private DisplayInventory displayInventory;

    private void Awake()
    {
        
    }
    public void executeScreenClick(GameObject caller, GameObject wristHUD)
    {
        if(caller.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
        {
            displayInventory = GetComponent<DisplayInventory>();
            ItemObject item = playerInventory.FirstItemRequest();
            if (item == null) return;
            GameObject temp = Instantiate(item.prefab3d, itemSpawnPoint);
            //if(temp.TryGetComponent<Rigidbody>(out Rigidbody rb))
            //{
            //    rb.isKinematic = true;
            //}
            displayInventory.UpdateDisplay(); //need to call this to ensure the inventory is purged
            wristHUD.SetActive(false);
            //ItemDeck.Instance.ReturnToDeck(item);
        }
        
    }

    
}
