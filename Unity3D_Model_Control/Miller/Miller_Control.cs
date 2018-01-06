using UnityEngine;
using System.Collections;

// This script is to control the movement of three axises with data from Miller_Data_Trans
public class Miller_Control : MonoBehaviour {

	// This variable can be fixed to adjust the performance
	public float xorigin = 0.0f, yorigin = 0.0f, zorigin = 0.0f;
	public double size = 1.0;


	// Use this for initialization
    public Transform xframe, yframe, zframe, knife;
	void Start () {
		// This set the scale rule
		size = size*1.0/12;
		// Set the initial position of different part
		xframe = transform.Find("xframe");
        yframe = transform.Find("xframe/yframe");
        zframe = transform.Find("xframe/yframe/zframe");
        knife = transform.Find("xframe/yframe/zframe/Knife");

		Vector3 pos = xframe.localPosition;
		xframe.localPosition = new Vector3 (pos.x, pos.y, 0);  // xframe.position.z
		pos = yframe.localPosition;
		yframe.localPosition = new Vector3 (0, pos.y, pos.z);  // yframe.position.x;
		pos = zframe.localPosition;
		zframe.localPosition = new Vector3 (pos.x, 0, pos.z);  // zframe.position.y;

	}


	// Variables used to calculate position
	double xnewpos, xoldpos, yoldpos;
	double ynewpos, zoldpos, znewpos;
	public Vector3 xspeed, yspeed, zspeed;
	float time = 1.0f;

	// Update is called once per frame
	void FixedUpdate () {

		//if (!this.GetComponent<Miller_Data_Trans>().running) return;   // This means clicking on the connect button

        // Case when it is connected (use online data)
        double xmove, ymove, zmove;
        xmove = this.GetComponent<Miller_Data_Trans>().x_axis*size;   // Unit is mm on Mach3
        ymove = this.GetComponent<Miller_Data_Trans>().y_axis*size;
        zmove = this.GetComponent<Miller_Data_Trans>().z_axis*size;
        // Time count an calculate xspeed, yspeed, zspeed
        time += Time.deltaTime; 
		if (time >= 1.0f)  // delay second : 1s
        {
            xoldpos = xnewpos; yoldpos = ynewpos; zoldpos = znewpos;
            xnewpos = xmove; ynewpos = ymove; znewpos = zmove;    // An easy control delay --- several seconds
            
            // Pay attention to the coordinate convention carefully. World, Unity Self coordinate
            xframe.localPosition = new Vector3(xframe.localPosition.x, xframe.localPosition.y,
            (float)(yorigin + yoldpos));
            xspeed = new Vector3(0.0f, 0.0f, (float)(yoldpos - ynewpos));

            yframe.localPosition = new Vector3((float)(xorigin + xoldpos), yframe.localPosition.y,
                yframe.localPosition.z);
            yspeed = new Vector3((float)(xnewpos - xoldpos), 0.0f, 0.0f);

            zframe.localPosition = new Vector3(zframe.localPosition.x, (float)(zorigin + zoldpos),
                zframe.localPosition.z);
            zspeed = new Vector3(0.0f, (float)(znewpos - zoldpos), 0.0f);

            time = 0.0f;  // Clear time

			/* Call Draw Line */
			// Calculate the position of line according to the knife position
			float height = knife.lossyScale.y;
			Vector3 pos = new Vector3 (knife.position.x, knife.position.y - height / 2, knife.position.z);
			this.GetComponent<DrawLine> ().vertex = pos;
			// Calculate the speed of spin to judge draw
			float v = Vector3.Magnitude (xspeed + yspeed + zspeed);

			if (zframe.localPosition.y < 0.8 && v < 2.4)
				this.GetComponent<DrawLine> ().draw = true;
			else {
				this.GetComponent<DrawLine> ().draw = false;
				this.GetComponent<DrawLine> ().newLine = true;
			}
        }

		// Set movement style
        xframe.Translate(xspeed * Time.deltaTime);
        yframe.Translate(yspeed * Time.deltaTime);
        zframe.Translate(zspeed * Time.deltaTime);
        knife.Rotate(Vector3.up, 1200.0f*Time.deltaTime);

	}
}
