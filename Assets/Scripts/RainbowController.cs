using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowController : MonoBehaviour
{
    [SerializeField] LevelThreeController controller;
     private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RainCloud"))
        {
            controller.PlayRainbow();
        }
    }
}
