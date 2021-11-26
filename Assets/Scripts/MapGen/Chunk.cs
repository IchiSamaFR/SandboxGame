﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chunk : MonoBehaviour
{
    [Header("Map stats")]

    [System.NonSerialized]
    public MapGenerator map;
    public int Width;
    public int Length;
    public int Height;

    public int PosX;
    public int PosZ;

    [Header("Other")]
    public GameObject InteractObjectsPref;
    public InteractObject[,,] InteractObjects;
    public float[,] NoiseMap;

    public void Set(int width, int height, int length, int posX, int posZ, MapGenerator mapGen = null)
    {
        if (mapGen)
            map = mapGen;
        else 
            map = MapGenerator.instance;

        Width = width;
        Height = height;
        Length = length;

        PosX = posX;
        PosZ = posZ;

        InteractObjects = new InteractObject[Width, Height, Length];

        Gen();
    }

    void Gen()
    {
        float[,] vec = map.GetNoiseChunk(PosX, PosZ);
        
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Length; z++)
            {
                if (vec[x, z] < 0.08f && x - 1 != Width && z - 1 != Length) continue;
                GameObject obj = Instantiate(InteractObjectsPref, transform);
                Tile _tile = obj.GetComponent<Tile>();
                _tile.Set(this, x, 0, z);
                _tile.SetColor(1 - vec[x, z]);
                _tile.name = "Tile[" + x + ";" + z + "]";
                InteractObjects[x, 0, z] = _tile;
            }
        }
    }

    void DestroyInteractObjects()
    {
        foreach (Transform item in transform.GetChild(0))
        {
            Destroy(item.gameObject);
        }
    }
    
    public InteractObject GetInteractObject(int x, int y, int z)
    {
        if (!MathT.IntBetween(x, 0, Width) || !MathT.IntBetween(z, 0, Length))
        {
            return map.GetTileByPos(PosX * Width + x, y, PosZ * Length + z);
        }
        else if(!MathT.IntBetween(y, 0, Height))
        {
            return null;
        }
        if (x < 0) x += Width;
        if (z < 0) z += Length;

        if (InteractObjects[x, y, z] != null && InteractObjects[x, y, z])
        {
            return InteractObjects[x, y, z];
        }
        return null;
    }

    public bool AvailableSpace(int x, int y, int z)
    {
        if (!MathT.IntBetween(x, 0, Width) || !MathT.IntBetween(y, 0, Height) || !MathT.IntBetween(z, 0, Length))
        {
            Debug.LogError("AvailableSpace() - Check position out of area.");
            return false;
        }

        if(InteractObjects[x, y, z] == null)
            return true;
        else
            return false;

    }

    public bool AddInteractObject(int x, int y, int z, InteractObject interactObject)
    {
        if (!AvailableSpace(x, y, z)) return false;
        InteractObjects[x, y, z] = interactObject;
        InteractObjects[x, y, z].SetAfterInit();
        return true;
    }
    public bool DestroyInteractObject(int x, int y, int z)
    {
        if (AvailableSpace(x, y, z)) return false;

        InteractObject toDelete = InteractObjects[x, y, z];
        InteractObjects[x, y, z] = null;
        toDelete.Destroy();

        return true;
    }
}


[System.Serializable]
public struct TileGen
{
    public char name;
    public GameObject pref;
}