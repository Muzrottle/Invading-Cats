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

    TextMeshPro label;
    Vector2Int coordinate = new Vector2Int();
    Vector2Int oldCoordinate = new Vector2Int();
    Waypoint waypoint;

    private void Awake()
    {
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        waypoint = GetComponentInParent<Waypoint>();
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
        coordinate.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coordinate.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);

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
        if(waypoint.IsPlaceable)
        {
            label.color = defaultColor;
        }
        else
        {
            label.color = blockedColor;
        }
    }
}
