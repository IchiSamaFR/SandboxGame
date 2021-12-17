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
    public int chunkWidth = 10;
    public int chunkHeight = 10;
    public int chunkLength = 10;

    public Chunk[,] chunks;

    public int chunksCount = 0;
    public int newChunkPrice { get => chunksCount * 100; }

    [Header("Noise Map")]
    public RawImage rawImage;

    public NoiseMap NoiseMap = new NoiseMap();
    float[,] Noise { get => NoiseMap.Noise; }

    [Header("Level generation")]
    public List<MapGenLevel> levels = new List<MapGenLevel>();

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
        if (chunkWidth < 0)
            chunkWidth = 0;
        if (chunkLength < 0)
            chunkLength = 0;
    }

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("MapGeneration already exist.");
            return;
        }
        instance = this;
        chunks = new Chunk[width, length];
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
        NoiseMap.GenNoiseMap(width * chunkWidth, length * chunkLength);

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

                chunkObj.transform.position = new Vector3(x * (chunkWidth), 0, z * (chunkLength));
                Chunk chunk = chunkObj.GetComponent<Chunk>();
                chunks[x, z] = chunk;
                chunk.Set(chunkWidth, chunkHeight, chunkLength, x, z, this);
            }
        }
        InitMeshes();
    }
    public void InitMeshes()
    {
        foreach (var item in chunks)
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
        NoiseMap.GenNoiseMap(width * chunkWidth, length * chunkLength);

        Texture2D tex = new Texture2D(width * chunkWidth, length * chunkLength);
        Color[] colourMap = new Color[(width * chunkWidth) * (length * chunkLength)];

        for (int z = 0; z < tex.height; z++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                colourMap[z * width * chunkWidth + x] = Color.Lerp(Color.black, Color.white, Noise[x, z]);
            }
        }
        tex.SetPixels(colourMap);
        tex.Apply();

        rawImage.texture = tex;
    }

    public float[,] GetNoiseChunk(int xPos, int zPos)
    {
        return NoiseMap.GetNoiseChunk(xPos, zPos, chunkWidth, chunkLength);
    }

    public Color GetLevelColor(float level)
    {
        Color _color = new Color();

        for (int i = 0; i < levels.Count; i++)
        {
            if (level > levels[i].level)
            {
                if(i + 1 == levels.Count)
                {
                    _color = levels[i].color;
                    return levels[i].color;
                }
                else if (level < levels[i + 1].level)
                {
                    _color = levels[i].color;
                    return levels[i].color;
                }
            }
        }
        return _color;
    }
    
    public Chunk GetChunk(int x, int z)
    {
        if (!MathT.IntBetween(x, 0, width) || !MathT.IntBetween(z, 0, length)) return null;

        return chunks[x, z];
    }
    public Chunk GetChunkByWorld(int x, int z)
    {
        int chunkX = Mathf.CeilToInt(x / chunkWidth);
        int chunkZ = Mathf.CeilToInt(z / chunkLength);

        return GetChunk(chunkX, chunkZ);
    }

    public InteractObject GetInteractObjectByWorld(int x, int y, int z)
    {
        int chunkX = Mathf.CeilToInt(x / chunkWidth);
        int chunkZ = Mathf.CeilToInt(z / chunkLength);
        int posx = x % chunkWidth;
        int posz = z % chunkLength;

        Chunk c = GetChunk(chunkX, chunkZ);

        if (c == null) return null;
        return c.GetInteractObject(posx, y, posz);
    }
}
[System.Serializable]
public class MapGenLevel
{
    public float level;
    public Color color = new Color(1, 1, 1, 1);
}

[System.Serializable]
public class NoiseMap
{
    public bool AutoUpdate = true;
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
        Noise = NoiseGenerator.Create(width, length, Scale, Seed, octaves, persistance, lacunarity, offset);
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