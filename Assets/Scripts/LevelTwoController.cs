using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoController : MonoBehaviour
{
    [SerializeField] List<BirdController> birds;
    [SerializeField] List<AudioSource> bellSound;
    public bool isStarted = false;
    int curBird = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartChasing() {
        if (isStarted) return;
        isStarted = true;
        // enter level 2
        ReleaseBird();
    }

    public void ReleaseBird() {
        if (!isStarted || curBird >= birds.Count) return;
        birds[curBird].gameObject.SetActive(true);
        birds[curBird].StartMovingToBear();
        curBird++;
    }

    public void GoBackToIsland(Transform island) {
        // play sound
        if (!isStarted) return;
        BellController bell = island.parent.GetComponent<BellController>();
        bell.PlaySound();
        bell.PlayWave();
        if (birds[curBird-1].Color.Equals(island.gameObject.tag) && !birds[curBird - 1].isMovingToIsland) {
            birds[curBird-1].island = island;
            birds[curBird-1].StartMovingToIsland();
            StartCoroutine(WaitForRelease());
        }
    }

    IEnumerator WaitForRelease() {
        yield return new WaitForSeconds(1.5f);
        ReleaseBird();
    }
}
