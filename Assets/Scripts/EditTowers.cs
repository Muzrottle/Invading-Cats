using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditTowers : MonoBehaviour
{
    [SerializeField] bool canModify;
    public bool CanModify { get { return canModify; } }

    [SerializeField] Button modifyBtn;
    [SerializeField] Sprite switchPressed;
    [SerializeField] Sprite switchNotPressed;

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
