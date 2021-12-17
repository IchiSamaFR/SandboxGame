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
        public BuildType type;
        public GameObject prefab;
        public Material Material;
    }

    public enum BuildType
    {
        wall,
        floor
    }

    public static BuildingCollection instance;
    public List<BuildPrefab> buildPrefabs = new List<BuildPrefab>();

    private void Awake()
    {
        instance = this;
    }
    
    public BuildPrefab GetBuild(string id)
    {
        for (int i = 0; i < buildPrefabs.Count; i++)
        {
            if (buildPrefabs[i].id == id) return buildPrefabs[i];
        }
        return null;
    }
    
    public Color GetMaterialColor(string id)
    {
        for (int i = 0; i < buildPrefabs.Count; i++)
        {
            if (buildPrefabs[i].id == id)
            {
                return buildPrefabs[i].Material.color;
            }
        }
        return new Color();
    }
    public Material GetMaterial(string id)
    {
        for (int i = 0; i < buildPrefabs.Count; i++)
        {
            if (buildPrefabs[i].id == id)
            {
                return buildPrefabs[i].Material;
            }
        }
        return new Material(Shader.Find("Standard"));
    }
}
