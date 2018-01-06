using UnityEngine;
using System.Collections;
using Npgsql;
using NpgsqlTypes;
using Mono.Security;

public class Miller_Data_Trans : MonoBehaviour {

	NpgsqlConnection conn;
    NpgsqlCommand command;
    NpgsqlDataReader dr;
    private string TestConnection(string address)
    {
        string add = "";
        if (address == "") add = "127.0.0.1";
        else add = address;
        string str = "Server=" + add + ";Port=5432;UserId=postgres;Password=siemens;Database=CNCXYZ;";

        string strMessage = string.Empty;
        try
        {
            conn = new NpgsqlConnection(str);
            conn.Open();
            strMessage = "Success";
        }
        catch
        {
            strMessage = "Failure";
        }
        return strMessage;
    }

    public double x_axis, y_axis, z_axis;

    void Start(){
        testconnection = false;
		reset = false;
		running = false;
		connect = true;
        id = "127.0.0.1";
        x_axis = y_axis = z_axis = 0;
    }

    public const int MAX_LEN = 50000;
    string[] data = new string[MAX_LEN];  // x_axis
    string[] data1 = new string[MAX_LEN];  //y_axis
    string[] data2 = new string[MAX_LEN];  //z_axis
	string[] data3 = new string[MAX_LEN];  //xaxisscale
	string[] data4 = new string[MAX_LEN];  //yaxisscale
	string[] data5 = new string[MAX_LEN];  //zaxisscale
	string[] data6 = new string[MAX_LEN];  //xaxisvelocity
	string[] data7 = new string[MAX_LEN];  //yaxisvelocity
	string[] data8 = new string[MAX_LEN];  //zaxisvelocity
	string[] data9 = new string[MAX_LEN];  //feedrate
	string[] data10 = new string[MAX_LEN];  //rpm
	string[] data11 = new string[MAX_LEN]; //unit
	string[] data12 = new string[MAX_LEN]; //total hour

	public bool connect = true, running = false, reset = false;
    void Update(){
		if (running) {
			if (connect)
				GetConnectData ();
			else
				GetHistoryData ();
			if (reset)
				x_axis = y_axis = z_axis = 0.0f;
		}
    }

    string query_sql = "select x_axis,y_axis,z_axis,xs,ys,zs,xv,yv,zv,feedrate,rpm,unit,ct from data_trans";
    private void GetConnectData()
    {
        if (!testconnection)
            if (TestConnection(id) == "Success")
            {
                testconnection = true;
                command = new NpgsqlCommand(query_sql, conn);
            }
        int k = 0;
        dr = command.ExecuteReader();
        while (dr.Read() && k < MAX_LEN)
        {
            data[0] = dr["x_axis"].ToString();
            data[1] = dr["y_axis"].ToString();
            data[2] = dr["z_axis"].ToString();
			data[3] = dr ["xs"].ToString ();
			data[4] = dr ["ys"].ToString ();
			data[5] = dr ["zs"].ToString ();
			data[6] = dr ["xv"].ToString ();
			data[7] = dr ["yv"].ToString ();
			data[8] = dr ["zv"].ToString ();
			data[9] = dr ["feedrate"].ToString ();
			data[10] = dr ["rpm"].ToString ();
			data[11] = dr ["unit"].ToString ();
			data[12] = dr ["ct"].ToString ();
            k++;
        }

        double.TryParse(data[0], out x_axis);
        double.TryParse(data[1], out y_axis);
        double.TryParse(data[2], out z_axis);
    }

