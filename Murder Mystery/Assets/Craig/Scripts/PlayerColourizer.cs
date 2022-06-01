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
            if((renderer.tag == "AI_Head")|| (renderer.tag == "AI_TieKnot") || (renderer.tag == "AI_TieTail") || (renderer.tag == "Map_Player_Icon"))
            {
                renderer.material = material;
            }

            if(renderer.tag == "Map_Player_Icon")
            {
                //turn on emmission
                EnableEmissive(renderer.material, renderer);
            }
        }

    }

    private void EnableEmissive(Material targetEmissiveMaterial, Renderer myRenderer)
    {
        //re light the target by switching the emissive property back on
        targetEmissiveMaterial.EnableKeyword("_EMISSION");
        targetEmissiveMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;

        // Update the emission color and intensity of the material.
        targetEmissiveMaterial.SetColor("_EmissionColor", targetEmissiveMaterial.color);

        // Makes the renderer update the emission and albedo maps of our material.
        RendererExtensions.UpdateGIMaterials(myRenderer);

        // Inform Unity's GI system to recalculate GI based on the new emission map.
        DynamicGI.SetEmissive(myRenderer, targetEmissiveMaterial.color);
        DynamicGI.UpdateEnvironment();
    }
}
