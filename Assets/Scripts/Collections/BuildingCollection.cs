using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollection : MonoBehaviour
{
    [System.Serializable]
    public class BuildPrefab
    {
        [Header("BaseInfo")]
        public string Id;
        public string Name;
        public GameObject Prefab;
        public Material Material;
        [Header("Path")]
        public bool WalkableOn;
        public bool WalkableIn;
        public bool BuildOn;
    }

    public static BuildingCollection Instance;
    public List<BuildPrefab> BuildBlocks = new List<BuildPrefab>();
    public List<BuildPrefab> BuildStructs = new List<BuildPrefab>();
    public List<BuildPrefab> RessourcesStructs = new List<BuildPrefab>();

    private void Awake()
    {
        Instance = this;
    }
    
    public BuildPrefab GetBuild(string id)
    {
        foreach (var item in BuildBlocks)
        {
            if (item.Id == id) return item;
        }
        foreach (var item in BuildStructs)
        {
            if (item.Id == id) return item;
        }
        foreach (var item in RessourcesStructs)
        {
            if (item.Id == id) return item;
        }
        return null;
    }
    
    public Color GetMaterialColor(string id)
    {
        for (int i = 0; i < BuildBlocks.Count; i++)
        {
            if (BuildBlocks[i].Id == id)
            {
                return BuildBlocks[i].Material.color;
            }
        }
        return new Color();
    }
    public Material GetMaterial(string id)
    {
        for (int i = 0; i < BuildBlocks.Count; i++)
        {
            if (BuildBlocks[i].Id == id)
            {
                return BuildBlocks[i].Material;
            }
        }
        return new Material(Shader.Find("Standard"));
    }
}
