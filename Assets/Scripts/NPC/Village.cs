using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    public static Village instance;
    public string Id;
    public int VillagerAmount;
    public List<Character> Villagers = new List<Character>();


    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void AddCharacter(int posX, int posY, int posZ)
    {
        Character obj = Instantiate(VillageManager.instance.CharacterPrefab, GameObject.Find("-- Objects --").transform).GetComponent<Character>();
        obj.transform.position = new Vector3(posX + 0.5f, posY, posZ + 0.5f);
        Villagers.Add(obj);
    }
}
