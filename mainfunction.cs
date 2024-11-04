using ExitGames.Client.Photon;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;


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

    public static void DestroyChildByName(Transform parent, string childName, int count)
    {
        int destroyedCount = 0; // 记录已销毁的子物体数量

        // 遍历父物体的所有子物体
        foreach (Transform child in parent)
        {
            // 检查子物体名称是否匹配
            if (child.name == childName)
            {
                // 销毁该子物体
                Destroy(child.gameObject);
                destroyedCount++; // 更新已销毁的子物体数量

                // 如果达到指定数量，停止操作
                if (destroyedCount >= count)
                {
                    break;
                }
            }
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
        Debug.Log(message);
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

        // 确保最终状态
        card.transform.localScale = Vector3.zero;
        string endindex = card.transform.parent.name.ToString();
        Destroy(card); // 可选择禁用物体
        if (mycard != null)
        {

            List<int> obstacles = mycard.GetComponent<MoveController>().obstacles;
            if (!obstacles.Contains(int.Parse(endindex)))
            {
                mycard.transform.SetParent(ValueHolder.棋盘[endindex].transform);
            }

        }
    }


    public static void 卡牌摧毁(GameObject card,GameObject mycard = null)
    {
        场上角色死亡触发(card);
        亡语触发(card);

        MonoBehaviour monoBehaviour = card.GetComponent<MonoBehaviour>();
        monoBehaviour.StartCoroutine(RotateAndScaleCoroutine(card, mycard));
    }

    public static void 场上角色死亡触发(GameObject card)
    {

        foreach(BaseSkill skill in ValueHolder.SkillAction.Values)
        {
            if (skill.场上角色死亡触发 == 1)
            {

                if (ValueHolder.is_myturn == 0 && skill.用户操作型技能 == 1)
                {
                    Send技能释放申请(skill.uid);
                }
                else
                {
                    运行下个技能阶段(skill);
                }
            }


            if (skill.场上敌方角色死亡触发 == 1 && card.GetComponent<MoveController>().cardType == 1)
            {
                if (ValueHolder.is_myturn == 0 && skill.用户操作型技能 == 1)
                {
                    Send技能释放申请(skill.uid);
                }
                else
                {
                    运行下个技能阶段(skill);
                }
            }

            if (skill.场上我方角色死亡触发 == 1 && card.GetComponent<MoveController>().cardType == 0)
            {
                if (ValueHolder.is_myturn == 0 && skill.用户操作型技能 == 1)
                {
                    Send技能释放申请(skill.uid);
                }
                else
                {
                    运行下个技能阶段(skill);
                }
            }
        }
    }

    public static void 亡语触发(GameObject card)
    {
        if (card.GetComponent<MoveController>().cardType == 0 && ValueHolder.SkillAction.ContainsKey(card.GetComponent<数据显示>().卡牌数据.uid))
        {
            BaseSkill skill = ValueHolder.SkillAction[card.GetComponent<数据显示>().卡牌数据.uid];
            if (skill.亡语 == 1)
            {
                if (ValueHolder.is_myturn == 0 && skill.用户操作型技能 == 1)
                {
                    Send技能释放申请(skill.uid);
                }
                else
                {
                    运行下个技能阶段(skill);
                }

            }
        }
    }

    public static void 技能释放结束()
    {
        SkillExecutor.currentRunningSkillUid = null;
    }
    public static void 技能释放未结束(string uid) 
    {
        SkillExecutor.currentRunningSkillUid = uid;
    }

    public static void cardAttack(GameObject mycard,GameObject hecard,int is_send)
    {
        卡牌数据 card_data1 = mycard.GetComponent<数据显示>().卡牌数据;
        卡牌数据 card_data2 = hecard.GetComponent<数据显示>().卡牌数据;

        if (mycard.GetComponent<MoveController>().无双 == 0)
        {
            card_data1.nowHp = card_data1.nowHp - card_data2.nowAttack;
        }

        card_data2.nowHp = card_data2.nowHp - card_data1.nowAttack;


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

    public static void Send卡牌移动(int start_index, int end_index)
    {
        start_index = ConvertPosition(start_index);
        end_index = ConvertPosition(end_index);
        ChangeSendMessage("Action", 10);
        ChangeSendMessage("start_index", start_index);
        ChangeSendMessage("end_index", end_index);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);

    }

    public static void Send卡牌生成(int cardID, int start_index, string uid)
    {
        start_index = ConvertPosition(start_index);
        ChangeSendMessage("Action", 9);
        ChangeSendMessage("cardID", cardID);
        ChangeSendMessage("start_index", start_index);
        ChangeSendMessage("uid", uid);
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

    public static void Send效果挂载(string uid,int effect,float delay)
    {
        ChangeSendMessage("Action", 26);
        ChangeSendMessage("uid", uid);
        ChangeSendMessage("effect", effect);
        ChangeSendMessage("turn", ValueHolder.turn +  delay);
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
        ChangeSendMessage("Action", 37);
        ChangeSendMessage("num", num);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send对方暂停()
    {
        if (ValueHolder.is_myturn == 1)
        {
            return;
        }
        ChangeSendMessage("Action", 38);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send对方继续()
    {
        if (ValueHolder.is_myturn == 1)
        {
            return;
        }
        禁用棋盘物件代码("b_moveca", 0);
        禁用手牌物件代码("b_cardaction");
        ValueHolder.下个回合.interactable = false;
        ValueHolder.下个回合.image.color = Color.gray;

        ChangeSendMessage("Action", 39);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void Send技能释放申请(string uid)
    {
        ChangeSendMessage("Action", 40);
        ChangeSendMessage("uid", uid);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);

    }

    public static void Send技能释放同意(string uid)
    {
        ChangeSendMessage("Action", 41);
        ChangeSendMessage("uid", uid);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    public static void 运行下个技能阶段(BaseSkill skill)
    {
        if (skill.activateTurn_1_finish == 0)
        {
            SkillExecutor.EnqueueSkillAtFront(skill, skill.Action_1);
            Debug.Log("运行技能1阶段");
            return;
        }
        if (skill.activateTurn_2_finish == 0)
        {
            SkillExecutor.EnqueueSkillAtFront(skill, skill.Action_2);
            Debug.Log("运行技能2阶段");
            return;
        }
        if (skill.activateTurn_3_finish == 0)
        {
            SkillExecutor.EnqueueSkillAtFront(skill, skill.Action_3);
            Debug.Log("运行技能3阶段");
            return;
        }
        if (skill.activateTurn_4_finish == 0)
        {
            SkillExecutor.EnqueueSkillAtFront(skill, skill.Action_4);
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
    
    public static List<GameObject> 获取我方全部人物()
    {
        List<GameObject> 全部我方人物 = new List<GameObject>();
        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card = grid.transform.GetChild(0).gameObject;
                if (card.GetComponent<MoveController>().cardType == 0 && card.GetComponent<数据显示>().卡牌数据.类别 == "角色")
                {
                    全部我方人物.Add(card);
                }
            }
        }
        return 全部我方人物;
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
                灵力 = Instantiate(ValueHolder.黄);
                缩放调整(灵力);
                灵力.transform.SetParent(ValueHolder.灵力栏.transform);
                ValueHolder.灵力当前状态[level] += 1;
            }
            else if (level == 2)
            {
                灵力 = Instantiate(ValueHolder.绿);
                缩放调整(灵力);
                灵力.transform.SetParent(ValueHolder.灵力栏.transform);
                ValueHolder.灵力当前状态[level] += 1;
            }
            else if (level == 3)
            {
                灵力 = Instantiate(ValueHolder.蓝);
                缩放调整(灵力);
                灵力.transform.SetParent(ValueHolder.灵力栏.transform);
                ValueHolder.灵力当前状态[level] += 1;
            }
            else if (level == 4)
            {
                灵力 = Instantiate(ValueHolder.紫);
                缩放调整(灵力);
                灵力.transform.SetParent(ValueHolder.灵力栏.transform);
                ValueHolder.灵力当前状态[level] += 1;
            }

        }

    }

    public static int 灵力减少(int level, int num)
    {
        string 灵力名 = "";
        switch (level)
        {
            case 1:
                灵力名 = "黄(Clone)";
                break;
            case 2:
                灵力名 = "绿(Clone)";
                break;
            case 3:
                灵力名 = "蓝(Clone)";
                break;
            case 4:
                灵力名 = "紫(Clone)";
                break;
        }

        DestroyChildByName(ValueHolder.灵力栏.transform, 灵力名, num);

        if (ValueHolder.灵力当前状态[level] < num)
        {
            return 0;
        }
        ValueHolder.灵力当前状态[level] -= num;

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
        for (int i = 1; i < 5; i++)
        {
            ValueHolder.灵力当前状态[i] = 0;
        }
        灵力增加(1, ValueHolder.灵力当前上限[1]);
        灵力增加(2, ValueHolder.灵力当前上限[2]);
        灵力增加(3, ValueHolder.灵力当前上限[3]);
        灵力增加(4, ValueHolder.灵力当前上限[4]);
    }

    public static void 选择我方卡牌施放(卡牌数据 card_data)//可以取消施法：1.不可以为0
    {
        ValueHolder.法术作用敌我类型 = 0;
        禁用棋盘物件代码("b_moveca", 0);
        禁用手牌物件代码("b_cardaction");
        ShowCardchoose(0);
        启用棋盘物件代码("b_choose_fa", 0);
        ValueHolder.释放法术uid = card_data.uid;

        ValueHolder.法术选择取消.gameObject.SetActive(true);

        技能释放未结束(card_data.uid);
    }

    public static void 选择敌方卡牌施放(卡牌数据 card_data)
    {
        ValueHolder.法术作用敌我类型 = 1;
        禁用棋盘物件代码("b_moveca", 0);
        禁用手牌物件代码("b_cardaction");
        ShowCardchoose(1);
        启用棋盘物件代码("b_choose_fa", 1);
        ValueHolder.释放法术uid = card_data.uid;

        ValueHolder.法术选择取消.gameObject.SetActive(true);

        技能释放未结束(card_data.uid);
    }

    public static void 点选格子(string 召唤物名字, string uid ,List<int> ClickList)
    {
        禁用棋盘物件代码("b_moveca", 0);
        禁用手牌物件代码("b_cardaction");
        ValueHolder.下个回合.interactable = false;
        ValueHolder.下个回合.image.color = Color.gray;
        格子绿色显示(ClickList);

        技能释放未结束(uid);


        ValueHolder.启用点选格子 = 1;
        ValueHolder.点选技能uid = uid;
        ValueHolder.hintManager.AddHint("请选择位置召唤：" + 召唤物名字);
    }

    public static void 格子绿色显示(List<int> availableMoves)
    {
        foreach (int i in availableMoves)
        {
            Color green = new Color(0.4f, 0.9f, 0.5f, 0.3f);
            ValueHolder.棋盘[i.ToString()].GetComponent<UnityEngine.UI.Image>().color = green;
        }
    }

    public static void 格子颜色还原()
    {
        List<int> availableMoves = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
        foreach (int i in availableMoves)
        {
            Color grenn = new Color(0.4f, 0.9f, 0.5f, 0f);
            ValueHolder.棋盘[i.ToString()].GetComponent<UnityEngine.UI.Image>().color = grenn;
        }
    }

    public static GameObject 指定位置生成卡牌(int index,int cardID,int have_skill)//have_skill标识是否有技能，1则触发技能，0则不触发
    {
        GameObject summonCard = Instantiate(ValueHolder.手牌对战牌);
        MonoBehaviour summonCardMono = ValueHolder.手牌区.GetComponent<MonoBehaviour>();
        Texture2D texture = Resources.Load<Texture2D>("card/" + cardID.ToString());
        卡牌数据 原卡牌数据 = ValueHolder.gloabCaedData[cardID];
        // 深拷贝
        卡牌数据 新卡牌数据 = new 卡牌数据(原卡牌数据);
        summonCard.GetComponent<数据显示>().卡牌数据 = 新卡牌数据;
        summonCard.GetComponent<数据显示>().卡牌数据.uid = System.Guid.NewGuid().ToString();
        summonCard.GetComponent<数据显示>().enabled = true;
        if (texture != null)
        {
            foreach (Image image in summonCard.GetComponentsInChildren<Image>())
            {
                if (image.ToString() == "pics (UnityEngine.UI.Image)")
                {
                    image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                }
            }
        }

        if (have_skill == 1)
        {
            //技能初始化
            BaseSkill skill = SkillFactory.CreateSkill(summonCard, summonCardMono);

            if (skill.activateTurn_1 == ValueHolder.turn)
            {
                skill.Action_1();
                Debug.Log("技能触发");
            }
            ValueHolder.SkillAction.Add(summonCard.GetComponent<数据显示>().卡牌数据.uid, skill);
        }

        改变卡牌汉字颜色(summonCard, "blue");
        summonCard.transform.SetParent(ValueHolder.棋盘[index.ToString()].transform);
        Send卡牌生成(cardID, index, summonCard.GetComponent<数据显示>().卡牌数据.uid);
        summonCard.GetComponent<b_moveca>().enabled = true;
        summonCard.GetComponent<b_cardaction>().enabled = false;

        return summonCard;

    }

    public static void 改变卡牌汉字颜色(GameObject card, string color)//支持"red"和"blue"
    {
        foreach (Transform child in card.transform)
        {
            TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                if (color == "red")
                {
                    text.color = UnityEngine.Color.red;
                }
                if (color == "blue")
                {
                    text.color = UnityEngine.Color.blue;
                }
            }
        }
    }


    public static void 效果卸载(GameObject card,int effect)
    {
        if (effect == 1) //击杀免疫
        {
            card.GetComponent<MoveController>().击杀免疫 = 0;
        }
        else if (effect == 2) //消灭免疫
        {
            card.GetComponent<MoveController>().消灭免疫 = 0;
        }
        else if (effect == 3) //眩晕
        {
            card.GetComponent<MoveController>().眩晕 = 0;
        }
        else if (effect == 4)//法术不可作用
        {
            card.GetComponent<MoveController>().法术可作用 = 1;
        }
        else if (effect == 5)//眩晕免疫
        {
            card.GetComponent<MoveController>().眩晕免疫 = 0;
        }
    }
    public static void 效果卸载遍历()
    {
        for (int i = 0; i < ValueHolder.效果卸载队列.Count; i++)
        {
            Effect effect = ValueHolder.效果卸载队列[i];
            if (effect.turn == ValueHolder.turn)
            {
                GameObject card = uid找卡(effect.uid);
                if (card != null) { 
                    效果卸载(card, effect.effectID);
                }
            }
        }
    }

    public static int 治疗(GameObject card,int num)
    {
        int 治疗量;
        卡牌数据 card_data = card.GetComponent<数据显示>().卡牌数据;
        card_data.nowHp += num;
        if (card_data.nowHp > card_data.maxHp)
        {
            治疗量=card_data.maxHp-card_data.nowHp;
            card_data.nowHp = card_data.maxHp;
            card.GetComponent<数据显示>().更新数据();
            return 治疗量;
        }

        card.GetComponent<数据显示>().更新数据();
        return num;
    }


}
