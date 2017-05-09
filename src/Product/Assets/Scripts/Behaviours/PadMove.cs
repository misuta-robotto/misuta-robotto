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

    public SteamVR_TrackedObject trackedObjLeft;
    public SteamVR_TrackedObject trackedObjRight;

    // Add controllers
    private SteamVR_Controller.Device ControllerLeft {
	    get { return SteamVR_Controller.Input((int)trackedObjLeft.index); }
    }

    private SteamVR_Controller.Device ControllerRight {
        get { return SteamVR_Controller.Input((int)trackedObjRight.index); }
    }

    void Start () {
        calibration.ToggleMode += SetEnabled;
        enabled = false;
    }

    void SetEnabled(bool b) {
        enabled = b;
    }

    void Update()
    {
        if (ControllerLeft.GetAxis() != null && ControllerRight.GetAxis() != null)
        {
            // ControllerLeft - Movement back and forth
            if (Math.Abs(ControllerLeft.GetAxis().y) > CONTROLLER_SENSITIVITY_LIMIT)
            {
                x = ControllerLeft.GetAxis().y;
            }
            else
            {
                x = 0;
            }

            // ControllerLeft - Movement left and right
            if (Math.Abs(ControllerLeft.GetAxis().x) > CONTROLLER_SENSITIVITY_LIMIT)
            {
                y = -ControllerLeft.GetAxis().x;
            }
            else
            {
                y = 0;
            }

            // ControllerRight - Rotation
            if (Math.Abs(ControllerRight.GetAxis().x) > CONTROLLER_SENSITIVITY_LIMIT)
            {
                theta = -ControllerRight.GetAxis().x;
            }
            else
            {
                theta = 0;
            }
            
            robCord.PadValues = new float[] { x, y, theta };
        }
        else
        {
            Debug.Log("Controller error.");
        }
    } 

}
