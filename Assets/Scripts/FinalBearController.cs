using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBearController : MonoBehaviour
{
    [SerializeField] ParticleSystem appearEffect;
    [SerializeField] AudioSource hint, moveHint;
    public bool playing = true;
    private void OnEnable()
    {
        appearEffect.Play();
        hint.Play();
        StartCoroutine(PlayHint());
    }

    IEnumerator PlayHint() {
        yield return new WaitUntil(()=>!hint.isPlaying);
        yield return new WaitForSeconds(10);
        hint.Play();
        yield return new WaitForSeconds(10);
        while (playing) {
            yield return new WaitForSeconds(15);
            if (playing) moveHint.Play();
        }
    }

    public void StopAllSound() {
        playing = false;
        if (hint.isPlaying) hint.Stop();
        if (moveHint.isPlaying) moveHint.Stop();
    }
}
