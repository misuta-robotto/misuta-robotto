using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class toCamera : MonoBehaviour {

	public Button camera_Button; 
	public int index = 0;

	// Use this for initialization
	void Start () {
		
		Button btn1 = camera_Button.GetComponent<Button> ();
		btn1.onClick.AddListener (press_Button);
	}

	public void press_Button (){
		Debug.Log("try to switch display");
		PlayerPrefs.SetInt ("UnitySelectMonitor", index++);
	}

}