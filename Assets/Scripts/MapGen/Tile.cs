using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : InteractObject
{
    public MeshRenderer TileRenderer;
    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void SetColor(float color)
    {
        TileRenderer.materials[1].color = TileRenderer.materials[1].color * color;
    }
}
