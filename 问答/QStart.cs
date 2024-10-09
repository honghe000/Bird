using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QStart : MonoBehaviour
{
    public MySqlConnection conn;
    public List<问答数据> questionList;
    public Image mainSystem;
    public Image Finsh;

    public TextMeshProUGUI questionText;

    public Image CorrectIMG;
    public TextMeshProUGUI correctText;
    public Image WrongIMG;
    public TextMeshProUGUI wrongText;
    public TextMeshProUGUI rightText;

    public int questionNum = 3;

    public int nowQuestion = 0;

    public List<TextMeshProUGUI> choice;

    public Button nextButton;

    public TMP_Dropdown dropdown;

    public string rightAnwser;
    public int righttimes = 0;

    public TextMeshProUGUI RT;
    public TextMeshProUGUI WR;
    public TextMeshProUGUI CO;


    public TextMeshProUGUI coin;
    // Start is called before the first frame update
    void Start()
    {
        string connectionString = $"Server={ValueHolder.mysql_ip};Port={ValueHolder.mysql_port};Database={ValueHolder.mysql_database};Uid={ValueHolder.mysql_username};Pwd={ValueHolder.mysql_pwd};";
        conn = new MySqlConnection(connectionString);
        conn.Open();
        questionList = ValueHolder.questionList;
        rightAnwser = questionList[nowQuestion].anwser;
        nextButton.onClick.AddListener(startQu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void startQu()
    {
        if ( ValueHolder.isRuning == 0)
        {
            StartCoroutine(AnwserQ());
        }

    }
    IEnumerator AnwserQ()
    {
        ValueHolder.isRuning = 1;
        int selectedIndex = dropdown.value;
        string selectedOption = dropdown.options[selectedIndex].text;

        if (selectedOption == rightAnwser )
        {
            righttimes++;
            CorrectIMG.gameObject.SetActive(true);
            correctText.gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            nowQuestion++;
            if (nowQuestion == questionNum)
            {
                finsh();
                yield break;
            }
            ChangQ(nowQuestion);

            CorrectIMG.gameObject.SetActive(false);
            correctText.gameObject.SetActive(false);
            ValueHolder.isRuning = 0;


        }
        else
        {
            wrongText.gameObject.SetActive(true);
            WrongIMG.gameObject.SetActive(true);
            rightText.gameObject.SetActive(true );
            rightText.text = rightAnwser;
            yield return new WaitForSeconds(3);
            nowQuestion++;
            if (nowQuestion == questionNum)
            {
                finsh();
                yield break;
            }
            ChangQ(nowQuestion);

            wrongText.gameObject.SetActive(false);
            WrongIMG.gameObject.SetActive(false);
            rightText.gameObject.SetActive(false);
            ValueHolder.isRuning = 0;

        }

    }

    void ChangQ(int nowQuestion)
    {
        questionText.text = questionList[nowQuestion].question;
        string[] questionFour = questionList[nowQuestion].option.Split("；");
        for (int i = 0; i < choice.Count; i++)
        {
            choice[i].text = questionFour[i];
        }
        rightAnwser = questionList[nowQuestion].anwser;
    }

    void finsh()
    {
        mainSystem.gameObject.SetActive(false);
        Finsh.gameObject.SetActive(true);
        RT.text = righttimes.ToString();
        WR.text = (questionNum - righttimes).ToString();
        CO.text = righttimes.ToString();
        int coinnow = int.Parse(ValueHolder.coin) + righttimes;
        mainfunction.updateCoin(conn, "user", "coin",coinnow.ToString(), coin);
         
    }
}
