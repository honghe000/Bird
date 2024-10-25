using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class b_cardaction : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler, IPointerClickHandler
{
    public GameObject canvas;
    public CanvasGroup CanvasGroup;
    public GameObject commoncard;
    public GameObject 手牌;
    public GameObject backpics;
    public HintManager hintManager;

    //private GameObject copycard;
    private float scale_x;
    private float scale_y;
    private Vector2 startpos;
    private void Start()
    {
        scale_x = canvas.transform.localScale.x;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        ValueHolder.拖拽序号 = transform.GetSiblingIndex();
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.alpha = 0.5f;
        transform.SetParent(canvas.transform);
        // 获取当前的子物体数量
        int childCount = canvas.transform.childCount;

        transform.SetSiblingIndex(childCount-3);
        startpos = transform.position;

        //更新卡牌内部的值，判断是否可以放置
        卡牌数据 card_data = eventData.pointerDrag.GetComponent<数据显示>().卡牌数据;
        eventData.pointerDrag.GetComponent<MoveController>().is_myturn = ValueHolder.is_myturn;
        eventData.pointerDrag.GetComponent<MoveController>().point = ValueHolder.point;
        //提示可以拖拽的放置的位置
        List<int> avalibaleDrag = eventData.pointerDrag.GetComponent<MoveController>().GetAvailableDrag();

        set_color(card_data.类别,avalibaleDrag);
    }

    public void OnDrag(PointerEventData eventData)
    {


        if(ValueHolder.is_myturn == 0)
        {
            return;
        }
        else
        {
            transform.position = Input.mousePosition;
        }



    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CanvasGroup.alpha = 1.0f;
        CanvasGroup.blocksRaycasts = true;
        if (is_out(Input.mousePosition, transform.gameObject) == 1) {
            transform.SetParent(ValueHolder.手牌区.transform);
            transform.SetSiblingIndex(ValueHolder.拖拽序号);
        }

        // 清除可防止提示
        List<int> avalibaleDrag = eventData.pointerDrag.GetComponent<MoveController>().GetAvailableDrag();

        set_color_white(avalibaleDrag);
        if (ValueHolder.is_myturn == 1)
        {
            法术(eventData);
        }


    }



    void 法术(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.GetComponent<数据显示>().卡牌数据.类别 != "法术")
        {
            return;
        }

        if (法术禁用(eventData) == 0)
        {
            return;
        }



        List<int> avalibaleDrag = eventData.pointerDrag.GetComponent<MoveController>().GetAvailableDrag();
        RectTransform targetRectTransform = ValueHolder.棋盘["0"].GetComponent<RectTransform>();

        // 获取当前鼠标位置
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetRectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localMousePosition
        );

        // 检查鼠标是否在目标物体的内部
        if (targetRectTransform.rect.Contains(localMousePosition))
        {
            int res = 消耗灵力(eventData);
            if (res == 0)
            {
                hintManager.AddHint("灵力不足！");
                return;
            }
            //技能初始化
            BaseSkill skill = SkillFactory.CreateSkill(eventData.pointerDrag, this);
            if (法术禁用(eventData) == 0)
            {
                return;
            }
            if (skill.activateTurn_1 == ValueHolder.turn)
            {
                skill.Action_1();
            }
            ValueHolder.释放法术uid = eventData.pointerDrag.GetComponent<数据显示>().卡牌数据.uid;
            if (eventData.pointerDrag.GetComponent<数据显示>().卡牌数据.uid == "0")
            {
                Debug.Log(eventData.pointerDrag.GetComponent<数据显示>().卡牌数据.名字);
            }
            ValueHolder.SkillAction.Add(eventData.pointerDrag.GetComponent<数据显示>().卡牌数据.uid, skill);
            Destroy(eventData.pointerDrag.gameObject);
        }
    }
    int 消耗灵力(PointerEventData eventData)
    {
        数据显示 card_show = eventData.pointerDrag.gameObject.GetComponent<数据显示>();
        int level = card_show.灵力消耗等级;
        int 灵力消耗 = card_show.卡牌数据.灵力消耗;
        if (灵力消耗 == -1)
        {
            return 1;
        }

        int res = mainfunction.灵力减少(level, 灵力消耗);
        return res;

    }
    void 敌方视角显示()
    {

    }

    int 法术禁用(PointerEventData eventData)
    {
        if (ValueHolder.法术禁用.Count == 0)
        {
            if (eventData.pointerDrag.gameObject.GetComponent<MoveController>().场上我方人数要求 > mainfunction.我方人物数量())
            {
                hintManager.AddHint("场上我方人物数量不足！");
                return 0;
            }else if (eventData.pointerDrag.gameObject.GetComponent<MoveController>().场上敌方人数要求 > mainfunction.敌方人物数量())
            {
                hintManager.AddHint("场上敌方人物数量不足！");
                return 0;
            }
            return 1;
        }
        else
        {
            foreach (KeyValuePair<string, float> kvp in ValueHolder.法术禁用)
            {
                hintManager.AddHint("该操作与" + kvp.Key + "冲突！");
            }
            return 0;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        return;
    }


    public Vector2 Limate_position(Vector2 mouse,GameObject card,GameObject Limtation)
    {
        RectTransform container = Limtation.GetComponent<RectTransform>();
        RectTransform cardRect = card.GetComponent<RectTransform>();

        float containerLeftEdge = Limtation.transform.position.x - container.rect.width/ 2f;
        float containerRightEdge = Limtation.transform.position.x + container.rect.width / 2f;
        float containerTopEdge = Limtation.transform.position.y + container.rect.height / 2f;
        float containerBottomEdge = Limtation.transform.position.y - container.rect.height / 2f;

        float cardWidth = cardRect.rect.width;
        float cardHeight = cardRect.rect.height;

        float min_x = containerLeftEdge + cardWidth / 2f;
        float max_x = containerRightEdge - cardWidth / 2f;
        float min_y = containerBottomEdge + cardHeight / 2f;
        float max_y = containerTopEdge - cardHeight / 2f;

        mouse = new Vector2(
            Mathf.Clamp(mouse.x, min_x, max_x),
            Mathf.Clamp(mouse.y, min_y, max_y)
        );

        return mouse;
    }

    public int is_out(Vector2 mouse, GameObject card)
    {
        RectTransform container = 手牌.GetComponent<RectTransform>();
        RectTransform cardRect = card.GetComponent<RectTransform>();

        float containerLeftEdge = 手牌.transform.position.x - container.rect.width / 2f;
        float containerRightEdge = 手牌.transform.position.x + container.rect.width / 2f;
        float containerTopEdge = 手牌.transform.position.y + container.rect.height / 2f;
        float containerBottomEdge = 手牌.transform.position.y - container.rect.height / 2f;

        float cardWidth = cardRect.rect.width;
        float cardHeight = cardRect.rect.height;

        float min_x = containerLeftEdge + cardWidth / 2f;
        float max_x = containerRightEdge - cardWidth / 2f;
        float min_y = containerBottomEdge + cardHeight / 2f;
        float max_y = containerTopEdge - cardHeight / 2f;

        if (mouse.x < min_x || mouse.x >  max_x || mouse.y < min_y || mouse.y > max_y)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }


    public void set_color(string type,List<int> availableMoves)
    {
        foreach (int i in availableMoves)
        {
            if (mainfunction.判断是否处于禁用位置(type,i) != "无")
            {
                Color red = new Color(0.9f, 0f, 0f, 0.6f);
                ValueHolder.棋盘[i.ToString()].GetComponent<UnityEngine.UI.Image>().color = red;
            }
            else
            {
                Color grenn = new Color(0.4f, 0.9f, 0.5f, 0.3f);
                ValueHolder.棋盘[i.ToString()].GetComponent<UnityEngine.UI.Image>().color = grenn;
            }

        }
    }

    public void set_color_white(List<int> availableMoves)
    {
        foreach (int i in availableMoves)
        {
            if (i == 0)
            {
                ValueHolder.棋盘[i.ToString()].GetComponent<UnityEngine.UI.Image>().color = Color.white;
                continue;
            }
            Color grenn = new Color(0.4f, 0.9f, 0.5f, 0f);
            ValueHolder.棋盘[i.ToString()].GetComponent<UnityEngine.UI.Image>().color = grenn;
        }
    }
}
