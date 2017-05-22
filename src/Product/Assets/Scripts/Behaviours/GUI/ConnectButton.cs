using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets;
using System;

public class ConnectButton : MonoBehaviour {

    public Button connectButton;
    public RobotCoordinator robotCoordinator;
    public Text buttonText;
    public InputField IP_input;
    public InputField port_input;

    public void ConnectingButton()
    {
        int portInt;
        if (Int32.TryParse(port_input.text, out portInt))
        {
            robotCoordinator.Connect(IP_input.text, portInt);
            buttonText.text = "Disconnect";
        }
        else
        {
            buttonText.text = "Invalid port";
        }
    }
}
