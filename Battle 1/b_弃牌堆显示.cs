using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class b_弃牌堆显示 : MonoBehaviour
{
    public Button 弃牌堆;
    // Start is called before the first frame update
    void Start()
    {
        弃牌堆.onClick.AddListener(弃牌显示);
    }

    void 弃牌显示()
    {
        List<int> lists = new List<int>();
        if (弃牌堆.transform.name == "我方弃牌堆")
        {
            lists = ValueHolder.我方弃牌堆卡牌编号;
        }
        else if (弃牌堆.transform.name == "敌方弃牌堆")
        {
            lists = ValueHolder.敌方弃牌堆卡牌编号;
        }

        for (int i = 0; i < lists.Count; i++)
        {
            GameObject cardone = Instantiate(ValueHolder.弃牌堆卡牌);

            GameObject commoncard = mainfunction.卡牌生成(cardone, lists[i]);

            commoncard.transform.SetParent(ValueHolder.弃牌堆.transform);
        }

        ValueHolder.幕布.SetActive(true);
        ValueHolder.弃牌堆.SetActive(true);
        ValueHolder.弃牌堆关闭.gameObject.SetActive(true);
    }
}
