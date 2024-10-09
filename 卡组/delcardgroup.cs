using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class delcardgroup : MonoBehaviour
{
    public MySqlConnection conn;
    public Button delButton;
    // Start is called before the first frame update
    void Start()
    {
        Loadresouce();
        delButton.onClick.AddListener(del);
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

    public void del()
    {
        GetComponent<mainfunction>().deljsonmysql(conn, "user", "cardgroup", ValueHolder.cardgroupName);
        SceneManager.LoadScene("mainpage");
    }
}
