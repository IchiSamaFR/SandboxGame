using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    private Character CharacterSelected;


    void Update()
    {
        CheckRaycast();
    }

    void CheckRaycast()
    {
        if (!MouseManager.IsNormalMode)
        {
            CharacterSelected = null;
            return;
        }
        
        Transform over = MouseManager.GetOver();


        if (Input.GetMouseButtonDown(0) && over)
        {
            if (over.GetComponent<Character>())
            {
                CharacterSelected = over.GetComponent<Character>();
                return;
            }

            if (CharacterSelected && over.GetComponent<InteractObject>())
            {
                CharacterSelected.GoTo(over.GetComponent<InteractObject>());
                return;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            CharacterSelected = null;
        }
    }
}
