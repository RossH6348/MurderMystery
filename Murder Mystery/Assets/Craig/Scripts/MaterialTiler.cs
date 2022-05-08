//Author: Craig Zeki
//Student ID: zek21003166

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTiler : MonoBehaviour
{
    [SerializeField] private Vector2 tileScale = new Vector2(1,1);


    private new Renderer renderer;
    void Start()
    {

        renderer = GetComponent<Renderer>();
        Vector2 newScale = new Vector2(tileScale.x, tileScale.y);
    
        
        renderer.material.mainTextureScale = newScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
