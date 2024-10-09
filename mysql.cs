using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;

public class mysql : MonoBehaviour
{
    private MySqlConnection conn;

    public void Awake()
    {
        string connectionString = $"Server={ValueHolder.mysql_ip};Port={ValueHolder.mysql_port};Database={ValueHolder.mysql_database};Uid={ValueHolder.mysql_username};Pwd={ValueHolder.mysql_pwd};";
        conn = new MySqlConnection(connectionString);
        conn.Open();

    }

    private void OnDestroy()
    {
        conn.Close();
    }


    public List<卡牌数据> Readmysql()
    {

        string sqlQuery = "SELECT * FROM card ORDER BY RAND() LIMIT 20"; 
        MySqlCommand mysqlcommand = new MySqlCommand(sqlQuery, conn);
        MySqlDataReader reader = mysqlcommand.ExecuteReader();
        List<卡牌数据> selectedCards = new List<卡牌数据>();
        while (reader.Read())
        {
            string name = reader.GetString("name");
            string level = reader.GetString("level");
            string skill = reader.GetString("skill");
            string type = reader.GetString("type");
            int id = reader.GetInt32("id");
            int maxHP = reader.GetInt32("HP");
            int maxATK = reader.GetInt32("ATK");
            卡牌数据 card = new 卡牌数据(name, skill, level, type,id,"0",maxHP,maxATK,maxHP, maxATK);
            selectedCards.Add(card);
        }
        reader.Close();

        return selectedCards;
    }
}
