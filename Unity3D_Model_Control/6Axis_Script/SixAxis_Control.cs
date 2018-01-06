using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixAxis_Control : MonoBehaviour {

	public Transform main1,main2, main3, bottom, knife;
	public Vector3 speed1, speed2, speed3;
	public Vector3 xPos, yPos, zPos;

	// Use this for initialization
	void Start () {
		/* (xPos, yPos, zPos) */
		main1 = transform.Find ("6Axis_Main1");
		main2 = transform.Find ("6Axis_Main1/6Axis_Main2");
		main3 = transform.Find ("VerBottom/Machine_Plane/6Axis_Main3");
		bottom = transform.Find ("VerBottom/Machine_Plane/6Axis_Main3/part6/Cylinder");
		knife = transform.Find ("6Axis_Main1/6Axis_Main2/part3/Cube/part7/line");

		xPos = new Vector3 (0.0f, 0.0f, 0.0f);
		yPos = new Vector3 (0.0f, 0.0f, 0.0f);
		zPos = new Vector3 (0.0f, 0.0f, 0.0f);
	}
		
	// Update is called once per frame
	void Update () {

	}

	public Vector3 GetPosition(){
		return new Vector3 (main1.position.x, main2.position.y, main3.position.z);
	}

	Vector3 xOld, yOld, zOld;
	float timeTick, startTime = 0.0f;
	public bool MoveTo(Vector3 tar, float time, bool newMove){
		if (newMove) {
			startTime = Time.time;
			xOld = main1.position;yOld = main2.position;zOld = main3.position;
		}
		float friction = ( Time.time - startTime ) / time;  

		Vector3 oriPos;
		oriPos = main1.position;
		main1.position = Vector3.Lerp (xOld, new Vector3 (tar.x, xOld.y, xOld.z), friction);
		/*
		if (main1.position.x > -16.0f || main1.position.x < -21.0f)
			main1.position = oriPos;
		*/
		oriPos = main2.position;
		main2.position = Vector3.Lerp (yOld, new Vector3 (tar.x - xOld.x + yOld.x, tar.y, yOld.z), friction);
		/*
		if (main2.position.y > 3.3f || main1.position.y < -0.4f)
			main2.position = oriPos;
		*/
		oriPos = main3.position;
		main3.position = Vector3.Lerp (zOld, new Vector3 (zOld.x, zOld.y, tar.z), friction);
		/*
		if (main3.position.z > -9.0f || main3.position.z<-13.0f)
			main3.position = oriPos;
		*/

		if (friction >= 1) return true;
		return false;
	}
}
