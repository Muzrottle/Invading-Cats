using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] GameObject dogPrefab;
    [SerializeField] ParticleSystem hoverDeployVFX;
    [SerializeField] ParticleSystem hoverDestroyVFX;

    [SerializeField] bool isPlaceable;

    private void OnMouseOver()
    {
        if (isPlaceable)
        {
            if (!hoverDeployVFX.isPlaying)
            {
                SelectorVFX(true, hoverDeployVFX);
            }

            if (Input.GetMouseButtonDown(0))
            {
                DogInstantiator();
            }
        }
        else
        {
            if (!hoverDestroyVFX.isPlaying)
            {
                SelectorVFX(true, hoverDestroyVFX);
            }

            if (Input.GetMouseButtonDown(0))
            {
                DogRemover();
            }
        }
    }

    private void OnMouseExit()
    {
        if (isPlaceable)
        {
            if (hoverDeployVFX.isPlaying)
            {
                SelectorVFX(false, hoverDeployVFX);
            }
        }
        else
        {
            if (hoverDestroyVFX.isPlaying)
            {
                SelectorVFX(false, hoverDestroyVFX);
            }
        }
    }

    private void DogInstantiator()
    {
        Instantiate(dogPrefab, transform.position, Quaternion.identity, transform);
        isPlaceable = false;

        SelectorVFX(false, hoverDeployVFX);
    }

    private void DogRemover()
    {
        GameObject dog = GetComponentInChildren<TargetLocator>().gameObject;
        Destroy(dog);
        isPlaceable = true;

        SelectorVFX(false, hoverDestroyVFX);
    }

    private void SelectorVFX(bool isHovering, ParticleSystem selectVFX)
    {
        if (isHovering)
        {
            selectVFX.Play();
        }
        else
        {
            selectVFX.Stop();
        }
    }
}
