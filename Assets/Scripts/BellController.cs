using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellController : MonoBehaviour
{
    public string Color;
    [SerializeField] ParticleSystem wave;
    [SerializeField] AudioSource ring;

    public void PlayWave() {
        if (wave.isPlaying) return;
        wave.Play();
    }

    public void PlaySound() {
        if (ring.isPlaying) return;
        ring.Play();
    }
}
