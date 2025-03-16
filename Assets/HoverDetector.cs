using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class HoverDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static bool IsHovering(GameObject go)
    {
        if (results.Count > 0)
            return results[0].gameObject == go;
        return false;
    }

    public static bool IsHoveringAll(GameObject go)
    {
        return results.Select(r => r.gameObject).Contains(go);
    }


    static List<RaycastResult> results = new();
    // Update is called once per frame
    void Update()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.mousePosition;

        EventSystem.current.RaycastAll(pointerData, results);

        //if (Input.GetMouseButton(0))
        //{
        //    Debug.Log("Hovering: " + string.Join("\n", results.Select(r => r.gameObject)));
        //}
    }
}
