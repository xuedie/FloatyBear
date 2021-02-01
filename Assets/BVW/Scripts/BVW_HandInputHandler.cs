using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class BVW_HandInputHandler : MonoBehaviour
{
    [SerializeField]
    private MLHandType handType = MLHandType.Left;

    [SerializeField]
    private MLKeyPointFilterLevel keyPointFilterLevel = MLKeyPointFilterLevel.ExtraSmoothed;

    [SerializeField]
    private MLPoseFilterLevel poseFilterLevel = MLPoseFilterLevel.ExtraRobust;

    private MLHandKeyPose[] handPoses = new[] { MLHandKeyPose.Ok };

    private MeshRenderer renderer;

    private void Start()
    {
        MLHands.Start();

        renderer = GetComponent<MeshRenderer>();
        Debug.Assert(renderer);

        MLHands.KeyPoseManager.EnableKeyPoses(handPoses, true);

        MLHands.KeyPoseManager.SetKeyPointsFilterLevel(keyPointFilterLevel);
        MLHands.KeyPoseManager.SetPoseFilterLevel(poseFilterLevel);
    }

    private void OnDestroy()
    {
        MLHands.Stop();
    }

    private void Update()
    {
        MLHand hand = (handType == MLHandType.Left) ? MLHands.Left : MLHands.Right;

        // Hand Wrist
        if (hand.IsVisible && hand.Wrist.KeyPoints.Count > 0)
        {
            // To get the direction of the hand, get multiple keypoints and calculate the direction
            var handPos = hand.Wrist.KeyPoints[0].Position;
            transform.position = handPos;
        }
        else if (hand.IsVisible)
        {
            Debug.Log("KeyPoints: " + hand.Wrist.KeyPoints.Count);
        }

        // Gesture Detection
        if (GetGesture(hand, handPoses[0]))
        {
            renderer.material.color = Color.red;
        }
        else
        {
            renderer.material.color = Color.white;
        }
    }

    private bool GetGesture(MLHand hand, MLHandKeyPose type)
    {
        if (hand != null)
        {
            if (hand.KeyPose == type)
            {
                if (hand.KeyPoseConfidence > 0.8f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
