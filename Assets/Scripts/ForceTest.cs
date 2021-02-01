using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTest : MonoBehaviour
{
    [SerializeField] float x, y, z;
    [SerializeField] ForceMode mode;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(x, y, z), mode);
        }
    }

    private void FixedUpdate()
    {
        
    }
}
