using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using AL;


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

		//port
		port_input.onEndEdit.AddListener(port_Enter);
	}

	public void IP_Enter (string IP){
		Debug.Log ("(Enter) IP: " + IP);
        //127.0.1.130 säkert fel
        RobotConfiguration.setAdress(IP);
        Debug.Log("Robot config addr: " + RobotConfiguration.ADRESS);
    }

	public void IP_Button (){
		string IP = IP_field.text.ToString ();
		Debug.Log("(Button) IP:" + IP);
        RobotConfiguration.setAdress(IP);
        Debug.Log("Robot config addr: " + RobotConfiguration.ADRESS);
    }

	public void port_Enter (string port){
        Debug.Log("(Enter) port: " + port);
        int portInt;
        if (Int32.TryParse(port, out portInt))
        {
            Debug.Log("(SUCCESS Enter) port: " + portInt);
            RobotConfiguration.setPort(portInt);
        }
        Debug.Log("Robot config port: " + RobotConfiguration.PORT);
    }

	public void port_Button (){
		string port = port_field.text.ToString ();
        Debug.Log("(Button) port: " + port);
        int portInt;
        if (Int32.TryParse(port, out portInt))
        {
            Debug.Log("(SUCCESS Button) port: " + portInt);
            RobotConfiguration.setPort(portInt);
        }
        Debug.Log("Robot config port: " + RobotConfiguration.PORT);
    }
		
	// Update is called once per frame
	void Update () {

	}
}