	string his_query_sql = "select x_axis,y_axis,z_axis,xs,ys,zs,xv,yv,zv,feedrate,rpm,unit,ct from linear_position";
    bool HaveHistoryData = false;
    int linenum = 0, linesum = 0;
    float time;
    private void GetHistoryData()
    {
        if (!HaveHistoryData)
        {
            if (TestConnection(id) == "Success")
            {
                HaveHistoryData = true;
                command = new NpgsqlCommand(his_query_sql, conn);
                dr = command.ExecuteReader();
            }

            linesum = 0;
            while (dr.Read() && linesum < MAX_LEN)
            {
                data[linesum] = dr["x_axis"].ToString();
                data1[linesum] = dr["y_axis"].ToString();
                data2[linesum] = dr["z_axis"].ToString();
				data3[linesum] = dr ["xs"].ToString ();
				data4[linesum] = dr ["ys"].ToString ();
				data5[linesum] = dr ["zs"].ToString ();
				data6[linesum] = dr ["xv"].ToString ();
				data7[linesum] = dr ["yv"].ToString ();
				data8[linesum] = dr ["zv"].ToString ();
				data9[linesum] = dr ["feedrate"].ToString ();
				data10[linesum] = dr ["rpm"].ToString ();
				data11[linesum] = dr ["unit"].ToString ();
				data12[linesum] = dr ["ct"].ToString ();
                linesum++;
            }
        }

        time += Time.deltaTime;
        if (time > 1.0f)
        {
            time -= 1.0f;
            linenum = (linenum + 1) % linesum;
        }

        double.TryParse(data[linenum], out x_axis);
        double.TryParse(data1[linenum], out y_axis);
        double.TryParse(data2[linenum], out z_axis);
    }

	// Some default value
    string id_text = "Target IP";
    public string id;
    bool testconnection;
	bool showTable = false;

