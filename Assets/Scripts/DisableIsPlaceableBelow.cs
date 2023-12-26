using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIsPlaceableBelow : MonoBehaviour
{
    void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
        {

            if (hit.collider.GetComponent<Waypoint>())
            {
                hit.collider.GetComponent<Waypoint>().MakeIsPlaceableFalse();
                hit.collider.GetComponent<Waypoint>().placedObject = gameObject;
            }
        }
    }
}
