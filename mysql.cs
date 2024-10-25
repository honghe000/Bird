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
            int 黄 = reader.GetInt32("yellow");
            int 绿 = reader.GetInt32("green");
            int 蓝 = reader.GetInt32("blue");
            int 紫 = reader.GetInt32("purple");
            int 灵力消耗 = -1;
            if (黄 >= 0)
            {
                灵力消耗 = 黄;
            }else if (绿 >= 0)
            {
                灵力消耗 = 绿;
            }else if(蓝 >= 0)
            {
                灵力消耗 = 蓝;
            }else if (紫 >= 0)
            {
                灵力消耗 = 紫;
            }
            卡牌数据 card = new 卡牌数据(name, skill, level, type, id, "0", maxHP, maxATK, maxHP, maxATK, 黄, 绿, 蓝, 紫,灵力消耗);
            selectedCards.Add(card);
        }
        reader.Close();

        return selectedCards;
    }
}
