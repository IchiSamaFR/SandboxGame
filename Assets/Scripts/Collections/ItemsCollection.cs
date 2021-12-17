using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCollection : MonoBehaviour
{
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

    public static ItemsCollection instance;

    [Header("Collection of items")]
    public List<Item> collectionItems = new List<Item>();


    private void Awake()
    {
        instance = this;
    }

    /* Return item by an id
     */
    public Item GetItem(string _id)
    {
        foreach (Item item in collectionItems)
        {
            if (item.id == _id)
            {
                return item;
            }
        }
        return null;
    }

    /* Return sprite by an id
     */
    public Sprite GetSprite(string _id)
    {
        foreach (Item item in collectionItems)
        {
            if (item.id == _id)
            {
                return item.preview;
            }
        }

        return null;
    }
}