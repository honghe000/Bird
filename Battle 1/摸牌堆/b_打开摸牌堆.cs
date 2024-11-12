using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class b_打开摸牌堆 : MonoBehaviour
{
    public Button 摸牌堆;
    void Start()
    {
        摸牌堆.onClick.AddListener(打开摸牌堆);
    }

    void 打开摸牌堆()
    {

        ValueHolder.摸牌堆.SetActive(true);
        ValueHolder.幕布.SetActive(true);

        for (int i = 0; i < ValueHolder.牌堆顶已知.Count; i++)
        {
            GameObject cardone = Instantiate(ValueHolder.弃牌堆卡牌);
            GameObject commoncard = mainfunction.卡牌生成(cardone, ValueHolder.牌堆顶已知[i]);
            commoncard.transform.SetParent(ValueHolder.摸牌堆显示区.transform);
        }

        Image 省略号 = Instantiate(ValueHolder.省略号);
        省略号.transform.SetParent(ValueHolder.摸牌堆显示区.transform);

        for (int i = 0;i < ValueHolder.牌堆底已知.Count; i++)
        {
            GameObject cardone = Instantiate(ValueHolder.弃牌堆卡牌);
            GameObject commoncard = mainfunction.卡牌生成(cardone, ValueHolder.牌堆底已知[i]);
            commoncard.transform.SetParent(ValueHolder.摸牌堆显示区.transform);
        }
    
    }
}
