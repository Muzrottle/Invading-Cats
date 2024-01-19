using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1f, 0.5f, 0f);

    TextMeshPro label;
    Vector2Int coordinate = new Vector2Int();
    Vector2Int oldCoordinate = new Vector2Int();
    GridManager gridManager;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        DisplayCoordinate();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinate();
            UpdateObjectName();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCoordinates();
        }

        SetLabelColor();
    }

    private void DisplayCoordinate()
    {
        if (gridManager == null)
        {
            return;
        }

        coordinate.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);
        coordinate.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);

        label.text = $"{coordinate.x},{coordinate.y}";
    }

    private void UpdateObjectName()
    {
        if (oldCoordinate == coordinate) return;
        oldCoordinate = coordinate;

        transform.parent.name = coordinate.ToString();
    }

    private void ToggleCoordinates()
    {
        label.enabled = !label.IsActive();
    }

    private void SetLabelColor()
    {
        if( gridManager == null )
        {
            return;
        }

        Node node = gridManager.GetNode( coordinate );

        if ( node == null )
        {
            return;
        }

        if (!node.isWalkable)
        {
            label.color = blockedColor;
        }
        else if (node.isPath)
        {
            label.color = pathColor;
        }
        else if (node.isExplored)
        {
            label.color = exploredColor;
        }
        else
        {
            label.color = defaultColor;
        }
    }
}
