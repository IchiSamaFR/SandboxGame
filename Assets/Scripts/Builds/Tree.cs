using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : InteractObject, IRessourceInteract
{
    [Header("Ressource Field")]
    [SerializeField]
    Ressource ressourceToGive = Ressource.wood;
    public int MaxRessourceAmount;

    public void HarvestHit()
    {
        throw new System.NotImplementedException();
    }

    public void SetToHarvest()
    {
        throw new System.NotImplementedException();
    }
}
