using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColourizer : MonoBehaviour
{
    List<Renderer> myRenderers = new List<Renderer>();

    private void Awake()
    {
        GetRenderers();
    }

    private void GetRenderers()
    {
        myRenderers.AddRange(gameObject.GetComponentsInChildren<Renderer>());
    }

    public void SetMaterial(Material material)
    {
        if (myRenderers.Count == 0) GetRenderers(); //just in case Awake has not been called yet.
        foreach(Renderer renderer in myRenderers)
        {
            if((renderer.tag == "AI_Head")|| (renderer.tag == "AI_TieKnot") || (renderer.tag == "AI_TieTail"))
            {
                renderer.material = material;
            }
        }

    }
}
