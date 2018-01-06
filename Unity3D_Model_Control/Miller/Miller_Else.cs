using UnityEngine;
using System.Collections;

// This is used to attach to other machine to control their movement
public class Miller_Else : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MainMiller = GameObject.Find("Miller");
        xframe = transform.Find("xframe");
        yframe = transform.Find("xframe/yframe");
        zframe = transform.Find("xframe/yframe/zframe");
        knife = transform.Find("xframe/yframe/zframe/Knife");
	}

    GameObject MainMiller;
    Vector3 xspeed, yspeed, zspeed;
    Transform xframe, yframe, zframe, knife;
	// Update is called once per frame
	void Update () {
        if (MainMiller == null) return;
        xspeed = MainMiller.GetComponent<Miller_Control>().xspeed;
        yspeed = MainMiller.GetComponent<Miller_Control>().yspeed;
        zspeed = MainMiller.GetComponent<Miller_Control>().zspeed;

        xframe.Translate(xspeed * Time.deltaTime);
        yframe.Translate(yspeed * Time.deltaTime);
        zframe.Translate(zspeed * Time.deltaTime);

        knife.Rotate(Vector3.up, 1200.0f * Time.deltaTime);
	}
}
