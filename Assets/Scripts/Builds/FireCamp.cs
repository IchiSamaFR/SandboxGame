using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCamp : InteractObject
{
    public override void SetAfterInit()
    {
        if (Village.instance == null) VillageManager.instance.AddFirstVillage();

        InstanciateNewCharacters(2);
    }

    void InstanciateNewCharacters(int amount)
    {
        List<InteractObject> lst = new List<InteractObject>();

        foreach (var item in GetAroundInteract(2))
        {
            if (item.WalkableOn())
                lst.Add(item);
        }

        for (int i = 0; i < amount; i++)
        {
            if (lst.Count <= 0) Debug.LogError("Out of space to instantiate a new character.");

            int rdm = Random.Range(0, lst.Count - 1);
            InteractObject itObject = lst[rdm];
            Village.instance.AddCharacter(itObject.WorldPosX,
                                          itObject.WorldPosY + 1,
                                          itObject.WorldPosZ);
            lst.RemoveAt(rdm);
        }
    }
}
