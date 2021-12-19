using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour
{
    public static VillageManager instance;

    public List<Village> Villages = new List<Village>();
    public GameObject CharacterPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void AddFirstVillage()
    {
        Village village = new Village();
        Village.instance = village;
        Villages.Add(village);
    }
    public void AddNewVillage(Village village)
    {
        if(Villages.IndexOf(village) == -1)
            Villages.Add(village);
    }
}
