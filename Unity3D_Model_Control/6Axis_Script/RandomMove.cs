using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour {

	Transform main1, main2, main3;
	// Use this for initialization
	void Start () {
		main1 = this.GetComponent<SixAxis_Control> ().main1;
		main2 = this.GetComponent<SixAxis_Control> ().main2;
		main3 = this.GetComponent<SixAxis_Control> ().main3;
	}
	
	// Update is called once per frame
	void Update () {
		float x, y, z;
		x = Random.value;
		y = Random.value;
		z = Random.value;

		this.GetComponent<SixAxis_Control> ().MoveTo (new Vector3(x,y,z), 1.0f, true);
		this.GetComponent<SixAxis_Control>().bottom.Rotate(Vector3.up, 300.0f*Time.deltaTime);
	}
}
