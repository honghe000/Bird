using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class libary : MonoBehaviour
{
    private MySqlConnection conn;
    public GameObject 卡牌;
    public GameObject layout;
    public GameObject layoutone;
    public TextMeshProUGUI cardgroupName;
    public Image renameBox;
    public Image delBox;
    public TextMeshProUGUI cardallnum;

    // Start is called before the first frame update
    void Start()
    {
        Loadresouce();
        InstantinateCard();
    }


    public void Loadresouce()
    {
        cardgroupName.text = ValueHolder.cardgroupName;
        renameBox.gameObject.SetActive(false);
        delBox.gameObject.SetActive(false);
    }



    public Dictionary<int, int> SubtractDictionary(Dictionary<int, int> mainDict, Dictionary<int, int> subDict)
    {
        Dictionary<int, int> resultDict = new Dictionary<int, int>();

        foreach (var kvp in mainDict)
        {
            int key = kvp.Key;
            int value1 = kvp.Value;
            int value2 = 0;

            // 检查次字典中是否包含当前键
            if (subDict.ContainsKey(key))
            {
                value2 = subDict[key];
            }

            // 计算差值并添加到结果字典中
            resultDict[key] = value1 - value2;
        }

        return resultDict;
    }





    public void summonC(卡牌数据 cardD,KeyValuePair<int, int> data)
    {
        if (data.Value > 0)
        {
            GameObject card = Instantiate(卡牌, layout.transform);
            card.GetComponent<cardShow>().卡牌数据 = cardD;
            card.GetComponent<cardShow>().num = data.Value;
            card.GetComponent<MonoBehaviour>().enabled = true;
        }
    }



    IEnumerator summonAll(KeyValuePair<int, int> data)
    {
        GameObject card = Instantiate(卡牌, layout.transform);
        card.GetComponent<cardShow>().卡牌数据 = ValueHolder.gloabCaedData[data.Key];

        // 设置卡牌图片
        GetComponent<mainfunction>().Loadimages(card,4, data.Key);
        card.GetComponent<cardShow>().num = data.Value;
        card.GetComponent<MonoBehaviour>().enabled = true;
        yield return 0;
    }




    public void InstantinateCard(string level = "ALL", string typ = "总")
    {
        int allnum = 0;
        //Debug.Log(level + ":" + typ);
        mainfunction.DestroyAllChildren(layout);
        if (level == "ALL" && typ == "总")
        {
            mainfunction.DestroyAllChildren(layout);
            Dictionary<int, int> cardData = SubtractDictionary(ValueHolder.cardData,ValueHolder.cardDataexsist);
            foreach (KeyValuePair<int, int> data in cardData)
            {
                if (data.Value > 0)
                {
                    StartCoroutine(summonAll(data));
                    allnum++;
                }

            }
        }
        if (level == "ALL" && typ != "总")
        {
            mainfunction.DestroyAllChildren(layout);
            Dictionary<int, int> cardData = SubtractDictionary(ValueHolder.cardData, ValueHolder.cardDataexsist);
            foreach (KeyValuePair<int, int> data in cardData)
            {
                卡牌数据 cardD = ValueHolder.gloabCaedData[data.Key];
                string typ0 = cardD.类别;
                if (typ0 == typ)
                {
                    StartCoroutine(summonAll(data));
                    allnum++;
                }

            }
        }
        if (level != "ALL" && typ == "总")
        {
            mainfunction.DestroyAllChildren(layout);
            Dictionary<int, int> cardData = SubtractDictionary(ValueHolder.cardData, ValueHolder.cardDataexsist);
            foreach (KeyValuePair<int, int> data in cardData)
            {
                卡牌数据 cardD = ValueHolder.gloabCaedData[data.Key];
                string level0 = cardD.等级;
                if (level0 == "SS" || level0 == "SSS")
                {
                    level0 = "S+";
                }
                if (level0 == level)
                {
                    StartCoroutine(summonAll(data));
                    allnum++;
                }
            }
        }
        if (level != "ALL" && typ != "总")
        {
            mainfunction.DestroyAllChildren(layout);
            Dictionary<int, int> cardData = SubtractDictionary(ValueHolder.cardData, ValueHolder.cardDataexsist);
            foreach (KeyValuePair<int, int> data in cardData)
            {
                卡牌数据 cardD = ValueHolder.gloabCaedData[data.Key];
                string level0 = cardD.等级;
                string typ0 = cardD.类别;
                if (level0 == "SS" || level0 == "SSS")
                {
                    level0 = "S+";
                }
                if (level0 == level && typ0 == typ)
                {
                    StartCoroutine(summonAll(data));
                    allnum++;
                }
            }
        }
        cardallnum.text = allnum.ToString();
    }
}
