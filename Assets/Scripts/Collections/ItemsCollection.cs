using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCollection : MonoBehaviour
{
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

    /* Return type by an id
     */
    public string GetType(string _id)
    {
        foreach (Item item in collectionItems)
        {
            if (item.id == _id)
            {
                return item.type;
            }
        }
        return "";
    }

    /* Get the first item with the type of
     */
    public string GetFirst(string _type)
    {
        foreach (Item item in collectionItems)
        {
            if (item.type == _type)
            {
                return item.id;
            }
        }

        return "";
    }

    public List<Item> GetItemWithLevel(int _level)
    {
        List<Item> toReturn = new List<Item>();
        foreach (Item item in collectionItems)
        {
            if (item.levelMin == _level)
            {
                toReturn.Add(item);
            }
        }

        return toReturn;
    }
}