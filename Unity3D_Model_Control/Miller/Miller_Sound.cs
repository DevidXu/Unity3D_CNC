using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miller_Sound : MonoBehaviour {
	public AudioSource rotate, move, slice;

	// Use this for initialization
	void Start () {
		
	}

	bool rotateStart = false, moveStart = false, sliceStart = false;
	// Update is called once per frame
	void Update () {
		if (this.GetComponent<Miller_Data_Trans> ().running) {
			if (!rotateStart) {
				rotate.Play ();
				rotateStart = true;
			}

			if (this.GetComponent<Miller_Control> ().xspeed.magnitude == 0.0f && this.GetComponent<Miller_Control> ().xspeed.magnitude == 0.0f) {
				move.Stop ();
				moveStart = false;
			} else {
				if (!moveStart) {
					move.Play ();
					moveStart = true;
				}
			}

			if (this.GetComponent<Miller_Control> ().zframe.localPosition.y<0.003f) {
				if (!sliceStart) {
					slice.Play ();
					sliceStart = true;
				}
			} else {
				slice.Stop ();
				sliceStart = false;
			}

		} else {
			rotate.Stop ();
			rotateStart = false;
		}
	}


}
