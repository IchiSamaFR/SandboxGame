using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : InteractObject, IRessourceInteract
{
    [Header("Ressource Field")]
    [SerializeField]
    RessourcesCollection.RessourceType RessourceToGive = RessourcesCollection.RessourceType.wood;
    public int MaxRessourceAmount;

    public void HarvestHit()
    {
        WorldRessource worldRess;
        InteractObject posRess = new InteractObject();
        bool findPos = false;
        foreach (var item in GetAroundInteract())
        {
            worldRess = item.GetComponent<WorldRessource>();
            if (worldRess != null && worldRess.AddRessource(RessourcesCollection.Instance.GetRessource(RessourceToGive)))
            {
                return;
            }
            else if (item.BuildOn())
            {
                posRess = item;
                findPos = true;
            }
        }

        if (findPos)
        {
            InteractObject worldRessource = posRess.ParentChunk.AddInteractObject(posRess.PosX, posRess.PosY + 1, posRess.PosZ, "wood");
            print(worldRessource.GetComponent<WorldRessource>());
            worldRessource.GetComponent<WorldRessource>().AddRessource(RessourcesCollection.Instance.GetRessource(RessourceToGive));
            return;
        }
    }

    public void SetToHarvest()
    {
        throw new System.NotImplementedException();
    }
}
