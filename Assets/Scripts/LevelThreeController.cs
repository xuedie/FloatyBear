using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelThreeController : MonoBehaviour
{
    [SerializeField] GameObject rainbow, rainbowTrigger, blackClouds;
    [SerializeField] List<BlackCloudController> rainClouds;
    [SerializeField] MainController mainController;
    [SerializeField] AudioSource rain, thunder, rainbowSound;
    [SerializeField] FinalBearController finalBear;
    [SerializeField] ParticleSystem rainEffect;

    public void EnableBlackClouds() {
        blackClouds.SetActive(true);
    }

    public void PlayRainbow() {
        finalBear.StopAllSound();
        rain.Stop();
        thunder.Stop();
        rainbowSound.Play();
        // mainController.StartBGM();
        // play rainbow
        rainbow.SetActive(true);

        // end animation
        foreach (BlackCloudController cloud in rainClouds)
        {
            cloud.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -1f, 0));
            cloud.gameObject.SetActive(false);
        }
        StartCoroutine(WaitForEnd());
    }

    IEnumerator WaitForRaining()
    {
        rainEffect.Play();
        yield return new WaitForSeconds(3f);
        rainEffect.Stop();
        PlayRainbow();
    }

    IEnumerator WaitForEnd() {
        yield return new WaitForSeconds(2f);
        mainController.OnWinGame();
    }

    public void PlayRainSound() {
        if (!rain.isPlaying) rain.Play();
    }

    public void PlayThunderSound()
    {
        if (!thunder.isPlaying) thunder.Play();
        mainController.StopBGM();
    }

    bool IsAllLinked() {
        foreach (BlackCloudController cloud in rainClouds) {
            if (!cloud.IsLinked) return false;
        }
        return true;
    }

    public void CheckAllLinked() {
        if (IsAllLinked()) {
            foreach (BlackCloudController cloud in rainClouds)
            {
                cloud.PlayRain();
            }
            // rainbowTrigger.SetActive(true);
            PlayRainSound();
            StartCoroutine(WaitForRaining());
        }
    }
}
