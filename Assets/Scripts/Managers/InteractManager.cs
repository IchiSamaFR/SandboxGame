using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    Character CharacterSelected;


    void Update()
    {
        CheckRaycast();
    }

    void CheckRaycast()
    {
        if (!MouseManager.IsNormalMode) return;
        
        Transform over = MouseManager.GetOver();


        if (Input.GetMouseButtonDown(0))
        {
            if (over.GetComponent<Character>())
            {
                CharacterSelected = over.GetComponent<Character>();
                return;
            }

            if (CharacterSelected && over.GetComponent<InteractObject>())
            {
                Vector3 vec = over.position;
                CharacterSelected.GoTo(vec);
                return;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            CharacterSelected = null;
        }
    }
}
