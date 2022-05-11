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
    public GameObject prefab;
    public ItemID type;
    [TextArea(15, 20)]
    public string description;
}
