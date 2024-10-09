using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class updateCoin : MonoBehaviour
{
    public MySqlConnection conn;
    public TextMeshProUGUI cointext;
    // Start is called before the first frame update
    void Start()
    {
        string connectionString = $"Server={ValueHolder.mysql_ip};Port={ValueHolder.mysql_port};Database={ValueHolder.mysql_database};Uid={ValueHolder.mysql_username};Pwd={ValueHolder.mysql_pwd};";
        conn = new MySqlConnection(connectionString);
        conn.Open();
        mainfunction.updateCoin(conn, "user", "coin", ValueHolder.coin, cointext);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
