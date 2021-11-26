using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    public float ScrollSensitivty = 0.3f;

    public float MinHeight = 2f;
    public float MaxHeight = 20f;
    public float ActualHeight { get => transform.position.y - target.position.y; }
    float newActualHeight;

    public float Speed = 2f;
    public float ActualSpeed { get => (Speed + ActualHeight / 4f) * Time.deltaTime; }

    void Start()
    {
        newActualHeight = ActualHeight;
        transform.position = new Vector3(transform.position.x, ActualHeight, transform.position.z);
        transform.LookAt(target);
    }
    
    void Update()
    {
        CheckMouseScroll();
        CheckMovement();
        if (ActualHeight != newActualHeight)
            SetHeight(newActualHeight);
    }

    void CheckMovement()
    {
        if (Input.GetKey(Keys.up))
        {
            target.position += new Vector3(0, 0, ActualSpeed);
        }
        if (Input.GetKey(Keys.down))
        {
            target.position += new Vector3(0, 0, -ActualSpeed);
        }
        if (Input.GetKey(Keys.right))
        {
            target.position += new Vector3(ActualSpeed, 0, 0);
        }
        if (Input.GetKey(Keys.left))
        {
            target.position += new Vector3(-ActualSpeed, 0, 0);
        }
    }

    void CheckMouseScroll()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newActualHeight = newActualHeight - Input.mouseScrollDelta.y * ScrollSensitivty;
        }
    }

    void SetHeight(float height)
    {
        if (height > MaxHeight) newActualHeight = height = MaxHeight;
        if (height < MinHeight) newActualHeight = height = MinHeight;

        Vector3 newpos = new Vector3(transform.position.x,
                                     height,
                                     transform.position.z);
        transform.position = Vector3.LerpUnclamped(newpos, transform.position, 0.8f);
        transform.LookAt(target);
    }
}
