using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditTowers : MonoBehaviour
{
    [SerializeField] GameObject dogDisplay;

    [SerializeField] Button modifyBtn;
    [SerializeField] Sprite switchPressed;
    [SerializeField] Sprite switchNotPressed;

    [SerializeField] bool canModify;
    public bool CanModify { get { return canModify; } }

    public void Displayer(bool isVisible, Transform tilePos)
    {
        dogDisplay.transform.position = tilePos.position;
        dogDisplay.SetActive(isVisible);
    }

    public void ModifySwitch()
    {
        canModify = !canModify;

        if (canModify)
        {
            ChangeModifyButtonSprite(switchPressed);
        }
        else
        {
            ChangeModifyButtonSprite(switchNotPressed);
        }
    }

    void ChangeModifyButtonSprite(Sprite sprite)
    {
        modifyBtn.image.sprite = sprite;
    }
}
