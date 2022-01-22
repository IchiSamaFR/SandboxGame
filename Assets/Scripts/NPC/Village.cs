using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    public static Village instance;
    public string Id;
    public int VillagerAmount;
    public List<Character> Villagers = new List<Character>();
    public List<InteractObject> InteractObjects = new List<InteractObject>();
    
    public void AddInteract(InteractObject interact)
    {
        if(InteractObjects.IndexOf(interact) == -1)
        {
            InteractObjects.Add(interact);
        }
    }
    public void RemoveInteract(InteractObject interact)
    {
        if (InteractObjects.IndexOf(interact) >= 0)
        {
            InteractObjects.Remove(interact);
        }
    }

    public void GetStockPile(ResourcesCollection.ResourceType ressource)
    {
        foreach (var item in InteractObjects)
        {
            if(item.Id == "stock")
            {

            }
        }
    }

    public void AddCharacter(int posX, int posY, int posZ)
    {
        Character obj = Instantiate(VillageManager.instance.CharacterPrefab, GameObject.Find("-- Objects --").transform).GetComponent<Character>();
        obj.transform.position = new Vector3(posX, posY, posZ);
    }
}
