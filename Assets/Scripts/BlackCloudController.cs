using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCloudController : MonoBehaviour
{
    [SerializeField] ParticleSystem rain, lightning;
    [SerializeField] LevelThreeController controller;
    public bool IsLinked = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("RainCloud"))
        {
            FixedJoint sj = gameObject.AddComponent<FixedJoint>();
            sj.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
            PlayLightning();
            collision.gameObject.GetComponent<BlackCloudController>().PlayLightning();
            controller.CheckAllLinked();
        }
    }

    public void PlayLightning() {
        if (IsLinked) return;
        IsLinked = true;
        if (!lightning.isPlaying)
            lightning.Play();
    }

    public void PlayRain()
    {
        lightning.Stop();
        rain.Play();
    }

    public void StopRain()
    {
        rain.Stop();
    }
}
