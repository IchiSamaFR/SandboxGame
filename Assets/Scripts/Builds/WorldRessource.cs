using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRessource : InteractObject
{
    [Header("Ressource Info")]
    public string RessourceId;
    public int AmountStacked;
    public int MaxAmountStacked;

    public bool AddRessource(RessourcesCollection.Ressource ressource)
    {
        if(RessourceId == string.Empty)
        {
            RessourceId = ressource.Id;
            AmountStacked = 1;
            MaxAmountStacked = 3;
            return true;
        }

        if(RessourceId == ressource.Id
            && AmountStacked < MaxAmountStacked)
        {
            AmountStacked++;
            return true;
        }

        return false;
    }
    public bool GetRessource()
    {
        if (AmountStacked > 0)
        {
            AmountStacked--;
            return true;
        }

        return false;
    }
}
