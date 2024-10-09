using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class rename : MonoBehaviour
{
    public MySqlConnection conn;
    public Button renameButton;
    public TMP_InputField inputField;
    public TextMeshProUGUI nameNow;
    public Image imgback;
    // Start is called before the first frame update
    void Start()
    {
        Loadresouce();
        renameButton.onClick.AddListener(startRename);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Loadresouce()
    {
        string connectionString = $"Server={ValueHolder.mysql_ip};Port={ValueHolder.mysql_port};Database={ValueHolder.mysql_database};Uid={ValueHolder.mysql_username};Pwd={ValueHolder.mysql_pwd};";
        conn = new MySqlConnection(connectionString);
        conn.Open();
    }
    public void startRename()
    {
        GetComponent<mainfunction>().deljsonmysql(conn, "user", "cardgroup", ValueHolder.cardgroupName);
        ValueHolder.cardgroupName = inputField.text;
        nameNow.text = ValueHolder.cardgroupName;



        imgback.gameObject.SetActive(false);

    }
}
