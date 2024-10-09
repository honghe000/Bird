using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class b_nexturn : MonoBehaviour
{

    public Button 下个回合;
    public TextMeshProUGUI 体力;
    public TextMeshProUGUI 回合数;
    public HintManager HintManager;
    // Start is called before the first frame update
    void Start()
    {
        下个回合.onClick.AddListener(NextTurn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NextTurn()
    {
        下个回合.interactable = false;
        下个回合.image.color = Color.gray;

        skillturn();

        ValueHolder.point = 0;
        体力.text = ValueHolder.point.ToString();
        ValueHolder.is_myturn = 0;
        del_action();




        ValueHolder.turn += 0.5f;
        回合数.text = ((int)Mathf.Floor(ValueHolder.turn)).ToString();

        if (ValueHolder.回合结束弃牌 == 1){
            mainfunction.弃牌();
        }
        else
        {
            mainfunction.ChangeSendMessage("Action", 12);
            ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
            mainfunction.倒计时回合变化();
            HintManager.AddHint("敌方回合！");
        }





    }

    void skillturn()
    {
        List<string> skillDelete = new List<string>();
        foreach (KeyValuePair<string, BaseSkill> item in ValueHolder.SkillAction)
        {
            string uid = item.Key;
            BaseSkill skill = item.Value;
            if (skill.skill_end == 0)
            {
                if (skill.activateTurn_1_finish == 0 && skill.activateTurn_1 == ValueHolder.turn)
                {
                    skill.Action_1();
                }
                if (skill.activateTurn_2_finish == 0 && skill.activateTurn_2 == ValueHolder.turn)
                {
                    skill.Action_2();
                }
                if (skill.activateTurn_3_finish == 0 && skill.activateTurn_3 == ValueHolder.turn)
                {
                    skill.Action_3();
                }
                if (skill.activateTurn_4_finish == 0 && skill.activateTurn_4 == ValueHolder.turn)
                {
                    skill.Action_4();
                }
            }
            mainfunction.DestroyAllChildren(ValueHolder.放大展示区1);
            mainfunction.DestroyAllChildren(ValueHolder.放大展示区2);

            if (skill.己方回合结束时触发 == 1)
            {
                mainfunction.运行下个技能阶段(skill);
            }

            if (skill.skill_end == 1)
            {
                skillDelete.Add(uid);
            }
        }

        foreach (string uid in skillDelete)
        {
            ValueHolder.SkillAction.Remove(uid);
        }

    }


    void del_action()
    {
        foreach (GameObject item in ValueHolder.棋盘.Values)
        {
            if (item.transform.gameObject.name == "棋盘")
            {
                continue;
            }
            if (item.transform.childCount > 0 && item.transform.GetChild(0).gameObject.GetComponent<MoveController>().cardType == 0)
            {
                item.transform.GetChild(0).gameObject.GetComponent<MoveController>().point = 0;
                item.transform.GetChild(0).gameObject.GetComponent<MoveController>().is_myturn = 0;
                item.transform.GetChild(0).gameObject.GetComponent<b_moveca>().enabled = false;
            }

        }
    }
    
}
