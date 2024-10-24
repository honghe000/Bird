using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


/*
Dictionary<int, int> SaveCard(string co)                        string转dictionary    


卡牌数据 ReadmysqlOne(MySqlConnection conn, int id)                id查其他卡牌数据
 
 
void errorShow(List<GameObject> errorList,GameObject errorShow)     报错显示
 

void DestroyAllChildren(GameObject parentObject)        摧毁子物体
 
 */




public class mainfunction : MonoBehaviour
{
    // Start is called before the first frame update
    public Dictionary<int, int> SaveCard(string co)
    {
        Dictionary<string,int> counts = JsonConvert.DeserializeObject<Dictionary<string, int>>(co);
        Dictionary<int, int> cardCounts = new Dictionary<int, int>();
        foreach (KeyValuePair<string, int> kvp in counts)
        {
            cardCounts[int.Parse(kvp.Key)] = kvp.Value;
        }
        return cardCounts;
    }

    public static void DestroyAllChildren(GameObject parentObject)
    {
        int childCount = parentObject.transform.childCount;
        GameObject[] children = new GameObject[childCount];

        // 将指定父物体的所有子物体存储到数组中
        for (int i = 0; i < childCount; i++)
        {
            children[i] = parentObject.transform.GetChild(i).gameObject;
        }

        // 批量销毁指定父物体的所有子物体
        for (int i = 0; i < childCount; i++)
        {
            Destroy(children[i]);
        }
    }

    public static void DestroyChildAtIndex(GameObject parent, int index)
    {
        // 检查父物体是否存在，并且子物体序号有效
        if (parent != null && index >= 0 && index < parent.transform.childCount)
        {
            // 获取指定序号的子物体
            Transform child = parent.transform.GetChild(index);

            // 销毁子物体
            Destroy(child.gameObject);
        }
        else
        {
            Debug.LogWarning("无效的父物体或子物体索引！");
        }
    }


    public void errorShow(List<GameObject> errorList,GameObject errorShow)
    {
        foreach (GameObject obj in errorList)
        {
            obj.gameObject.SetActive(false);
        }
        errorShow.gameObject.SetActive(true);
    }


    public string dicToString(Dictionary<int,int> dictionary)
    {
        string dataString = "";
        foreach(KeyValuePair<int, int> kvp in dictionary)
        {
            dataString = dataString +  kvp.Key.ToString() + "," + kvp.Value.ToString() + ",";
        }
        return dataString.TrimEnd(",");
    }

    public void updatemysql<T>(MySqlConnection conn,string tableName,string columnName,T data)
    {
        string changedData = null;
        if (data is int)
        {
            changedData = data.ToString();
        }else if (data is string)
        {
            changedData= data.ToString();
        }
        else
        {
            changedData = JsonConvert.SerializeObject(data);
        }
        string cmd = $"UPDATE {tableName} SET {columnName} = @data WHERE username = @username;";

        MySqlCommand mySqlCommand = new MySqlCommand(cmd,conn);
        mySqlCommand.Parameters.AddWithValue("@username", ValueHolder.username);

        mySqlCommand.Parameters.AddWithValue("@data", changedData);


        mySqlCommand.ExecuteNonQuery();

    }

    public string readmysql(MySqlConnection conn ,string tablename,string columnname)
    {
        string cardgroup = null;
        string cmd = $"select {columnname} from {tablename} WHERE username = @username";
        MySqlCommand mySqlCommand = new MySqlCommand(cmd, conn);
        mySqlCommand.Parameters.AddWithValue("@username", ValueHolder.username);
        MySqlDataReader reader = mySqlCommand.ExecuteReader();
        while (reader.Read())
        {
            cardgroup = reader.GetString("cardgroup");
        }

        reader.Close();
        return cardgroup;

    }

    public void deljsonmysql(MySqlConnection conn, string tableName, string columnName,string keyword)
    {
        string jsonstring = readmysql(conn, tableName, columnName);
        Dictionary<string, Dictionary<string, int>> jsondata = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(jsonstring);
        jsondata.Remove(keyword);
        updatemysql(conn, tableName, columnName, jsondata);
    }

