using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageHandler : MonoBehaviour
{
    [Header("Button Explanation")]
    [SerializeField] GameObject descBackground;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] float descOpenDuration = 2f;
    [SerializeField] float descRotationDuration = 2f;
    [SerializeField] float descRotationAngle = 30f;
    [SerializeField] float warningDelayTime = 2.5f;

    Image descBackgroundImg;
    Sequence descBackgroundRotateSeq;
    Sequence descBackgroundSeq;
    Vector2 descriptionOpenPos;
    Vector2 descriptionClosePos;
    Vector3 descLeftRotation;
    Vector3 descRightRotation;
    
    Color messageColor;

    private void Start()
    {
        descBackgroundImg = descBackground.GetComponent<Image>();

        SetDescBackgroundTweens();
        descBackground.SetActive(true);
        RotateDesc();
    }

    void SetDescBackgroundTweens()
    {
        descLeftRotation = new Vector3(0, 0, descBackground.transform.rotation.z - descRotationAngle);
        descRightRotation = new Vector3(0, 0, descBackground.transform.rotation.z + descRotationAngle);
        descriptionOpenPos = new Vector2(descBackgroundImg.rectTransform.rect.width, descBackgroundImg.rectTransform.rect.height);
        descriptionClosePos = new Vector2(0, descBackgroundImg.rectTransform.rect.height);
        descBackgroundImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
    }

    void RotateDesc()
    {
        descBackgroundRotateSeq = DOTween.Sequence();

        descBackgroundRotateSeq.Append(descBackground.GetComponent<RectTransform>().DORotate(descLeftRotation, descRotationDuration)
            .OnComplete(() =>
                descBackground.GetComponent<RectTransform>().DORotate(descRightRotation, descRotationDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)));
    }

    public void ShowDescription(TextMeshProUGUI currentDescription)
    {
        messageColor = new Color(1f, 0.9370283f, 0.664151f);

        if (currentDescription != null)
        {
            desc.text = currentDescription.text;
            desc.color = messageColor;
        }

        descBackgroundSeq.Kill();
        descBackgroundSeq = DOTween.Sequence();


        //if (descBackground.activeInHierarchy)
        //{
        //    descBackgroundSeq.Kill();
        //}
        //else
        //{
        //    descBackground.SetActive(true);
        //}

        descBackgroundRotateSeq.Play();

        descBackgroundSeq.Append(descBackground.GetComponent<RectTransform>().DOSizeDelta(descriptionOpenPos, descOpenDuration));
    }

    public void CloseDescription()
    {
        descBackgroundSeq.Kill();
        descBackgroundSeq = DOTween.Sequence();


        if (descBackground.activeInHierarchy)
        {
            descBackgroundSeq.Append(descBackground.GetComponent<RectTransform>().DOSizeDelta(descriptionClosePos, descOpenDuration)
                .OnComplete(() => {
                    descBackgroundRotateSeq.Pause();
                    //descBackground.SetActive(false);
                }));
        }
    }

    public void ShowWarningMessage(String message)
    {
        descBackgroundSeq.Kill();
        descBackgroundSeq = DOTween.Sequence();

        messageColor = new Color(1f, 0.7116596f, 0.6627451f);

        desc.text = message;
        desc.color = messageColor;

        descBackgroundRotateSeq.Play();

        descBackgroundSeq.Append(descBackground.GetComponent<RectTransform>().DOSizeDelta(descriptionOpenPos, descOpenDuration)
            .OnComplete(() => {
                descBackgroundSeq.Kill();
                descBackgroundSeq = DOTween.Sequence();
                descBackgroundSeq.Append(descBackground.GetComponent<RectTransform>().DOSizeDelta(descriptionClosePos, descOpenDuration)
                    .SetDelay(warningDelayTime)
                    .OnComplete(() => {
                        descBackgroundRotateSeq.Pause();
                    }));
            }));
    }
}
