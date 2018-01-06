using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkMove : MonoBehaviour {

	public AudioSource rotateSound, moveSound, sliceSound;

	Transform main1, main2, main3, bottom, knife, workPiece;
	GameObject wp1, wp2, machine2, container;
	Vector3 center;
	Vector3 main3Pos, piecePos;
	// Use this for initialization
	void Start () {
		center = this.transform.position;

		main1 = transform.Find ("6Axis_Main1");
		main2 = transform.Find ("6Axis_Main1/6Axis_Main2");
		main3 = transform.Find ("VerBottom/Machine_Plane/6Axis_Main3");
		bottom = transform.Find ("VerBottom/Machine_Plane/6Axis_Main3/part6/Cylinder");
		knife = transform.Find ("6Axis_Main1/6Axis_Main2/part3/Cube/part7/line");
		workPiece = transform.Find ("VerBottom/Machine_Plane/6Axis_Main3/part6/Cylinder/piece");
		wp1 = GameObject.Find ("6AxisMachine0/VerBottom/Machine_Plane/6Axis_Main3/part6/Cylinder/piece");
		wp2 = GameObject.Find("6AxisMachine1/VerBottom/Machine_Plane/6Axis_Main3/part6/Cylinder/piece");
		machine2 = GameObject.Find ("6AxisMachine1");
		container = GameObject.Find ("container");

		wp1.SetActive (false);
		wp2.SetActive (false);

		main3Pos = main3.position;
		piecePos = wp1.transform.position;
		//DataReset ();
		machine2.GetComponent<WorkMove> ().DataReset ();
		GameObject.Find ("6AxisMachine0").GetComponent<WorkMove> ().DataReset ();
	}

	/* Definition of State:
	/  0: Free
	/  1: Running (Still)
	/  2: Random
	/  3: Slice
	/  4: DrillBegin(conveyer motion)
	/  5: DrillRun
	 */
	public int state = 0;
	public bool startPipe = false;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown(KeyCode.Menu) || startPipe) {
			DataReset ();
		}
		SliceBegin ();
		DrillBegin ();
		DrillRun ();
	}

	public bool prepare = false, prepare2 = false, slice = false;
	bool moveBegin = false;
	Vector3 prepareTarget = new Vector3(0.0f, 0.0f, 0.0f);
	float angle = 0.0f, radius = 0.68f;
	int dir;

	// Slice Process Codes
	void SliceBegin(){
		if (state != 3) 
			return;

		if (!rotateSound.isPlaying)
			rotateSound.Play ();

		Vector3 curPos = this.GetComponent<SixAxis_Control> ().GetPosition () - center;
		// End condition
		if (radius < 0.5f) {
			sliceSound.Stop ();
			prepare2 = true;
			moveBegin = true;
			state = 4;
			return;
		}

		knife.Rotate(Vector3.up, 3600.0f*Time.deltaTime);
		// Prepare: Target time: 4.0s
		if (prepare) {
			if (this.GetComponent<SixAxis_Control> ().MoveTo (center+prepareTarget, 4.0f, moveBegin)){
				prepare = false;
				moveBegin = true;
				slice = true;
				return;
			}
			moveBegin = false;
			return;
		}

		// Slice the surface
		if (!sliceSound.isPlaying) sliceSound.Play();

		Vector3 pos = new Vector3(0.51f-radius*Mathf.Cos(angle/180.0f*3.14f), curPos.y, 2.76f-radius*Mathf.Sin(angle/180.0f*3.14f));
		if (slice) {
			if (this.GetComponent<SixAxis_Control> ().MoveTo (center + pos, 0.5f, moveBegin)) {
				if (angle >= 0.0f) {
					radius -= 0.05f;
					dir = -1;
				}
				if (angle <= -180.0f) {
					radius -= 0.06f;
					dir = 1;
				}
				angle += dir * 15.0f;
				moveBegin = true;
				return;
			}
			moveBegin = false;
		}

		return;
	}

	float timeCount = 0.0f, timeCount2 = 0.0f, friction = 0.0f, startTime;
	Vector3 startPos, endPos;
	bool putIn;

	// Drill Prepare Codes
	void DrillBegin(){
		if (state != 4) return;

		if (timeCount <= 1.0f) {
			timeCount += Time.deltaTime;
			startTime = Time.time;
			return;
		}
		if (prepare2) {
			friction = (Time.time - startTime) / 4.0f;
			if (friction < 1.0f)
				workPiece.transform.position = Vector3.Lerp (startPos, endPos, friction);
			else
				prepare2 = false;

			return;
		}

		if (timeCount2 <= 1.0f) {
			timeCount2 += Time.deltaTime;
			return;
		} else {
			wp1.SetActive (false);
			wp2.SetActive (true);
			state = 0;
			machine2.GetComponent<WorkMove> ().SetState (5);
			for (int i = 1; i < 7; i++) {
				string cylName = "6AxisMachine1/VerBottom/Machine_Plane/6Axis_Main3/part6/Cylinder/piece/fill" + i.ToString();
				GameObject.Find (cylName).SetActive (true);
			}
		}

	}

	bool prepare3 = false, moveBegin2 = false;
	int flow = 1;
	float[] drillPos;
	Vector3 downPos;
	// Drill running process
	void DrillRun(){
		if (state != 5) return;

		if (!moveSound.isPlaying)
			moveSound.Play ();
		
		for (int i = 1; i < 7; i++) {
			if (flow == i) {
				downPos = new Vector3 (drillPos[(i-1)*2], 2.95f, drillPos[i*2-1]);
				if (moveBegin) {
					moveBegin2 = true;
					ResetPass ();
					moveBegin = false;
				}
				DownDrill (downPos, i);
				if (pass3) {
					flow = i + 1;
					if (flow == 7) {
						wp2.SetActive (false);
						GameObject.Find ("container").GetComponent<ShowPiece> ().SetNum (true);
						state = 0;
						rotateSound.Stop ();
						moveSound.Stop ();
						sliceSound.Stop ();
						machine2.GetComponent<WorkMove> ().DataReset ();
						GameObject.Find ("6AxisMachine0").GetComponent<WorkMove> ().DataReset ();
					}
					moveBegin = true;
				}
			}
		}


	}

	bool pass1, pass2, pass3;
	void ResetPass(){
		pass1 = false;
		pass2 = false;
		pass3 = false;
	}
	void DownDrill(Vector3 pos, int id){
		if (!pass1) {
			if (this.GetComponent<SixAxis_Control> ().MoveTo (center + pos, 1.0f, moveBegin2)) {
				pass1 = true;
				moveBegin2 = true;
			} else {
				moveBegin2 = false;
				return;
			}
			return;
		}
		if (!pass2){
			if (this.GetComponent<SixAxis_Control> ().MoveTo (center + pos + new Vector3 (0.0f, -0.35f, 0.0f), 1.0f, moveBegin2)) {
				pass2 = true;
				moveBegin2 = true;
				string cylName = "6AxisMachine1/VerBottom/Machine_Plane/6Axis_Main3/part6/Cylinder/piece/fill" + id.ToString();
				GameObject.Find (cylName).SetActive (false);
			} else {
				moveBegin2 = false;
				return;
			}
			return;
		}
			
		if (!pass3) {
			if (this.GetComponent<SixAxis_Control> ().MoveTo (center + pos, 1.0f, moveBegin2)) {
				moveBegin2 = true;
				pass3 = true;
			} else {
				moveBegin2 = false;
				return;
			}
			return;
		}
	}

	public void SetState(int k){
		state = k;
	}

	public void DataReset(){
		if (this.name == "6AxisMachine0")
			state = 3;
		else
			state = 0;

		prepare = true;
		prepare2 = false;
		prepare3 = true;
		moveBegin = true;
		moveBegin2 = true;
		flow = 1;
		slice = false;
		prepareTarget = new Vector3(-0.17f, 2.9f, 2.76f);
		angle = 0.0f;
		radius = 0.67f;
		dir = -1;
		timeCount = 0.0f;
		timeCount2 = 0.0f;
		startPos = center + new Vector3 (3.5f,0.5f, 3.9f);
		endPos = center + new Vector3 (11.0f, 0.5f, 3.9f);
		drillPos = new float[]{-0.3f, 3.6f, -0.1f, 3.1f, 0.4f, 2.9f, 0.8f, 2.9f, 1.3f, 3.1f, 1.5f, 3.5f};
		friction = 0.0f;
		putIn = false;
		wp1.SetActive (true);
		wp2.SetActive (false);
		rotateSound.Stop ();
		moveSound.Stop ();
		sliceSound.Stop ();
		main3.position = main3Pos;
		if (this.name == "6AxisMachine0") workPiece.position = piecePos;
	}

	void OnGUI(){
		/*
		if (GUI.Button (NewPos (0.83f, 0.80f, 0.15f, 0.15f), "Start Produce")) {
			machine2.GetComponent<WorkMove> ().DataReset ();
			GameObject.Find ("6AxisMachine0").GetComponent<WorkMove> ().DataReset ();
		}
		*/
	}

	Rect NewPos(float xbegin, float ybegin, float xlen, float ylen)
	{
		return new Rect(xbegin * Screen.width, ybegin * Screen.height, xlen * Screen.width, ylen * Screen.height);
	}
}
