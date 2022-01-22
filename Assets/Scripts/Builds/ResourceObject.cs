using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObject : InteractObject
{
    [Header("Ressource Field")]
    [SerializeField]
    ResourcesCollection.Resource RessourceToGive = new ResourcesCollection.Resource();

    public void HarvestHit()
    {
        ResourcesCollection.Resource toGive = RessourceToGive.Clone();
        toGive.Amount = 1;

        WorldDroppedRessource worldRess;
        InteractObject posRes = null;
        bool findPos = false;
        foreach (var item in GetAroundInteract())
        {
            worldRess = item.GetComponent<WorldDroppedRessource>();
            if (worldRess != null)
            {
                if (worldRess.AddRessource(toGive) == null)
                {
                    GetDamage();
                    return;
                }
            }
            else if (item.BuildOn())
            {
                posRes = item;
                findPos = true;
            }
        }

        if (findPos)
        {
            InteractObject worldRessource = posRes.ParentChunk.AddInteractObject(posRes.PosX, posRes.PosY + 1, posRes.PosZ, "wood");
            worldRessource.GetComponent<WorldDroppedRessource>().AddRessource(toGive);
            GetDamage();
            return;
        }
    }
}
