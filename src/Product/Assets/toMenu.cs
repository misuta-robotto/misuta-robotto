using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class toMenu : MonoBehaviour
{

    public Button menu_Button;
    public GameObject MainMenuView;
    //public int index = 0;

    // Use this for initialization
    void Start()
    {
        Debug.Log("to menu");
    }

    public void press_Button1()
    {
        Debug.Log("try to switch display");
        gameObject.SetActive(false);
    }

}