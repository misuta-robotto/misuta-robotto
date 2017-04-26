using UnityEngine;
public class DisplayWebcam : MonoBehaviour {
	void Start() {
		Debug.Log("DisplayWebcam Initialize");
		// Transform values of the plane to get you started:
		// position 0   0   200
		// rotation 90  180 0
		// scale    80  1   40
		var devices = WebCamTexture.devices;
		//WebCamTexture.videoVerticallyMirrored = false;
		var backCamName = "";
		if(devices.Length > 0) backCamName = devices[0].name;
		for(int i = 0; i < devices.Length; i++) {
			Debug.Log("Device:" + devices[i].name + "IS FRONT FACING:" + devices[i].isFrontFacing);
			if(!devices[i].isFrontFacing) {
				backCamName = devices[i].name;
			}
		}
		var CameraTexture = new WebCamTexture(backCamName, 10000, 10000, 30);
		CameraTexture.Play();
		var renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = CameraTexture;
	}
	void Update () {
	}
}