using UnityEngine;
using System.Collections;

public class left_right : MonoBehaviour {

    float max = 0.0f;
    Vector3 dir = new Vector3(0.0f, 0.0f, 1.0f);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(dir * Time.deltaTime * 30);
        max += Time.deltaTime * 10;
        if (max > 30) dir = new Vector3(0.0f, 0.0f, -1.0f);
        if (max > 90) dir = new Vector3(0.0f, 0.0f, 1.0f);
        if (max > 150) dir = new Vector3(0.0f, 0.0f, -1.0f);
	}
}
