using UnityEngine;
using System.Collections;

public class point : MonoBehaviour {

	// Use this for initialization
    bool finish = false;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //当用户接触到目标物体改变参数到下一个目标物体
    void OnTriggerEnter(Collider cos)
    {

            move.Add();
    }
}
