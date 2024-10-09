using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class save : MonoBehaviour
{
    public GameObject error1;//S+、S、A卡只能上场一张
    public GameObject error2;//CD卡至少需要5张
    public GameObject error3;//S卡数量小于等于5(S+记两张)
    public GameObject error4;//全部卡数应为40张
    public GameObject error5;//B,C,D同名卡只能上场1张
    public List<GameObject> errorList = new List<GameObject>();


    public GameObject SS;
    public GameObject S;
    public GameObject A;
    public GameObject B;
    public GameObject CD;
    public List<GameObject> cardGroupList = new List<GameObject>();


    public int numS = 0;    
    public int numCD = 0;
    public int numall = 0;


    public Dictionary<string,int> allCardList = new Dictionary<string,int>();

    public Button button;

    public MySqlConnection conn;



    void Start()
    {

        //链接数据库
        string connectionString = $"Server={ValueHolder.mysql_ip};Port={ValueHolder.mysql_port};Database={ValueHolder.mysql_database};Uid={ValueHolder.mysql_username};Pwd={ValueHolder.mysql_pwd};";
        conn = new MySqlConnection(connectionString);
        conn.Open();


        errorList.AddRange(new List<GameObject> {error1,error2,error3,error4,error5});

        //初始进入关闭报错显示
        foreach (GameObject error in errorList)
        {
            error.gameObject.SetActive(false);
        }

        cardGroupList.AddRange(new List<GameObject> { SS, S, A, B, CD });
        button.onClick.AddListener(savecard);
    }

    void Update()
    {
        
    }


    public void savecard()
    {
        numCD = 0;
        numS = 0;
        numall = 0;
        allCardList.Clear();
        //遍历所有等级的卡组，存到总字典中
        foreach (GameObject cardGroup in cardGroupList)
        {
            Dictionary<string,int> carddictOne =  cardToDict(cardGroup);
            if (carddictOne == null)
            {
                return;
            }
            allCardList = allCardList.Concat(carddictOne)
                          .GroupBy(pair => pair.Key)
                          .ToDictionary(group => group.Key, group => group.First().Value);
        }
        //if (numCD < 1)
        //{
            
        //    GetComponent<mainfunction>().errorShow(errorList, error2);
        //    return;
        //}else if(numS > 5)
        //{
            
        //    GetComponent<mainfunction>().errorShow(errorList, error3);
        //    return;
        //}else if (numall != 5)
        //{
            
        //    GetComponent<mainfunction>().errorShow(errorList, error4);
        //    return;
        //}
        Dictionary<string, Dictionary<string, int>> savedict = new Dictionary<string, Dictionary<string, int>>
        {
            { ValueHolder.cardgroupName, allCardList }
        };
        savejsondata(conn,JsonConvert.SerializeObject(savedict));
        ValueHolder.cardDataexsist = new Dictionary<int, int>();
        SceneManager.LoadScene("mainpage");

    }


    //将卡组下的所有子卡牌的信息储存进一个字典，以字典的形式int:int
    public Dictionary<string,int> cardToDict(GameObject cardGroup)
    {
        Transform cardGrouptrans = cardGroup.transform;
        Dictionary<string, int> carddictone = new Dictionary<string, int>(); 

        //获取子物体信息并存进字典
        for (int i = 0; i < cardGrouptrans.childCount; i++)
        {
            numall++;
            if (cardGrouptrans.name == "CD")
            {
                numCD++;
            }else if(cardGrouptrans.name == "S+")
            {
                numS += 2;
            }else if(cardGrouptrans.name == "S")
            {
                numS++;
            }
            GameObject childobject = cardGrouptrans.GetChild(i).gameObject;
            int id = childobject.GetComponent<smallCard>().卡牌数据.id;
            string level = childobject.GetComponent<smallCard>().卡牌数据.等级;
            if (carddictone.ContainsKey(id.ToString()))
            {
                //if (new List<string> { "SSS","SS","S","A" }.Contains(level)){
                //    GetComponent<mainfunction>().errorShow(errorList, error1);
                //    return null;
                //}else if (carddictone[id.ToString()] == 2)
                //{
                //    GetComponent<mainfunction>().errorShow(errorList, error5);
                //    return null;
                //}
                carddictone[id.ToString()]++;

            }
            else
            {
                carddictone[id.ToString()] = 1;
            }
        }
        //Debug.Log(cardGroup.name +carddictone.Count);
        return carddictone;
    }


    public void savejsondata(MySqlConnection conn,string jsondata)
    {
        string jsondataall = GetComponent<mainfunction>().readmysql(conn,"user","cardgroup");
        Dictionary<string, Dictionary<string, int>> jsonDataALL = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(jsondataall);
        Dictionary<string, Dictionary<string, int>> jsonData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(jsondata);
        foreach (var pair in jsonData)
        {
            jsonDataALL[pair.Key] = pair.Value;
        }
        GetComponent<mainfunction>().updatemysql(conn, "user", "cardgroup", jsonDataALL);
    }
}