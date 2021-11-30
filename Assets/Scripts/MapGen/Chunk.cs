using System.Collections;
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
        NoiseMap = map.GetNoiseChunk(PosX, PosZ);

        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Length; z++)
            {
                // 0.08f is the maximum darkest accepted
                if (NoiseMap[x, z] < 0.08f || x - 1 == Width || z - 1 == Length) continue;

                int height = (int)Mathf.Clamp((NoiseMap[x, z] * Height), 0, Height);
                for (int y = 0; y <= height; y++)
                {
                    GameObject obj;
                    if (y == height)
                        obj = Instantiate(BuildingCollection.instance.GetBuild("grass").prefab, transform);
                    else
                        obj = Instantiate(BuildingCollection.instance.GetBuild("dirt").prefab, transform);
                    InteractObject interact = obj.GetComponent<InteractObject>();
                    AddInteractObject(x, y, z, interact);
                }

                /*
                Tile _tile = obj.GetComponent<Tile>();
                _tile.Set(this, x, 0, z);
                _tile.SetColor(1 - vec[x, z]);
                _tile.name = "Tile[" + x + ";" + z + "]";
                InteractObjects[x, 0, z] = _tile;
                */
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
        if (!MathT.IntBetween(y, 0, Height))
            return null;
        if (!MathT.IntBetween(x, 0, Width) || !MathT.IntBetween(z, 0, Length))
        {
            return map.GetInteractObjectByWorld(PosX * Width + x, y, PosZ * Length + z);
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
        if (!MathT.IntBetween(y, 0, Height))
            return false;
        if (!MathT.IntBetween(x, 0, Width) || !MathT.IntBetween(z, 0, Length))
        {
            Chunk chunk = map.GetChunkByWorld(PosX * Width + x, PosZ * Length + z);
            if (chunk)
                return chunk.AvailableSpace(x >= 0 ? x % Width : x + Width, y, z >= 0 ? z % Length : z + Length);
            else
                return false;
        }

        if(InteractObjects[x, y, z] == null)
            return true;
        else
            return false;

    }

    public bool AddInteractObject(int x, int y, int z, InteractObject interactObject)
    {
        //print($"{x} / {y} / {z}    {AvailableSpace(x, y, z)}");

        if (!AvailableSpace(x, y, z)) return false;

        if (!MathT.IntBetween(x, 0, Width) || !MathT.IntBetween(z, 0, Length))
        {
            Chunk chunk = map.GetChunkByWorld(PosX * Width + x, PosZ * Length + z);
            if (chunk)
            {
                
                return chunk.AddInteractObject(x >= 0 ? x % Width : x + Width, y, z >= 0 ? z % Length : z + Length, interactObject);
            }
            else
                return false;
        }


        InteractObjects[x, y, z] = interactObject;
        InteractObjects[x, y, z].name = $"InteractObject[{x};{y};{z}]";
        InteractObjects[x, y, z].Set(this, x, y, z);
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