    public int Loadimages(GameObject card,int index,int id)
    {
        // 从Resources文件夹中加载图片
        Texture2D texture = Resources.Load<Texture2D>("card/" + id.ToString());
        // 将纹理转换为精灵
        if (texture != null)
        {
            card.GetComponentsInChildren<Image>()[index].sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public static List<问答数据> mysqlquestionList(MySqlConnection conn, string tableName, int num)
    {
        // Construct the SQL query
        string query;
        if (num == -1)
        {
            query = $"SELECT id, question, `option`, anwser FROM {tableName} ORDER BY RAND()";
        }
        else
        {
            query = $"SELECT id, question, `option`, anwser FROM {tableName} ORDER BY RAND() LIMIT @num";
        }

        // Create a list to hold the results
        List<问答数据> resultList = new List<问答数据>();

        // Create a MySqlCommand object
        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        {
            // Add the parameter to the command if num is not -1
            if (num != -1)
            {
                cmd.Parameters.AddWithValue("@num", num);
            }

            // Open the connection if it's not already open
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }

            // Execute the command and read the data
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Read each field from the current record
                    int id = reader.GetInt32("id");
                    string question = reader.GetString("question");
                    string option = reader.GetString("option");
                    string anwser = reader.GetString("anwser");

                    // Create a new 问答数据 object and add it to the list
                    问答数据 data = new 问答数据(question, option, anwser, id);
                    resultList.Add(data);
                }
            }
        }

