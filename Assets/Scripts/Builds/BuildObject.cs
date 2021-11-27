using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildObject : InteractObject
{
    public enum BuildState
    {
        blueprint,
        built
    }

    public BuildState State = BuildState.blueprint;
    List<int> triangles = new List<int>();
    List<Vector3> vertices = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<Vector3> normales = new List<Vector3>();
    Color MaterialColor;

    public override void Set(Chunk chunk, int posX, int posY, int posZ)
    {
        base.Set(chunk, posX, posY, posZ);
    }
    public override void SetAfterInit()
    {
        InitMesh(true);
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

        List<BuildObject> builds = GetAroundBuilds();
        if (refresh)
            for (int i = 0; i < builds.Count; i++) builds[i].InitMesh();
    }


    public void SetColor(float colorMutliplicator)
    {
        MaterialColor = BuildingCollection.instance.GetMaterialColor(id) * colorMutliplicator;
        SetColor(MaterialColor);
    }
    public void SetColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
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
    }
    void SetMesh(bool nleft, bool nright, bool nforward, bool nbackward, bool nup, bool ndown)
    {
        left = nleft;
        right = nright;
        forward = nforward;
        backward = nbackward;
        up = nup;
        down = ndown;


        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
        normales.Clear();

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
            triangles.Add(3 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(0 + 4 * ind);
            triangles.Add(3 + 4 * ind);
            triangles.Add(2 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            ind++;

            vertices.Add(new Vector3(0, 0, 0));
            vertices.Add(new Vector3(1, 0, 0));
            vertices.Add(new Vector3(1, 1, 0));
            vertices.Add(new Vector3(0, 1, 0));

            uvs.Add(new Vector2(0.125f * 0, 0));
            uvs.Add(new Vector2(0.125f * 1, 0));
            uvs.Add(new Vector2(0.125f * 1, 1f));
            uvs.Add(new Vector2(0.125f * 0, 1));

            normales.Add(Vector3.forward);
            normales.Add(Vector3.forward);
            normales.Add(Vector3.forward);
            normales.Add(Vector3.forward);
        }
        if (!right)
        {
            triangles.Add(3 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(0 + 4 * ind);
            triangles.Add(3 + 4 * ind);
            triangles.Add(2 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            ind++;

            vertices.Add(new Vector3(1, 0, 0));
            vertices.Add(new Vector3(1, 0, 1));
            vertices.Add(new Vector3(1, 1, 1));
            vertices.Add(new Vector3(1, 1, 0));

            uvs.Add(new Vector2(0.125f * 1, 0));
            uvs.Add(new Vector2(0.125f * 2, 0));
            uvs.Add(new Vector2(0.125f * 2, 1f));
            uvs.Add(new Vector2(0.125f * 1, 1));

            normales.Add(Vector3.right);
            normales.Add(Vector3.right);
            normales.Add(Vector3.right);
            normales.Add(Vector3.right);
        }

        if (!backward)
        {
            triangles.Add(3 + 4 * ind);
            triangles.Add(0 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(3 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(2 + 4 * ind);
            ind++;

            vertices.Add(new Vector3(0, 0, 1));
            vertices.Add(new Vector3(1, 0, 1));
            vertices.Add(new Vector3(1, 1, 1));
            vertices.Add(new Vector3(0, 1, 1));

            uvs.Add(new Vector2(0.125f * 2, 0));
            uvs.Add(new Vector2(0.125f * 3, 0));
            uvs.Add(new Vector2(0.125f * 3, 1f));
            uvs.Add(new Vector2(0.125f * 2, 1));

            normales.Add(Vector3.back);
            normales.Add(Vector3.back);
            normales.Add(Vector3.back);
            normales.Add(Vector3.back);
        }
        if (!left)
        {
            triangles.Add(3 + 4 * ind);
            triangles.Add(0 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(3 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(2 + 4 * ind);
            ind++;

            vertices.Add(new Vector3(0, 0, 0));
            vertices.Add(new Vector3(0, 0, 1));
            vertices.Add(new Vector3(0, 1, 1));
            vertices.Add(new Vector3(0, 1, 0));

            uvs.Add(new Vector2(0.125f * 3, 0));
            uvs.Add(new Vector2(0.125f * 4, 0));
            uvs.Add(new Vector2(0.125f * 4, 1f));
            uvs.Add(new Vector2(0.125f * 3, 1));

            normales.Add(Vector3.left);
            normales.Add(Vector3.left);
            normales.Add(Vector3.left);
            normales.Add(Vector3.left);
        }

        if (!up)
        {
            triangles.Add(3 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(0 + 4 * ind);
            triangles.Add(3 + 4 * ind);
            triangles.Add(2 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            ind++;

            vertices.Add(new Vector3(0, 1, 0));
            vertices.Add(new Vector3(1, 1, 0));
            vertices.Add(new Vector3(1, 1, 1));
            vertices.Add(new Vector3(0, 1, 1));

            uvs.Add(new Vector2(0.125f * 4, 0));
            uvs.Add(new Vector2(0.125f * 5, 0));
            uvs.Add(new Vector2(0.125f * 5, 1f));
            uvs.Add(new Vector2(0.125f * 4, 1));

            normales.Add(Vector3.up);
            normales.Add(Vector3.up);
            normales.Add(Vector3.up);
            normales.Add(Vector3.up);
        }
        if (!down)
        {
            triangles.Add(3 + 4 * ind);
            triangles.Add(0 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(3 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(2 + 4 * ind);
            ind++;

            vertices.Add(new Vector3(0, 0, 0));
            vertices.Add(new Vector3(1, 0, 0));
            vertices.Add(new Vector3(1, 0, 1));
            vertices.Add(new Vector3(0, 0, 1));

            uvs.Add(new Vector2(0.125f * 5, 0));
            uvs.Add(new Vector2(0.125f * 6, 0));
            uvs.Add(new Vector2(0.125f * 6, 1f));
            uvs.Add(new Vector2(0.125f * 5, 1));

            normales.Add(Vector3.down);
            normales.Add(Vector3.down);
            normales.Add(Vector3.down);
            normales.Add(Vector3.down);
        }

        BuildMesh();
    }
    void BuildMesh()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;

        MeshRenderer meshR = GetComponent<MeshRenderer>();
        if (!meshR.material)
            meshR.material = BuildingCollection.instance.GetMaterial(id);

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.normals = normales.ToArray();
    }
}
