using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class ControllerEventManager : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction OnClicked;

    public void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            OnClicked();
        }
    }
}