    void OnGUI()
    {
        //GUI.TextField(NewPos(0.01f, 0.1f, 0.07f, 0.05f), id_text);
        //id = GUI.TextField(NewPos(0.1f, 0.1f, 0.2f, 0.05f), id, 20);
		/*
		if (GUI.Button (NewPos (0.88f, 0.1f, 0.08f, 0.05f), "Running")) {
			running = true;
			reset = false;
		}
		if (GUI.Button (NewPos (0.88f, 0.2f, 0.08f, 0.05f), "Pause"))
			running = false;
		if (GUI.Button (NewPos (0.88f, 0.3f, 0.08f, 0.05f), "Reset")) {
			running = true;
			reset = true;
		}

		if (GUI.Button (NewPos (0.88f, 0.4f, 0.08f, 0.05f), "Data Table"))
			showTable = !showTable;
		if (showTable) {
			GUI.TextField (NewPos (0.0f, 0.15f, 0.1f, 0.05f), "X_AXIS(cm)");
			GUI.TextField (NewPos (0.1f, 0.15f, 0.1f, 0.05f), x_axis.ToString());
			GUI.TextField (NewPos (0.0f, 0.20f, 0.1f, 0.05f), "Y_AXIS(cm)");
			GUI.TextField (NewPos (0.1f, 0.20f, 0.1f, 0.05f), y_axis.ToString());
			GUI.TextField (NewPos (0.0f, 0.25f, 0.1f, 0.05f), "Z_AXIS(cm)");
			GUI.TextField (NewPos (0.1f, 0.25f, 0.1f, 0.05f), z_axis.ToString());
			if (running) {
				if (connect) {
					GUI.TextField (NewPos (0.0f, 0.30f, 0.1f, 0.05f), "XAXIS_V(cm/s)");
					GUI.TextField (NewPos (0.1f, 0.30f, 0.1f, 0.05f), data [3]);
					GUI.TextField (NewPos (0.0f, 0.35f, 0.1f, 0.05f), "YAXIS_V(cm/s)");
					GUI.TextField (NewPos (0.1f, 0.35f, 0.1f, 0.05f), data [4]);
					GUI.TextField (NewPos (0.0f, 0.40f, 0.1f, 0.05f), "ZAXIS_V(cm/s)");
					GUI.TextField (NewPos (0.1f, 0.40f, 0.1f, 0.05f), data [5]);
					GUI.TextField (NewPos (0.0f, 0.45f, 0.1f, 0.05f), "XAXIS_SCALE(cm/s)");
					GUI.TextField (NewPos (0.1f, 0.45f, 0.1f, 0.05f), data [6]);
					GUI.TextField (NewPos (0.0f, 0.50f, 0.1f, 0.05f), "YAXIS_SCALE(cm/s)");
					GUI.TextField (NewPos (0.1f, 0.50f, 0.1f, 0.05f), data [7]);
					GUI.TextField (NewPos (0.0f, 0.55f, 0.1f, 0.05f), "ZAXIS_SCALE(cm/s)");
					GUI.TextField (NewPos (0.1f, 0.55f, 0.1f, 0.05f), data [8]);
					GUI.TextField (NewPos (0.0f, 0.60f, 0.1f, 0.05f), "FeedRate");
					GUI.TextField (NewPos (0.1f, 0.60f, 0.1f, 0.05f), data [9]);
					GUI.TextField (NewPos (0.0f, 0.65f, 0.1f, 0.05f), "RPM");
					GUI.TextField (NewPos (0.1f, 0.65f, 0.1f, 0.05f), data [10]);
					GUI.TextField (NewPos (0.0f, 0.70f, 0.1f, 0.05f), "UNIT(TypeID)");
					GUI.TextField (NewPos (0.1f, 0.70f, 0.1f, 0.05f), data [11]);
					GUI.TextField (NewPos (0.0f, 0.75f, 0.1f, 0.05f), "Total Hour(h)");
					GUI.TextField (NewPos (0.1f, 0.75f, 0.1f, 0.05f), data [12]);
				} else {
					GUI.TextField (NewPos (0.0f, 0.30f, 0.1f, 0.05f), "XAXIS_V(cm/s)");
					GUI.TextField (NewPos (0.1f, 0.30f, 0.1f, 0.05f), data3 [linenum]);
					GUI.TextField (NewPos (0.0f, 0.35f, 0.1f, 0.05f), "YAXIS_V(cm/s)");
					GUI.TextField (NewPos (0.1f, 0.35f, 0.1f, 0.05f), data4 [linenum]);
					GUI.TextField (NewPos (0.0f, 0.40f, 0.1f, 0.05f), "ZAXIS_V(cm/s)");
					GUI.TextField (NewPos (0.1f, 0.40f, 0.1f, 0.05f), data5 [linenum]);
					GUI.TextField (NewPos (0.0f, 0.45f, 0.1f, 0.05f), "XAXIS_SCALE(cm/s)");
					GUI.TextField (NewPos (0.1f, 0.45f, 0.1f, 0.05f), data6 [linenum]);
					GUI.TextField (NewPos (0.0f, 0.50f, 0.1f, 0.05f), "YAXIS_SCALE(cm/s)");
					GUI.TextField (NewPos (0.1f, 0.50f, 0.1f, 0.05f), data7 [linenum]);
					GUI.TextField (NewPos (0.0f, 0.55f, 0.1f, 0.05f), "ZAXIS_SCALE(cm/s)");
					GUI.TextField (NewPos (0.1f, 0.55f, 0.1f, 0.05f), data8 [linenum]);
					GUI.TextField (NewPos (0.0f, 0.60f, 0.1f, 0.05f), "FeedRate");
					GUI.TextField (NewPos (0.1f, 0.60f, 0.1f, 0.05f), data9 [linenum]);
					GUI.TextField (NewPos (0.0f, 0.65f, 0.1f, 0.05f), "RPM");
					GUI.TextField (NewPos (0.1f, 0.65f, 0.1f, 0.05f), data10 [linenum]);
					GUI.TextField (NewPos (0.0f, 0.70f, 0.1f, 0.05f), "UNIT(TypeID)");
					GUI.TextField (NewPos (0.1f, 0.70f, 0.1f, 0.05f), data11 [linenum]);
					GUI.TextField (NewPos (0.0f, 0.75f, 0.1f, 0.05f), "Total Hour(h)");
					GUI.TextField (NewPos (0.1f, 0.75f, 0.1f, 0.05f), data12 [linenum]);
				}
			}
		}
		*/
    }

	Rect NewPos(float xbegin, float ybegin, float xlen, float ylen)
	{
		return new Rect(xbegin * Screen.width, ybegin * Screen.height, xlen * Screen.width, ylen * Screen.height);
	}

    ~Miller_Data_Trans() { conn.Close(); }
}