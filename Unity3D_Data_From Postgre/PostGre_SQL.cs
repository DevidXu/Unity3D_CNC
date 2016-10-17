using UnityEngine;
using System.Collections;
using Npgsql;
using NpgsqlTypes;
using Mono.Security;
using System.Data;
using System.Data.SqlClient;

public class PostGre_SQL : MonoBehaviour {
    NpgsqlConnection conn;
    NpgsqlCommand command;
    NpgsqlDataReader dr1, dr2;
    public const int MAX_LEN = 10000;

    public double x_axis, y_axis;
    int k = 0, i = 0;
    int count_time = 0;
    string[] x = new string[MAX_LEN];
    string[] y = new string[MAX_LEN];
    string x_axis_sql = "select cx_axis from data_update";
    string y_axis_sql = "select cy_axis from data_update";

    void Start(){
        print(TestConnection());
    }

    void Update(){
        command = new NpgsqlCommand(x_axis_sql, conn);
        dr1 = command.ExecuteReader();
        k = 0;
        while (dr1.Read() && k < MAX_LEN) x[k++] = dr1[0].ToString();

        command = new NpgsqlCommand(y_axis_sql, conn);
        dr2 = command.ExecuteReader();
        k = 0;
        dr2 = command.ExecuteReader();
        while (dr2.Read() && k < MAX_LEN) y[k++] = dr2[0].ToString();

        count_time = count_time + (int)(Time.deltaTime * 1000);
        if (count_time > 1000)
        {
            i++;
            count_time = count_time % 1000;
        }
        
        if (i >= 1000) i -= 1000;
        i = i % MAX_LEN;

        double.TryParse(x[0], out x_axis);
        double.TryParse(y[0], out y_axis);
    }

    private string TestConnection()
    {
        string str = "Server=127.0.0.1;Port=5432;UserId=postgres;Password=siemens;Database=CNCXYZ;";
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

    ~PostGre_SQL() { conn.Close(); }
}
