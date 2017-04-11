using AL;
using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VR;

public class PadMove : MonoBehaviour {
    public SteamVR_TrackedController leftController;
    public SteamVR_TrackedController rightController;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private float userHeight;
    public script robotCoordinator;

    private void OnEnable() {
        leftController.TriggerClicked += HandleTriggerClicked;
        rightController.TriggerClicked += HandleTriggerClicked;
        leftController.MenuButtonClicked += HandleMenuButtonClicked;
        rightController.MenuButtonClicked += HandleMenuButtonClicked;
    }

    private void OnDisable() {
        leftController.TriggerClicked -= HandleTriggerClicked;
        rightController.TriggerClicked -= HandleTriggerClicked;
        leftController.MenuButtonClicked += HandleMenuButtonClicked;
        rightController.MenuButtonClicked += HandleMenuButtonClicked;
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e) {
        if( leftController.GetPressDown(triggerButton) && rightController.GetPressDown(triggerButton) && !robotCoordinator.Enable) {
            userHeight = InputTracking.GetLocalPosition(VRNode.Head).y;
            Debug.Log(userHeight);        
        }
    }

    private void HandleMenuButtonClicked(object sender, ClickedEventArgs e) {
        robotCoordinator.Enable = !robotCoordinator.Enable;
    }

}
