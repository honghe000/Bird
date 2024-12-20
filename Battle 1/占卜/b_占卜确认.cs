using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class b_占卜确认 : MonoBehaviour
{
    public Button 占卜确认按钮;
    void Start()
    {
        占卜确认按钮.onClick.AddListener(占卜确认);
    }

    void 占卜确认()
    {
        List<int> StartID = new List<int>();
        List<int> EndID = new List<int>();
        for (int i = 0; i < ValueHolder.占卜牌堆顶.transform.childCount; i++)
        {
            StartID.Add(ValueHolder.占卜牌堆顶.transform.GetChild(i).GetComponent<数据显示>().卡牌数据.id);
        }

        for (int i = 0; i < ValueHolder.占卜牌堆底.transform.childCount; i++)
        {
            EndID.Add(ValueHolder.占卜牌堆底.transform.GetChild(i).GetComponent<数据显示>().卡牌数据.id);
        }


        //牌堆顶
        if (ValueHolder.牌堆顶已知.Count <= StartID.Count)
        {
            ValueHolder.牌堆顶已知.Clear();
            ValueHolder.牌堆顶已知.AddRange(StartID);
        }
        else
        {
            ValueHolder.牌堆顶已知.RemoveRange(0, StartID.Count);
            ValueHolder.牌堆顶已知.InsertRange(0,StartID);
        }

        ValueHolder.牌堆底已知.AddRange(EndID);

        ValueHolder.random_card.RemoveRange(0, StartID.Count + EndID.Count);
        ValueHolder.random_card.InsertRange(0, StartID);
        ValueHolder.random_card.AddRange(EndID);


        mainfunction.DestroyAllChildren(ValueHolder.占卜牌堆顶);
        mainfunction.DestroyAllChildren(ValueHolder.占卜牌堆底);

        ValueHolder.占卜数量 = 0;

        ValueHolder.占卜区.SetActive(false);
        ValueHolder.幕布.SetActive(false);
        ValueHolder.占卜确认按钮.SetActive(false);

        if (ValueHolder.释放法术uid != null && ValueHolder.占卜后立即执行技能 == 1)
        {
            if (ValueHolder.SkillAction.ContainsKey(ValueHolder.释放法术uid))
            {
                BaseSkill skill = ValueHolder.SkillAction[ValueHolder.释放法术uid];
                mainfunction.运行下个技能阶段(skill);
                ValueHolder.释放法术uid = null;
            }
        }

        mainfunction.技能释放结束();


    }

}
