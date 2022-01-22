using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDroppedRessource : InteractObject
{
    [Header("Ressource Info")]
    public ResourcesCollection.Resource Resource;
    public int MaxAmountStacked;
    public Transform modelContainer;

    public override void SetAfterInit()
    {
        base.SetAfterInit();

        if (Id == string.Empty)
        {
            Debug.LogError("Aucun id disponible.");
        }
        Resource = ResourcesCollection.Instance.GetRessource(Id).Clone();
    }

    public ResourcesCollection.Resource AddRessource(ResourcesCollection.Resource resource)
    {
        if(Resource.Type == resource.Type)
        {
            if (Resource.Amount + resource.Amount <= MaxAmountStacked)
            {
                Resource.Amount += resource.Amount;
                ChangeModel();
                return null;
            }
            else
            {
                int rest = Resource.Amount + resource.Amount - MaxAmountStacked;
                Resource.Amount = MaxAmountStacked;
                ChangeModel();

                resource.Amount = rest;
                return resource;
            }
        }

        return resource;
    }
    public ResourcesCollection.Resource GetRessource(int amount)
    {
        if (Resource.Amount < amount)
        {
            return Resource;
        }
        
        if (Resource.Amount > amount)
        {
            Resource.Amount -= amount;
            ChangeModel();

            ResourcesCollection.Resource toReturn = Resource.Clone();
            toReturn.Amount = amount;
            return toReturn;
        }

        return null;
    }

    public void ChangeModel()
    {
        foreach (Transform item in modelContainer)
        {
            item.gameObject.SetActive(false);
        }
        if(Resource.Amount == 0)
        {
            return;
        }
        else if(modelContainer.childCount > Resource.Amount)
        {
            modelContainer.GetChild(Resource.Amount - 1).gameObject.SetActive(true);
        }
        else
        {
            modelContainer.GetChild(modelContainer.childCount - 1).gameObject.SetActive(true);
        }
    }
}
