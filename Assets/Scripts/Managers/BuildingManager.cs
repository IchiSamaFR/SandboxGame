using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    BuildingCollection buildingCollection;
    GameObject Preview;

    private string ToBuild = "firecamp";
    public List<InteractObject> OrderList = new List<InteractObject>();

    private void Start()
    {
        buildingCollection = BuildingCollection.Instance;
    }

    void Update()
    {
        CheckRaycast();
    }

    void CheckRaycast()
    {
        if (!MouseManager.IsBuildingMode) return;

        RaycastHit hit;
        InteractObject interactObject = MouseManager.GetOverInteract(out hit);

        if (!interactObject || !interactObject.WalkableOn() || 
            MapGenerator.instance.GetInteractObjectByWorld(interactObject.WorldPosX, interactObject.WorldPosY + 1, interactObject.WorldPosZ))
        {
            if (Preview) Destroy(Preview);
            return;
        }

        if (!Preview)
        {
            Preview = Instantiate(buildingCollection.GetBuild(ToBuild).Prefab, transform);
            if(Preview.GetComponent<Collider>())
                Preview.GetComponent<Collider>().enabled = false;
            Preview.name = "PreviewPlacement";
        }
        Preview.transform.position = interactObject.transform.position + new Vector3(0, 1, 0);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 vec = interactObject.Pos + new Vector3(0, 1, 0);
            Chunk chunk = interactObject.ParentChunk;
            if (chunk.AvailableSpace((int)vec.x, (int)vec.y, (int)vec.z))
            {
                chunk.AddInteractObject((int)vec.x, (int)vec.y, (int)vec.z, ToBuild);
                if (Preview) Destroy(Preview);
                MouseManager.SetNormalMouse();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Chunk chunk = interactObject.ParentChunk;
            chunk?.DestroyInteractObject(interactObject.PosX, interactObject.PosY, interactObject.PosZ);
        }
    }

    public Vector3 GetHitPos(RaycastHit hit)
    {
        InteractObject interactObject = hit.transform.GetComponent<InteractObject>();
        Vector3 _vec = hit.point;

        int pX = 0;
        int pY = 0;
        int pZ = 0;

        if (_vec.x == interactObject.transform.position.x)
        {
            pX = interactObject.PosX - 1;
            pY = interactObject.PosY;
            pZ = interactObject.PosZ;
        }
        else if (_vec.x == interactObject.transform.position.x + 1)
        {
            pX = interactObject.PosX + 1;
            pY = interactObject.PosY;
            pZ = interactObject.PosZ;
        }
        else if (_vec.y == interactObject.transform.position.y)
        {
            pX = interactObject.PosX;
            pY = interactObject.PosY - 1;
            pZ = interactObject.PosZ;
        }
        else if (_vec.y == interactObject.transform.position.y + 1)
        {
            pX = interactObject.PosX;
            pY = interactObject.PosY + 1;
            pZ = interactObject.PosZ;
        }
        else if (_vec.z == interactObject.transform.position.z)
        {
            pX = interactObject.PosX;
            pY = interactObject.PosY;
            pZ = interactObject.PosZ - 1;
        }
        else if (_vec.z == interactObject.transform.position.z + 1)
        {
            pX = interactObject.PosX;
            pY = interactObject.PosY;
            pZ = interactObject.PosZ + 1;
        }
        return new Vector3(pX, pY, pZ);
    }
}
