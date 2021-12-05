using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Character : MonoBehaviour
{
    List<Node> Nodes = new List<Node>();
    public float speed = 2;
    public Vector3 actualPos = new Vector3();
    public Vector3 destPos = new Vector3();
    
    void Start()
    {
        StartCoroutine("Starter");
    }

    private IEnumerator Starter()
    {
        yield return new WaitForSeconds(1);
        Thread trd = new Thread(new ThreadStart(GetPath));
        trd.IsBackground = true;
        trd.Start();
    }

    void Update()
    {
        actualPos = transform.position;

        for (int i = 0; i < Nodes.Count; i++)
        {
            InteractObject obj = MapGenerator.instance.GetInteractObjectByWorld((int)Nodes[i].Pos.x, (int)Nodes[i].Pos.y, (int)Nodes[i].Pos.z);
            obj.GetComponent<BuildObject>().SetColor(new Color());
        }

        FollowPath();
    }

    void FollowPath()
    {
        if (Nodes.Count <= 0) return;

        float errorMarge = 0.05f;
        
        Vector3 nodePos = Nodes[0].Pos;
        if (MathT.FloatBetween(actualPos.x, nodePos.x - errorMarge, nodePos.x + errorMarge)
            && MathT.FloatBetween(actualPos.z, nodePos.z - errorMarge, nodePos.z + errorMarge))
        {
            InteractObject obj = MapGenerator.instance.GetInteractObjectByWorld((int)nodePos.x, (int)nodePos.y, (int)nodePos.z);
            obj.GetComponent<BuildObject>().SetColor();

            Nodes.RemoveAt(0);
        }
        if (!MathT.FloatBetween(actualPos.x, nodePos.x - errorMarge, nodePos.x + errorMarge))
        {
            int multiplier = actualPos.x < nodePos.x ? 1 : -1;
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0) * multiplier;
        }
        if (actualPos.y != nodePos.y + 1)
        {
            transform.position = new Vector3(actualPos.x, nodePos.y + 1, actualPos.z);
        }
        if (!MathT.FloatBetween(actualPos.z, nodePos.z - errorMarge, nodePos.z + errorMarge))
        {
            int multiplier = actualPos.z < nodePos.z ? 1 : -1;
            transform.position += new Vector3(0, 0, speed * Time.deltaTime) * multiplier;
        }
    }

    void GetPath()
    {
        Nodes = PathFinder.instance.GetPath(new Vector3(actualPos.x, actualPos.y - 1, actualPos.z), new Vector3(51, 2, 62));
    }
}
