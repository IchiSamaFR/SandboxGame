using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildObject : InteractObject
{
    private List<int> Triangles = new List<int>();
    private List<Vector3> Vertices = new List<Vector3>();
    private List<Vector2> Uvs = new List<Vector2>();
    private List<Vector3> Normales = new List<Vector3>();
    private Color MaterialColor;

    public override void SetAfterInit()
    {
        InitMesh(true);
        SetColor();
    }

    List<BuildObject> GetAroundBuilds()
    {
        List<InteractObject> interacts = GetAroundInteract();
        List<BuildObject> builds = new List<BuildObject>();
        for (int i = 0; i < interacts.Count; i++)
        {
            BuildObject obj = interacts[i].GetComponent<BuildObject>();
            if (obj != null) builds.Add(obj);
        }
        return builds;
    }

    public void InitMesh(bool refresh = false)
    {
        CalculateMesh();
        BuildMesh();

        if (refresh)
        {
            List<BuildObject> builds = GetAroundBuilds();
            for (int i = 0; i < builds.Count; i++) builds[i].InitMesh();
        }
    }


    public void SetColor()
    {
        MaterialColor = BuildingCollection.Instance.GetMaterialColor(Id) * (1 - PosY * (0.4f / MapGenerator.instance.ChunkHeight));
        GetComponent<MeshRenderer>().material.color = MaterialColor;
    }
    public void SetColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }


    public override List<InteractObject> GetAroundInteract()
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
    public override void Destroy()
    {
        List<BuildObject> interacts = GetAroundBuilds();
        for (int i = 0; i < interacts.Count; i++)
        {
            interacts[i].InitMesh();
        }
        base.Destroy();
    }
    bool right = false, left = false, forward = false, backward = false, up = false, down = false;
    void CalculateMesh()
    {
        bool nright = false, nleft = false, nforward = false, nbackward = false, nup = false, ndown = false;

        List<BuildObject> builds = GetAroundBuilds();
        for (int i = 0; i < builds.Count; i++)
        {
            if (WorldPosX > builds[i].WorldPosX)
            {
                nleft = true;
            }
            else if (WorldPosX < builds[i].WorldPosX)
            {
                nright = true;
            }
            else if (WorldPosZ > builds[i].WorldPosZ)
            {
                nforward = true;
            }
            else if (WorldPosZ < builds[i].WorldPosZ)
            {
                nbackward = true;
            }
            else if (WorldPosY > builds[i].WorldPosY)
            {
                ndown = true;
            }
            else if (WorldPosY < builds[i].WorldPosY)
            {
                nup = true;
            }
        }

        if(nleft != left || nright != right || nforward != forward || nbackward != backward || ndown != down || nup != up)
            SetMesh(nleft, nright, nforward, nbackward, nup, ndown);
        if (left && right && forward && backward && down && up)
            GetComponent<Collider>().enabled = false;
        else
            GetComponent<Collider>().enabled = true;
    }
    void SetMesh(bool nleft, bool nright, bool nforward, bool nbackward, bool nup, bool ndown)
    {
        left = nleft;
        right = nright;
        forward = nforward;
        backward = nbackward;
        up = nup;
        down = ndown;


        Vertices.Clear();
        Triangles.Clear();
        Uvs.Clear();
        Normales.Clear();

        if(left && right && forward && backward && up && down)
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            return;
        }
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;

        /* Triangles start with 3 :
         * 0 - 1  |  0   1
         *   /    |      |
         * 3   2  |  3 - 2
         * 
         * Inverted for back / down / left :
         * 0 - 1  |  0   1
         * |      |    / |
         * 3   2  |  3   2
         */
        int ind = 0;
        if (!forward)
        {
            Triangles.Add(3 + 4 * ind);
            Triangles.Add(1 + 4 * ind);
            Triangles.Add(0 + 4 * ind);
            Triangles.Add(3 + 4 * ind);
            Triangles.Add(2 + 4 * ind);
            Triangles.Add(1 + 4 * ind);
            ind++;

            Vertices.Add(new Vector3(0, 0, 0));
            Vertices.Add(new Vector3(1, 0, 0));
            Vertices.Add(new Vector3(1, 1, 0));
            Vertices.Add(new Vector3(0, 1, 0));

            Uvs.Add(new Vector2(0.125f * 0, 0));
            Uvs.Add(new Vector2(0.125f * 1, 0));
            Uvs.Add(new Vector2(0.125f * 1, 1f));
            Uvs.Add(new Vector2(0.125f * 0, 1));

            Normales.Add(Vector3.forward);
            Normales.Add(Vector3.forward);
            Normales.Add(Vector3.forward);
            Normales.Add(Vector3.forward);
        }
        if (!right)
        {
            Triangles.Add(3 + 4 * ind);
            Triangles.Add(1 + 4 * ind);
            Triangles.Add(0 + 4 * ind);
            Triangles.Add(3 + 4 * ind);
            Triangles.Add(2 + 4 * ind);
            Triangles.Add(1 + 4 * ind);
            ind++;

            Vertices.Add(new Vector3(1, 0, 0));
            Vertices.Add(new Vector3(1, 0, 1));
            Vertices.Add(new Vector3(1, 1, 1));
            Vertices.Add(new Vector3(1, 1, 0));

            Uvs.Add(new Vector2(0.125f * 1, 0));
            Uvs.Add(new Vector2(0.125f * 2, 0));
            Uvs.Add(new Vector2(0.125f * 2, 1f));
            Uvs.Add(new Vector2(0.125f * 1, 1));

            Normales.Add(Vector3.right);
            Normales.Add(Vector3.right);
            Normales.Add(Vector3.right);
            Normales.Add(Vector3.right);
        }

        if (!backward)
        {
            Triangles.Add(3 + 4 * ind);
            Triangles.Add(0 + 4 * ind);
            Triangles.Add(1 + 4 * ind);
            Triangles.Add(3 + 4 * ind);
            Triangles.Add(1 + 4 * ind);
            Triangles.Add(2 + 4 * ind);
            ind++;

            Vertices.Add(new Vector3(0, 0, 1));
            Vertices.Add(new Vector3(1, 0, 1));
            Vertices.Add(new Vector3(1, 1, 1));
            Vertices.Add(new Vector3(0, 1, 1));

            Uvs.Add(new Vector2(0.125f * 2, 0));
            Uvs.Add(new Vector2(0.125f * 3, 0));
            Uvs.Add(new Vector2(0.125f * 3, 1f));
            Uvs.Add(new Vector2(0.125f * 2, 1));

            Normales.Add(Vector3.back);
            Normales.Add(Vector3.back);
            Normales.Add(Vector3.back);
            Normales.Add(Vector3.back);
        }
        if (!left)
        {
            Triangles.Add(3 + 4 * ind);
            Triangles.Add(0 + 4 * ind);
            Triangles.Add(1 + 4 * ind);
            Triangles.Add(3 + 4 * ind);
            Triangles.Add(1 + 4 * ind);
            Triangles.Add(2 + 4 * ind);
            ind++;

            Vertices.Add(new Vector3(0, 0, 0));
            Vertices.Add(new Vector3(0, 0, 1));
            Vertices.Add(new Vector3(0, 1, 1));
            Vertices.Add(new Vector3(0, 1, 0));

            Uvs.Add(new Vector2(0.125f * 3, 0));
            Uvs.Add(new Vector2(0.125f * 4, 0));
            Uvs.Add(new Vector2(0.125f * 4, 1f));
            Uvs.Add(new Vector2(0.125f * 3, 1));

            Normales.Add(Vector3.left);
            Normales.Add(Vector3.left);
            Normales.Add(Vector3.left);
            Normales.Add(Vector3.left);
        }

        if (!up)
        {
            Triangles.Add(3 + 4 * ind);
            Triangles.Add(1 + 4 * ind);
            Triangles.Add(0 + 4 * ind);
            Triangles.Add(3 + 4 * ind);
            Triangles.Add(2 + 4 * ind);
            Triangles.Add(1 + 4 * ind);
            ind++;

            Vertices.Add(new Vector3(0, 1, 0));
            Vertices.Add(new Vector3(1, 1, 0));
            Vertices.Add(new Vector3(1, 1, 1));
            Vertices.Add(new Vector3(0, 1, 1));

            Uvs.Add(new Vector2(0.125f * 4, 0));
            Uvs.Add(new Vector2(0.125f * 5, 0));
            Uvs.Add(new Vector2(0.125f * 5, 1f));
            Uvs.Add(new Vector2(0.125f * 4, 1));

            Normales.Add(Vector3.up);
            Normales.Add(Vector3.up);
            Normales.Add(Vector3.up);
            Normales.Add(Vector3.up);
        }
        if (!down)
        {
            Triangles.Add(3 + 4 * ind);
            Triangles.Add(0 + 4 * ind);
            Triangles.Add(1 + 4 * ind);
            Triangles.Add(3 + 4 * ind);
            Triangles.Add(1 + 4 * ind);
            Triangles.Add(2 + 4 * ind);
            ind++;

            Vertices.Add(new Vector3(0, 0, 0));
            Vertices.Add(new Vector3(1, 0, 0));
            Vertices.Add(new Vector3(1, 0, 1));
            Vertices.Add(new Vector3(0, 0, 1));

            Uvs.Add(new Vector2(0.125f * 5, 0));
            Uvs.Add(new Vector2(0.125f * 6, 0));
            Uvs.Add(new Vector2(0.125f * 6, 1f));
            Uvs.Add(new Vector2(0.125f * 5, 1));

            Normales.Add(Vector3.down);
            Normales.Add(Vector3.down);
            Normales.Add(Vector3.down);
            Normales.Add(Vector3.down);
        }

        BuildMesh();
    }
    void BuildMesh()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;

        MeshRenderer meshR = GetComponent<MeshRenderer>();
        if (!meshR.material)
            meshR.material = BuildingCollection.Instance.GetMaterial(Id);

        mesh.Clear();
        mesh.vertices = Vertices.ToArray();
        mesh.triangles = Triangles.ToArray();
        mesh.uv = Uvs.ToArray();
        mesh.normals = Normales.ToArray();
    }
}
