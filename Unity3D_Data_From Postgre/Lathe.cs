using UnityEngine;
using System.Collections;

public enum CTRotationType { Uniform, AccelerateUniformly }
public class Lathe : MonoBehaviour {

	// Use this for initialization
	void Start () {
        cir = GameObject.FindWithTag("Cube_Cir");
        cut = GameObject.FindWithTag("Cube_Cut");
        cut_ext = GameObject.FindWithTag("Cube_Cut_Ext");
        cut_ext2 = GameObject.FindWithTag("Cube_Cut_Ext2");
        dome1 = GameObject.FindWithTag("Cube_Dome1");
        dome1_dc = GameObject.FindWithTag("Cube_Dome1_DC");
        fte = GameObject.FindWithTag("Cube_Fte");

	}

    GameObject cir, cut, cut_ext, cut_ext2, dome1, dome1_dc, fte;

	// Update is called once per frame
	void Update () {
        if (!convert)
        {
            speed1 = (float)this.GetComponent<PostGre_SQL>().x_axis * 100;
            speed2 = (float)this.GetComponent<PostGre_SQL>().y_axis * 100;
        }
        cir.GetComponent<Axis_Rotate_Cir>().RotateTo(speed1 / 2, 2, 1.0f);
        cut.GetComponent<Axis_Rotate_Cut>().RotateTo(speed1, 2, 1.0f);
        cut_ext.GetComponent<Axis_Rotate_Cut_Ext>().RotateTo(speed2, 0, 1.0f);
        cut_ext2.GetComponent<Axis_Rotate_Cut_Ext2>().RotateTo(speed2, 2, 1.0f);
        dome1.GetComponent<Axis_Rotate_Dome1>().RotateTo(speed1, 0, 1.0f);
        dome1_dc.GetComponent<Axis_Rotate_Dome1_DC>().RotateTo(speed2, 2, 1.0f);
        fte.GetComponent<Axis_Rotate_Fte>().RotateTo(speed1, 2, 1.0f);
        print(speed1);
        print(speed2);
	}

    public string speed1_text = "Speed： ";
    public string speed1_input = "Input Speed  ";
    public string speed2_text = "Speed： ";
    public string speed2_input = "Input Speed  ";
    float speed1 = 100.0f, speed2 = 100.0f;
    bool convert = false;

    void OnGUI()
    {
        GUI.TextField(new Rect(0, 10, 70, 20), speed1_text, 10);
        GUI.TextField(new Rect(0, 30, 70, 40), speed2_text, 10);
        speed1_input = GUI.TextField(new Rect(70, 10, 120, 20), speed1_input, 10);
        speed2_input = GUI.TextField(new Rect(70, 30, 120, 40), speed2_input, 10);

        if (float.TryParse(speed1_input, out speed1) && float.TryParse(speed2_input, out speed2))
            convert = true;
        else convert = false;
        if (speed1 > 1000.0f) speed1 = 1000.0f;
        if (speed2 > 1000.0f) speed2 = 1000.0f;
    }

}
