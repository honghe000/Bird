using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class register : MonoBehaviour
{
    private MySqlConnection conn;
    protected string connectionString;
    public TMP_InputField uname;
    public TMP_InputField pwd;
    public TMP_InputField pwd1;
    public TextMeshProUGUI error;
    public TextMeshProUGUI error1;
    public TextMeshProUGUI error2;
    public TextMeshProUGUI error3;
    public TextMeshProUGUI success;
    public Button button;
    public int times = 0;
    // Start is called before the first frame update
    void Start()
    {
        error.gameObject.SetActive(false);
        error1.gameObject.SetActive(false);
        error2.gameObject.SetActive(false);
        error3.gameObject.SetActive(false);
        success.gameObject.SetActive(false);
        button.onClick.AddListener(Register);

    }

    // Update is called once per frame
    void Update()
    {

    }



    void Register()
    {
        string name = uname.text;
        string pwda = pwd.text;
        string pwdb = pwd1.text;
        if (times == 1)
        {
            error1.gameObject.SetActive (true);
            error2.gameObject.SetActive(false);
            error3.gameObject.SetActive(false);
            success.gameObject.SetActive(false);
            error.gameObject.SetActive(false);
            return;
        }
        if (name == "" || pwda == "" || pwdb == "")
        {
            error3.gameObject.SetActive(true);
            error1.gameObject.SetActive(false);
            error2.gameObject.SetActive(false);
            success.gameObject.SetActive(false);
            error.gameObject.SetActive(false);
            return;
        }
        if (pwda != pwdb)
        {
            error.gameObject.SetActive (true);
            error1.gameObject.SetActive(false);
            error2.gameObject.SetActive(false);
            error3.gameObject.SetActive(false);
            success.gameObject.SetActive(false);
            return;
        }
        else
        {
            InsertData(name, pwda,"100");
        }

    }



    public void InsertData(string username, string pwdk, string coin)
    {
        string connectionString = $"Server={ValueHolder.mysql_ip};Port={ValueHolder.mysql_port};Database={ValueHolder.mysql_database};Uid={ValueHolder.mysql_username};Pwd={ValueHolder.mysql_pwd};";
        conn = new MySqlConnection(connectionString);
        conn.Open();

        string sql = "INSERT INTO user (username, pwd, coin,carddata,cardgroup) VALUES (@username, @pwd, @coin,@carddata,@cardgroup)";
        using (MySqlCommand command = new MySqlCommand(sql, conn))
        {
            // 添加参数
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@pwd", pwdk);
            command.Parameters.AddWithValue("@coin", coin);
            command.Parameters.AddWithValue("@carddata", "1,2,2,2,3,2,4,2,5,2");
            command.Parameters.AddWithValue("@cardgroup", "{}");

            try
            {
                int rowsAffected = command.ExecuteNonQuery();
                Debug.Log("Rows Affected: " + rowsAffected);
                error2.gameObject.SetActive(false);
                error1.gameObject.SetActive(false);
                error.gameObject.SetActive(false);
                error3.gameObject.SetActive(false);
                success.gameObject.SetActive(true);
                uname.text = null;
                pwd.text = null;
                pwd1.text = null;
                times = 1;
        }
            catch (MySqlException)
            {
            error2.gameObject.SetActive(true);
            error1.gameObject.SetActive(false);
            error.gameObject.SetActive(false);
            error3.gameObject.SetActive(false);
            success.gameObject.SetActive(false);
            return;
        }
    }



    }
}
