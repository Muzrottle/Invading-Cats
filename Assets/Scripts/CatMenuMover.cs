using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMenuMover : MonoBehaviour
{
    Vector3 endPos;

    // Start is called before the first frame update
    void Start()
    {
        endPos = new Vector3(-transform.position.x, transform.position.y, transform.position.z);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(endPos, 10f).SetEase(Ease.Linear).SetDelay(2f))
            .SetSpeedBased()
            .SetLoops(-1, LoopType.Restart);

    }
}
