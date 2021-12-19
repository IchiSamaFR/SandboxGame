using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollection : MonoBehaviour
{
    [System.Serializable]
    public class BuildPrefab
    {
        public string id;
        public string name;
        public GameObject prefab;
        public Material Material;
    }

    public static BuildingCollection instance;
    public List<BuildPrefab> buildBlocks = new List<BuildPrefab>();
    public List<BuildPrefab> buildStructs = new List<BuildPrefab>();

    private void Awake()
    {
        instance = this;
    }
    
    public BuildPrefab GetBuild(string id)
    {
        for (int i = 0; i < buildBlocks.Count; i++)
        {
            if (buildBlocks[i].id.ToLower() == id.ToLower()) return buildBlocks[i];
        }
        for (int i = 0; i < buildStructs.Count; i++)
        {
            if (buildStructs[i].id.ToLower() == id.ToLower()) return buildStructs[i];
        }
        return null;
    }
    
    public Color GetMaterialColor(string id)
    {
        for (int i = 0; i < buildBlocks.Count; i++)
        {
            if (buildBlocks[i].id == id)
            {
                return buildBlocks[i].Material.color;
            }
        }
        return new Color();
    }
    public Material GetMaterial(string id)
    {
        for (int i = 0; i < buildBlocks.Count; i++)
        {
            if (buildBlocks[i].id == id)
            {
                return buildBlocks[i].Material;
            }
        }
        return new Material(Shader.Find("Standard"));
    }
}
