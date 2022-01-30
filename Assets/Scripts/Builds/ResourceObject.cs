using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObject : InteractObject
{
    [Header("Ressource Field")]
    [SerializeField]
    private ResourcesCollection.Resource RessourceToGive = new ResourcesCollection.Resource();

    public void HarvestHit(int hitAmount = 1, bool returnRessources = false)
    {
        ResourcesCollection.Resource toDrop = RessourceToGive.Clone();
        toDrop.Amount = toDrop.Amount >= hitAmount ? hitAmount : toDrop.Amount;
        if (!returnRessources)
        {
            DropRessource(toDrop);
        }
        else if (returnRessources)
        {

        }
    }
    private void DropRessource(ResourcesCollection.Resource toDrop)
    {
        WorldDroppedRessource worldRess;
        List<InteractObject> posRes = new List<InteractObject>();
        bool findPos = false;
        foreach (var item in GetAroundInteract())
        {
            worldRess = item.GetComponent<WorldDroppedRessource>();
            if (worldRess != null)
            {
                int toDmg = toDrop.Amount;
                toDrop = worldRess.AddRessource(toDrop);
                toDmg = toDrop == null ? toDmg : toDmg - toDrop.Amount;
                GetDamage(toDmg);
                if (toDrop == null)
                {
                    return;
                }
            }
            else if (item.BuildOn())
            {
                posRes.Add(item);
                findPos = true;
            }
        }

        if (findPos)
        {
            foreach (var item in posRes)
            {
                if (toDrop == null || toDrop.Amount == 0) return;
                int toDmg = toDrop.Amount;
                InteractObject worldRessource = item.ParentChunk.AddInteractObject(item.PosX, item.PosY + 1, item.PosZ, toDrop.Type.ToString());
                toDrop = worldRessource.GetComponent<WorldDroppedRessource>().AddRessource(toDrop);
                toDmg = toDrop == null ? toDmg : toDmg - toDrop.Amount;
                GetDamage(toDmg);
            }
        }
    }
    public override void GetDamage(int amount = 1)
    {
        HealthPoints -= amount;
        RessourceToGive.Amount -= amount;
        if (HealthPoints <= 0 || RessourceToGive.Amount <= 0)
        {
            Destroy();
        }
    }
}
