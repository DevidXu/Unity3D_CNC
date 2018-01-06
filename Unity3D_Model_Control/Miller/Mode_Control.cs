using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Initilize all data and read mode from data_cube
public class Mode_Control : MonoBehaviour {

	public int mode;
	// Use this for initialization
	void Start () {
		// Get the current control mode
		GameObject data_cube = GameObject.Find ("Data_Cube");
		if (data_cube)
			mode = data_cube.GetComponent<Data_Manage> ().mode;

		GameObject miller = GameObject.Find ("Miller");
		miller.GetComponent<Miller_Data_Trans>().running = true;
		if (mode == 1)
			miller.GetComponent<Miller_Data_Trans> ().connect = true;
		if (mode == 2)
			miller.GetComponent<Miller_Data_Trans> ().connect = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
