using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using UnityEngine;

public class globalLoad : MonoBehaviour
{
    static MySqlConnection conn;

    [RuntimeInitializeOnLoadMethod]
    static void Global()
    {
        //mysql
        string connectionString = $"Server={ValueHolder.mysql_ip};Port={ValueHolder.mysql_port};Database={ValueHolder.mysql_database};Uid={ValueHolder.mysql_username};Pwd={ValueHolder.mysql_pwd}";
        conn = new MySqlConnection(connectionString);
        conn.Open();
        readBase(conn);
        readUserCard(conn);

        //socket
        //ValueHolder.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //ValueHolder.socket.Connect(IPAddress.Parse(ValueHolder.socket_ip), ValueHolder.socket_port);

    }

    static void readBase(MySqlConnection conn)
    {
        卡牌数据 card = null;
        string sqlQuery = $"SELECT * FROM card;";
        MySqlCommand mysqlcommand = new MySqlCommand(sqlQuery, conn);
        //Debug.Log(sqlQuery);
        MySqlDataReader reader = mysqlcommand.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32("id");
            string name = reader.GetString("name");
            string level = reader.GetString("level");
            string skill = reader.GetString("skill");
            string type = reader.GetString("type");
            int maxHP = reader.GetInt32("HP");
            int maxATK = reader.GetInt32("ATK");
            int 黄 = reader.GetInt32("yellow");
            int 绿 = reader.GetInt32("green");
            int 蓝 = reader.GetInt32("blue");
            int 紫 = reader.GetInt32("purple");
            card = new 卡牌数据(name, skill, level, type, id, "0", maxHP, maxATK, maxHP, maxATK,黄,绿,蓝,紫);
            ValueHolder.gloabCaedData[id] = card;
        }
        reader.Close();
    }

    static void readUserCard(MySqlConnection conn)
    {

        string sqlQuery = "SELECT * FROM user WHERE username=@username;";
        MySqlCommand mysqlcommand = new MySqlCommand(sqlQuery, conn);
        mysqlcommand.Parameters.AddWithValue("@username", ValueHolder.username);

        //Debug.Log(sqlQuery);
        MySqlDataReader reader = mysqlcommand.ExecuteReader();
        while (reader.Read())
        {
            string name = reader.GetString("carddata");
            string groupdata = reader.GetString("cardgroup");
            string coin = reader.GetString("coin");
            Dictionary<string, Dictionary<string, int>> dictinfo = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(groupdata);
            ValueHolder.cardgroupdata = dictinfo;
            ValueHolder.coin = coin;
        }
        reader.Close();
    }





}
