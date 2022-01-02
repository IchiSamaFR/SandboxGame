using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : InteractObject, IRessourceInteract
{
    [Header("Ressource Field")]
    [SerializeField]
    RessourcesCollection.RessourceType RessourceToGive = RessourcesCollection.RessourceType.stone;
    public int MaxRessourceAmount;

    public void HarvestHit()
    {
        WorldDroppedRessource worldRess;
        InteractObject posRess = new InteractObject();
        bool findPos = false;
        foreach (var item in GetAroundInteract())
        {
            worldRess = item.GetComponent<WorldDroppedRessource>();
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
            InteractObject worldRessource = posRess.ParentChunk.AddInteractObject(posRess.PosX, posRess.PosY + 1, posRess.PosZ, "stone");
            worldRessource.GetComponent<WorldDroppedRessource>().AddRessource(RessourcesCollection.Instance.GetRessource(RessourceToGive));
            return;
        }
    }

    public void SetToHarvest()
    {
        throw new System.NotImplementedException();
    }
}
