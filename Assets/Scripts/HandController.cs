using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class HandController : MonoBehaviour
{
    MLInputController controller;
    [SerializeField] MainController mainController;
    [SerializeField] LevelTwoController levelTwoController;
    [SerializeField] LineRenderer line;
    [SerializeField] ParticleSystem wind;
    [SerializeField] float minimizer = 0.1f;
    [SerializeField] Transform start, end;
    [SerializeField] AudioSource windSound;
    public bool disabled = false;
    void Start()
    {
        MLInput.Start();
        controller = MLInput.GetController(MLInput.Hand.Left);
        Debug.Assert(controller != null);

        MLInput.OnControllerButtonDown += OnButtonDown;
        MLInput.OnControllerButtonUp += OnButtonUp;

        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = controller.Position;
        transform.rotation = controller.Orientation;

        line.SetPosition(0, start.position);
        Vector3 dir = end.position - start.position;
        line.SetPosition(1, 3 * dir);
       // Debug.DrawRay(transform.position, transform.forward.normalized * 10, Color.green);
        
    }

    private void FixedUpdate()
    {
        if (controller != null && controller.TriggerValue > 0)
        {
            if (!wind.isPlaying) {
                wind.Play();
            }
            if (!windSound.isPlaying)
            {
                windSound.Play();
            }

            if (disabled) return;
            Vector3 dir = end.position - start.position;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir.normalized, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);

                // Debug.DrawLine(transform.position, hit.point, Color.green);

                if (hit.rigidbody)
                {
                    Vector3 f = dir.normalized;
                    if (hit.rigidbody.gameObject.CompareTag("Start"))
                    {
                        mainController.OnStartGame();
                    }
                    else if (hit.rigidbody.gameObject.CompareTag("Restart"))
                    {
                        mainController.OnRestartGame();
                    }
                    else if (hit.rigidbody.gameObject.CompareTag("Bear")) {
                        f = minimizer * f;
                        hit.rigidbody.AddForce(f, ForceMode.Acceleration);
                    }
                    else {
                        if (hit.rigidbody.gameObject.CompareTag("Land"))
                        {
                            f = minimizer * f;
                        }
                        hit.rigidbody.AddForce(f, ForceMode.Acceleration);
                    }
                } else if (hit.transform.gameObject.CompareTag("Red") ||
                    hit.transform.gameObject.CompareTag("Green") ||
                    hit.transform.gameObject.CompareTag("Blue")) {
                    levelTwoController.GoBackToIsland(hit.transform);
                }
            }
        }
        else { 
            wind.Stop();
            windSound.Stop();
        }
    }

    void OnDestroy()
    {
        MLInput.OnControllerButtonDown -= OnButtonDown;
        MLInput.OnControllerButtonUp -= OnButtonUp;

        MLInput.Stop();
    }

    void OnButtonDown(byte controllerId, MLInputControllerButton button) {

    }

    void OnButtonUp(byte controllerId, MLInputControllerButton button) {

    }
}
