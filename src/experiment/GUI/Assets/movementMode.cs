using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movementMode : MonoBehaviour {
	string[] options = new string[] { "Pad based", "Location based"};
	Rect position = new Rect(231, 200, 230, 100);
	int selected = 0;

	void OnGUI(){
			selected = GUI.SelectionGrid(position, selected, options, options.Length, GUI.skin.toggle);
			//Debug.Log ("Selected:" + selected);
	}
}
