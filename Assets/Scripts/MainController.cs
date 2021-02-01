using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MainController : MonoBehaviour
{
    [SerializeField] AudioSource intro, gameplay, introVoice;
    [SerializeField] GameObject startUI, endUI;
    [SerializeField] Transform bear, bearStartPos;
    [SerializeField] HandController handController;
    [SerializeField] Transform girl;
    [SerializeField] ParticleSystem indicator;
    [SerializeField] float indicatorSpeed = 0.1f;
    [SerializeField] bool isFlying = false;
    [SerializeField] PlayableDirector endingScene;
    void Start()
    {
        startUI.SetActive(false);
        endUI.SetActive(false);
        // StartCoroutine(IndicatorControl());
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlying) {
            float step = indicatorSpeed * Time.deltaTime;
            indicator.gameObject.transform.position = Vector3.MoveTowards(indicator.gameObject.transform.position, bear.position, step);
            if (Vector3.Distance(indicator.gameObject.transform.position, bear.position) < 0.01f) {
                isFlying = false;
                indicator.Stop();
            }
        }
    }

    public void OnPlacementFinished() {
        startUI.SetActive(true);
        intro.Play();
    }

    
    public void OnStartGame() {
        intro.Stop();
        gameplay.Play();
        introVoice.Play();
        startUI.SetActive(false);
        StartCoroutine(WaitIntroVoice());
        StartCoroutine(IndicatorControl());
    }

    IEnumerator WaitIntroVoice() {
        yield return new WaitUntil( () => !introVoice.isPlaying);
        bear.GetComponent<BearController>().isStarted = true;
    }

    public void OnWinGame() {
        endingScene.stopped += OnPlayableDirectorStopped;
        endingScene.Play();
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        endUI.SetActive(true);
    }

    public void OnLoseGame()
    {
        endUI.SetActive(true);
    }

    public void OnRestartGame()
    {
        endUI.SetActive(false);
        // reset bear position
        bear.gameObject.SetActive(false);
        bear.transform.position = bearStartPos.transform.position;
        bear.transform.rotation = bearStartPos.transform.rotation;
        bear.gameObject.SetActive(true);
        StartCoroutine(EndingCutScene());
    }

    IEnumerator EndingCutScene()
    {
        handController.disabled = true;
        yield return new WaitForSeconds(3);
        handController.disabled = false;
    }

    public void PlayIndicator() {
        indicator.Stop();
        indicator.gameObject.transform.position = bear.position;
        indicator.Play();
        isFlying = true;
    }

    IEnumerator IndicatorControl() {
        while (true) {
            yield return new WaitForSeconds(5f);
            if (Vector3.Distance(girl.position, bear.position) > 1f && !isFlying) {
                PlayIndicator();
            }         
        }
    }

    public void StopBGM() {
        if (gameplay.isPlaying) gameplay.Stop();
    }

    public void StartBGM()
    {
        if (!gameplay.isPlaying) gameplay.Play();
    }
}
