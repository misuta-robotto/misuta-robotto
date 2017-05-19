using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class toCamera : MonoBehaviour {

	public Button camera_Button; 
	//public int index = 0;

	// Use this for initialization
	void Start () {
        Debug.Log("Disabling camera");
        gameObject.SetActive(false);
    }

	public void press_Button (){
		Debug.Log("Abling camera");
        gameObject.SetActive(true);
    }

}