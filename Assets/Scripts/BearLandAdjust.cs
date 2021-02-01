using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearLandAdjust : MonoBehaviour
{
    [SerializeField] Transform finalBear;
    [SerializeField] Vector3 offset;

    void Start()
    {
        transform.position = finalBear.position + offset;
    }
}
