using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] Tower tower;
    [SerializeField] GameObject dog;
    [SerializeField] EditTowers editTowers;
    [SerializeField] ParticleSystem hoverDeployVFX;
    [SerializeField] ParticleSystem hoverDestroyVFX;

    [SerializeField] bool hasPlaced;
    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get{ return isPlaceable; } }


    private void Awake()
    {
        editTowers = FindObjectOfType<EditTowers>();
    }

    private void OnMouseOver()
    {
        if (!editTowers.CanModify)
            return;

        if (isPlaceable && !hasPlaced)
        {
            if (!hoverDeployVFX.isPlaying)
            {
                dog.SetActive(true);
                SelectorVFX(true, hoverDeployVFX);
            }

            if (Input.GetMouseButtonDown(0))
            {
                DogInstantiator();
            }
        }
        else if (isPlaceable && hasPlaced)
        {
            if (!hoverDestroyVFX.isPlaying)
            {
                dog.SetActive(false);
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
        if (!editTowers.CanModify)
            return;

        if (isPlaceable && !hasPlaced)
        {
            if (hoverDeployVFX.isPlaying)
            {
                dog.SetActive(false);
                SelectorVFX(false, hoverDeployVFX);
            }
        }
        else if (isPlaceable && hasPlaced)
        {
            if (hoverDestroyVFX.isPlaying)
            {
                dog.SetActive(false);
                SelectorVFX(false, hoverDestroyVFX);
            }
        }
    }

    private void DogInstantiator()
    {
        hasPlaced = tower.CreateTower(tower, transform.position, transform);

        if (hasPlaced)
        {
            SelectorVFX(false, hoverDeployVFX);
        }
    }

    private void DogRemover()
    {
        GameObject dog = GetComponentInChildren<TargetLocator>().gameObject;
        Destroy(dog);
        hasPlaced = false;

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
