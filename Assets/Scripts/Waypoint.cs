using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] GameObject dogPrefab;
    ParticleSystem hoverVFX;

    [SerializeField] bool isPlaceable;

    private void Start()
    {
        hoverVFX = GetComponent<ParticleSystem>();
    }

    private void OnMouseOver()
    {
        if (!isPlaceable)
            return;

        if (!hoverVFX.isPlaying)
        {
            hoverVFX.Play();
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            Instantiate(dogPrefab, gameObject.transform);
        }
    }

    private void OnMouseExit()
    {
        if (!isPlaceable)
            return;

        if (hoverVFX.isPlaying)
        {
            hoverVFX.Stop();
        }
    }
}
