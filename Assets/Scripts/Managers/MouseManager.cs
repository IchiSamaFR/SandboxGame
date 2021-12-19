using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{
    enum Mode
    {
        normal,
        building
    }

    public static LayerMask UILayer = 5;
    public static LayerMask InteractLayer = 8;
    
    static Mode MouseMode = Mode.building;
    public static bool IsNormalMode { get => MouseMode == Mode.normal; }
    public static bool IsBuildingMode { get => MouseMode == Mode.building; }

    private void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
        InteractLayer = LayerMask.NameToLayer("Interact");
    }

    public static void SetNormalMouse()
    {
        MouseMode = Mode.normal;
    }
    public static void SetBuildingMouse()
    {
        MouseMode = Mode.building;
    }

    public static Transform GetOver()
    {
        RaycastHit hitPoint;
        return GetOver(out hitPoint);

    }
    public static Transform GetOver(out RaycastHit hitPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitPoint, Mathf.Infinity) && !IsPointerOverUIElement())
        {
            return hitPoint.transform;
        }
        else
        {
            return null;
        }
    }

    public static Character GetOverCharacter()
    {
        RaycastHit hitPoint;
        return GetOverCharacter(out hitPoint);

    }
    public static Character GetOverCharacter(out RaycastHit hitPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitPoint, Mathf.Infinity) && !IsPointerOverUIElement())
        {
            return hitPoint.transform.GetComponent<Character>();
        }
        else
        {
            return null;
        }
    }

    public static InteractObject GetOverInteract()
    {
        RaycastHit hitPoint;
        return GetOverInteract(out hitPoint);
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
