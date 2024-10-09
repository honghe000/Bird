using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class loadcardgroup : MonoBehaviour
{
    MySqlConnection conn;
    public List<string> cardgroupname = new List<string>();

    public UnityEngine.UI.Button groupButtonlay;
    public UnityEngine.UI.Button library;

    public GameObject content;
    // Start is called before the first frame update
    void Start()
    {
        string connectionString = $"Server={ValueHolder.mysql_ip};Port={ValueHolder.mysql_port};Database={ValueHolder.mysql_database};Uid={ValueHolder.mysql_username};Pwd={ValueHolder.mysql_pwd};";
        conn = new MySqlConnection(connectionString);
        conn.Open();
        Dictionary<string, Dictionary<string, int>> cardjson = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(GetComponent<mainfunction>().readmysql(conn, "user", "cardgroup"));
        ValueHolder.cardgroupdata = cardjson;
        foreach (string s in cardjson.Keys)
        {
            cardgroupname.Add(s);
        }

        library.onClick.AddListener(() => instantinateButton(cardgroupname));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void instantinateButton(List<string> cardgroupname)
    {
        if (content.transform.childCount == 0)
        {
            foreach (string s in cardgroupname)
            {
                UnityEngine.UI.Button buttonOne = Instantiate(groupButtonlay, content.transform);
                buttonOne.GetComponentInChildren<TextMeshProUGUI>().text = s;
            }
        }
    }
}
