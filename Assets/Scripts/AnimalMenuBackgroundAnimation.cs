using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimalMenuBackgroundAnimation : MonoBehaviour
{
    [SerializeField] int animalRowCount = 5;
    [SerializeField] float animationDuration = 2f;
    [SerializeField] float rowDelay;
    [SerializeField] Transform[] animalRows;

    Vector3 currentRes;
    Vector3 startPos;
    Vector3 endPos;

    // Start is called before the first frame update
    void Start()
    {
        currentRes = new Vector3(Screen.currentResolution.width, Screen.currentResolution.height);
        startPos = new Vector3(-currentRes.x + (-currentRes.x / 2), currentRes.y / 2);
        endPos = new Vector3(-currentRes.x + (-currentRes.x / (animalRowCount + 1)), (-currentRes.y) - (-currentRes.y / (animalRowCount - 1)));
        Debug.Log(startPos);
        Debug.Log(endPos);

        StartCoroutine(SetNewDuration());
    }

    IEnumerator SetNewDuration()
    {
        foreach (Transform row in animalRows)
        {
            // Create a sequence for each image
            Sequence sequence = DOTween.Sequence();

            // Move the image to the end position over the specified duration
            sequence.Append(row.DOLocalMove(endPos, animationDuration).SetEase(Ease.Linear))
                .SetSpeedBased(true)
                .SetLoops(-1, LoopType.Restart);

            yield return new WaitForSeconds(rowDelay);
        }
    }

    //private void Update()
    //{
    //    Vector3 currentPos = transform.localPosition;

    //    if (currentPos.y <= endPos.y)
    //    {
    //        Debug.Log("gÝRDÝM");
    //        transform.localPosition = startPos;
    //        inCycle = false;
    //    }

    //    if (!inCycle)
    //    {
    //        inCycle = true;
    //        BackgroundAnimation();
    //    }
    //}

    //void BackgroundAnimation()
    //{

    //    transform.DOLocalMove(endPos, animationDuration);


    //    Debug.Log(endPos);
    //}
}
