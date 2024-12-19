using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 扣发触发确定 : MonoBehaviour
{
    public Button 扣发确定;

    void Start()
    {
        扣发确定.onClick.AddListener(扣发确定点击);
    }

    public void 扣发确定点击()
    {
        GameObject card = ValueHolder.扣发grid.transform.GetChild(0).gameObject;
        卡牌数据 carddata = card.GetComponent<数据显示>().卡牌数据;

        BaseSkill skill = ValueHolder.扣发技能[carddata.uid];

        mainfunction.运行扣发_敌方攻击技能阶段(skill);

        ValueHolder.幕布.SetActive(false);
        ValueHolder.扣发显示.SetActive(false);
        mainfunction.技能释放结束();
        ValueHolder.等待攻击扣发技能数量 -= 1;

    }
}
