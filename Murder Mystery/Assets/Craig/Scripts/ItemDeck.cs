using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeck : MonoBehaviour
{
    [SerializeField] List<ItemObject> unshuffledItemDeck = new List<ItemObject>();
    [SerializeField] float chanceOfUseableItem = 0.5f;
    [SerializeField] int numberOfWeapons = 2;

    private List<ItemObject> shuffledItemDeck = new List<ItemObject>();
    private float maxChance = 1.0f;
    private float minChance = 0.1f;
    private int factor = 100;

    private static ItemDeck instance;

    public static ItemDeck Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<ItemDeck>();
            return instance;
        }
    }

    private void Awake()
    {
        ResetDeck();
    }

    private void ShuffleDeckAndAssignWeapons(int numOfWeapons)
    {
        shuffledItemDeck = unshuffledItemDeck;
        shuffledItemDeck.Shuffle();
        foreach(ItemObject item in shuffledItemDeck)
        {
            if (numOfWeapons > 0)
            {
                item.type = ItemID.Weapon;
                numOfWeapons--;
            }
            else
            {
                item.type = ItemID.Passive;
            }

        }
        shuffledItemDeck.Shuffle();
    }

    public void ResetDeck()
    {
        numberOfWeapons = Mathf.Clamp(numberOfWeapons, 0, unshuffledItemDeck.Count);
        ShuffleDeckAndAssignWeapons(numberOfWeapons);
        chanceOfUseableItem = Mathf.Clamp(chanceOfUseableItem, minChance, maxChance);
    }

    public ItemObject DrawFromDeck()
    {
        if (shuffledItemDeck.Count <= 0) return null;

        ItemObject temp = null;
        int rangeMax = Mathf.RoundToInt((maxChance * factor) / chanceOfUseableItem);

        if (Random.Range((int)0, rangeMax) < factor)
        {
            temp = shuffledItemDeck[0];
            shuffledItemDeck.RemoveAt(0);
        }

        return temp;

    }

    public void ReturnToDeck(ItemObject itemObject)
    {
        foreach(ItemObject item in shuffledItemDeck)
        {
            if (item == itemObject) return;
        }
        shuffledItemDeck.Add(itemObject);
    }
}
