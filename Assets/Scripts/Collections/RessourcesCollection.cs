using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourcesCollection : MonoBehaviour
{
    public enum RessourceType
    {
        wood,
        stone,
        iron,
        copper
    }

    [System.Serializable]
    public class Ressource
    {
        [Header("BaseInfo")]
        public string Name;
        public RessourceType Type;
    }

    public static RessourcesCollection Instance;
    public List<Ressource> Ressources = new List<Ressource>();

    public RessourcesCollection()
    {
        Instance = this;
    }

    public Ressource GetRessource(string id)
    {
        foreach (var item in Ressources)
        {
            if (item.Name == id) return item;
        }
        return null;
    }
    public Ressource GetRessource(RessourceType type)
    {
        foreach (var item in Ressources)
        {
            if (item.Type == type) return item;
        }
        return null;
    }

}
