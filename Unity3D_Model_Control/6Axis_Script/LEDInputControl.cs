using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LEDInputControl : MonoBehaviour {

	// Use this for initialization
	Vector3 xPos, yPos, zPos;
	TextMesh xtext, ytext, ztext;
	GameObject machine;
	void Start () {
		string name = this.name;
		xtext = GameObject.Find (name+"6Axis_Main1/part2/Cube/Screen/XInput").GetComponent<TextMesh> ();
		ytext = GameObject.Find (name+"6Axis_Main1/part2/Cube/Screen/YInput").GetComponent<TextMesh> ();
		ztext = GameObject.Find (name+"6Axis_Main1/part2/Cube/Screen/ZInput").GetComponent<TextMesh> ();

	}
	
	// Update is called once per frame
	void Update () {
		xPos = this.GetComponent<SixAxis_Control> ().xPos;
		yPos = this.GetComponent<SixAxis_Control> ().yPos;
		zPos = this.GetComponent<SixAxis_Control> ().zPos;

		xPos *= 100;
		yPos *= 100;
		zPos *= 100;
		xtext.text =xPos.ToString();
		ytext.text = yPos.ToString();
		ztext.text = zPos.ToString();
	}
}
