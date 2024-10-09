using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class 卡组接收 : MonoBehaviour, IDropHandler
{
    public GameObject laySS;
    public GameObject layS;
    public GameObject layA;
    public GameObject layB;
    public GameObject layCD;
    public GameObject cardSmall;
    public TextMeshProUGUI SSnum;
    public TextMeshProUGUI Snum;
    public TextMeshProUGUI Anum;
    public TextMeshProUGUI Bnum;
    public TextMeshProUGUI CDnum;
    public TextMeshProUGUI rennum;
    public TextMeshProUGUI fanum;
    public TextMeshProUGUI zhunum;


    private void Start()
    {
        List<string> existlist = ConvertDictionaryStringToList(JsonConvert.SerializeObject(ValueHolder.cardgroupchoose));
        foreach (string s in existlist)
        {
            卡牌数据 carddata = ValueHolder.gloabCaedData[int.Parse(s.Trim('"'))];
            string level = carddata.等级.ToString();



            if (ValueHolder.cardDataexsist.ContainsKey(carddata.id))
            {
                ValueHolder.cardDataexsist[carddata.id] += 1;
            }
            else
            {
                ValueHolder.cardDataexsist.Add(carddata.id, 1);
            }

            if (level == "SS" || level == "SSS")
            {
                sum(laySS, carddata, SSnum);
            }
            else if (level == "S")
            {
                sum(layS, carddata, Snum);
            }
            else if (level == "A")
            {
                sum(layA, carddata, Anum);
            }
            else if (level == "B")
            {
                sum(layB, carddata, Bnum);
            }
            else
            {
                sum(layCD, carddata, CDnum);
            }
        }
    }

    public void sum(GameObject laywhere, 卡牌数据 carddata,TextMeshProUGUI num)
    {
        Transform cardalready =  laywhere.transform.Find(carddata.名字);

        if (cardalready != null)
        {
            int index = cardalready.transform.GetSiblingIndex();
            GameObject newcard = Instantiate(cardSmall, laywhere.transform);
            newcard.GetComponent<MonoBehaviour>().enabled = true;
            newcard.GetComponent<smallCard>().卡牌数据 = carddata;
            newcard.transform.SetSiblingIndex(index);
            newcard.name = carddata.名字;
            ValueHolder.isplace = true;
            if (newcard.GetComponent<smallCard>().卡牌数据.类别 == "角色")
            {
                rennum.text = (int.Parse(rennum.text) + 1).ToString();
            }
            else if (newcard.GetComponent<smallCard>().卡牌数据.类别 == "法术")
            {
                fanum.text = (int.Parse(fanum.text) + 1).ToString();
            }
            else
            {
                zhunum.text = (int.Parse(zhunum.text) + 1).ToString();
            }
        }
        else
        {
            GameObject newcard = Instantiate(cardSmall, laywhere.transform);
            newcard.GetComponent<MonoBehaviour>().enabled = true;
            newcard.GetComponent<smallCard>().卡牌数据 = carddata;
            newcard.name = carddata.名字;
            ValueHolder.isplace = true;
            if(newcard.GetComponent<smallCard>().卡牌数据.类别 == "角色")
            {
                rennum.text = (int.Parse(rennum.text) + 1).ToString();
            }else if (newcard.GetComponent<smallCard>().卡牌数据.类别 == "法术")
            {
                fanum.text = (int.Parse(fanum.text) + 1).ToString();
            }
            else
            {
                zhunum.text = (int.Parse(zhunum.text) + 1).ToString();
            }
        }

        num.text = laywhere.transform.childCount.ToString();

        
    }



    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.transform.parent.gameObject.name == "Canvas")
        {
            卡牌数据 carddata = eventData.pointerDrag.GetComponent<cardShow>().卡牌数据;
            string level = carddata.等级.ToString();



            if (ValueHolder.cardDataexsist.ContainsKey(carddata.id))
            {
                ValueHolder.cardDataexsist[carddata.id] += 1;
            }
            else
            {
                ValueHolder.cardDataexsist.Add(carddata.id, 1);
            }

            if (level == "SS" || level == "SSS")
            {
                sum(laySS, carddata, SSnum);
            }
            else if (level == "S")
            {
                sum(layS, carddata, Snum);
            }
            else if (level == "A")
            {
                sum(layA, carddata, Anum);
            }
            else if (level == "B")
            {
                sum(layB, carddata, Bnum);
            }
            else
            {
                sum(layCD, carddata, CDnum);
            }


        }

    }


    public static List<string> ConvertDictionaryStringToList(string input)
    {
        List<string> resultList = new List<string>();

        // 去除字符串中的大括号，然后按逗号分隔成键值对数组
        string[] pairs = input.Replace("{", "").Replace("}", "").Split(',');

        foreach (string pair in pairs)
        {
            // 再次分隔每个键值对，获取键和值
            string[] parts = pair.Split(':');

            if (parts.Length == 2 && int.TryParse(parts[1], out int count))
            {
                // 将键重复添加到结果列表中，次数由值决定
                for (int i = 0; i < count; i++)
                {
                    resultList.Add(parts[0]);
                }
            }
            else
            {
                // 处理无效的输入格式
                Console.WriteLine("Invalid input format: " + pair);
            }
        }

        return resultList;
    }
}

