using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Character : MonoBehaviour
{
    List<Vector3> Nodes = new List<Vector3>();
    public float speed = 2;
    public Vector3 transformPos = new Vector3();
    public Vector3 actualPos = new Vector3();
    public Transform model;

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
                model.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (actualPos.x < pos.x && actualPos.z < pos.z)
            {
                model.rotation = Quaternion.Euler(0, 45, 0);
            }
            else if (actualPos.x < pos.x && actualPos.z == pos.z)
            {
                model.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (actualPos.x < pos.x && actualPos.z > pos.z)
            {
                model.rotation = Quaternion.Euler(0, 135, 0);
            }
            else if (actualPos.x == pos.x && actualPos.z > pos.z)
            {
                model.rotation = Quaternion.Euler(0, 180, 0);//
            }
            else if (actualPos.x > pos.x && actualPos.z > pos.z)
            {
                model.rotation = Quaternion.Euler(0, 225, 0);
            }
            else if (actualPos.x > pos.x && actualPos.z == pos.z)
            {
                model.rotation = Quaternion.Euler(0, 270, 0);
            }
            else if (actualPos.x > pos.x && actualPos.z < pos.z)
            {
                model.rotation = Quaternion.Euler(0, 315, 0);
            }
        }
    }
    void FollowPath()
    {
        if (Nodes.Count <= 0) return;
        LookNode();

        float errorMarge = 0.1f;

        Vector3 nodePos = Nodes[0];
        if (!MathT.FloatBetween(transformPos.x, nodePos.x - errorMarge, nodePos.x + errorMarge))
        {
            int multiplier = transformPos.x < nodePos.x ? 1 : -1;
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0) * multiplier;
        }
        else
        {
            transform.position = new Vector3(nodePos.x, transform.position.y, transform.position.z);
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
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, nodePos.z);
        }

        if (MathT.FloatBetween(transformPos.x, nodePos.x - errorMarge, nodePos.x + errorMarge)
            && MathT.FloatBetween(transformPos.z, nodePos.z - errorMarge, nodePos.z + errorMarge))
        {
            actualPos = Nodes[0] + new Vector3(0, 1, 0);
            Nodes.RemoveAt(0);
        }
    }

    #region PathFinding
    Thread pathFinderThread;
    public void GoTo(InteractObject interact)
    {
        Vector3 posToGo = interact.transform.position;

        if (interact.GetComponent<IRessourceInteract>() != null)
        {
            interact.GetComponent<IRessourceInteract>().HarvestHit();
            InteractObject interactAround = interact.GetWalkableAround(transform.position);
            if (!interactAround) return;
            posToGo = interactAround.transform.position;
        }

        if (pathFinderThread != null)
            pathFinderThread.Abort();
        pathFinderThread = new Thread(() => GetPathTo(posToGo));
        pathFinderThread.Start();
        pathFinderThread.IsBackground = true;
    }
    private void GetPathTo(Vector3 postoGo)
    {
        Nodes = PathFinder.GetPath(actualPos + new Vector3(0, -1, 0), 
                                   new Vector3(postoGo.x, postoGo.y, postoGo.z));
    }
    #endregion
}
