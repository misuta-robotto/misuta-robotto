using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IPandPort : MonoBehaviour {

	public InputField IP_input; 
	public Button IP_button; 
	public Text IP_field;
	public InputField port_input; 
	public Button port_button; 
	public Text port_field;

	// Use this for initialization
	void Start () {
		//IP
		IP_input.onEndEdit.AddListener(IP_Enter);
		Button btn1 = IP_button.GetComponent<Button> ();
		btn1.onClick.AddListener (IP_Button);

		//port
		port_input.onEndEdit.AddListener(port_Enter);
		Button btn2 = port_button.GetComponent<Button> ();
		btn2.onClick.AddListener (port_Button);
	}

	public void IP_Enter (string IP){
		Debug.Log ("(Enter) IP: " + IP);	
		//127.0.1.130 säkert fel
	}

	public void IP_Button (){
		string IP = IP_field.text.ToString ();
		Debug.Log("(Button) IP:" + IP);
	}

	public void port_Enter (string port){
		Debug.Log ("(Enter) port: " + port);	
	}

	public void port_Button (){
		string port = port_field.text.ToString (); 
		//Debug.Log("(Button) port: " + port);
	}

		
	// Update is called once per frame
	void Update () {

	}
}
