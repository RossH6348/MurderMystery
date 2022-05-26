using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public int SpaceBetweenItemsinX;
    public int ColNum;
    public int SpaceBetweenItemsinY;
    public int Xstart;
    public int Ystart;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    void Start()
    {
        CreateDisplay();
    }
    void Update()
    {
        UpdateDisplay();
    }
    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Bag.Count; i++)
        {
            var obj = Instantiate(inventory.Bag[i].item.prefab2d, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetDisplayPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Bag[i].amount.ToString("n0");
            obj.transform.localRotation = Quaternion.identity; //This fixes weird shearing rotation for the VR's wrist hud.
            itemsDisplayed.Add(inventory.Bag[i], obj);
        }
    }
    public Vector3 GetDisplayPosition(int i)
    {
        return new Vector3(Xstart + (SpaceBetweenItemsinX * (i % ColNum)), Ystart + (-SpaceBetweenItemsinY * (i / ColNum)), 0f);
    }
    public void UpdateDisplay()
    {// removes all slots with 0 objects.
        //remove all references to from dictionary of same object.
        //reconstruct display based on number of inventory slots used
        bool requiresReconst = false;
        for (int i = 0; i < inventory.Bag.Count; i++)
        {
            Debug.Log(inventory.Bag[i].amount);
            if (inventory.Bag[i].amount == 0)
            {
                requiresReconst = true;
                break;
            }
            if (itemsDisplayed.ContainsKey(inventory.Bag[i]))
            {
                itemsDisplayed[inventory.Bag[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Bag[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventory.Bag[i].item.prefab2d, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetDisplayPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Bag[i].amount.ToString("n0");
                obj.transform.localRotation = Quaternion.identity; //This fixes weird shearing rotation for the VR's wrist hud.
                itemsDisplayed.Add(inventory.Bag[i], obj);
            }
        }
        if (requiresReconst == true)
        {
            for (int i = 0; i < itemsDisplayed.Count; i++)
            {
                GameObject obj = itemsDisplayed.ElementAt(i).Value;
                Destroy(obj);
            }
            itemsDisplayed.Clear();
            inventory.PurgeInventory();
            CreateDisplay();
        }
    }

    public void DisplayReconstruct()
    {
        for (int i = 0; i < itemsDisplayed.Count; i++)
        {
            GameObject obj = itemsDisplayed.ElementAt(i).Value;
            obj.GetComponent<RectTransform>().localPosition = GetDisplayPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Bag[i].amount.ToString("n0");
        }
    }

}
