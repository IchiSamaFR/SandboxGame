using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    [Header("Path")]
    public bool WalkableOn = false;
    public bool WalkableIn = false;

    [Header("Position")]
    public Chunk ParentChunk;
    public int PosX;
    public int PosY;
    public int PosZ;

    public int WorldPosX { get => ParentChunk.PosX * ParentChunk.Width + PosX; }
    public int WorldPosY { get => PosY; }
    public int WorldPosZ { get => ParentChunk.PosZ * ParentChunk.Length + PosZ; }

    [Header("Informations")]
    public string id;

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
        InteractObject tempObj;

        for (int x = PosX - 1; x <= PosX + 1; x++)
        {
            for (int z = PosZ - 1; z <= PosZ + 1; z++)
            {
                if (x != PosX || z != PosZ)
                {
                    InteractObject newInteract;
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
        tempObj = ParentChunk.GetInteractObject(PosX + 1, PosY, PosZ);
        if (tempObj != null)
            around.Add(tempObj);
        tempObj = ParentChunk.GetInteractObject(PosX - 1, PosY, PosZ);
        if (tempObj != null)
            around.Add(tempObj);

        tempObj = ParentChunk.GetInteractObject(PosX, PosY, PosZ + 1);
        if (tempObj != null)
            around.Add(tempObj);
        tempObj = ParentChunk.GetInteractObject(PosX, PosY, PosZ - 1);
        if (tempObj != null)
            around.Add(tempObj);


        tempObj = ParentChunk.GetInteractObject(PosX, PosY + 1, PosZ);
        if (tempObj != null)
            around.Add(tempObj);
        tempObj = ParentChunk.GetInteractObject(PosX, PosY - 1, PosZ);
        if (tempObj != null)
            around.Add(tempObj);

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

}
