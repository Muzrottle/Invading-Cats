using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    EditTowers editTowers;
    GridManager gridManager;
    Pathfinder pathfinder;

    public GameObject placedObject;

    [SerializeField] bool hasPlaced;
    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get{ return isPlaceable; } }
    public bool HasPlaced { get { return hasPlaced; } }

    Vector2Int coordinates = new Vector2Int();

    private void Awake()
    {
        editTowers = FindObjectOfType<EditTowers>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    private void Start()
    {
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if (!isPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    private void OnMouseEnter()
    {
        //We are checking is modify button clicked or not.
        if (!editTowers.CanModify)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!gridManager.Grid.ContainsKey(coordinates))
        {
            return;
        }

        editTowers.SetTile(GetComponent<Tile>(), coordinates);
    }

    public void MakeIsPlaceableFalse()
    {
        isPlaceable = false;
    }

    public void MakeIsPlaceableTrue()
    {
        isPlaceable = true;
        gridManager.UnblockNode(coordinates);
        pathfinder.NotifyRecievers();
    }

    public void SetTilePlacement(bool placed)
    {
        hasPlaced = placed;
    }

    public void SetTilePlacement(bool placed, GameObject currentObject)
    {
        if (placed)
        {
            placedObject = currentObject;
            hasPlaced = placed;
        }
        else
        {
            placedObject = null;
            hasPlaced = placed;
        }
        
    }
}
