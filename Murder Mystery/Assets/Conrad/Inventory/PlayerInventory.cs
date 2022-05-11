using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public ItemObject testItem;
    public string testPlayerType;
    
    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<ItemFoundation>();
        if (item)
        {
            inventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Bag.Clear();
    }


    public void ItemRequest()
    {
        inventory.GatherItem(testItem,testPlayerType);
    }
}
