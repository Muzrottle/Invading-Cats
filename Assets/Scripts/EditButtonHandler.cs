using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditButtonHandler : MonoBehaviour
{
    [Header("Side Tabs")]
    [SerializeField] List<GameObject> sideTabs = new List<GameObject>();

    [Header("Side Menu Buttons")]
    [SerializeField] List<EditButton> editButtons = new List<EditButton>();

    [Header("Button Backgrounds")]
    [SerializeField] Sprite buttonDefault;
    [SerializeField] Sprite buttonPressed;
    [SerializeField] Sprite buttonHovered;

    EditButton pressedButton;

    private void Start()
    {
        FindEditButtons();
    }

    private void FindEditButtons()
    {
        foreach (GameObject sideTab in sideTabs)
        {
            foreach (EditButton button in sideTab.GetComponentsInChildren<EditButton>())
            {
                if (button != null)
                {
                    editButtons.Add(button);

                    //Default Selected Button is Placing Dogs Button. So we are referencing to it first.
                    if (button.isPressed)
                    {
                        pressedButton = button;
                        ButtonChangeBackground(pressedButton.button, pressedButton.isHovered, pressedButton.isPressed);
                    }
                }
            }
        }
    }

    public void ButtonNotHover(EditButton editButton)
    {
        editButton.isHovered = false;
        ButtonChangeBackground(editButton.button, editButton.isHovered, editButton.isPressed);
    }

    public void ButtonHover(EditButton editButton)
    {
        editButton.isHovered = true;
        ButtonChangeBackground(editButton.button, editButton.isHovered, editButton.isPressed);
    }

    public void ButtonPress(EditButton editButton)
    {
        editButton.isPressed = !editButton.isPressed;
        ButtonChangeBackground(editButton.button, editButton.isHovered, editButton.isPressed);
        
        if (editButtons.Contains(editButton))
        {
            Debug.Log(editButton.name);
            pressedButton.isPressed = !pressedButton.isPressed;
            ButtonChangeBackground(pressedButton.button, pressedButton.isHovered, pressedButton.isPressed);

            pressedButton = editButton;
        }
    }

    private void ButtonChangeBackground(Button button, bool isHovering, bool isPressed)
    {
        if (isPressed)
        {
            button.image.sprite = buttonPressed;
        }
        else if (isHovering)
        {
            button.image.sprite = buttonHovered;
        }
        else
        {
            button.image.sprite = buttonDefault;
        }
    }

    
}
