using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Bag = new List<InventorySlot>();
    public List<int> EmptySlots = new List<int>();
    public void AddItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Bag.Count; i++)
        {
            if (Bag[i].item == _item)
            {
                Bag[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }
        if (!hasItem)
        {
            Bag.Add(new InventorySlot(_item, _amount));
        }
    }

    public void GatherItem(ItemObject _item,string CharacterType)
    {
        
            for (int i = 0; i < Bag.Count; i++)
            {
                if (Bag[i].item == _item)
                {
                    Bag[i].AddAmount(-1);
                    if (Bag[i].amount <= 0)
                    {
                        
                        if (CharacterType == "Non-Human")
                        {
                            Bag.RemoveAt(i);
                        }
                        else
                        {
                            EmptySlots.Add(i);
                        }
                        break;
                    }
                }
            }
        
        
    }

    public void PurgeInventory()
    {
        foreach (int i in EmptySlots)
        {
            Bag.RemoveAt(i);
        }
        EmptySlots.Clear();
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;
    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
            
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
}