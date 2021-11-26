using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuild : BuildObject, IConnectedBuild
{
    [Header("Model connections")]
    public List<string> ConectionsAble = new List<string>();
    public List<GameObject> Models = new List<GameObject>();

    public override void Set(Chunk chunk, int posX, int posY, int posZ)
    {
        base.Set(chunk, posX, posY, posZ);
    }

    public override void SetAfterInit()
    {
        base.SetAfterInit();
        SetConnections();
    }

    bool right = false, left = false, forward = false, backward = false;
    public void SetConnections()
    {
        bool findRight = false, findLeft = false, findForward = false, findBackward = false;

        List<InteractObject> aroundInteracts = GetAroundInteract();
        List<InteractObject> toRefresh = new List<InteractObject>();

        for (int i = 0; i < aroundInteracts.Count; i++)
        {
            if (WorldPosX > aroundInteracts[i].WorldPosX)
            {
                if (!left)
                {
                    left = true;
                    toRefresh.Add(aroundInteracts[i]);
                }
                findLeft = true;
            }
            else if (WorldPosX < aroundInteracts[i].WorldPosX)
            {
                if (!right)
                {
                    right = true;
                    toRefresh.Add(aroundInteracts[i]);
                }
                findRight = true;
            }
            else if (WorldPosZ > aroundInteracts[i].WorldPosZ)
            {
                if (!backward)
                {
                    backward = true;
                    toRefresh.Add(aroundInteracts[i]);
                }
                findBackward = true;
            }
            else if (WorldPosZ < aroundInteracts[i].WorldPosZ)
            {
                if (!forward)
                {
                    forward = true;
                    toRefresh.Add(aroundInteracts[i]);
                }
                findForward = true;
            }
        }
        left = findLeft;
        right = findRight;
        forward = findForward;
        backward = findBackward;

        ConnectModel(right, left, forward, backward);
        for (int i = 0; i < toRefresh.Count; i++)
        {
            toRefresh[i].GetComponent<IConnectedBuild>()?.SetConnections();
        }
    }

    public void ConnectModel(bool right, bool left, bool forward, bool backward)
    {
        InactiveModels();

        if (right && left && forward && backward)
        {
            if(Models[3] != null)
                Models[3].SetActive(true);
        }
        else if (right && left && forward && !backward)
        {
            if (Models[2] != null)
            {
                Models[2].SetActive(true);
                Models[2].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (right && !left && forward && backward)
        {
            if (Models[2] != null)
            {
                Models[2].SetActive(true);
                Models[2].transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
        else if (right && left && !forward && backward)
        {
            if (Models[2] != null)
            {
                Models[2].SetActive(true);
                Models[2].transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else if (!right && left && forward && backward)
        {
            if (Models[2] != null)
            {
                Models[2].SetActive(true);
                Models[2].transform.rotation = Quaternion.Euler(0, 270, 0);
            }
        }
        else if (right && !left && forward && !backward)
        {
            if (Models[1] != null)
            {
                Models[1].SetActive(true);
                Models[1].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (right && !left && !forward && backward)
        {
            if (Models[1] != null)
            {
                Models[1].SetActive(true);
                Models[1].transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
        else if (!right && left && !forward && backward)
        {
            if (Models[1] != null)
            {
                Models[1].SetActive(true);
                Models[1].transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else if (!right && left && forward && !backward)
        {
            if (Models[1] != null)
            {
                Models[1].SetActive(true);
                Models[1].transform.rotation = Quaternion.Euler(0, 270, 0);
            }
        }
        else if (right || left)
        {
            if (Models[0] != null)
            {
                Models[0].SetActive(true);
                Models[0].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (forward || backward)
        {
            if (Models[0] != null)
            {
                Models[0].SetActive(true);
                Models[0].transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
        else
        {
            if (Models[0] != null)
            {
                Models[0].SetActive(true);
                Models[0].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            //Debug.LogError($"Error connectAround.\nRight : {right}\nLeft : {left}\nForward : {forward}\nBack : {backward}");
        }
    }

    public void InactiveModels()
    {
        for (int i = 0; i < Models.Count; i++)
        {
            Models[i].SetActive(false);
        }
    }

    public override void Destroy()
    {
        List<InteractObject> lst = GetAroundInteract();
        for (int i = 0; i < lst.Count; i++)
        {
            lst[i].GetComponent<IConnectedBuild>()?.SetConnections();
        }
        base.Destroy();
    }
}
