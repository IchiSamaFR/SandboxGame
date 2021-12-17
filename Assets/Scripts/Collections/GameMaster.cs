using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    void Start()
    {
        Vector3 newPos = new Vector3(MapGenerator.instance.width * MapGenerator.instance.chunkWidth / 2, 
                                     0,
                                     MapGenerator.instance.length * MapGenerator.instance.chunkLength / 2);
        PlayerManager.instance.transform.position = newPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
