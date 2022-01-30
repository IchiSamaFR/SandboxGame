using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;

    public int width;
    public int length;

    public GameObject chunkPref;
    public int ChunkWidth = 10;
    public int ChunkHeight = 10;
    public int ChunkLength = 10;

    public Chunk[,] Chunks;

    [Header("Noise Map")]
    public RawImage rawImage;
    public float MinNoise = 0.08f;
    public float MinForestNoise = 0.7f;
    public float MinRockNoise = 0.7f;

    public NoiseMap NoiseMap = new NoiseMap();
    public NoiseMap ForestNoiseMap = new NoiseMap();
    public NoiseMap RockNoiseMap = new NoiseMap();
    private float[,] Noise { get => NoiseMap.Noise; }
    private float[,] ForestNoise { get => ForestNoiseMap.Noise; }
    private float[,] RockNoise { get => RockNoiseMap.Noise; }

    [Header("Map gen")]
    Transform Content;


    private void OnValidate()
    {
        if (NoiseMap.persistance < 0)
            NoiseMap.persistance = 0;
        if (NoiseMap.lacunarity < 0)
            NoiseMap.lacunarity = 0;
        if (NoiseMap.Scale < 0)
            NoiseMap.Scale = 0;
        if (ChunkWidth < 0)
            ChunkWidth = 0;
        if (ChunkLength < 0)
            ChunkLength = 0;
    }

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("MapGeneration already exist.");
            return;
        }
        instance = this;
        Chunks = new Chunk[width, length];
    }

    void Start()
    {
        Gen();
    }
    
    void Update()
    {

    }
    
    public void Gen()
    {
        DestroyChunks();
        NoiseMap.GenNoiseMap(width * ChunkWidth, length * ChunkLength);
        ForestNoiseMap.GenNoiseMap(width * ChunkWidth, length * ChunkLength);
        RockNoiseMap.GenNoiseMap(width * ChunkWidth, length * ChunkLength);

        Content = GameObject.Instantiate(new GameObject(), transform).transform;
        Content.name = "Map";

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                GameObject chunkObj;
                if (Content)
                    chunkObj = GameObject.Instantiate(chunkPref, Content);
                else
                    chunkObj = GameObject.Instantiate(chunkPref, transform);

                chunkObj.transform.position = new Vector3(x * (ChunkWidth), 0, z * (ChunkLength));
                Chunk chunk = chunkObj.GetComponent<Chunk>();
                Chunks[x, z] = chunk;
                chunk.Set(ChunkWidth, ChunkHeight, ChunkLength, x, z, this);
            }
        }
        InitMeshes();
    }
    public void InitMeshes()
    {
        foreach (var item in Chunks)
        {
            item.InitMeshes();
        }
    }

    void DestroyChunks()
    {
        Destroy(Content);
        return;
    }

    public void GenImage()
    {
        NoiseMap.GenNoiseMap(width * ChunkWidth, length * ChunkLength);
        ForestNoiseMap.GenNoiseMap(width * ChunkWidth, length * ChunkLength);
        RockNoiseMap.GenNoiseMap(width * ChunkWidth, length * ChunkLength);

        Texture2D tex = new Texture2D(width * ChunkWidth, length * ChunkLength);
        Color[] colourMap = new Color[(width * ChunkWidth) * (length * ChunkLength)];

        for (int z = 0; z < tex.height; z++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                if (Noise[x, z] < MinNoise)
                    colourMap[z * width * ChunkWidth + x] = Color.black;
                else
                {
                    colourMap[z * width * ChunkWidth + x] = Color.Lerp(new Color(0.5f, 1, 0.5f), new Color(0.4f, 0.9f, 0.4f), Noise[x, z]);
                    if(ForestNoise[x, z] >= MinForestNoise)
                    {
                        colourMap[z * width * ChunkWidth + x] = new Color(0.1f, 0.6f, 0.1f);
                    }
                    else if (RockNoise[x, z] >= MinRockNoise)
                    {
                        colourMap[z * width * ChunkWidth + x] = Color.red;
                    }
                }
            }
        }
        tex.SetPixels(colourMap);
        tex.Apply();

        rawImage.texture = tex;
    }

    public float[,] GetNoiseChunk(int xPos, int zPos)
    {
        return NoiseMap.GetNoiseChunk(xPos, zPos, ChunkWidth, ChunkLength);
    }
    public float[,] GetForestNoiseChunk(int xPos, int zPos)
    {
        return ForestNoiseMap.GetNoiseChunk(xPos, zPos, ChunkWidth, ChunkLength);
    }
    public float[,] GetRockNoiseChunk(int xPos, int zPos)
    {
        return RockNoiseMap.GetNoiseChunk(xPos, zPos, ChunkWidth, ChunkLength);
    }

    public Chunk GetChunk(int x, int z)
    {
        if (!MathT.IntBetween(x, 0, width) || !MathT.IntBetween(z, 0, length)) return null;

        return Chunks[x, z];
    }
    public Chunk GetChunkByWorld(int x, int z)
    {
        int chunkX = Mathf.CeilToInt(x / ChunkWidth);
        int chunkZ = Mathf.CeilToInt(z / ChunkLength);

        return GetChunk(chunkX, chunkZ);
    }

    public InteractObject GetInteractObjectByWorld(int x, int y, int z)
    {
        int chunkX = Mathf.CeilToInt(x / ChunkWidth);
        int chunkZ = Mathf.CeilToInt(z / ChunkLength);
        int posx = x % ChunkWidth;
        int posz = z % ChunkLength;

        Chunk c = GetChunk(chunkX, chunkZ);

        if (c == null) return null;
        return c.GetInteractObject(posx, y, posz);
    }
}

[System.Serializable]
public class NoiseMap
{
    public bool AutoUpdate = true;
    public bool FallOff = true;
    public int Seed = 321354;
    public float Scale = 1;
    [Range(1, 20)]
    public int octaves = 4;
    [Range(0, 1)]
    public float persistance = 0.5f;
    public float lacunarity = 1;
    public Vector2 offset;

    public float[,] Noise;

    public void GenNoiseMap(int width, int length)
    {
        Noise = NoiseGenerator.Create(width, length, Scale, Seed, octaves, persistance, lacunarity, offset, FallOff);
    }

    public float[,] GetNoiseChunk(int xPos, int zPos, int width, int length)
    {
        float[,] toReturn = new float[width, length];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                float fl = Noise[xPos * width + x, zPos * length + z];
                toReturn[x, z] = fl;
            }
        }
        return toReturn;
    }
}