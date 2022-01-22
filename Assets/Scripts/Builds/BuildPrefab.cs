using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New build prefab")]
public class BuildPrefab : ScriptableObject
{
    [Header("BaseInfo")]
    public string Name;
    public GameObject Prefab;
    [Header("Build")]
    public bool Inventoried;
    [Header("Path")]
    public bool WalkableOn;
    public bool WalkableIn;
    public bool BuildOn;
    public bool BuildIn;
}
