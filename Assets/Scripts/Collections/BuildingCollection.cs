using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollection : MonoBehaviour
{
    public static BuildingCollection Instance;
    public List<BuildPrefab> Builds = new List<BuildPrefab>();

    private void Awake()
    {
        Instance = this;
    }
    
    public BuildPrefab GetBuild(string id)
    {
        foreach (var item in Builds)
        {
            if (item.Name == id) return item;
        }
        return null;
    }
    
    public Color GetMaterialColor(string id)
    {
        for (int i = 0; i < Builds.Count; i++)
        {
            if (Builds[i].Name == id)
            {
                return Builds[i].Prefab.GetComponent<MeshRenderer>().material.color;
            }
        }
        return new Color();
    }
    public Material GetMaterial(string id)
    {
        for (int i = 0; i < Builds.Count; i++)
        {
            if (Builds[i].Name == id)
            {
                return Builds[i].Prefab.GetComponent<MeshRenderer>().material;
            }
        }
        return new Material(Shader.Find("Standard"));
    }
}