        return resultList;
    }

    public static void updateCoin(MySqlConnection conn, string tableName, string columnName, string coin,TextMeshProUGUI cointext)
    {
        ValueHolder.coin = coin;


        string cmd = $"UPDATE {tableName} SET {columnName} = @data WHERE username = @username;";
        MySqlCommand mySqlCommand = new MySqlCommand(cmd, conn);
        mySqlCommand.Parameters.AddWithValue("@username", ValueHolder.username);
        mySqlCommand.Parameters.AddWithValue("@data", coin);
        mySqlCommand.ExecuteNonQuery();

        cointext.text = coin;
    }

    public static List<int> SumomonHandCardID(int num)
    {
        // 检查是否有足够的卡牌
        int availableCards = ValueHolder.random_card.Count;

        // 如果请求的数量超过剩余的卡牌数，取所有可用的卡牌
        if (num > availableCards)
        {
            num = availableCards;
        }

        // 提取指定数量的卡牌
        List<int> result = ValueHolder.random_card.Take(num).ToList();

        // 更新剩余卡牌列表
        ValueHolder.random_card = ValueHolder.random_card.Skip(num).ToList();

        return result;
    }

    public static int NametoID(string name)
    {
        for (int i = 0; i < ValueHolder.gloabCaedData.Count; i++)
        {
            if (ValueHolder.gloabCaedData[i].名字 == name)
            {
                return i;
            }
        }
        return 0;
    }




    public static void SendMessages()
    {
        string message = ValueHolder.sendQueue.Dequeue();
        NetworkStream stream = ValueHolder.client.GetStream();
        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    public static void ChangeSendMessage<T>(string propertyName,T values)
    {
        Message mes = JsonUtility.FromJson<Message>(ValueHolder.SendMessages);
        if (propertyName == "SendUser")
        {
            mes.SendUser = ValueHolder.username;
        }else if(propertyName == "Action")
        {
            mes.Action = (int)(object)values;
        }else if(propertyName == "RoomName")
        {
            mes.RoomName = (string)(object)values;
        }
        else if (propertyName == "is_used")
        {
            mes.is_used = (int)(object)values;
        }
        else if (propertyName == "cardgroup")
        {
            mes.cardgroup = (string)(object)values;
        }
        else if (propertyName == "ReceiveUser")
        {
            mes.ReceiveUser = (string)(object)values;
        }else if (propertyName == "cardID")
        {
            mes.cardID = (int)(object)values;
        }else if (propertyName == "start_index")
        {
            mes.start_index = (int)(object)values;
        }else if (propertyName == "end_index")
        {
            mes.end_index = (int)(object)values;
        }else if (propertyName == "turn")
        {
            mes.turn = (float)(object)values;
        }else if (propertyName == "uid")
        {
            mes.uid = (string)(object)values;
        }else if (propertyName == "uid1")
        {
            mes.uid1 = (string)(object)values;
        }
        else if (propertyName == "effect")
        {
            mes.effect = (int)(object)values;
        }else if (propertyName == "num")
        {
            mes.num = (int)(object)values;
        }
        ValueHolder.SendMessages = JsonUtility.ToJson(mes);
    }

    public static void ChangeRecieveMessage<T>(string propertyName, T values)
    {
        Message mes = JsonUtility.FromJson<Message>(ValueHolder.ResieveMessages);
        if (propertyName == "SendUser")
        {
            mes.SendUser = ValueHolder.username;
        }
        else if (propertyName == "Action")
        {
            mes.Action = (int)(object)values;
        }
        else if (propertyName == "RoomName")
        {
            mes.RoomName = (string)(object)values;
        }
        else if (propertyName == "is_used")
        {
            mes.is_used = (int)(object)values;
        }
        else if (propertyName == "cardgroup")
        {
            mes.cardgroup = (string)(object)values;
        }
        else if (propertyName == "ReceiveUser")
        {
            mes.ReceiveUser = (string)(object)values;
        }else if (propertyName == "cardID")
        {
            mes.cardID = (int)(object)values;
        }
        else if (propertyName == "start_index")
        {
            mes.start_index = (int)(object)values;
        }
        else if (propertyName == "end_index")
        {
            mes.end_index = (int)(object)values;
        }
        else if (propertyName == "turn")
        {
            mes.turn = (float)(object)values;
        }
        else if (propertyName == "uid")
        {
            mes.uid = (string)(object)values;
        }
        else if (propertyName == "uid1")
        {
            mes.uid1 = (string)(object)values;
        }
        else if (propertyName == "effect")
        {
            mes.effect = (int)(object)values;
        }
        else if (propertyName == "num")
        {
            mes.num = (int)(object)values;
        }
        ValueHolder.ResieveMessages = JsonUtility.ToJson(mes);
    }

    public static Dictionary<int,GameObject> 消灭_destroy(List<int> indexs)
    {
        Dictionary<int, GameObject> kill_dict = new Dictionary<int, GameObject>();
        foreach (KeyValuePair<string, GameObject> kvp in ValueHolder.棋盘)
        {
            if (indexs.Contains(int.Parse(kvp.Key)) && kvp.Value.transform.childCount != 0){
                GameObject cardTodel = kvp.Value.transform.GetChild(0).gameObject;
                if (cardTodel.GetComponent<MoveController>().cardType == 1 && cardTodel.GetComponent<MoveController>().消灭免疫 == 0)
                {
                    kill_dict[int.Parse(kvp.Key)] = cardTodel;
               }
            }
        }

        return kill_dict;
    }

    public static void 缩放调整(GameObject gameObject)
    {
        Vector3 scale = gameObject.transform.localScale;
        scale.x = scale.x * ValueHolder.scale_x;
        scale.y = scale.y * ValueHolder.scale_y;
        gameObject.transform.localScale = scale;
    }
    public static int Convertposition(int position)
    {
        // 将位置编号转换为行和列坐标
        int row = (position - 1) / 5;
        int column = (position - 1) % 5;

        // 计算对手看到的行和列坐标
        int opponentRow = 5 - 1 - row;
        int opponentColumn = 5 - 1 - column;

        // 将对手看到的行和列坐标转换回位置编号
        int opponentPosition = (opponentRow * 5 + opponentColumn) + 1;

        return opponentPosition;
    }
    public static void change_card_color(GameObject card, string color)
    {
        foreach (Transform child in card.transform)
        {
            TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                if (color == "red")
                {
                    text.color = Color.red;
                }
                if (color == "blue")
                {
                    text.color = Color.blue;
                }
            }
        }
    }

    public static void 位置禁用(string type,string cardname,List<int> indexs)
    {
        if (type == "角色")
        {
            if (!ValueHolder.人物禁止放置位置.ContainsKey(cardname))
            {
                ValueHolder.人物禁止放置位置.Add(cardname, new List<int>());
            }
            ValueHolder.人物禁止放置位置[cardname].AddRange(indexs);
        }
        if (type == "建筑")
        {
            if (!ValueHolder.建筑禁止放置位置.ContainsKey(cardname))
            {
                ValueHolder.建筑禁止放置位置.Add(cardname, new List<int>());
            }
            ValueHolder.建筑禁止放置位置[cardname].AddRange(indexs);
        }
    }

    public static void 位置禁用取消(string type, string cardname)
    {
        if (type == "角色")
        {
            if (ValueHolder.人物禁止放置位置.ContainsKey(cardname))
            {
                ValueHolder.人物禁止放置位置.Remove(cardname);
            }
        }
        if (type == "建筑")
        {
            if (ValueHolder.人物禁止放置位置.ContainsKey(cardname))
            {
                ValueHolder.人物禁止放置位置.Remove(cardname);
            }
        }
    }

    public static int ConvertPosition(int position)
    {
        // 将位置编号转换为行和列坐标
        int row = (position - 1) / 5;
        int column = (position - 1) % 5;

        // 计算对手看到的行和列坐标
        int opponentRow = 5 - 1 - row;
        int opponentColumn = 5 - 1 - column;

        // 将对手看到的行和列坐标转换回位置编号
        int opponentPosition = (opponentRow * 5 + opponentColumn) + 1;

        return opponentPosition;
    }

    public static string 判断是否处于禁用位置(string type,int index)
    {
        if (type == "角色")
        {
            foreach (KeyValuePair<string,List<int>> kvp in ValueHolder.人物禁止放置位置)
            {
                if (kvp.Value.Contains(index))
                {
                    return kvp.Key;
                }
            }
        }
        else if (type == "建筑")
        {
            foreach (KeyValuePair<string, List<int>> kvp in ValueHolder.建筑禁止放置位置)
            {
                if (kvp.Value.Contains(index))
                {
                    return kvp.Key;
                }
            }
        }
        return "无";
    }

    public static IEnumerator RotateAndScaleCoroutine(GameObject card,GameObject mycard = null)
    {
        Vector3 originalScale = card.transform.localScale;
        float elapsedTime = 0f;

        float rotationSpeed = 360f;
        float fadeDuration = 2f; // 持续时间

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration; // 时间比例 [0, 1]

            // 计算缩放
            float scale = Mathf.Lerp(1f, 0f, t);
            card.transform.localScale = new Vector3(scale, scale, 1f);

            // 计算旋转
            float rotation = rotationSpeed * Time.deltaTime;
            card.transform.Rotate(Vector3.forward, rotation);

            yield return null;
        }

        if (card.GetComponent<MoveController>().cardType == 0 && ValueHolder.SkillAction.ContainsKey(card.GetComponent<数据显示>().卡牌数据.uid))
        {
            BaseSkill skill = ValueHolder.SkillAction[card.GetComponent<数据显示>().卡牌数据.uid];
            if (skill.亡语 == 1)
            {
                运行下个技能阶段(skill);
            }
        }

        // 确保最终状态
        card.transform.localScale = Vector3.zero;
        string endindex = card.transform.parent.name.ToString();
        Destroy(card); // 可选择禁用物体
        if (mycard != null)
        {
            mycard.transform.SetParent(ValueHolder.棋盘[endindex].transform);
        }
    }


    public static void 卡牌摧毁(GameObject card,GameObject mycard = null)
    {
        场上角色死亡触发();
        MonoBehaviour monoBehaviour = card.GetComponent<MonoBehaviour>();
        monoBehaviour.StartCoroutine(RotateAndScaleCoroutine(card, mycard));
    }

    public static void 场上角色死亡触发()
    {
        foreach(BaseSkill skill in ValueHolder.SkillAction.Values)
        {
            if (skill.场上角色死亡触发 == 1)
            {
                运行下个技能阶段(skill);
            }
        }
    }

    public static void Winmove(GameObject card)
    {
        List<int> winpos = card.GetComponent<MoveController>().GetWinpos();

        Debug.Log(card.transform.parent.name);
        if (card.transform.parent.name == "commonCard(Clone)")
        {
            if (winpos.Contains(int.Parse(card.transform.parent.parent.name)))
            {
                卡牌摧毁(card);
                Debug.Log("win");
            }
        }
        else
        {

            if (winpos.Contains(int.Parse(card.transform.parent.name)))
            {
                卡牌摧毁(card);
                Debug.Log("win");
            }
        }
    }

    public static void cardAttack(GameObject mycard,GameObject hecard,int is_send)
    {
        卡牌数据 card_data1 = mycard.GetComponent<数据显示>().卡牌数据;
        卡牌数据 card_data2 = hecard.GetComponent<数据显示>().卡牌数据;

        Debug.Log("我方血量:" + card_data1.nowHp);
        Debug.Log("敌方血量:" + card_data2.nowHp);

        Debug.Log("我方攻击力:" + card_data1.nowAttack);
        Debug.Log("敌方攻击力:" + card_data2.nowAttack);

        card_data1.nowHp = card_data1.nowHp - card_data2.nowAttack;
        card_data2.nowHp = card_data2.nowHp - card_data1.nowAttack;

        Debug.Log("我方血量：" + card_data1.nowHp);
        Debug.Log("敌方血量：" + card_data2.nowHp);

        mycard.GetComponent<数据显示>().更新数据();
        hecard.GetComponent<数据显示>().更新数据();

        if (is_send == 0)
        {
            Send攻击(card_data1.uid, card_data2.uid);
        }



        ValueHolder.is_choose = 0;
        mycard.GetComponent<CanvasGroup>().alpha = 1.0f;
        mycard.GetComponent<CanvasGroup>().blocksRaycasts = true;




        if (card_data1.nowHp > 0 && card_data2.nowHp <= 0)
        {

            if (mycard.GetComponent<MoveController>().杀人后触发==1)
            {
                BaseSkill skill = ValueHolder.SkillAction[mycard.GetComponent<数据显示>().卡牌数据.uid];
                运行下个技能阶段(skill);
            }

            卡牌摧毁(hecard,mycard);

            Winmove(mycard);

        }else if (card_data1.nowHp <= 0 && card_data2.nowHp > 0)
        {
            卡牌摧毁(mycard);

        }else if (card_data1.nowHp <= 0 && card_data2.nowHp <= 0)
        {
            if (mycard.GetComponent<MoveController>().杀人后触发 == 1)
            {
                BaseSkill skill = ValueHolder.SkillAction[mycard.GetComponent<数据显示>().卡牌数据.uid];
                运行下个技能阶段(skill);
            }
            卡牌摧毁(mycard);
            卡牌摧毁(hecard);
        }



    }

    public static void Send中立法术创建(int cardID)
    {
        ChangeSendMessage("Action", 14);
        ChangeSendMessage("cardID", cardID);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send中立法术销毁(int cardID)
    {
        ChangeSendMessage("Action", 15);
        ChangeSendMessage("cardID", cardID);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }





    public static void Send法术禁用(int cardID,float turn)
    {
        ChangeSendMessage("Action", 16);
        ChangeSendMessage("cardID", cardID);
        ChangeSendMessage("turn", turn);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send法术禁用取消(int cardID)
    {
        ChangeSendMessage("Action", 17);
        ChangeSendMessage("cardID", cardID);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }



    public static void Send人物位置禁用(int cardID, List<int> indexs)
    {
        foreach (int index in indexs)
        {
            int index1 = ConvertPosition(index);
            ChangeSendMessage("Action", 18);
            ChangeSendMessage("start_index", index1);
            ChangeSendMessage("cardID", cardID);
            ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
        }
    }

    public static void Send人物位置禁用取消(int cardID)
    {
        ChangeSendMessage("Action", 19);
        ChangeSendMessage("cardID", cardID);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send我方红牌法术创建(int cardID,string uid)
    {
        ChangeSendMessage("Action", 20);
        ChangeSendMessage("cardID", cardID);
        ChangeSendMessage("uid", uid);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send我方红牌法术销毁(int cardID,string uid)
    {
        ChangeSendMessage("Action", 21);
        ChangeSendMessage("cardID", cardID);
        ChangeSendMessage("uid", uid);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send人物禁用(string uid, float turn)
    {
        ChangeSendMessage("Action", 22);
        ChangeSendMessage("uid", uid);
        ChangeSendMessage("turn", turn);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send人物禁用取消(string uid)
    {
        ChangeSendMessage("Action", 23);
        ChangeSendMessage("uid", uid);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send敌方红牌法术创建(int cardID,string uid)
    {
        ChangeSendMessage("Action", 24);
        ChangeSendMessage("cardID", cardID);
        ChangeSendMessage("uid", uid);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send敌方红牌法术销毁(int cardID,string uid)
    {
        ChangeSendMessage("Action", 25);
        ChangeSendMessage("cardID", cardID);
        ChangeSendMessage("uid", uid);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send效果挂载(string uid,int effect)
    {
        ChangeSendMessage("Action", 26);
        ChangeSendMessage("uid", uid);
        ChangeSendMessage("effect", effect);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send效果卸载(string uid,int effect)
    {
        ChangeSendMessage("Action", 27);
        ChangeSendMessage("uid", uid);
        ChangeSendMessage("effect", effect);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send卡牌摧毁(string uid)
    {
        ChangeSendMessage("Action", 28);
        ChangeSendMessage("uid", uid);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send攻击(string myuid, string heuid)
    {
        ChangeSendMessage("Action", 29);
        ChangeSendMessage("uid", myuid);
        ChangeSendMessage("uid1", heuid);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send血量改变(string uid,int num)
    {
        ChangeSendMessage("Action", 30);
        ChangeSendMessage("uid", uid);
        ChangeSendMessage("num", num);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send攻击力改变(string uid,int num)
    {
        ChangeSendMessage("Action", 31);
        ChangeSendMessage("uid", uid);
        ChangeSendMessage("num", num);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send倒计时(string uid,int time)
    {
        ChangeSendMessage("Action", 32);
        ChangeSendMessage("uid", uid);
        ChangeSendMessage("num", time);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send倒计时改变(string uid,int time)
    {
        ChangeSendMessage("Action", 33);
        ChangeSendMessage("uid", uid);
        ChangeSendMessage("num", time);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send回合结束弃牌(int num)
    {
        ChangeSendMessage("Action", 34);
        ChangeSendMessage("num", num);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send摸牌(int num)
    {
        ChangeSendMessage("Action", 35);
        ChangeSendMessage("num", num);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }



    public static void 运行下个技能阶段(BaseSkill skill)
    {
        if (skill.activateTurn_1_finish == 0)
        {
            skill.Action_1();
            Debug.Log("运行技能1阶段");
            return;
        }
        if (skill.activateTurn_2_finish == 0)
        {
            skill.Action_2();
            Debug.Log("运行技能2阶段");
            return;
        }
        if (skill.activateTurn_3_finish == 0)
        {
            skill.Action_3();
            Debug.Log("运行技能3阶段");
            return;
        }
        if (skill.activateTurn_4_finish == 0)
        {
            skill.Action_4();
            Debug.Log("运行技能4阶段");
            return;
        }

        Debug.Log("技能阶段运行完毕");
    }

    public static void 禁用棋盘物件代码(string scriptName,int cardtype)
    {
        foreach (KeyValuePair<string,GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card = grid.transform.GetChild(0).gameObject;
                if (card.GetComponent<MoveController>().cardType == cardtype)
                {
                    MonoBehaviour[] components = card.GetComponents<MonoBehaviour>();
                    foreach (MonoBehaviour component in components)
                    {
                        // 判断组件的类型名是否与输入的脚本名字匹配
                        if (component.GetType().Name == scriptName)
                        {
                            // 禁用该脚本
                            component.enabled = false;
                            Debug.Log("禁用棋盘物件代码" + card.GetComponent<数据显示>().卡牌数据.名字);
                        }
                    }
                }
            }
        }
    }

    public static void 启用棋盘物件代码(string scriptName, int cardtype)
    {
        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card = grid.transform.GetChild(0).gameObject;
                if (card.GetComponent<MoveController>().cardType == cardtype)
                {
                    MonoBehaviour[] components = card.GetComponents<MonoBehaviour>();
                    foreach (MonoBehaviour component in components)
                    {
                        // 判断组件的类型名是否与输入的脚本名字匹配
                        if (component.GetType().Name == scriptName)
                        {
                            // 禁用该脚本
                            component.enabled = true;
                        }
                    }
                }
            }
        }
    }

    public static void 禁用手牌物件代码(string scriptName)
    {
        for (int i = 0; i < ValueHolder.手牌区.transform.childCount; i++)
        {
            GameObject card = ValueHolder.手牌区.transform.GetChild(i).gameObject;
            MonoBehaviour[] components = card.GetComponents<MonoBehaviour>();
            if (card.GetComponent<MoveController>().cardType == 0)
            {
                foreach (MonoBehaviour component in components)
                {
                    // 判断组件的类型名是否与输入的脚本名字匹配
                    if (component.GetType().Name == scriptName)
                    {
                        // 禁用该脚本
                        component.enabled = false;
                        break;
                    }
                }
            }
        }
    }

    public static void 启用手牌物件代码(string scriptName)
    {
        for (int i = 0; i < ValueHolder.手牌区.transform.childCount; i++)
        {
            GameObject card = ValueHolder.手牌区.transform.GetChild(i).gameObject;
            if (card.GetComponent<MoveController>().cardType == 0)
            {
                MonoBehaviour[] components = card.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour component in components)
                {
                    // 判断组件的类型名是否与输入的脚本名字匹配
                    if (component.GetType().Name == scriptName)
                    {
                        // 禁用该脚本
                        component.enabled = true;
                        break;
                    }
                }
            }

        }
    }

    public static void ShowCardchoose(int cardtype)
    {
        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card = grid.transform.GetChild(0).gameObject;
                if (card.GetComponent<MoveController>().cardType == cardtype && card.GetComponent<数据显示>().卡牌数据.类别 == "角色")
                {
                    Transform chooseChild = card.transform.Find("choose");

                    if (chooseChild != null)
                    {
                        // 设置子物件为可见
                        chooseChild.gameObject.SetActive(true);
                        card.GetComponent<MoveController>().法术可作用 = 1;
                    }
                }
            }
        }
    }

    public static void HideCardchoose()
    {
        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card = grid.transform.GetChild(0).gameObject;
                if (card.GetComponent<数据显示>().卡牌数据.类别 == "角色")
                {
                    Transform chooseChild = card.transform.Find("choose");

                    if (chooseChild != null)
                    {
                        // 设置子物件为可见
                        chooseChild.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public static int 我方人物数量()
    {
        int i = 0;
        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card = grid.transform.GetChild(0).gameObject;
                if (card.GetComponent<MoveController>().cardType == 0 && card.GetComponent<数据显示>().卡牌数据.类别 == "角色")
                {
                    i++;
                }
            }
        }
        return i;
    }

    public static List<GameObject> 获取全部人物()
    {
       List<GameObject> 全部人物 = new List<GameObject>();
        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card = grid.transform.GetChild(0).gameObject;
                if (card.GetComponent<数据显示>().卡牌数据.类别 == "角色")
                {
                    全部人物.Add(card);
                }
            }
        }
        return 全部人物;
    }
    public static List<GameObject> 获取敌方全部人物()
    {
        List<GameObject> 全部敌方人物 = new List<GameObject>();
        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card = grid.transform.GetChild(0).gameObject;
                if (card.GetComponent<MoveController>().cardType == 1 && card.GetComponent<数据显示>().卡牌数据.类别 == "角色")
                {
                    全部敌方人物.Add(card);
                }
            }
        }
        return 全部敌方人物;
    }
    public static int 敌方人物数量()
    {
        int i = 0;
        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card = grid.transform.GetChild(0).gameObject;
                if (card.GetComponent<MoveController>().cardType == 1 && card.GetComponent<数据显示>().卡牌数据.类别 == "角色")
                {
                    i++;
                }
            }
        }
        return i;
    }

    public static GameObject uid找卡(string uid) {

        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card = grid.transform.GetChild(0).gameObject;
                if (card.GetComponent<数据显示>().卡牌数据.uid == uid)
                {
                    return card;
                }
            }
        }

        return null;

    }

    public static void 倒计时回合变化()
    {
        List<string> keytoMove = new List<string>();

        for (int i = 0; i < ValueHolder.倒计时储存.Count; i++)
        {
            string key = ValueHolder.倒计时储存.ElementAt(i).Key;
            ValueHolder.倒计时储存[key] -= 0.5f;
            if (ValueHolder.倒计时储存[key] == 0)
            {
                keytoMove.Add(key);
            }

        }

        foreach (string key in keytoMove)
        {
            ValueHolder.倒计时储存.Remove(key);
        }
    }

    public static GameObject 图片挂载(GameObject commoncard)
    {
        int id = commoncard.GetComponent<数据显示>().卡牌数据.id;
        commoncard.GetComponent<数据显示>().enabled = true;
        Texture2D texture = Resources.Load<Texture2D>("card/" + id.ToString());
        if (texture != null)
        {
            foreach (Image image in commoncard.GetComponentsInChildren<Image>())
            {
                if (image.ToString() == "pics (UnityEngine.UI.Image)")
                {
                    image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                }
            }
        }

        return commoncard;
    }
    public static void 弃牌()
    {
        if (ValueHolder.回合结束弃牌 == 1) {

            ValueHolder.弃牌数量.text = ValueHolder.弃牌数量限制.ToString();
            ValueHolder.弃牌显示.gameObject.SetActive(true);
            for (int i = 0; i < ValueHolder.手牌区.transform.childCount; i++)
            {
                GameObject card = ValueHolder.手牌区.transform.GetChild(i).gameObject;
                GameObject card_copy = Instantiate(ValueHolder.弃牌, ValueHolder.弃牌区.transform);
                card_copy.GetComponent<数据显示>().卡牌数据 = new 卡牌数据(card.GetComponent<数据显示>().卡牌数据);
                card_copy = 图片挂载(card_copy);
            }
            ValueHolder.回合结束弃牌 = 0;

        }


        ValueHolder.弃牌数量限制 = 0;
    }

    public static List<GameObject> 获取桥上卡牌()
    {
        List<GameObject> 桥上卡牌 = new List<GameObject>();
        foreach (int i in new List<int> { 11, 13, 15 })
        {
            GameObject grid = ValueHolder.棋盘[i.ToString()];
            if (grid.transform.childCount != 0)
            {
                GameObject card = grid.transform.GetChild(0).gameObject;
                桥上卡牌.Add(card);
            }
        }
        return 桥上卡牌;
    }

    public static void 灵力增加(int level,int num)
    {
        if (num <= 0)
        {
            return;
        }
        GameObject 灵力 = new GameObject();
        for (int i = 0; i < num; i++)
        {
            if (ValueHolder.灵力当前状态[level] >= ValueHolder.灵力当前上限[level])
            {
                break;
            }

            if (level == 1)
            {
                灵力 = Instantiate(ValueHolder.黄, ValueHolder.灵力栏.transform);
                ValueHolder.灵力当前状态[level] += 1;
            }
            else if (level == 2)
            {
                灵力 = Instantiate(ValueHolder.绿, ValueHolder.灵力栏.transform);
                ValueHolder.灵力当前状态[level] += 1;
            }
            else if (level == 3)
            {
                灵力 = Instantiate(ValueHolder.蓝, ValueHolder.灵力栏.transform);
                ValueHolder.灵力当前状态[level] += 1;
            }
            else if (level == 4)
            {
                灵力 = Instantiate(ValueHolder.紫, ValueHolder.灵力栏.transform);
                ValueHolder.灵力当前状态[level] += 1;
            }

        }

    }

    public static int 灵力减少(int level, int num)
    {
        if (ValueHolder.灵力当前状态[level] < num)
        {
            return 0;
        }

        for (int i = 0; i < num; i++)
        {
            DestroyChildAtIndex(ValueHolder.灵力栏, ValueHolder.灵力当前状态[level] - 1);
            ValueHolder.灵力当前状态[level] -= 1;
        }
        return 1;
    }

    public static void 灵力回合更新()
    {
        if (ValueHolder.灵力当前上限[1] < 4)
        {
            ValueHolder.灵力当前上限[1] += 1;
        }else if (ValueHolder.灵力当前上限[2] < 3)
        {
            ValueHolder.灵力当前上限[2] += 1;
        }else if (ValueHolder.灵力当前上限[3] < 2)
        {
            ValueHolder.灵力当前上限[3] += 1;
        }else if (ValueHolder.灵力当前上限[4] < 1)
        {
            ValueHolder.灵力当前上限[4] += 1;
        }

        DestroyAllChildren(ValueHolder.灵力栏);
        灵力增加(1, ValueHolder.灵力当前上限[1]);
        灵力增加(2, ValueHolder.灵力当前上限[2]);
        灵力增加(3, ValueHolder.灵力当前上限[3]);
        灵力增加(4, ValueHolder.灵力当前上限[4]);
    }


}
