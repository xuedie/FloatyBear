using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class BVW_ControllerInputHandler : MonoBehaviour
{
    [SerializeField]
    private Transform innerBall;
    private MeshRenderer innerBallRenderer;

    private MLInputController controller;
    private MeshRenderer renderer;

    private void Start()
    {
        MLInput.Start();

        Debug.Assert(innerBall != null);
        innerBallRenderer = innerBall.GetComponent<MeshRenderer>();

        renderer = GetComponent<MeshRenderer>();
        Debug.Assert(renderer != null);

        // For BVW, we only have one controller bound to "left hand"
        controller = MLInput.GetController(MLInput.Hand.Left);
        Debug.Assert(controller != null);

        // Examples of registering events
        MLInput.OnControllerButtonDown += OnButtonDown;
        MLInput.OnControllerButtonUp += OnButtonUp;
    }

    private void OnDestroy()
    {
        MLInput.OnControllerButtonDown -= OnButtonDown;
        MLInput.OnControllerButtonUp -= OnButtonUp;

        MLInput.Stop();
    }

    private void Update()
    {
        if (controller.TriggerValue > 0.5f)
        {
            controller.StartFeedbackPatternVibe(
                MLInputControllerFeedbackPatternVibe.Buzz, MLInputControllerFeedbackIntensity.High);
        }

        if (controller.IsBumperDown)
        {
            // Bumper key can be also detected in this way
        }

        var touchForce = controller.Touch1PosAndForce.z;
        if (controller.Touch1Active && touchForce > 0.0f)
        {
            Vector2 touchPadPos = new Vector2(controller.Touch1PosAndForce.x, controller.Touch1PosAndForce.y) * 2f;
            var innerBallPos = new Vector3(touchPadPos.x, 0f, touchPadPos.y);
            innerBall.localPosition = innerBallPos;
            innerBallRenderer.material.color = Color.Lerp(Color.white, Color.cyan, touchForce);
        }
        else
        {
            var innerBallPos = new Vector3(0f, 0f, 0f);
            innerBall.localPosition = innerBallPos;
            innerBallRenderer.material.color = Color.white;
        }
    }

    private void OnButtonDown(byte controllerId, MLInputControllerButton button)
    {
        if (button == MLInputControllerButton.Bumper)
        {
            renderer.material.color = Color.red;

            controller.StartFeedbackPatternVibe(
                MLInputControllerFeedbackPatternVibe.ForceDown, MLInputControllerFeedbackIntensity.Low);
        }
    }

    private void OnButtonUp(byte controllerId, MLInputControllerButton button)
    {
        if (button == MLInputControllerButton.Bumper)
        {
            renderer.material.color = Color.white;

            controller.StopFeedbackPatternVibe();
        }
    }

}
