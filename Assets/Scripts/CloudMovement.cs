using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    [SerializeField] float kx, ky, kz;
    [SerializeField] float x, y, z;
    Vector3 originPos;
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        // Debug.Log(originPos);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.anyKey) {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, y, 0));
        }
        */
        if (Input.GetMouseButton(0))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(x, y, z), ForceMode.Acceleration);
        }
    }

    private void FixedUpdate()
    {
        Vector3 f = transform.position - originPos;
        // Debug.Log(f);
        GetComponent<Rigidbody>().AddForce(new Vector3(-f.x * kx, -f.y * ky, -f.z * kz));
    }
}
