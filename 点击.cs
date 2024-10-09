using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class 点击 : MonoBehaviour
{
    public MySqlConnection conn;
    public GameObject pool;
    public GameObject commonCard;
    public Button click;
    public Dictionary<int, int> GetCard;

    public TextMeshProUGUI name1;
    public TextMeshProUGUI coin;
    public HintManager hintManager;

    public GameObject imageToRotate; // 要旋转的图片
    public float rotationSpeed = 100f; // 旋转速度
    public float rotationTime = 2f; // 旋转持续的时间

    private bool isRotating = false; // 检查是否已经在旋转
    void Start()
    {
        string connectionString = $"Server={ValueHolder.mysql_ip};Port={ValueHolder.mysql_port};Database={ValueHolder.mysql_database};Uid={ValueHolder.mysql_username};Pwd={ValueHolder.mysql_pwd};";
        conn = new MySqlConnection(connectionString);
        conn.Open();


        GetCard = ValueHolder.cardData;
        name1.text = ValueHolder.username;
        coin.text = ValueHolder.coin;

        click.onClick.AddListener(bcc);

    }

    public void bcc()
    {
        if (!isRotating) // 防止重复点击启动多个协程
        {
            StartCoroutine(RotateImage());
        }
    }

    卡牌数据 get_one()
    {
        List<卡牌数据> values = new List<卡牌数据>(ValueHolder.gloabCaedData.Values);
        卡牌数据 randomValue = values[Random.Range(0, values.Count)];
        return randomValue;
    }

    void summon()
    {
        if (int.Parse(ValueHolder.coin) >= 2)
        {
            mainfunction.DestroyAllChildren(pool);


            GameObject card = Instantiate(commonCard, pool.transform);
            卡牌数据 cardInfo = get_one();
            card.GetComponent<数据显示>().卡牌数据 = cardInfo;
            card.transform.localScale = new Vector3(2.8f, 2.8f, 2.8f);
            card.GetComponent<数据显示>().enabled = true;
            int status = GetComponent<mainfunction>().Loadimages(card, 3, cardInfo.id);


            GetCard = AddDictionaries(GetCard, new Dictionary<int, int> { { cardInfo.id, 1 } });
            coin.text = (int.Parse(coin.text) - 2).ToString();
            ValueHolder.coin = coin.text;
            ValueHolder.cardData = GetCard;
        }
        else
        {
            mainfunction.DestroyAllChildren(pool);
            hintManager.AddHint("金币不足，请获取金币！");
        }


        //GetComponent<mainfunction>().updatemysql(conn, "user", "carddata", JsonUtility.ToJson(GetCard).ToString());
    }

    public Dictionary<int, int> AddDictionaries(Dictionary<int, int> dict1, Dictionary<int, int> dict2)
    {
        // 创建一个新的字典来存储总结果
        Dictionary<int, int> resultDict = new Dictionary<int, int>();

        // 遍历第一个字典并添加到结果字典中
        foreach (var kvp in dict1)
        {
            resultDict[kvp.Key] = kvp.Value;
        }

        // 遍历第二个字典并添加到结果字典中
        foreach (var kvp in dict2)
        {
            // 如果键已存在，则累加值，否则添加新的键值对
            if (resultDict.ContainsKey(kvp.Key))
            {
                resultDict[kvp.Key] += kvp.Value;
            }
            else
            {
                resultDict[kvp.Key] = kvp.Value;
            }
        }

        return resultDict;
    }

    void OnDestroy()
    {

        // 将字符串类型的字典序列化为JSON字符串，便于存储到数据库
        string jsonString = JsonConvert.SerializeObject(GetCard);
        GetComponent<mainfunction>().updatemysql(conn, "user", "carddata", jsonString);
    }

    private IEnumerator RotateImage()
    {
        isRotating = true; // 标记为正在旋转

        float elapsedTime = 0f;

        while (elapsedTime < rotationTime)
        {
            // 按时间递增旋转图片
            imageToRotate.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime; // 计算时间
            yield return null; // 等待下一帧
        }

        isRotating = false; // 完成旋转后标记为不再旋转
        summon();
    }
}
