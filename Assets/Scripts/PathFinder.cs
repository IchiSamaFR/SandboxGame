using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public static PathFinder instance;
    public Node[,,] Nodes;

    public bool PathFind;

    public static int Width { get => MapGenerator.instance.width * MapGenerator.instance.chunkWidth; }
    public static int Height { get => MapGenerator.instance.chunkHeight;  }
    public static int Length { get => MapGenerator.instance.length * MapGenerator.instance.chunkLength; }

    public Vector3 StartingNode;
    public Vector3 EndingNode;

    public bool diagonal = true;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        //SelectNextNode();
    }

    public List<Node> GetPath(Vector3 start, Vector3 end)
    {
        PathFind = false;
        Vector3 StartingNode = start;
        Vector3 EndingNode = end;

        Nodes = new Node[Width, Height, Length];
        Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z] = new Node(StartingNode, this);
        Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z].IsStartNode = true;
        Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z].SetCost(0, EndingNode);
        Nodes[(int)EndingNode.x, (int)EndingNode.y, (int)EndingNode.z] = new Node(EndingNode, this);
        Nodes[(int)EndingNode.x, (int)EndingNode.y, (int)EndingNode.z].IsEndNode = true;

        while (!Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z].PathFound)
        {
            SelectNextNode();
        }

        return Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z].NodesPath;
    }

    public bool SelectNextNode()
    {
        if (PathFind) return false;

        Node nodeToSelect = null;
        foreach (var node in Nodes)
        {
            if (node == null) continue;

            if (!node.isChecked && (nodeToSelect == null || node.Fcost < nodeToSelect.Fcost) && node.Fcost > 0)
            {
                nodeToSelect = node;
            }
        }
        if (nodeToSelect != null)
        {
            nodeToSelect.Select();
            return true;
        }
        else
        {
            PathFind = true;
            return false;
        }
    }
    

    public List<Node> GetNodesAround(Vector3 pos)
    {
        List<Node> nodes = new List<Node>();

        int posx = (int)pos.x;
        int posy = (int)pos.y;
        int posz = (int)pos.z;
        if (diagonal)
        {
            for (int x = posx - 1; x <= posx + 1; x++)
            {
                for (int z = posz - 1; z <= posz + 1; z++)
                {
                    if (x != posx || z != posz)
                    {
                        if(GetNode(x, posy, z) != null
                            && GetNode(x, posy + 1, z) == null)
                        {
                            nodes.Add(GetNode(x, posy, z));
                        }
                        else if(GetNode(x, posy + 1, z) != null
                            && GetNode(x, posy + 2, z) == null)
                        {
                            nodes.Add(GetNode(x, posy + 1, z));
                        }
                    }
                }
            }
        }
        else
        {
            if (GetNode(posx + 1, posy, posz) != null
                && GetNode(posx + 1, posy + 1, posz) == null)
            {
                nodes.Add(GetNode(posx + 1, posy, posz));
            }
            else if (GetNode(posx + 1, posy + 1, posz) != null
                && GetNode(posx + 1, posy + 2, posz) == null)
            {
                nodes.Add(GetNode(posx + 1, posy + 1, posz));
                print("More");
            }

            if (GetNode(posx - 1, posy, posz) != null
                && GetNode(posx - 1, posy + 1, posz) == null)
            {
                nodes.Add(GetNode(posx - 1, posy, posz));
            }
            else if (GetNode(posx - 1, posy + 1, posz) != null
                && GetNode(posx - 1, posy + 2, posz) == null)
            {
                nodes.Add(GetNode(posx - 1, posy + 1, posz));
                print("More");
            }

            if (GetNode(posx, posy, posz + 1) != null
                && GetNode(posx, posy + 1, posz + 1) == null)
            {
                nodes.Add(GetNode(posx, posy, posz + 1));
            }
            else if (GetNode(posx, posy + 1, posz + 1) != null
                && GetNode(posx, posy + 2, posz + 1) == null)
            {
                nodes.Add(GetNode(posx, posy + 1, posz + 1));
                print("More");
            }

            if (GetNode(posx, posy, posz - 1) != null
                && GetNode(posx, posy + 1, posz - 1) == null)
            {
                nodes.Add(GetNode(posx, posy, posz - 1));
            }
            else if (GetNode(posx, posy + 1, posz - 1) != null
                && GetNode(posx, posy + 2, posz - 1) == null)
            {
                nodes.Add(GetNode(posx, posy + 1, posz - 1));
                print("More");
            }
        }

        return nodes;
    }
    public Node GetNode(int x, int y, int z)
    {
        if (x < 0 || x >= Width || z < 0 || z >= Length) return null;

        if(Nodes[x, y, z] == null)
        {
            InteractObject obj = MapGenerator.instance.GetInteractObjectByWorld(x, y, z);
            if (obj != null)
                Nodes[x, y, z] = new Node(new Vector3(obj.WorldPosX, obj.WorldPosY, obj.WorldPosZ), this);
        }

        return Nodes[x, y, z];
    }
}

