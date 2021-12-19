using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public class Node
    {
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

        public Node(Vector3 pos)
        {
            Pos = pos;
        }

        public void Select(Node[,,] Nodes)
        {
            isChecked = true;
            CheckAround(Nodes);
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
            if (path != null)
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
                SetEndPath(this);
                return;
            }

            Hcost = SumHcost(end);
        }

        public int SumHcost(Vector3 end)
        {
            int x = (int)(end.x - Pos.x >= 0 ? end.x - Pos.x : Pos.x - end.x);
            int z = (int)(end.z - Pos.z >= 0 ? end.z - Pos.z : Pos.z - end.z);

            int val = 0;
            if (x - z >= 0)
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
        public void CheckAround(Node[,,] Nodes)
        {
            List<Node> nodes = PathFinder.GetNodesAround(Nodes, Pos);
            for (int i = 0; i < nodes.Count; i++)
            {
                int newGCost = 0;
                if (nodes[i].Pos.x != Pos.x && nodes[i].Pos.z != Pos.z)
                    newGCost = Gcost + 14;
                else
                    newGCost = Gcost + 10;

                if (!nodes[i].IsStartNode && (nodes[i].Gcost <= 0 || nodes[i].Gcost > newGCost))
                {
                    nodes[i].Previous = this;
                    nodes[i].SetCost(newGCost, EndNodePos);
                    if (nodes[i].PathFound) break;
                }
            }
        }
    }

    public static PathFinder instance;

    public static int Width { get => MapGenerator.instance.width * MapGenerator.instance.chunkWidth; }
    public static int Height { get => MapGenerator.instance.chunkHeight;  }
    public static int Length { get => MapGenerator.instance.length * MapGenerator.instance.chunkLength; }
    
    private void Awake()
    {
        instance = this;
    }

    public static Task<List<Node>> GetPath(Vector3 start, Vector3 end)
    {
        if ((int)start.x == (int)end.x
           && (int)start.y == (int)end.y
           && (int)start.z == (int)end.z)
            return Task.FromResult(new List<Node>());

        Vector3 StartingNode = start;
        Vector3 EndingNode = end;

        Node[,,] Nodes = new Node[Width, Height, Length];
        Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z] = new Node(StartingNode);
        Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z].IsStartNode = true;
        Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z].SetCost(0, EndingNode);
        Nodes[(int)EndingNode.x, (int)EndingNode.y, (int)EndingNode.z] = new Node(EndingNode);
        Nodes[(int)EndingNode.x, (int)EndingNode.y, (int)EndingNode.z].IsEndNode = true;

        Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z].Select(Nodes);

        bool next = true;
        while (!Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z].PathFound && next)
        {
            next = SelectNextNode(Nodes);
        }

        return Task.FromResult(Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z].NodesPath);
    }
    public static bool SelectNextNode(Node[,,] Nodes)
    {
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
            nodeToSelect.Select(Nodes);
            return true;
        }
        return false;
    }
    
    public static List<Node> GetNodesAround(Node[,,] Nodes, Vector3 pos)
    {
        List<Node> nodes = new List<Node>();

        int posx = (int)pos.x;
        int posy = (int)pos.y;
        int posz = (int)pos.z;

        for (int x = posx - 1; x <= posx + 1; x++)
        {
            for (int z = posz - 1; z <= posz + 1; z++)
            {
                if (x != posx || z != posz)
                {
                    if (GetNode(Nodes, x, posy, z) != null
                        && GetNode(Nodes, x, posy + 1, z) == null)
                    {
                        nodes.Add(GetNode(Nodes, x, posy, z));
                    }
                    else if (GetNode(Nodes, x, posy + 1, z) != null
                        && GetNode(Nodes, x, posy + 2, z) == null)
                    {
                        nodes.Add(GetNode(Nodes, x, posy + 1, z));
                    }
                    else if (GetNode(Nodes, x, posy - 1, z) != null
                        && GetNode(Nodes, x, posy, z) == null)
                    {
                        nodes.Add(GetNode(Nodes, x, posy - 1, z));
                    }
                }
            }
        }
        return nodes;
    }
    public static Node GetNode(Node[,,] Nodes, int x, int y, int z)
    {
        if (x < 0 || x >= Width || z < 0 || z >= Length || y < 0 || y >= Height) return null;

        if(Nodes[x, y, z] == null)
        {
            InteractObject obj = MapGenerator.instance.GetInteractObjectByWorld(x, y, z);
            if (obj != null)
                Nodes[x, y, z] = new Node(new Vector3(obj.WorldPosX, obj.WorldPosY, obj.WorldPosZ));
        }

        return Nodes[x, y, z];
    }
}
