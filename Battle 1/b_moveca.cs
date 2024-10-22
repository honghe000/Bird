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
    private GameObject copycard;
    //public GameObject canvas;
    //private float scale_x;

    //private void Start()
    //{
    //    scale_x = canvas.transform.localScale.x;
    //}

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (ValueHolder.can_copycard == 1 && ValueHolder.is_choose == 0)
    //    {
    //        ValueHolder.can_copycard = 0;

    //        // 生成卡牌复制品
    //        copycard = Instantiate(commoncard);

    //        // 复制卡牌数据
    //        copycard.GetComponent<数据显示>().卡牌数据 = transform.gameObject.GetComponent<数据显示>().卡牌数据;
    //        copycard.GetComponent<数据显示>().enabled = true;


    //        // 设置复制品的父物体
    //        copycard.transform.SetParent(canvas.transform, false);

    //        // 获取复制品的RectTransform
    //        RectTransform magnifiedRect = copycard.GetComponent<RectTransform>();

    //        // 设置复制品的缩放比例
    //        float scaleFactor = 1.8f; // 放大1.5倍（你可以根据需要调整此值）
    //        magnifiedRect.localScale = new Vector3(scaleFactor, scaleFactor, 1);

    //        // 计算放大后的位置（水平放置在原始卡牌的左边）
    //        float adjustedWidth = transform.gameObject.GetComponent<RectTransform>().rect.width * scaleFactor * scale_x;

    //        float new_x = transform.position.x - adjustedWidth;
    //        float new_y = transform.position.y;
    //        float new_z = transform.position.z;


    //        if ((transform.position.y + transform.gameObject.GetComponent<RectTransform>().rect.height * scaleFactor / 2f) > Screen.height)
    //        {
    //            new_y = Screen.height - transform.gameObject.GetComponent<RectTransform>().rect.height * scaleFactor / 2f;
    //        }
    //        else if ((transform.position.y - transform.gameObject.GetComponent<RectTransform>().rect.height * scaleFactor / 2f) < 0)
    //        {
    //            new_y = transform.gameObject.GetComponent<RectTransform>().rect.height * scaleFactor / 2f;
    //        }

    //        magnifiedRect.position = new Vector3(new_x, new_y,new_z);
    //    }

    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    // 删除实例化的物件
    //    if (copycard != null)
    //    {
    //        Destroy(copycard);
    //        ValueHolder.can_copycard = 1;
    //    }
    //}

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

        //选中一张卡时，处理点击另一张卡的情况
        if (ValueHolder.choosed_object!= null && eventData.pointerClick.GetComponent<CanvasGroup>().alpha == 1f)
        {
            List<int> availableMoves = ValueHolder.choosed_object.GetComponent<MoveController>().GetAvailableMoves(int.Parse(ValueHolder.choosed_object.transform.parent.name));
            set_color_white(availableMoves);
            ValueHolder.choosed_object.GetComponent<CanvasGroup>().alpha = 1f;
        }
        if (eventData.pointerClick.GetComponent<CanvasGroup>().alpha == 1f)
        {
            Destroy(copycard);
            eventData.pointerClick.GetComponent<CanvasGroup>().alpha = 0.8f;
            ValueHolder.is_choose = 1;
            ValueHolder.choosed_object = eventData.pointerClick;

            List<int> availableMoves = ValueHolder.choosed_object.GetComponent<MoveController>().GetAvailableMoves(int.Parse(eventData.pointerClick.transform.parent.name));
            set_color(availableMoves);

        }
        else
        {
            eventData.pointerClick.GetComponent<CanvasGroup>().alpha = 1f;
            ValueHolder.is_choose = 0;
            List<int> availableMoves = ValueHolder.choosed_object.GetComponent<MoveController>().GetAvailableMoves(int.Parse(eventData.pointerClick.transform.parent.name));
            set_color_white(availableMoves);
            ValueHolder.choosed_object = null;
        }

    }

    public void set_color(List<int> availableMoves)
    {
        foreach (int i in availableMoves)
        {
            Color grenn = new Color(0.4f, 0.9f, 0.5f, 0.3f);
            ValueHolder.棋盘[i.ToString()].GetComponent<UnityEngine.UI.Image>().color = grenn;
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
}
