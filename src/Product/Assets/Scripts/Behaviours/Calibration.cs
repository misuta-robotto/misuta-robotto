using AL;
using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VR;

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
        if( leftController.triggerPressed && rightController.triggerPressed && calibrationMode) {
            userHeight = HeightTranslator.CalculateHeight(InputTracking.GetLocalPosition(VRNode.Head));
            hud.UpdateHeight(userHeight);
            ResizeKyle();
        }
    }

    private void ResizeKyle() {
        sizeRatio = HeightTranslator.CalculateSizeRatio(userHeight);
        kyle.localScale = new Vector3(sizeRatio, sizeRatio, sizeRatio);
    }

    private void HandleMenuButtonClicked(object sender, ClickedEventArgs e) {
        calibrationMode = !calibrationMode;
        if(ToggleMode != null) {
            ToggleMode(!calibrationMode);
        }
    }
}
