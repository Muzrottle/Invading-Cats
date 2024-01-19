using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIsPlaceableBelow : MonoBehaviour
{
    void OnEnable()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
        {

            if (hit.collider.GetComponent<Tile>())
            {
                hit.collider.GetComponent<Tile>().MakeIsPlaceableFalse();
                hit.collider.GetComponent<Tile>().placedObject = gameObject;
            }
        }
    }
}
