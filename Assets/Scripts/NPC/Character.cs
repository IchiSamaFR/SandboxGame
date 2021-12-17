using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Character : MonoBehaviour
{
    List<PathFinder.Node> Nodes = new List<PathFinder.Node>();
    private bool resetLook = true;
    public float speed = 2;
    public Vector3 actualPos = new Vector3();
    public Vector3 destPos = new Vector3();
    
    void Start()
    {
    }

    void Update()
    {
        actualPos = transform.position;

        LookNode();

        FollowPath();
    }

    void FollowPath()
    {
        if (Nodes.Count <= 0) return;

        float errorMarge = 0.05f;
        
        Vector3 nodePos = Nodes[0].Pos + new Vector3(0.5f, 0, 0.5f);
        if (!MathT.FloatBetween(actualPos.x, nodePos.x - errorMarge, nodePos.x + errorMarge))
        {
            int multiplier = actualPos.x < nodePos.x ? 1 : -1;
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0) * multiplier;
        }
        if (actualPos.y != nodePos.y + 1
            && MathT.FloatBetween(actualPos.x, nodePos.x - 0.5f, nodePos.x + 0.5f)
            && MathT.FloatBetween(actualPos.z, nodePos.z - 0.5f, nodePos.z + 0.5f))
        {
            transform.position = new Vector3(actualPos.x, nodePos.y + 1, actualPos.z);
        }
        if (!MathT.FloatBetween(actualPos.z, nodePos.z - errorMarge, nodePos.z + errorMarge))
        {
            int multiplier = actualPos.z < nodePos.z ? 1 : -1;
            transform.position += new Vector3(0, 0, speed * Time.deltaTime) * multiplier;
        }

        if (MathT.FloatBetween(actualPos.x, nodePos.x - errorMarge, nodePos.x + errorMarge)
            && MathT.FloatBetween(actualPos.z, nodePos.z - errorMarge, nodePos.z + errorMarge))
        {
            Nodes.RemoveAt(0);
            resetLook = true;
        }
    }
    
    public void GoTo(Vector3 postoGo)
    {
        Thread trd = new Thread(() => GetPathTo(postoGo));
        trd.Start();
        trd.IsBackground = true;
    }

    void GetPathTo(Vector3 postoGo)
    {
        Nodes = PathFinder.GetPath(new Vector3(actualPos.x, actualPos.y - 1, actualPos.z), new Vector3(postoGo.x, postoGo.y, postoGo.z));
        resetLook = true;
    }
    void LookNode()
    {
        if (Nodes.Count > 0 && resetLook)
        {
            Vector3 pos = Nodes[0].Pos;
            transform.LookAt(new Vector3(pos.x, transform.position.y, pos.z));
            resetLook = false;
        }
    }
}
