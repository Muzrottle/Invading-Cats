using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType(typeof(EnemyMover)) != null)
        {
            target = FindObjectOfType(typeof(EnemyMover)).GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
