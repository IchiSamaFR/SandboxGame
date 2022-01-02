using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDroppedRessource : InteractObject
{
    [Header("Ressource Info")]
    public int AmountStacked;
    public int MaxAmountStacked;
    public Transform modelContainer;

    public bool AddRessource(RessourcesCollection.Ressource ressource)
    {
        if(Id == string.Empty)
        {
            Debug.LogError("Aucun id disponible.");
        }

        if(Id == ressource.Id
            && AmountStacked < MaxAmountStacked)
        {
            AmountStacked++;
            ChangeModel();
            return true;
        }

        return false;
    }
    public bool GetRessource()
    {
        if (AmountStacked > 0)
        {
            AmountStacked--;
            ChangeModel();
            return true;
        }

        return false;
    }

    public void ChangeModel()
    {
        foreach (Transform item in modelContainer)
        {
            item.gameObject.SetActive(false);
        }
        if(modelContainer.childCount > AmountStacked)
        {
            modelContainer.GetChild(AmountStacked - 1).gameObject.SetActive(true);
        }
        else
        {
            modelContainer.GetChild(modelContainer.childCount - 1).gameObject.SetActive(true);
        }
    }
}
