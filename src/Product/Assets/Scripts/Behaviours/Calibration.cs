using AL;
using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VR;

public class Calibration : MonoBehaviour {
    private const float VRNODE_TO_REAL_HEIGHT_RATIO = 1.16f;
    private const float KYLE_HEIGHT = 1.8f;

    public SteamVR_TrackedController leftController;
    public SteamVR_TrackedController rightController;

    public delegate void ToggleCalibrationMode(bool b);
    public event ToggleCalibrationMode ToggleMode;
    public bool calibrationMode;

    public Transform kyle;
    public float sizeRatio;

    void Start() {
        calibrationMode = false;
    }

    private void OnEnable() {
        leftController.TriggerClicked += HandleTriggerClicked;
        rightController.TriggerClicked += HandleTriggerClicked;
        leftController.MenuButtonClicked += HandleMenuButtonClicked;
        rightController.MenuButtonClicked += HandleMenuButtonClicked;
    }

    private void OnDisable() {
        leftController.TriggerClicked -= HandleTriggerClicked;
        rightController.TriggerClicked -= HandleTriggerClicked;
        leftController.MenuButtonClicked -= HandleMenuButtonClicked;
        rightController.MenuButtonClicked -= HandleMenuButtonClicked;
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e) {
        if( leftController.triggerPressed && rightController.triggerPressed && !calibrationMode) {
            float userHeight = InputTracking.GetLocalPosition(VRNode.Head).y * VRNODE_TO_REAL_HEIGHT_RATIO;
            ResizeKyle(userHeight);
        }
    }

    private void ResizeKyle(float userHeight) {
        sizeRatio = userHeight / KYLE_HEIGHT;
        kyle.localScale = new Vector3(sizeRatio, sizeRatio, sizeRatio);
    }

    private void HandleMenuButtonClicked(object sender, ClickedEventArgs e) {
        calibrationMode = !calibrationMode;
        if(ToggleMode != null) {
            ToggleMode(!calibrationMode);
        }
    }
}
