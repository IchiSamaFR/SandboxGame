using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingViewer : MonoBehaviour
{
    BuildingManager manager;
    BuildingCollection collection;
    List<BuildPrefab> prefabs = new List<BuildPrefab>();

    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    Transform content;

    void Start()
    {
        manager = BuildingManager.Instance;
        collection = BuildingCollection.Instance;

        prefabs.Add(collection.GetBuild("firecamp"));
        prefabs.Add(collection.GetBuild("stock"));

        RefreshButtons();
    }

    public void RefreshButtons()
    {
        foreach (Transform item in content)
        {
            Destroy(item.gameObject);
        }
        BuildingButton obj = Instantiate(buttonPrefab, content).GetComponent<BuildingButton>();
        obj.Set(this, "", "");
        foreach (var item in prefabs)
        {
            obj = Instantiate(buttonPrefab, content).GetComponent<BuildingButton>();
            obj.Set(this, item.name, item.name);
        }
    }

    public void SelectBuilding(string build = "")
    {
        manager.SelectBuilding(build);
    }
}
