using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{
    static int UILayer;
    static int InteractLayer;

    private void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
        InteractLayer = LayerMask.NameToLayer("Interact");
    }

    private void Update()
    {

    }

    public static InteractObject GetOverInteract()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && !IsPointerOverUIElement())
        {
            return hit.transform.GetComponent<InteractObject>();
        }
        else
        {
            return null;
        }
    }
    public static InteractObject GetOverInteract(out RaycastHit hitPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitPoint, Mathf.Infinity) && !IsPointerOverUIElement())
        {
            return hitPoint.transform.GetComponent<InteractObject>();
        }
        else
        {
            return null;
        }
    }

    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    private static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
