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
    [System.Serializable]
    public class BuildMaterial
    {
        public string id;
        public Color color = new Color(1, 1, 1);

        public Material Material;

        public void Set()
        {
            if(Material == null)
            {
                Material = new Material(Shader.Find("Standard"));
                Material.color = color;
            }
        }
    }

    public enum BuildType
    {
        wall,
        floor
    }

    public static BuildingCollection instance;
    public List<BuildPrefab> buildPrefabs = new List<BuildPrefab>();
    public List<BuildMaterial> buildMaterials = new List<BuildMaterial>();
    GameObject Preview;

    private void Awake()
    {
        instance = this;
        SetMaterials();
    }

    private void Update()
    {
        CheckRaycast();
    }

    void CheckRaycast()
    {
        RaycastHit hit;
        InteractObject interactObject = MouseManager.GetOverInteract(out hit);

        if (!interactObject)
        {
            if(Preview) Destroy(Preview);
            return;
        }

        if (!Preview)
        {
            Preview = Instantiate(GetBuild("grass").prefab, transform);
            Preview.GetComponent<Collider>().enabled = false;
        }
        Preview.transform.position = interactObject.transform.position + new Vector3(0, 1, 0);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 vec = GetHitPos(hit);
            Chunk chunk = interactObject.ParentChunk;
            if(chunk.AvailableSpace((int)vec.x, (int)vec.y, (int)vec.z))
            {
                InteractObject obj = Instantiate(GetBuild("grass").prefab, chunk.transform).GetComponent<InteractObject>();
                chunk.AddInteractObject((int)vec.x, (int)vec.y, (int)vec.z, obj);
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
        else if(_vec.y == interactObject.transform.position.y + 1)
        {
            pX = interactObject.PosX;
            pY = interactObject.PosY + 1;
            pZ = interactObject.PosZ;
        }
        else if(_vec.z == interactObject.transform.position.z)
        {
            pX = interactObject.PosX;
            pY = interactObject.PosY;
            pZ = interactObject.PosZ - 1;
        }
        else if(_vec.z == interactObject.transform.position.z + 1)
        {
            pX = interactObject.PosX;
            pY = interactObject.PosY;
            pZ = interactObject.PosZ + 1;
        }
        return new Vector3(pX, pY, pZ);
    }

    public BuildPrefab GetBuild(string id)
    {
        for (int i = 0; i < buildPrefabs.Count; i++)
        {
            if (buildPrefabs[i].id == id) return buildPrefabs[i];
        }
        return null;
    }

    void SetMaterials()
    {
        for (int i = 0; i < buildMaterials.Count; i++)
            buildMaterials[i].Set();
    }
    public Color GetMaterialColor(string id)
    {
        for (int i = 0; i < buildMaterials.Count; i++)
        {
            if (buildMaterials[i].id == id)
            {
                return (buildMaterials[i].Material != null ? buildMaterials[i].Material.color : buildMaterials[i].color);
            }
        }
        return new Color();
    }
    public Material GetMaterial(string id)
    {
        for (int i = 0; i < buildMaterials.Count; i++)
        {
            if (buildMaterials[i].id == id) return buildMaterials[i].Material;
        }
        return new Material(Shader.Find("Standard"));
    }
}
