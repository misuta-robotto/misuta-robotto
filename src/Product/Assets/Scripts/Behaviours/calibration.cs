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
    public SteamVR_TrackedController leftController;
    public SteamVR_TrackedController rightController;
    private float userHeight;
    public HeadMove robotCoordinator;

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
        if( leftController.triggerPressed && rightController.triggerPressed && !robotCoordinator.enabled) {
            userHeight = InputTracking.GetLocalPosition(VRNode.Head).y * VRNODE_TO_REAL_HEIGHT_RATIO;
            Debug.Log(userHeight);        
        }
    }

    private void HandleMenuButtonClicked(object sender, ClickedEventArgs e) {
        robotCoordinator.enabled = !robotCoordinator.enabled;
    }

}
