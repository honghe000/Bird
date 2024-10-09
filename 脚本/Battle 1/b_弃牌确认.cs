using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class b_弃牌确认 : MonoBehaviour
{
    public Button 确认;

    // Start is called before the first frame update
    void Start()
    {
        确认.onClick.AddListener(确认点击);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void 确认点击()
    {
        if (ValueHolder.弃牌数量.text != "0")
        {
            ValueHolder.hintManager.AddHint("请正确弃置手牌！");
            return;
        }


        foreach (string uid in ValueHolder.弃牌uid)
        {
            for (int i = 0; i < ValueHolder.手牌区.transform.childCount; i++)
            {
                Transform child = ValueHolder.手牌区.transform.GetChild(i);
                数据显示 cardData = child.GetComponent<数据显示>();

                if (cardData != null && cardData.卡牌数据.uid == uid)
                {
                    Destroy(child.gameObject);
                    break; 
                }
            }
        }

        for (int i = 0; i < ValueHolder.弃牌区.transform.childCount; i++)
        {
            Destroy(ValueHolder.弃牌区.transform.GetChild(i).gameObject);
        }
        ValueHolder.弃牌显示.SetActive(false);


        mainfunction.ChangeSendMessage("Action", 12);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
        mainfunction.倒计时回合变化();
        ValueHolder.hintManager.AddHint("敌方回合！");
    }
}
