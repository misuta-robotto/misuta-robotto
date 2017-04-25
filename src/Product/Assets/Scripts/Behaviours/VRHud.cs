using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRHud : MonoBehaviour {
    public Text heightText;

    public void UpdateHeight(float height)
    {
        heightText.text = Mathf.Round((height * 100f)).ToString();
    }
}
