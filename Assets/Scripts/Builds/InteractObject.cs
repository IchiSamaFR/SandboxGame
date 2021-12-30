using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public bool WalkableOn() { return BuildingCollection.instance.GetBuild(id).WalkableOn; }
    public bool WalkableIn() { return BuildingCollection.instance.GetBuild(id).WalkableIn; }

    [Header("Position")]
    public Chunk ParentChunk;
    public int PosX;
    public int PosY;
    public int PosZ;

    public int WorldPosX { get => ParentChunk.PosX * ParentChunk.Width + PosX; }
    public int WorldPosY { get => PosY; }
    public int WorldPosZ { get => ParentChunk.PosZ * ParentChunk.Length + PosZ; }

    [Header("Interact Informations")]
    public string id;
    public int healthPoints;
    public int MaxHealthPoints;
    public BuildState buildState;

    public virtual void Set(Chunk chunk, int posX, int posY, int posZ)
    {
        ParentChunk = chunk;
        PosX = posX;
        PosY = posY;
        PosZ = posZ;

        transform.position = chunk.transform.position + new Vector3(PosX, PosY, PosZ);
    }
    public virtual void SetAfterInit()
    {

    }

    public virtual void Over()
    {

    }

    public virtual void Action()
    {

    }
    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
    public virtual List<InteractObject> GetAroundInteract()
    {
        List<InteractObject> around = new List<InteractObject>();
        int range = 1;

        InteractObject newInteract;
        for (int x = PosX - range; x <= PosX + range; x++)
        {
            for (int z = PosZ - range; z <= PosZ + range; z++)
            {
                if (x != PosX || z != PosZ)
                {
                    if ((newInteract = ParentChunk.GetInteractObject(x, PosY, z)) != null
                        && ParentChunk.GetInteractObject(x, PosY + 1, z) == null)
                    {
                        around.Add(newInteract);
                    }
                    else if ((newInteract = ParentChunk.GetInteractObject(x, PosY + 1, z)) != null
                        && ParentChunk.GetInteractObject(x, PosY + 2, z) == null)
                    {
                        around.Add(newInteract);
                    }
                    else if ((newInteract = ParentChunk.GetInteractObject(x, PosY - 1, z)) != null
                        && ParentChunk.GetInteractObject(x, PosY, z) == null)
                    {
                        around.Add(newInteract);
                    }
                }
            }
        }

        return around;
    }
    public List<InteractObject> GetAroundInteract(int range = 1)
    {
        List<InteractObject> around = new List<InteractObject>();

        InteractObject newInteract;
        for (int x = PosX - range; x <= PosX + range; x++)
        {
            for (int z = PosZ - range; z <= PosZ + range; z++)
            {
                if (x != PosX || z != PosZ)
                {
                    if ((newInteract = ParentChunk.GetInteractObject(x, PosY, z)) != null
                        && ParentChunk.GetInteractObject(x, PosY + 1, z) == null)
                    {
                        around.Add(newInteract);
                    }
                    else if ((newInteract = ParentChunk.GetInteractObject(x, PosY + 1, z)) != null
                        && ParentChunk.GetInteractObject(x, PosY + 2, z) == null)
                    {
                        around.Add(newInteract);
                    }
                    else if ((newInteract = ParentChunk.GetInteractObject(x, PosY - 1, z)) != null
                        && ParentChunk.GetInteractObject(x, PosY, z) == null)
                    {
                        around.Add(newInteract);
                    }
                }
            }
        }

        return around;
    }

    public InteractObject GetWalkableAround(Vector3 pos)
    {
        List<InteractObject> around = new List<InteractObject>();

        foreach (var item in GetAroundInteract())
        {
            if (item.WalkableOn())
                around.Add(item);
        }
        if (around.Count <= 0) return null;

        InteractObject obj = around[0];
        int minValue = -1;

        foreach (var item in around)
        {
            int newVal = MathT.DistanceCost(pos, item.transform.position);
            if (minValue == -1 || (minValue != -1 && newVal < minValue))
            {
                obj = item;
                minValue = newVal;
            }
        }
        return obj;
    }
}
