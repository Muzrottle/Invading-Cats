using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Waypoint : MonoBehaviour
{
    //[SerializeField] Tower tower;
    [SerializeField] EditTowers editTowers;

    //[SerializeField] ParticleSystem hoverDeployVFX;
    //[SerializeField] ParticleSystem hoverDestroyVFX;

    public GameObject placedObject;

    [SerializeField] bool hasPlaced;
    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get{ return isPlaceable; } }
    public bool HasPlaced { get { return hasPlaced; } }


    private void Awake()
    {
        editTowers = FindObjectOfType<EditTowers>();
    }

    private void OnMouseEnter()
    {
        //We are checking is modify button clicked or not.
        if (!editTowers.CanModify)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        editTowers.SetWaypoint(GetComponent<Waypoint>());
        ////We check hasPlaced besides isPlaceable because of VFX. If we only checked isPlaceable then our cats path also uses our destroyVFX.
        ////hasPlaced is helping us to know if a dog is deployed or not. isPlaceable on the other hand is checks for the tile is usable for towers.
        //if (isPlaceable && !hasPlaced)
        //{
        //    if (!hoverDeployVFX.isPlaying)
        //    {
        //        editTowers.Displayer(true, transform);
        //        SelectorVFX(true, hoverDeployVFX);
        //    }

        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        DogInstantiator();
        //    }
        //}
        //else if (isPlaceable && hasPlaced)
        //{
        //    if (!hoverDestroyVFX.isPlaying)
        //    {
        //        editTowers.Displayer(false, transform);
        //        SelectorVFX(true, hoverDestroyVFX);
        //    }

        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        DogRemover();
        //    }
        //}
    }

    //private void OnMouseExit()
    //{
    //    if (!editTowers.CanModify)
    //        return;

    //    if (isPlaceable && !hasPlaced)
    //    {
    //        if (hoverDeployVFX.isPlaying)
    //        {
    //            editTowers.Displayer(false, transform);
    //            SelectorVFX(false, hoverDeployVFX);
    //        }
    //    }
    //    else if (isPlaceable && hasPlaced)
    //    {
    //        if (hoverDestroyVFX.isPlaying)
    //        {
    //            editTowers.Displayer(false, transform);
    //            SelectorVFX(false, hoverDestroyVFX);
    //        }
    //    }
    //}

    //private void DogInstantiator()
    //{
    //    hasPlaced = tower.CreateTower(tower, transform.position, transform);

    //    if (hasPlaced)
    //    {
    //        SelectorVFX(false, hoverDeployVFX);
    //    }
    //}

    //private void DogRemover()
    //{
    //    GameObject dog = GetComponentInChildren<Tower>().gameObject;
    //    hasPlaced = tower.DestroyTower(dog);

    //    SelectorVFX(false, hoverDestroyVFX);
    //}

    //private void SelectorVFX(bool isHovering, ParticleSystem selectVFX)
    //{
    //    if (isHovering)
    //    {
    //        selectVFX.Play();
    //    }
    //    else
    //    {
    //        selectVFX.Stop();
    //    }
    //}

    public void MakeIsPlaceableFalse()
    {
        isPlaceable = false;
    }

    public void MakeIsPlaceableTrue()
    {
        isPlaceable = true;
    }

    public void SetTilePlacement(bool placed)
    {
        hasPlaced = placed;
    }
}
