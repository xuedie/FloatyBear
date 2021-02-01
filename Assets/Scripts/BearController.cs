using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BearController : MonoBehaviour
{
    [SerializeField] MainController mainController;
    [SerializeField] LevelThreeController levelThreeController;
    [SerializeField] LevelTwoController levelTwoController;
    [SerializeField] AudioSource level1Clear, level2BirdHint, level2BirdLoop, level3Hint, ballonSound;
    [SerializeField] List<AudioSource> level1Cloud;
    [SerializeField] List<AudioSource> level2BirdFeedback;
    [SerializeField] bool disableSound = false, disableBirdSound = false;
    [SerializeField] PlayableDirector fallingScene;
    [SerializeField] GameObject ballon, thisBear, finalBear, level1;
    [SerializeField] ParticleSystem appearEffect, ballonEffect;
    Renderer m_Renderer;
    bool isBlockingByCloud = false, isBlockingByBird = false;
    int isLevel2HintPlayed = 0;
    public bool isStarted = false;
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
       //  Debug.Log(m_Renderer.isVisible);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cloud") && isStarted)
        {
            isBlockingByCloud = true;
            if (!level1Cloud[0].isPlaying && !level1Cloud[1].isPlaying && !level1Clear.isPlaying && !disableSound) {
                int n = Random.Range(0, 2);
                level1Cloud[n].Play();
                StartCoroutine(PauseSound());
            }
            
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cloud") && isStarted)
        {
            isBlockingByCloud = true;
            if (!level1Cloud[0].isPlaying && !level1Cloud[1].isPlaying && !level1Clear.isPlaying && !disableSound)
            {
                int n = Random.Range(0, 2);
                level1Cloud[n].Play();
                StartCoroutine(PauseSound());
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cloud") && isStarted)
        {
            isBlockingByCloud = false;
            if (!level1Cloud[0].isPlaying && !level1Cloud[1].isPlaying && !level1Clear.isPlaying && !disableSound) {
                level1Clear.Play();
                StartCoroutine(PauseSound());
            } 
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayBallon"))
        {
            isStarted = false;
            PlayingFalling();
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("LevelThreeTrigger"))
        {
            levelThreeController.PlayThunderSound();
            if (level2BirdHint.isPlaying) level2BirdHint.Stop();
            if (level2BirdLoop.isPlaying) level2BirdLoop.Stop();
            levelTwoController.isStarted = false;
            levelThreeController.EnableBlackClouds();
        }

        if (other.gameObject.CompareTag("LevelTwoTrigger"))
        {
            levelTwoController.StartChasing();
            level1.SetActive(false);
            isBlockingByCloud = false;
        }

        if (other.gameObject.CompareTag("Bird"))
        {
            isBlockingByBird = true;
            if (!level2BirdHint.isPlaying && !level2BirdLoop.isPlaying && 
                !IsLevelSoundPlaying() && !disableBirdSound)
            {
                if (isLevel2HintPlayed >= 2)
                {
                    level2BirdLoop.Play();
                    StartCoroutine(PauseBirdSound(10f));
                }
                else {
                    level2BirdHint.Play();
                    isLevel2HintPlayed++;
                    StartCoroutine(PauseBirdSound(20f));
                }    
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Bird"))
        {
            if (!level2BirdHint.isPlaying && !level2BirdLoop.isPlaying &&
                !IsLevelSoundPlaying() && !disableBirdSound)
            {
                if (isLevel2HintPlayed >= 2)
                {
                    level2BirdLoop.Play();
                    StartCoroutine(PauseBirdSound(10f));
                }
                else
                {
                    level2BirdHint.Play();
                    isLevel2HintPlayed++;
                    StartCoroutine(PauseBirdSound(20f));
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bird"))
        {
            isBlockingByBird = false;
            if (!level2BirdHint.isPlaying && !level2BirdLoop.isPlaying &&
                !IsLevelSoundPlaying())
            {
                int n = Random.Range(0, 3);
                level2BirdFeedback[n].Play();
            }     
        }
    }

    private void FixedUpdate()
    {
        if (!isBlockingByCloud && !isBlockingByBird && isStarted) {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0.1f, 0), ForceMode.Acceleration);
        }
    }

    bool IsLevelSoundPlaying() {
        foreach (AudioSource s in level2BirdFeedback) {
            if (s.isPlaying) return true;
        }
        return false;
    }

    IEnumerator PauseSound() {
        disableSound = true;
        yield return new WaitForSeconds(15f);
        disableSound = false;
    }

    IEnumerator PauseBirdSound(float sec)
    {
        disableBirdSound = true;
        yield return new WaitForSeconds(sec);
        disableBirdSound = false;
    }

    void PlayingFalling() {
        ballon.SetActive(false);
        fallingScene.stopped += OnPlayableDirectorStopped;
        ballonSound.Play();
        ballonEffect.Play();
        fallingScene.Play();
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
        finalBear.SetActive(true);
        gameObject.SetActive(false);  
    }

}
