using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesCollection : MonoBehaviour
{
    public enum ResourceType
    {
        wood,
        stone,
        iron,
        copper
    }

    [System.Serializable]
    public class Resource
    {
        [Header("BaseInfo")]
        public ResourceType Type;
        public int Amount;

        public Resource Clone()
        {
            return new Resource() { Amount = this.Amount, Type = this.Type };
        }
    }

    public static ResourcesCollection Instance;
    public List<Resource> Resources = new List<Resource>();

    public ResourcesCollection()
    {
        Instance = this;
    }

    public Resource GetRessource(string id)
    {
        foreach (var item in Resources)
        {
            if (item.Type.ToString() == id) return item;
        }
        return null;
    }
    public Resource GetRessource(ResourceType type)
    {
        foreach (var item in Resources)
        {
            if (item.Type == type) return item;
        }
        return null;
    }

}
