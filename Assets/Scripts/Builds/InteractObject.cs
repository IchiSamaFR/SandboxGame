using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
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

    public List<InteractObject> GetAroundInteract()
    {
        List<InteractObject> around = new List<InteractObject>();
        InteractObject tempObj;
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

}
