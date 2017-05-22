using Assets;
using UnityEngine;
using UnityEngine.VR;
/*
 * Calibration handles the calibration of the users 3D representation in Unity
 * so that its size is a good estimate of the users size. This helps to more
 * accuratly represent the users arm movements.  
 */
public class Calibration : MonoBehaviour {
    public SteamVR_TrackedController leftController;
    public SteamVR_TrackedController rightController;
    public VRHud hud;

    public delegate void ToggleCalibrationMode(bool b);
    public event ToggleCalibrationMode ToggleMode;
    public bool calibrationMode;

    public Transform kyle;
    public float sizeRatio;
    private float userHeight;

    void Start() {
        calibrationMode = true;
        userHeight = HeightTranslator.KYLE_HEIGHT;
        sizeRatio = 1f;

        Invoke("AutomaticToggle", 3);
    }

    void AutomaticToggle()
    {
        HandleMenuButtonClicked(null, new ClickedEventArgs());
    }

    /*
    When the script is enabled it subscribes itself to events for the controller
     actions TriggerClicked and MenuButtonClicked.
    */
    private void OnEnable() {
        leftController.TriggerClicked += HandleTriggerClicked;
        rightController.TriggerClicked += HandleTriggerClicked;
        leftController.MenuButtonClicked += HandleMenuButtonClicked;
        rightController.MenuButtonClicked += HandleMenuButtonClicked;
    }

    /*
    When the script is disabled it unsubscribes itself from the events.
    */
    private void OnDisable() {
        leftController.TriggerClicked -= HandleTriggerClicked;
        rightController.TriggerClicked -= HandleTriggerClicked;
        leftController.MenuButtonClicked -= HandleMenuButtonClicked;
        rightController.MenuButtonClicked -= HandleMenuButtonClicked;
    }

    /*
    Resizes the 3D representation of the user in Unity when both triggers are
    pressed and in system is in calibration mode.
    */
    private void HandleTriggerClicked(object sender, ClickedEventArgs e) {
        if (leftController.triggerPressed && rightController.triggerPressed && calibrationMode) {
            userHeight = HeightTranslator.CalculateHeight(InputTracking.GetLocalPosition(VRNode.Head));
            hud.UpdateHeight(userHeight);
            ResizeKyle();
        }
    }

    private void ResizeKyle() {
        sizeRatio = HeightTranslator.CalculateSizeRatio(userHeight);
        kyle.localScale = new Vector3(sizeRatio, sizeRatio, sizeRatio);
    }

    /*
    Toggels calibration mode and signals the toggle to all subscribers of the
    ToggleMode event when the menubutton is pressed. The subscribers use this
    for example to keep the robot still or render different objects of the scene
    during calibration.
    */
    private void HandleMenuButtonClicked(object sender, ClickedEventArgs e) {
        calibrationMode = !calibrationMode;
        if (ToggleMode != null) {
            ToggleMode(!calibrationMode);
        }
    }
}
