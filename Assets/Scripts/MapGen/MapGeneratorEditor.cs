﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mapGen.NoiseMap.AutoUpdate)
            {
                mapGen.GenImage();
            }
        }

        if (GUILayout.Button("Gen Image"))
        {
            mapGen.GenImage();
        }
        if (GUILayout.Button("Gen"))
        {
            mapGen.Gen();
        }
    }
}
