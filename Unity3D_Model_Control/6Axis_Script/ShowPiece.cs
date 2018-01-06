using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPiece : MonoBehaviour {
	int num = 0;
	GameObject[] piece = new GameObject[4];
	// Use this for initialization
	void Start () {
		for (int i = 0; i < 4; i++) {
			piece [i] = GameObject.Find ("container/workPiece" + i.ToString ());
			piece [i].SetActive (false);
		}
	}


	// Update is called once per frame
	void Update () {
		
	}

	public void SetNum(bool addOne){
		if (addOne) {
			num += 1;
			if (num > 4)
				num = 4;
			for (int i = 0; i < num; i++)
				piece [i].SetActive (true);
		} else {
			for (int i = 0; i < 4; i++)
				piece [i].SetActive (false);
		}
	}
		
}
