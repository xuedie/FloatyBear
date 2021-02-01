using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class PlacementControl : MonoBehaviour
{
    [SerializeField] MLSpatialMapper mapper;
    [SerializeField] ControllerConnectionHandler handler;
    void Start()
    {
        MLInput.OnControllerButtonDown += HandleOnButtonDown;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void HandleOnButtonDown(byte controllerId, MLInputControllerButton button)
    {
        if (handler.IsControllerValid() && handler.ConnectedController.Id == controllerId &&
            button == MLInputControllerButton.Bumper)
        {
            mapper.DestroyAllMeshes();
        }
    }
}
