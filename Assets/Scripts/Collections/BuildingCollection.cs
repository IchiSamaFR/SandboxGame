using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollection : MonoBehaviour
{
    [System.Serializable]
    public class BuildPrefab
    {
        public string id;
        public string name;
        public BuildType type;
        public GameObject prefab;
    }

    public enum BuildType
    {
        wall,
        floor
    }

    public List<BuildPrefab> buildPrefabs = new List<BuildPrefab>();
    GameObject Preview;
    

    private void Update()
    {
        CheckRaycast();
    }

    void CheckRaycast()
    {
        InteractObject interactObject = MouseManager.GetOverInteract();

        if (!interactObject)
        {
            if(Preview) Destroy(Preview);
            return;
        }

        if (!Preview)
        {
            Preview = Instantiate(GetBuild().prefab, transform);
            Preview.GetComponent<Collider>().enabled = false;
        }
        Preview.transform.position = interactObject.transform.position + new Vector3(0, 1, 0);

        if (Input.GetMouseButtonDown(0))
        {
            Chunk chunk = interactObject.ParentChunk;
            if(chunk.AvailableSpace(interactObject.PosX, interactObject.PosY + 1, interactObject.PosZ))
            {
                InteractObject obj = Instantiate(GetBuild().prefab, chunk.transform).GetComponent<InteractObject>();
                obj.Set(chunk, interactObject.PosX, interactObject.PosY + 1, interactObject.PosZ);
                chunk.AddInteractObject(interactObject.PosX, interactObject.PosY + 1, interactObject.PosZ, obj);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Chunk chunk = interactObject.ParentChunk;
            if (!chunk.AvailableSpace(interactObject.PosX, interactObject.PosY + 1, interactObject.PosZ))
            {
                chunk.DestroyInteractObject(interactObject.PosX, interactObject.PosY + 1, interactObject.PosZ);
            }
        }
    }

    public BuildPrefab GetBuild()
    {
        return buildPrefabs[0];
    }
}
