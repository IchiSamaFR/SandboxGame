using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chunk : MonoBehaviour
{
    [Header("Map stats")]

    [System.NonSerialized]
    public MapGenerator Map;
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
            Map = mapGen;
        else 
            Map = MapGenerator.instance;

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
        NoiseMap = Map.GetNoiseChunk(PosX, PosZ);
        float[,] forestNoise = Map.GetForestNoiseChunk(PosX, PosZ);
        float[,] rockNoise = Map.GetRockNoiseChunk(PosX, PosZ);

        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Length; z++)
            {
                float minimum = 0.08f;
                if (NoiseMap[x, z] < minimum || x - 1 == Width || z - 1 == Length) continue;

                int height = (int)Mathf.Clamp(((NoiseMap[x, z] - minimum) * Height), 0, Height);
                for (int y = 0; y <= height; y++)
                {
                    if (y == height)
                        AddInitInteractObject(x, y, z, "grass");
                    else
                        AddInitInteractObject(x, y, z, "dirt");
                }
                if (forestNoise[x,z] >= Map.MinForestNoise)
                {
                    AddInitInteractObject(x, height + 1, z, "treepine");
                }
                else if (rockNoise[x, z] >= Map.MinRockNoise)
                {
                    AddInitInteractObject(x, height + 1, z, "rock");
                }
            }
        }
    }
    public void InitMeshes()
    {
        foreach (var item in InteractObjects)
        {
            item?.GetComponent<BuildObject>()?.InitMesh(false);
            item?.GetComponent<BuildObject>()?.SetColor();
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
            return Map.GetInteractObjectByWorld(PosX * Width + x, y, PosZ * Length + z);
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
            Chunk chunk = Map.GetChunkByWorld(PosX * Width + x, PosZ * Length + z);
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

    public InteractObject AddInteractObject(int x, int y, int z, string toBuild)
    {
        if (!AvailableSpace(x, y, z)) return null;

        if (!MathT.IntBetween(x, 0, Width) || !MathT.IntBetween(z, 0, Length))
        {
            Chunk chunk = Map.GetChunkByWorld(PosX * Width + x, PosZ * Length + z);
            if (chunk)
            {
                return chunk.AddInteractObject(x >= 0 ? x % Width : x + Width, y, z >= 0 ? z % Length : z + Length, toBuild);
            }
            else
                return null;
        }
        InteractObjects[x, y, z] = Instantiate(BuildingCollection.Instance.GetBuild(toBuild).Prefab).GetComponent<InteractObject>();
        InteractObjects[x, y, z].name = $"InteractObject[{x};{y};{z}]";
        InteractObjects[x, y, z].Set(this, x, y, z, toBuild);
        InteractObjects[x, y, z].SetAfterInit();
        return InteractObjects[x, y, z];
    }
    public bool AddInitInteractObject(int x, int y, int z, string toBuild)
    {
        if (!AvailableSpace(x, y, z)) return false;

        if (!MathT.IntBetween(x, 0, Width) || !MathT.IntBetween(z, 0, Length))
        {
            Debug.LogWarning("AddInitInteract out of size.");
            return false;
        }

        InteractObjects[x, y, z] = Instantiate(BuildingCollection.Instance.GetBuild(toBuild).Prefab).GetComponent<InteractObject>();
        InteractObjects[x, y, z].name = $"InteractObject[{x};{y};{z}]";
        InteractObjects[x, y, z].Set(this, x, y, z, toBuild);
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