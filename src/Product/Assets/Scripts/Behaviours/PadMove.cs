using System;
using UnityEngine;

public class PadMove : MonoBehaviour {
    public SteamVR_TrackedController leftController;
    public SteamVR_TrackedController rightController;
    public RobotCoordinator robCord;
    public Calibration calibration;

    private const float CONTROLLER_SENSITIVITY_LIMIT = 0.2f;
    
    private float x = 0;
    private float y = 0;
    private float theta = 0;

    void Start () {
        calibration.ToggleMode += SetEnabled;
        enabled = false;
    }

    void SetEnabled(bool b) {
        enabled = b;
    }

    void Update()
    {
        // ControllerLeft - Movement back and forth
        if (Math.Abs(leftController.controllerState.rAxis0.y) > CONTROLLER_SENSITIVITY_LIMIT)
        {
            x = leftController.controllerState.rAxis0.y;
        }
        else
        {
            x = 0;
        }

        // ControllerLeft - Movement left and right
        if (Math.Abs(leftController.controllerState.rAxis0.x) > CONTROLLER_SENSITIVITY_LIMIT)
        {
            y = -leftController.controllerState.rAxis0.x;
        }
        else
        {
            y = 0;
        }

        // ControllerRight - Rotation
        if (Math.Abs(rightController.controllerState.rAxis0.x) > CONTROLLER_SENSITIVITY_LIMIT)
        {
            theta = -rightController.controllerState.rAxis0.x;
        }
        else
        {
            theta = 0;
        }
            
        robCord.PadValues = new float[] { x, y, theta };
    } 

}
