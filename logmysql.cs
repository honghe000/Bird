using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;

public class logmysql : MonoBehaviour
{
    public MySqlConnection conn;
    public List<string> datalist = new List<string>();

    private void Awake()
    {
        string connectionString = $"Server={ValueHolder.mysql_ip};Port={ValueHolder.mysql_port};Database={ValueHolder.mysql_database};Uid={ValueHolder.mysql_username};Pwd={ValueHolder.mysql_pwd};";
        conn = new MySqlConnection(connectionString);
        conn.Open();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDestroy()
    {
        conn.Close();
    }

    public List<string> Searchuser(string username , string pwd)
    {
        string a = $"select * from user where username = '{username}' && pwd = '{pwd}';";
        //Debug.Log(a);
        MySqlCommand mysqlcommand = new MySqlCommand(a, conn);
        MySqlDataReader reader = mysqlcommand.ExecuteReader();
        while (reader.Read())
        {

            string coin2 = reader.GetString("coin");
            string cardDate = reader.GetString("carddata");
            //Debug.Log(cardDate);
            datalist.Add(username);
            datalist.Add(coin2);
            datalist.Add(cardDate);
            reader.Close();
            return datalist;
        }
        reader.Close ();
        return null;



    }
}
