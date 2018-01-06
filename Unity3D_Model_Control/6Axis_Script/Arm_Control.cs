using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm_Control : MonoBehaviour {

	Transform bottom, arm1, arm2;
	float angle_x, angle_y, angle_z;  // Unit: in radius

	Vector3 position;
	float r1, r2;  // r1 is the low arm, r2 is the high arm
	Vector3 spiale1, spiale2; // spiale1 is for old frame, 2 is for new frame
	// Use this for initialization

	float scale = 1000.0f;
	void Start () {
		bottom = transform.Find ("Main1");
		arm1 = transform.Find("Main1/Main2");
		arm2 = transform.Find ("Main1/Main2/Main3");
		angle_x = 0.0f;angle_y = 0.0f;angle_z = 0.0f;

		r1 = 0.107f*scale;
		r2 = 0.135f*scale;
		position = new Vector3 (r1-r2, 0.0f, 0.0f);
	}

	float time, alltime;float testSpeed = 1.5f;
	// Update is called once per frame
	void Update () {

		bottom.localEulerAngles = new Vector3 (bottom.localEulerAngles.x, bottom.localEulerAngles.y+angle_x * Time.deltaTime * 180.0f / 3.14f, bottom.localEulerAngles.z);
		arm1.localEulerAngles = new Vector3 (arm1.localEulerAngles.x, arm1.localEulerAngles.y, arm1.localEulerAngles.z + angle_y * Time.deltaTime * 180.0f / 3.14f);
		arm2.localEulerAngles = new Vector3 (arm2.localEulerAngles.x, arm2.localEulerAngles.y, arm2.localEulerAngles.z - angle_z * Time.deltaTime * 180.0f / 3.14f);
		/*
		bottom.Rotate (Vector3.up, angle_x * Time.deltaTime * 180.0f / 3.14f);
		arm1.Rotate (Vector3.forward, angle_y * Time.deltaTime * 180.0f / 3.14f);
		arm2.Rotate (Vector3.left, angle_z * Time.deltaTime * 180.0f / 3.14f);
		*/
	}

	// Called when need the pin to move to target pos
	public void MoveTo(Vector3 pos){ 
		if (Vector3.Magnitude (pos) > r1 + r2 && pos.y > 0.0f)
			return;

		Vector3 spiale1 = GetSpiale (position);
		Vector3 spiale2 = GetSpiale (pos);

		Vector3 t1, t2;
		t1 = Vector3.Normalize (new Vector3 (position.x, 0.0f, position.z));
		t2 = Vector3.Normalize (new Vector3 (pos.x, 0.0f, pos.z));
		angle_x = Mathf.Acos (t2.x) - Mathf.Acos (t1.x);
		if (angle_x < 0.0f)
			angle_x += 6.28f;
		angle_y = Mathf.Acos (spiale1.y / Vector3.Magnitude (spiale1)) - Mathf.Acos (spiale2.y / Vector3.Magnitude (spiale2));
		
		spiale1 = position - spiale1;
		spiale2 = pos - spiale2;
		angle_z = Mathf.Acos (spiale1.y / Vector3.Magnitude (spiale1)) - Mathf.Acos (spiale2.y / Vector3.Magnitude (spiale2));

	}

	// Calculate the A when moving to pos (This pos should be relative to arm origin)
	Vector3 GetSpiale(Vector3 pos){
		float len = Vector3.Magnitude (pos);
		float t_angle = Mathf.Acos (Vector3.Magnitude (new Vector3 (pos.x, 0.0f, pos.z)) / len);
		float t_angle2 = Mathf.Acos((r1 * r1 + len * len - r2 * r2) / (2 * r1 * len));
		float angle = t_angle - t_angle2;
		float height = Mathf.Tan (angle) * Vector3.Magnitude (new Vector3(pos.x, 0.0f, pos.z));
		Vector3 tar_pos = Vector3.Normalize (new Vector3 (pos.x, height, pos.z));
		tar_pos = new Vector3 (tar_pos.x * r1, tar_pos.y * r1, tar_pos.z * r1);
		return tar_pos;
	}
}
