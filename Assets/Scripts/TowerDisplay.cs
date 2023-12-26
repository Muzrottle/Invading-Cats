using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerDisplay : MonoBehaviour
{
    [Header("Positioning Variables")]
    [SerializeField] float displayMaxYPos;
    [SerializeField] float displayMinYPos;
    [SerializeField] float positionPeriodTime = 1f;

    [Header("Rotating Variables")]
    [SerializeField] float rotationPeriodTime = 4f;
    
    [Header("Sizing Variables")]
    [SerializeField] Vector3 displayMaxSize;
    [SerializeField] Vector3 displayMinSize;
    [SerializeField] float sizePeriodTime = 1f;
    [SerializeField] float sizingSpeed = 1f;

    private void OnEnable()
    {
        StartCoroutine(DogDisplay());
    }

    IEnumerator DogDisplay()
    {
        //For Position
        float positionTimer = 0f;
        float yPos = displayMinYPos;
        bool yPosUp = true;

        //For Rotation
        float rotationTimer = 0f;
        float yRotation = transform.rotation.y;

        //For Scale
        float sizeTimer = 0f;
        float sizeRate = (1f / sizePeriodTime) * sizingSpeed;
        bool scaleUp = true;

        while (true)
        {
            positionTimer += Time.deltaTime;
            rotationTimer += Time.deltaTime;
            sizeTimer += Time.deltaTime * sizeRate;

            //Position
            if (positionTimer >= positionPeriodTime)
            {
                positionTimer = 0f;
                yPosUp = !yPosUp;
            }

            //Rotation
            if (rotationTimer >= rotationPeriodTime)
            {
                rotationTimer = 0f;
            }

            //Size
            if (sizeTimer >= sizePeriodTime)
            {
                sizeTimer = 0f;
                scaleUp = !scaleUp;
            }

            Positioner(ref positionTimer, ref yPos, ref yPosUp);

            Rotator(ref rotationTimer, ref yRotation);
            
            Sizer(ref sizeTimer, ref scaleUp);

            yield return new WaitForEndOfFrame();
        }
    }

    private void Positioner(ref float positionTimer, ref float yPos, ref bool yPosUp)
    {
        if (yPosUp)
        {
            yPos = Mathf.Lerp(displayMinYPos, displayMaxYPos, positionTimer);
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        }
        else
        {
            yPos = Mathf.Lerp(displayMaxYPos, displayMinYPos, positionTimer);
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        }
    }

    private void Rotator(ref float rotationTimer, ref float yRotation)
    {
        if (transform.rotation.y >= 360)
        {
            transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
        }

        yRotation = Mathf.Lerp(0, 360, rotationTimer / rotationPeriodTime);
        transform.eulerAngles = new Vector3(transform.rotation.x, yRotation, transform.rotation.z);
    }

    private void Sizer(ref float sizeTimer, ref bool scaleUp)
    {
        if (scaleUp)
        {
            transform.localScale = Vector3.Lerp(displayMinSize, displayMaxSize, sizeTimer);
        }
        else
        {
            transform.localScale = Vector3.Lerp(displayMaxSize, displayMinSize, sizeTimer);
        }
    }
}
