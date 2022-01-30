using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Character : MonoBehaviour
{
    private List<Vector3> nodes = new List<Vector3>();
    public float Speed = 2;
    public Vector3 TransformPos = new Vector3();
    public Vector3 ActualPos = new Vector3();
    public Transform Model;

    private void Start()
    {
        ActualPos = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
    }
    
    private void FixedUpdate()
    {
        TransformPos = transform.position;
        FollowPath();
    }

    void LookNode()
    {
        if (nodes.Count > 0)
        {
            Vector3 pos = nodes[0];

            if (ActualPos.x == pos.x && ActualPos.z < pos.z)
            {
                Model.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (ActualPos.x < pos.x && ActualPos.z < pos.z)
            {
                Model.rotation = Quaternion.Euler(0, 45, 0);
            }
            else if (ActualPos.x < pos.x && ActualPos.z == pos.z)
            {
                Model.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (ActualPos.x < pos.x && ActualPos.z > pos.z)
            {
                Model.rotation = Quaternion.Euler(0, 135, 0);
            }
            else if (ActualPos.x == pos.x && ActualPos.z > pos.z)
            {
                Model.rotation = Quaternion.Euler(0, 180, 0);//
            }
            else if (ActualPos.x > pos.x && ActualPos.z > pos.z)
            {
                Model.rotation = Quaternion.Euler(0, 225, 0);
            }
            else if (ActualPos.x > pos.x && ActualPos.z == pos.z)
            {
                Model.rotation = Quaternion.Euler(0, 270, 0);
            }
            else if (ActualPos.x > pos.x && ActualPos.z < pos.z)
            {
                Model.rotation = Quaternion.Euler(0, 315, 0);
            }
        }
    }
    void FollowPath()
    {
        if (nodes.Count <= 0) return;
        LookNode();

        float errorMarge = 0.1f;

        Vector3 nodePos = nodes[0];
        if (!MathT.FloatBetween(TransformPos.x, nodePos.x - errorMarge, nodePos.x + errorMarge))
        {
            int multiplier = TransformPos.x < nodePos.x ? 1 : -1;
            transform.position += new Vector3(Speed * Time.deltaTime, 0, 0) * multiplier;
        }
        else
        {
            transform.position = new Vector3(nodePos.x, transform.position.y, transform.position.z);
        }
        if (TransformPos.y != nodePos.y + 1
            && MathT.FloatBetween(TransformPos.x, nodePos.x - 0.5f, nodePos.x + 0.5f)
            && MathT.FloatBetween(TransformPos.z, nodePos.z - 0.5f, nodePos.z + 0.5f))
        {
            transform.position = new Vector3(TransformPos.x, nodePos.y + 1, TransformPos.z);
        }
        if (!MathT.FloatBetween(TransformPos.z, nodePos.z - errorMarge, nodePos.z + errorMarge))
        {
            int multiplier = TransformPos.z < nodePos.z ? 1 : -1;
            transform.position += new Vector3(0, 0, Speed * Time.deltaTime) * multiplier;
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, nodePos.z);
        }

        if (MathT.FloatBetween(TransformPos.x, nodePos.x - errorMarge, nodePos.x + errorMarge)
            && MathT.FloatBetween(TransformPos.z, nodePos.z - errorMarge, nodePos.z + errorMarge))
        {
            ActualPos = nodes[0] + new Vector3(0, 1, 0);
            nodes.RemoveAt(0);
        }
    }

    #region PathFinding
    Thread pathFinderThread;
    public void GoTo(InteractObject interact)
    {
        Vector3 posToGo = interact.transform.position;

        if (interact.GetComponent<ResourceObject>() != null)
        {
            interact.GetComponent<ResourceObject>().HarvestHit(4);
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
        nodes = PathFinder.GetPath(ActualPos + new Vector3(0, -1, 0), 
                                   new Vector3(postoGo.x, postoGo.y, postoGo.z));
    }
    #endregion
}
