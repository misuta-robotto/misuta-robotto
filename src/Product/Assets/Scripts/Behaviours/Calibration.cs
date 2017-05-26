using Assets;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.UI;
using MishutaRoboto;
/*
 * Calibration handles the calibration of the users 3D representation in Unity
 * so that its size is a good estimate of the users size. This helps to more
 * accuratly represent the users arm movements.  
 */

public class Calibration : MonoBehaviour
{
    public SteamVR_TrackedController leftController;
    public SteamVR_TrackedController rightController;
    public VRHud hud;

    public delegate void ToggleCalibrationMode(bool b);
    public event ToggleCalibrationMode ToggleMode;
    public bool calibrationMode;

    public Transform kyle;
    public float sizeRatio;
    public float userHeight; //m

    public Toggle toggle;
    public InputField manual_calibration_input;
    public Button manual_calibration_button;

    public float MIN_HEIGHT = 0.5f;
    public float MAX_HEIGHT = 2.5f;

    void Start()
    {
        toggle.isOn = true;
        userHeight = HeightTranslator.KYLE_HEIGHT;
        sizeRatio = 1f;

        // Uncomment to enable automatic calibration when testing
        //Invoke("ToggleManual", 3);
    }

    void ToggleManual()
    {
        ToggleMode(true);
    }
    
    /*
    When the script is enabled it subscribes itself to events for the controller
        actions TriggerClicked and MenuButtonClicked.
    */
    private void OnEnable()
    {
        leftController.TriggerClicked += HandleTriggerClicked;
        rightController.TriggerClicked += HandleTriggerClicked;
        leftController.MenuButtonClicked += HandleMenuButtonClicked;
        rightController.MenuButtonClicked += HandleMenuButtonClicked;
        toggle.onValueChanged.AddListener(delegate { UpdateCalibrationMode(); });
    }

    /*
    When the script is disabled it unsubscribes itself from the events.
    */
    private void OnDisable()
    {
        leftController.TriggerClicked -= HandleTriggerClicked;
        rightController.TriggerClicked -= HandleTriggerClicked;
        leftController.MenuButtonClicked -= HandleMenuButtonClicked;
        rightController.MenuButtonClicked -= HandleMenuButtonClicked;
    }

    /*
    Resizes the 3D representation of the user in Unity when both triggers are
    pressed and in system is in calibration mode.
    */
    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (leftController.triggerPressed && rightController.triggerPressed && toggle.isOn)
        {
            userHeight = HeightTranslator.CalculateHeight(InputTracking.GetLocalPosition(VRNode.Head));
            hud.UpdateHeight(userHeight);
            ResizeKyle();
        }
    }


    public void ResizeKyle()
    {
        sizeRatio = HeightTranslator.CalculateSizeRatio(userHeight);
        kyle.localScale = new Vector3(sizeRatio, sizeRatio, sizeRatio);
    }

    /*
    Toggels calibration mode and signals the toggle to all subscribers of the
    ToggleMode event when the menubutton is pressed. The subscribers use this
    for example to keep the robot still or render different objects of the scene
    during calibration.
    */
    private void HandleMenuButtonClicked(object sender, ClickedEventArgs e)
    {
        toggle.isOn = !toggle.isOn;
    }

    private void UpdateCalibrationMode()
    {
        ToggleMode(!toggle.isOn);
    }

    public void ManualCalibration()
    {
        string height = manual_calibration_input.text.ToString();
        float heightFloat;
        if (float.TryParse(height, out heightFloat))
        {
            
            userHeight = heightFloat.Clamp(MIN_HEIGHT, MAX_HEIGHT);
            if (toggle.isOn)
            {
                hud.UpdateHeight(userHeight);
                ResizeKyle();
                Debug.Log("(SUCCESS manual) height: " + userHeight);
            }
        }

    }
}

