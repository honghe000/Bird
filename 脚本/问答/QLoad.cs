using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QLoad : MonoBehaviour
{
    public MySqlConnection conn;
    public List<问答数据> questionList;

    public Image mainSystem;

    public TextMeshProUGUI questionText;

    public Image CorrectIMG;
    public Image WrongIMG;

    public int questionNum = 3;

    public List<TextMeshProUGUI> choice;

    public Button startButton;


    void Start()
    {

        startButton.onClick.AddListener(loadquestion);

    }

    void loadquestion()
    {
        if (ValueHolder.isRuning1 == 1)
        {
            return;
        }
        string connectionString = $"Server={ValueHolder.mysql_ip};Port={ValueHolder.mysql_port};Database={ValueHolder.mysql_database};Uid={ValueHolder.mysql_username};Pwd={ValueHolder.mysql_pwd};";
        conn = new MySqlConnection(connectionString);
        conn.Open();
        mainSystem.gameObject.SetActive(true);
        questionList = mainfunction.mysqlquestionList(conn, "questionsystem", questionNum);
        CorrectIMG.gameObject.SetActive(false);
        WrongIMG.gameObject.SetActive(false);
        questionText.text = questionList[0].question;
        string[] questionFour = questionList[0].option.Split("；");
        for (int i = 0; i < choice.Count; i++) {
            choice[i].text = questionFour[i];
            //Debug.Log(choice[i].text);
        }

        ValueHolder.questionList = questionList;
        ValueHolder.isRuning1 = 1;
    }

}
