﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Character : MonoBehaviour
{
    List<Vector3> Nodes = new List<Vector3>();
    private bool resetLook = true;
    public float speed = 2;
    public Vector3 transformPos = new Vector3();
    public Vector3 actualPos = new Vector3();

    private void Start()
    {
        actualPos = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
    }
    
    private void FixedUpdate()
    {
        transformPos = transform.position;
        FollowPath();
    }

    void LookNode()
    {
        if (Nodes.Count > 0)
        {
            Vector3 pos = Nodes[0];

            if (actualPos.x == pos.x && actualPos.z < pos.z)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (actualPos.x < pos.x && actualPos.z < pos.z)
            {
                transform.rotation = Quaternion.Euler(0, 45, 0);
            }
            else if (actualPos.x < pos.x && actualPos.z == pos.z)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (actualPos.x < pos.x && actualPos.z > pos.z)
            {
                transform.rotation = Quaternion.Euler(0, 135, 0);
            }
            else if (actualPos.x == pos.x && actualPos.z > pos.z)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);//
            }
            else if (actualPos.x > pos.x && actualPos.z > pos.z)
            {
                transform.rotation = Quaternion.Euler(0, 225, 0);
            }
            else if (actualPos.x > pos.x && actualPos.z == pos.z)
            {
                transform.rotation = Quaternion.Euler(0, 270, 0);
            }
            else if (actualPos.x > pos.x && actualPos.z < pos.z)
            {
                transform.rotation = Quaternion.Euler(0, 315, 0);
            }
        }
    }
    void FollowPath()
    {
        if (Nodes.Count <= 0) return;
        LookNode();

        float errorMarge = 0.05f;

        Vector3 nodePos = Nodes[0] + new Vector3(0.5f, 0, 0.5f);
        if (!MathT.FloatBetween(transformPos.x, nodePos.x - errorMarge, nodePos.x + errorMarge))
        {
            int multiplier = transformPos.x < nodePos.x ? 1 : -1;
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0) * multiplier;
        }
        if (transformPos.y != nodePos.y + 1
            && MathT.FloatBetween(transformPos.x, nodePos.x - 0.5f, nodePos.x + 0.5f)
            && MathT.FloatBetween(transformPos.z, nodePos.z - 0.5f, nodePos.z + 0.5f))
        {
            transform.position = new Vector3(transformPos.x, nodePos.y + 1, transformPos.z);
        }
        if (!MathT.FloatBetween(transformPos.z, nodePos.z - errorMarge, nodePos.z + errorMarge))
        {
            int multiplier = transformPos.z < nodePos.z ? 1 : -1;
            transform.position += new Vector3(0, 0, speed * Time.deltaTime) * multiplier;
        }

        if (MathT.FloatBetween(transformPos.x, nodePos.x - errorMarge, nodePos.x + errorMarge)
            && MathT.FloatBetween(transformPos.z, nodePos.z - errorMarge, nodePos.z + errorMarge))
        {
            actualPos = Nodes[0];
            Nodes.RemoveAt(0);
        }
    }

    #region PathFinding
    Thread pathFinderThread;
    public void GoTo(Vector3 postoGo)
    {
        if (pathFinderThread != null)
        {
            pathFinderThread.Abort();
        }
        pathFinderThread = new Thread(() => GetPathTo(postoGo));
        pathFinderThread.Start();
        pathFinderThread.IsBackground = true;
    }
    private void GetPathTo(Vector3 postoGo)
    {
        Nodes = PathFinder.GetPath(new Vector3(transformPos.x, transformPos.y - 1, transformPos.z), 
                                   new Vector3(postoGo.x, postoGo.y, postoGo.z));
    }
    #endregion
}
