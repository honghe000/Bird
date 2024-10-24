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
                send_summon_card(eventData.pointerDrag.GetComponent<数据显示>().卡牌数据.id, int.Parse(transform.name), eventData.pointerDrag.gameObject.GetComponent<数据显示>().卡牌数据.uid);
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.GetComponent<CanvasGroup>().alpha = 1.0f;
                eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;
                eventData.pointerDrag.GetComponent<b_cardaction>().enabled = false;
                eventData.pointerDrag.GetComponent<b_moveca>().enabled = true;
                Debug.Log(eventData.pointerDrag.gameObject.GetComponent<数据显示>().卡牌数据.uid);

                change_card_color(eventData.pointerDrag,"blue");
                set_color_white(avalibaleDrag);

                if (eventData.pointerDrag.GetComponent<数据显示>().卡牌数据.类别 == "角色")
                {
                    //更新体力
                    ValueHolder.point -= 1;
                    体力.text = ValueHolder.point.ToString();
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
        int level = card_show.灵力消耗等级;
        int num = int.Parse(card_show.灵力.text);
        int res = mainfunction.灵力减少(level, num);
        return res;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ValueHolder.choosed_object!= null && ValueHolder.point> 0)
        {
            int start_index = int.Parse(ValueHolder.choosed_object.transform.parent.name);
            int end_index = int.Parse(gameObject.name);
            List<int> availableMoves = ValueHolder.choosed_object.GetComponent<MoveController>().GetAvailableMoves(start_index);




            if (availableMoves.Contains(end_index))
            {
                send_move_card(start_index, end_index);
                ValueHolder.is_choose = 0;
                ValueHolder.choosed_object.GetComponent<CanvasGroup>().alpha = 1.0f;
                ValueHolder.choosed_object.GetComponent<CanvasGroup>().blocksRaycasts = true;
                ValueHolder.choosed_object.transform.SetParent(transform);
                Winmove(ValueHolder.choosed_object);

                ValueHolder.choosed_object = null;
                set_color_white(availableMoves);


                //更新体力
                ValueHolder.point -= 1;
                体力.text = ValueHolder.point.ToString();

            }

        }


    }

    public void set_color_white(List<int> availableMoves)
    {
        foreach (int i in availableMoves)
        {
            Color grenn = new Color(0.4f, 0.9f, 0.5f, 0f);
            ValueHolder.棋盘[i.ToString()].GetComponent<UnityEngine.UI.Image>().color = grenn;
        }
    }

    public int ConvertPosition(int position)
    {
        // 将位置编号转换为行和列坐标
        int row = (position - 1) / 5;
        int column = (position - 1) % 5;

        // 计算对手看到的行和列坐标
        int opponentRow = 5 - 1 - row;
        int opponentColumn = 5 - 1 - column;

        // 将对手看到的行和列坐标转换回位置编号
        int opponentPosition = (opponentRow * 5 + opponentColumn) + 1;

        return opponentPosition;
    }

    void send_summon_card(int cardID,int start_index,string uid)
    {
        start_index = ConvertPosition(start_index);
        mainfunction.ChangeSendMessage("Action", 9);
        mainfunction.ChangeSendMessage("cardID", cardID);
        mainfunction.ChangeSendMessage("start_index", start_index);
        mainfunction.ChangeSendMessage("uid", uid);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    void send_move_card(int start_index,int end_index)
    {
        start_index = ConvertPosition(start_index);
        end_index = ConvertPosition(end_index);
        mainfunction.ChangeSendMessage("Action", 10);
        mainfunction.ChangeSendMessage("start_index", start_index);
        mainfunction.ChangeSendMessage("end_index", end_index);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);

    }

    void change_card_color(GameObject card,string color)
    {
        foreach (Transform child in card.transform)
        {
            TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                if (color == "red")
                {
                    text.color = Color.red;
                }if (color == "blue")
                {
                    text.color = Color.blue;
                }
            }
        }
    }

    void Winmove(GameObject card) { 
        List<int> winpos = card.GetComponent<MoveController>().GetWinpos();

        if (winpos.Contains(int.Parse(card.transform.parent.name))){
            StartCoroutine(RotateAndScaleCoroutine(card));
            Debug.Log("win");
        }
    
    }

    IEnumerator RotateAndScaleCoroutine(GameObject card)
    {
        Vector3 originalScale = card.transform.localScale;
        float elapsedTime = 0f;

        float rotationSpeed = 360f;
        float fadeDuration = 2f; // 持续时间

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration; // 时间比例 [0, 1]

            // 计算缩放
            float scale = Mathf.Lerp(1f, 0f, t);
            card.transform.localScale = new Vector3(scale, scale, 1f);

            // 计算旋转
            float rotation = rotationSpeed * Time.deltaTime;
            card.transform.Rotate(Vector3.forward, rotation);

            yield return null;
        }

        // 确保最终状态
        card.transform.localScale = Vector3.zero;
        Destroy(card); // 可选择禁用物体
    }
}
