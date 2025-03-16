using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInMoveDirection : MonoBehaviour
{
    Vector2 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 point = (Vector2) transform.position - lastPos;
        Debug.Log($"{point.x}, {point.y}");
        if (point.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, point);
        }

        lastPos = transform.position;
    }
}
