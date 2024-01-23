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

    [Header("Button Explanation")]
    [SerializeField] GameObject descBackground;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] float descOpenDuration = 2f;
    [SerializeField] float descRotationDuration = 2f;
    [SerializeField] float descRotationAngle = 30f;

    EditButton pressedButton;
    Image descBackgroundImg;
    Sequence descBackgroundRotateSeq;
    Vector2 descriptionOpenPos;
    Vector2 descriptionClosePos;
    Vector3 descLeftRotation;
    Vector3 descRightRotation;

    private void Start()
    {
        FindEditButtons();

        descBackgroundImg = descBackground.GetComponent<Image>();
        SetDescBackgroundTweens();
    }

    void SetDescBackgroundTweens()
    {

        descLeftRotation = new Vector3(0, 0, descBackground.transform.rotation.z - descRotationAngle);
        descRightRotation = new Vector3(0, 0, descBackground.transform.rotation.z + descRotationAngle);
        descriptionOpenPos = new Vector2(descBackgroundImg.rectTransform.rect.width, descBackgroundImg.rectTransform.rect.height);
        descriptionClosePos = new Vector2(0, descBackgroundImg.rectTransform.rect.height);
        descBackgroundImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);

        Sequence descBackgroundRotateSeq = DOTween.Sequence();

        descBackgroundRotateSeq.Append(descBackground.GetComponent<RectTransform>().DORotate(descLeftRotation, descRotationDuration)
            .OnComplete(() =>
                descBackground.GetComponent<RectTransform>().DORotate(descRightRotation, descRotationDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)));
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

    public void ShowDescription(TextMeshProUGUI currentDescription)
    {
        if (currentDescription != null)
        {
            desc.text = currentDescription.text;
        }

        descBackground.SetActive(true);
        descBackground.GetComponent<RectTransform>().DOSizeDelta(descriptionOpenPos, descOpenDuration);
        descBackgroundRotateSeq.Play();
    }

    public void CloseDescription()
    {
        if (descBackground.activeInHierarchy)
        {
            descBackground.GetComponent<RectTransform>().DOSizeDelta(descriptionClosePos, descOpenDuration);
            descBackgroundRotateSeq.Pause();
        }
    }
}