public class Node
{
    public PathFinder Path;
    public List<Node> NodesPath = new List<Node>();

    public bool IsEndNode; //Is the ending node
    public bool IsStartNode; //Is the start node
    public bool PathFound;

    public bool isChecked;

    public Node Previous;
    public Vector3 Pos;
    public Vector3 EndNodePos;

    public int Gcost = 0;
    public int Hcost = 0;
    public int Fcost { get => Gcost + Hcost; }

    public Node(Vector3 pos, PathFinder path)
    {
        Pos = pos;
        Path = path;
    }

    public void Select()
    {
        isChecked = true;
        CheckAround();
    }

    public void ResetValues()
    {
        Gcost = 0;
        Hcost = 0;
        isChecked = false;
        PathFound = false;
    }

    public void SetEndPath(Node node, List<Node> path = null)
    {
        PathFound = true;
        InteractObject obj = MapGenerator.instance.GetInteractObjectByWorld((int)Pos.x, (int)Pos.y, (int)Pos.z);

        NodesPath.Add(node);
        if(path != null)
            NodesPath.AddRange(path);

        if (Previous != null)
        {
            Previous.SetEndPath(this, NodesPath);
        }
    }

    public void SetCost(int gcost, Vector3 end)
    {
        Gcost = gcost;
        EndNodePos = end;

        if (IsEndNode)
        {
            Path.PathFind = true;
            Previous.SetEndPath(this);
            return;
        }

        Hcost = SumHcost(end);
    }

    public int SumHcost(Vector3 end)
    {
        int x = (int)(end.x - Pos.x >= 0 ? end.x - Pos.x : Pos.x - end.x);
        int z = (int)(end.z - Pos.z >= 0 ? end.z - Pos.z : Pos.z - end.z);

        int val = 0;
        if (x -z >= 0)
        {
            val += x * 10;
            val += (x - (x - z)) * 4;
        }
        else
        {
            val += z * 10;
            val += (z - (z - x)) * 4;
        }
        return val;
    }

    public void SetCost(int gcost, int hcost)
    {
        Gcost = gcost;
        Hcost = hcost;
    }
    public void CheckAround()
    {
        List<Node> nodes = Path.GetNodesAround(Pos);

        for (int i = 0; i < nodes.Count; i++)
        {
            int newGCost = 0;
            if (nodes[i].Pos.x != Pos.x && nodes[i].Pos.y != Pos.y)
                newGCost = Gcost + 14;
            else
                newGCost = Gcost + 10;

            if (!nodes[i].IsStartNode && (nodes[i].Gcost <= 0 || nodes[i]?.Gcost > newGCost))
            {
                nodes[i].Previous = this;
                nodes[i].SetCost(newGCost, EndNodePos);
                if (nodes[i].PathFound) break;
            }
        }
    }
}
