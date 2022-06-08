using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemID
{
    Passive,
    Weapon,
    Default,
}

public abstract class ItemObject : ScriptableObject
{
    public string itemName;
    public GameObject prefab2d;
    public GameObject prefab3d;
    public ItemID type;
    [TextArea(15, 20)]
    public string description;
}
