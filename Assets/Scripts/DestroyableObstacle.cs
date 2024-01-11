using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObstacle : MonoBehaviour
{
    [SerializeField] int removePrice;
    public int RemovePrice { get { return removePrice; } }
}
