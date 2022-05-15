//Author: Craig Zeki
//Student ID: zek21003166

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MaterialTiler : MonoBehaviour
{
    [SerializeField] private Vector2 tileScale = new Vector2(1,1);
    [SerializeField] private List<Vector2> materialTileScales = new List<Vector2>();

    private new Renderer renderer;
    void Start()
    {
        if (materialTileScales.Count == 0) return;
        int count = 0;
        


        renderer = GetComponent<Renderer>();
        //Vector2 newScale = new Vector2(tileScale.x, tileScale.y);


        //renderer.material.mainTextureScale = newScale;
        
        foreach (Material material in renderer.materials)
        {
            material.mainTextureScale = materialTileScales[count];
            if (++count >= materialTileScales.Count) break; //run out of scaling to apply - stop here
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
