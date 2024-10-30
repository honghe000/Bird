using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class b_slot : MonoBehaviour,IDropHandler,IPointerClickHandler
{
    public TextMeshProUGUI 体力;
    public HintManager hintManager;
    public struct CardTurn
    {
        public int start_turn;
        public GameObject card;
    }

    public void OnDrop(PointerEventData eventData)
    {
        List<int> avalibaleDrag = new List<int>();
        try
        {
            avalibaleDrag = eventData.pointerDrag.GetComponent<MoveController>().GetAvailableDrag();
        }
        catch
        {
            return;
        }


        int index = int.Parse(gameObject.name);
        string 禁止放置来源 = mainfunction.判断是否处于禁用位置(eventData.pointerDrag.GetComponent<数据显示>().卡牌数据.类别,index);

        if (eventData.pointerDrag.GetComponent<数据显示>().卡牌数据.类别 == "角色" &&  禁止放置来源 != "无")
        {
            hintManager.AddHint("该操作与" +禁止放置来源 + "冲突！");
            return;
        }
        if (transform.childCount == 0 && avalibaleDrag.Contains(int.Parse(gameObject.name))) {

            if (eventData.pointerDrag.GetComponent<数据显示>().卡牌数据.类别 == "角色" || eventData.pointerDrag.GetComponent<数据显示>().卡牌数据.类别 == "建筑")
            {

                //人物禁用
                if (eventData.pointerDrag.GetComponent<数据显示>().卡牌数据.类别 == "角色" && ValueHolder.人物禁用.Count != 0)
                {
                    foreach (KeyValuePair<string, float> kvp in ValueHolder.人物禁用)
                    {
                        hintManager.AddHint("该操作与" + ValueHolder.uid_to_name[kvp.Key] + "冲突！");
                    }
                    return;
                }



                int res = 消耗灵力(eventData);
                if (res == 0)
                {
                    hintManager.AddHint("灵力不足！");
                    return;
                }
                mainfunction.Send卡牌生成(eventData.pointerDrag.GetComponent<数据显示>().卡牌数据.id, int.Parse(transform.name), eventData.pointerDrag.gameObject.GetComponent<数据显示>().卡牌数据.uid);
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.GetComponent<CanvasGroup>().alpha = 1.0f;
                eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;
                eventData.pointerDrag.GetComponent<b_cardaction>().enabled = false;
                eventData.pointerDrag.GetComponent<b_moveca>().enabled = true;
                Debug.Log(eventData.pointerDrag.gameObject.GetComponent<数据显示>().卡牌数据.uid);

                mainfunction.改变卡牌汉字颜色(eventData.pointerDrag,"blue");
                mainfunction.格子颜色还原();

                if (eventData.pointerDrag.GetComponent<数据显示>().卡牌数据.类别 == "角色")
                {
                    //更新体力
                    ValueHolder.point -= 1;
                    ValueHolder.体力.text = ValueHolder.point.ToString();
                }


                //技能初始化
                BaseSkill skill = SkillFactory.CreateSkill(eventData.pointerDrag,this);

                if (skill.activateTurn_1 == ValueHolder.turn)
                {
                    skill.Action_1();
                    Debug.Log("技能触发");
                }
                ValueHolder.SkillAction.Add(eventData.pointerDrag.GetComponent<数据显示>().卡牌数据.uid, skill);
            }
        }
    }

    int 消耗灵力(PointerEventData eventData)
    {
        数据显示 card_show = eventData.pointerDrag.gameObject.GetComponent<数据显示>();
        int level = card_show.卡牌数据.灵力消耗等级;
        int 灵力消耗 = card_show.卡牌数据.灵力消耗数量;
        if (灵力消耗 == -1)
        {
            return 1;
        }

        int res = mainfunction.灵力减少(level, 灵力消耗);
        return res;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //点选格子（召唤）
        if (ValueHolder.启用点选格子 == 1 && ValueHolder.点选技能uid.Count > 1)
        {
            int clicked_index = int.Parse(gameObject.name);
            ValueHolder.点击格子编号 = clicked_index;


            BaseSkill skill = ValueHolder.SkillAction[ValueHolder.点选技能uid.Dequeue()];
            mainfunction.运行下个技能阶段(skill);


            string 召唤物名称 = ValueHolder.gloabCaedData[ValueHolder.SkillAction[ValueHolder.点选技能uid.Peek()].召唤物id].名字;
            ValueHolder.hintManager.AddHint("请选择位置召唤：" + 召唤物名称);

            return;
        }

        if (ValueHolder.启用点选格子 == 1 && ValueHolder.点选技能uid.Count == 1)
        {
            mainfunction.启用棋盘物件代码("b_moveca", 0);
            mainfunction.启用手牌物件代码("b_cardaction");
            mainfunction.禁用棋盘物件代码("b_cardaction", 0);
            ValueHolder.下个回合.interactable = true;
            ValueHolder.下个回合.image.color = Color.white;
            mainfunction.格子颜色还原();

            mainfunction.Send对方继续();



            int clicked_index = int.Parse(gameObject.name);
            ValueHolder.点击格子编号 = clicked_index;


            BaseSkill skill = ValueHolder.SkillAction[ValueHolder.点选技能uid.Dequeue()];
            mainfunction.运行下个技能阶段(skill);

            ValueHolder.启用点选格子 = 0;
            return;
        }


        //点击移动
        if (ValueHolder.choosed_object!= null)
        {
            int start_index = int.Parse(ValueHolder.choosed_object.transform.parent.name);
            int end_index = 0;
            try
            {
                end_index = int.Parse(gameObject.name);
            }
            catch
            {
                Debug.Log(gameObject.name);
                return;
            }

            List<int> availableMoves = ValueHolder.choosed_object.GetComponent<MoveController>().GetAvailableMoves(start_index);
            int 行动点 = ValueHolder.choosed_object.GetComponent<MoveController>().行动点;



            if (availableMoves.Contains(end_index))
            {
                if (ValueHolder.point < 0 && 行动点 < 0)
                {
                    return;
                }

                mainfunction.Send卡牌移动(start_index, end_index);
                ValueHolder.is_choose = 0;
                ValueHolder.choosed_object.GetComponent<CanvasGroup>().alpha = 1.0f;
                ValueHolder.choosed_object.GetComponent<CanvasGroup>().blocksRaycasts = true;
                ValueHolder.choosed_object.transform.SetParent(transform);
                //Winmove(ValueHolder.choosed_object);




                if (行动点 > 0)
                {
                    ValueHolder.choosed_object.GetComponent<MoveController>().行动点 -= 1;
                }
                else
                {
                    ValueHolder.point -= 1;
                    体力.text = ValueHolder.point.ToString();
                }

                ValueHolder.choosed_object = null;
                mainfunction.格子颜色还原();


            }

        }


    }

    void Winmove(GameObject card) { 
        List<int> winpos = card.GetComponent<MoveController>().GetWinpos();

        if (winpos.Contains(int.Parse(card.transform.parent.name))){
            mainfunction.卡牌摧毁(card);
            Debug.Log("win");
        }
    
    }
}
