using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BirdController : MonoBehaviour
{
    public string Color;
    [SerializeField] Transform bear;
    public Transform island;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float speed = 0.01f, rotSpeed = 0.01f, threshold = 0.1f;
    public bool isMovingToBear = false, isMovingToIsland = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingToBear && Vector3.Distance(transform.position, bear.position) > threshold) {
            
            transform.position = Vector3.MoveTowards(transform.position, bear.position, speed * Time.deltaTime);
            Vector3 targetDir = bear.position - transform.position;
            Vector3 newDir =  Vector3.RotateTowards(transform.forward, targetDir, rotSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }

        if (isMovingToIsland)
        {

            transform.position = Vector3.MoveTowards(transform.position, island.position, speed * Time.deltaTime);
            Vector3 targetDir = island.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }

    }

    public void StartMovingToBear() {
        isMovingToBear = true;
    }

    public void StartMovingToIsland()
    {
        isMovingToBear = false;
        isMovingToIsland = true;
        StartCoroutine(DisappearTimer());
    }

    IEnumerator DisappearTimer() {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}
