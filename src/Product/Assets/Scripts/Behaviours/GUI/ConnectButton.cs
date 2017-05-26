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

    private bool connected = false;

    public void ConnectingButton()
    {
        if (!connected)
        {
            int portInt;
            if (Int32.TryParse(port_input.text, out portInt))
            {
                robotCoordinator.Connect(IP_input.text, portInt);
                buttonText.text = "Disconnect";
                connected = true;
            }
            else
            {
                buttonText.text = "Invalid port";
            }
        }
        else
        {
            robotCoordinator.Disconnect();
            buttonText.text = "Connect";
        }
    }
}
