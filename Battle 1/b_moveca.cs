using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class b_moveca : MonoBehaviour,IPointerClickHandler
{
    public GameObject commoncard;

    private void Start()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.pointerClick.GetComponent<数据显示>().卡牌数据.类别 == "建筑")
        {
            return;
        }

        if (eventData.pointerClick.GetComponent<MoveController>().眩晕 == 1)
        {
            ValueHolder.hintManager.AddHint("此牌被眩晕，无法行动！");
            return;
        }

        if (ValueHolder.point <= 0 && eventData.pointerClick.GetComponent<MoveController>().行动点 <= 0)
        {
            ValueHolder.hintManager.AddHint("体力不足！");
            return;
        }

        //选中一张卡时，处理点击另一张卡的情况
        if (ValueHolder.choosed_object!= null && eventData.pointerClick.GetComponent<CanvasGroup>().alpha == 1f)
        {
            List<int> availableMoves = ValueHolder.choosed_object.GetComponent<MoveController>().GetAvailableMoves(int.Parse(ValueHolder.choosed_object.transform.parent.name));
            mainfunction.格子颜色还原();
            ValueHolder.choosed_object.GetComponent<CanvasGroup>().alpha = 1f;
        }
        if (eventData.pointerClick.GetComponent<CanvasGroup>().alpha == 1f)
        {
            eventData.pointerClick.GetComponent<CanvasGroup>().alpha = 0.8f;
            ValueHolder.is_choose = 1;
            ValueHolder.choosed_object = eventData.pointerClick;

            List<int> availableMoves = ValueHolder.choosed_object.GetComponent<MoveController>().GetAvailableMoves(int.Parse(eventData.pointerClick.transform.parent.name));
            mainfunction.格子绿色显示(availableMoves);

        }
        else
        {
            eventData.pointerClick.GetComponent<CanvasGroup>().alpha = 1f;
            ValueHolder.is_choose = 0;
            List<int> availableMoves = ValueHolder.choosed_object.GetComponent<MoveController>().GetAvailableMoves(int.Parse(eventData.pointerClick.transform.parent.name));
            mainfunction.格子颜色还原();
            ValueHolder.choosed_object = null;
        }

    }




}
