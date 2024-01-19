using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimalMenuBackgroundAnimation : MonoBehaviour
{
    [SerializeField] int animalRows = 5;
    [SerializeField] float animationSpeed = 2f;

    Vector2 currentRes;
    Vector2 startPos;
    Vector2 endPos;

    // Start is called before the first frame update
    void Start()
    {
        currentRes = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        startPos = new Vector2 (currentRes.x - (currentRes.x / animalRows), currentRes.y + (currentRes.y / animalRows));
        endPos = new Vector2(currentRes.x + (currentRes.x / animalRows), currentRes.y - (currentRes.y / animalRows));
    }

    private void Update()
    {
        BackgroundAnimation();
    }

    void BackgroundAnimation()
    {
        Vector2 currentPos = transform.position;

        while (true)
        {
            if (currentPos.y >= currentRes.y - (currentRes.y / animalRows))
            {
                currentRes = startPos;
            }

            transform.DOMove(endPos, animationSpeed).SetEase(Ease.InOutQuad);
        }
    }
}
