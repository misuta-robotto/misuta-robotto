using Assets;
using UnityEngine;
using UnityEngine.VR;

public class HeadMove : MonoBehaviour {
    public RobotCoordinator robCord;
    public Calibration calibration;

    private HeadTranslator headTranslator;

    void Start () {
        headTranslator = new HeadTranslator();
        calibration.ToggleMode += SetEnabled;
        enabled = false;
    }

    void SetEnabled (bool b) {
        enabled = b;
    }

	void Update () {
        float pitch = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.x; //get Vive pitch
        float yaw = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.y; //get Vive yaw

        robCord.HeadPitch = headTranslator.TranslatePitch(pitch);
        robCord.HeadYaw = yaw;
    }
}
