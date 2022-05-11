using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Passive Object", menuName = "Inventory System/Items/Passive")]
public class PassiveObject : ItemObject
{
    public void Awake()
    {
        type = ItemID.Passive;
    }
}
