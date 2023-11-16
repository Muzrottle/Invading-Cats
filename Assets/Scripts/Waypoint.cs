using System;
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
        if (isPlaceable)
        {
            if (!hoverVFX.isPlaying)
            {
                SelectorVFX(true);
            }

            if (Input.GetMouseButtonDown(0))
            {
                DogInstantiator();
            }
        }
    }

    private void OnMouseExit()
    {
        if (isPlaceable)
        {
            if (hoverVFX.isPlaying)
            {
                SelectorVFX(false);
            }
        }
    }

    private void DogInstantiator()
    {
        Instantiate(dogPrefab, transform.position, Quaternion.identity);
        isPlaceable = false;

        SelectorVFX(false);
    }

    private void SelectorVFX(bool isHovering)
    {
        if (isHovering)
        {
            hoverVFX.Play();
        }
        else
        {
            hoverVFX.Stop();
        }
    }
}
