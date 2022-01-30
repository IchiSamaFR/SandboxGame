using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class PathFinder
{
    public class Node
    {
        public List<Vector3> NodesPath = new List<Vector3>();

        public bool IsEndNode;
        public bool IsStartNode;
        public bool PathFound;

        public bool isChecked;

        public Node Previous;
        public Vector3 Pos;
        public Vector3 EndNodePos;

        public int Gcost = 0;
        public int Hcost = 0;
        public int Fcost { get => Gcost + Hcost; }

        public bool WalkableOn;
        public bool WalkableIn;

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

        public void SetEndPath(Node node, List<Vector3> path = null)
        {
            PathFound = true;
            InteractObject obj = MapGenerator.instance.GetInteractObjectByWorld((int)Pos.x, (int)Pos.y, (int)Pos.z);

            if(node.WalkableIn)
                NodesPath.Add(node.Pos + new Vector3(0, - 1, 0));
            else
                NodesPath.Add(node.Pos);

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
            return MathT.DistanceCost(end, Pos);
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
    public static int Width { get => MapGenerator.instance.width * MapGenerator.instance.ChunkWidth; }
    public static int Height { get => MapGenerator.instance.ChunkHeight;  }
    public static int Length { get => MapGenerator.instance.length * MapGenerator.instance.ChunkLength; }

    public static List<Vector3> GetPath(Vector3 start, Vector3 end)
    {
        if ((int)start.x == (int)end.x && MathT.FloatBetween(start.y - end.y, -1, 1) && (int)start.z == (int)end.z)
            return new List<Vector3>();
        
        Node[,,] Nodes = new Node[Width, Height, Length];
        Vector3 StartingNode = start;
        Vector3 EndingNode = end;

        while (true)
        {
            if (GetNode(Nodes, (int)EndingNode.x, (int)EndingNode.y, (int)EndingNode.z).WalkableIn)
            {
                if (EndingNode.y == 0)
                    return new List<Vector3>();

                EndingNode.y -= 1;
            }
            break;
        }


        Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z] = GetNode(Nodes, (int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z);
        Nodes[(int)EndingNode.x, (int)EndingNode.y, (int)EndingNode.z] = GetNode(Nodes, (int)EndingNode.x, (int)EndingNode.y, (int)EndingNode.z);

        //if (Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z] == null || Nodes[(int)EndingNode.x, (int)EndingNode.y, (int)EndingNode.z] == null) return new List<Vector3>();

        Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z].IsStartNode = true;
        Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z].SetCost(0, EndingNode);
        Nodes[(int)EndingNode.x, (int)EndingNode.y, (int)EndingNode.z].IsEndNode = true;

        Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z].Select(Nodes);

        if(!Nodes[(int)EndingNode.x, (int)EndingNode.y, (int)EndingNode.z].WalkableIn &&
           !Nodes[(int)EndingNode.x, (int)EndingNode.y, (int)EndingNode.z].WalkableOn)
        {
            return new List<Vector3>();
        }

        bool next = true;
        while (!Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z].PathFound && next)
        {
            next = SelectNextNode(Nodes);
        }

        return Nodes[(int)StartingNode.x, (int)StartingNode.y, (int)StartingNode.z].NodesPath;
    }
    public static bool SelectNextNode(Node[,,] Nodes)
    {
        Node nodeToSelect = null;
        foreach (var node in Nodes)
        {
            if (node == null) continue;
            
            if (!node.isChecked && (nodeToSelect == null || node.Fcost < nodeToSelect.Fcost) && node.Fcost > 0
                && (node.WalkableIn || node.WalkableOn))
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
                    Node nodeToCheck;
                    Node upNodeToCheck;
                    for (int i = -1; i <= 1; i++)
                    {
                        nodeToCheck = GetNode(Nodes, x, posy + i, z);
                        upNodeToCheck = GetNode(Nodes, x, posy + i + 1, z);
                        if (nodeToCheck != null
                            && (upNodeToCheck == null || upNodeToCheck.WalkableIn))
                        {
                            nodes.Add(nodeToCheck);
                            break;
                        }
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
            {
                Nodes[x, y, z] = new Node(new Vector3(obj.WorldPosX, obj.WorldPosY, obj.WorldPosZ));
                Nodes[x, y, z].WalkableIn = obj.WalkableIn();
                Nodes[x, y, z].WalkableOn = obj.WalkableOn();
            }
        }

        return Nodes[x, y, z];
    }
}
