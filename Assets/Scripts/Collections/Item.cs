using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public string id;
    public string type;
    public bool hideInStore = false;
    public bool staticBuild = false;
    public GameObject prefab;
    public Sprite preview;

    public int amount;

    public int moneyValue;
    public int expValue;
    public int costToPlace;
    public int levelMin;

    public Item(string _name, string _id, string _type, int _amount = 0)
    {
        name = _name;
        id = _id;
        type = _type;
        amount = _amount;
    }
    public Item(string _name, string _id, Sprite _preview, int _amount = 0)
    {
        name = _name;
        id = _id;
        preview = _preview;
        amount = _amount;
    }

    public Item(Item item)
    {
        this.name = item.name;
        this.id = item.id;
        this.type = item.type;
        this.hideInStore = item.hideInStore;
        this.staticBuild = item.staticBuild;
        this.prefab = item.prefab;
        this.preview = item.preview;
        this.costToPlace = item.costToPlace;
        this.levelMin = item.levelMin;
    }
}
