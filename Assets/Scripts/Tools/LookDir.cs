using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class to make the UI look the camera
 */
public class LookDir : MonoBehaviour
{
    public GameObject toLook;
    void Start()
    {
        transform.LookAt(toLook.transform);
        transform.rotation = Quaternion.Euler(-transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180, transform.rotation.eulerAngles.z);
    }
    
    void Update()
    {

    }
}
