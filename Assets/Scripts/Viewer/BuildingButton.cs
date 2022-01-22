using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI TextField;
    BuildingViewer Viewer;

    string Text
    {
        get
        {
            return TextField.text;
        }
        set
        {
            TextField.text = value;
        }
    }
    string Id;

    public void Set(BuildingViewer viewer, string text, string id)
    {
        Viewer = viewer;
        Text = text;
        Id = id;
    }

    public void SetClick()
    {
        Viewer.SelectBuilding(Id);
    }
}
