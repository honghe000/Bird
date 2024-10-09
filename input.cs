using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;
using MySql.Data.MySqlClient;

public class input : MonoBehaviour
{
    public TMP_InputField user;
    public TMP_InputField pwd;
    public TextMeshProUGUI error;
    public logmysql logmysql;

    private void Awake()
    {
        error.gameObject.SetActive(false);
    }
    private void Start()
    {
        UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(Click);
    }
    public void Click()
    {
        string uname = user.text;
        string password = pwd.text;
        List<string> b = logmysql.Searchuser(uname,password);
        if (b == null)
        {
            error.gameObject.SetActive(true);
            return;
        }
        else
        {
            ValueHolder.username = b[0];
            ValueHolder.coin = b[1];
            ValueHolder.cardData = GetComponent<mainfunction>().SaveCard(b[2]);

            SceneManager.LoadScene("mainpage");

        }





    }
}
