using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class b_enemy : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI 体力;
    private void Start()
    {
       
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ValueHolder.倒计时显示1.gameObject.SetActive(false);
        ValueHolder.倒计时显示2.gameObject.SetActive(false);
        int enemy_index = int.Parse(this.transform.parent.name);
        if (ValueHolder.choosed_object != null)
        {

            int start_index = int.Parse(ValueHolder.choosed_object.transform.parent.name);
            List<int> availableMoves = ValueHolder.choosed_object.GetComponent<MoveController>().GetAvailableAtack(start_index);
            ValueHolder.choosed_object.GetComponent<CanvasGroup>().alpha = 1f;
            int 行动点 = ValueHolder.choosed_object.GetComponent<MoveController>().行动点;

            mainfunction.格子颜色还原();
            if (availableMoves.Contains(enemy_index))
            {
                if (ValueHolder.point < 0 && 行动点 < 0)
                {
                    return;
                }

                if (ValueHolder.copyed_object != null)
                {
                    Destroy(ValueHolder.copyed_object);
                    Debug.Log("copyed_object");
                }

                if (效果判断(eventData) == 0)
                {
                    return;
                }
                //mainfunction.cardAttack(int.Parse(ValueHolder.choosed_object.transform.parent.name), int.Parse(this.gameObject.transform.parent.name), 0);
                mainfunction.Send攻击申请(ValueHolder.choosed_object.GetComponent<数据显示>().卡牌数据.uid,this.gameObject.GetComponent<数据显示>().卡牌数据.uid);

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


            }


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

    int 效果判断(PointerEventData eventData)
    {
        if (eventData.pointerClick.gameObject.GetComponent<MoveController>().击杀免疫 == 1)
        {
            ValueHolder.hintManager.AddHint("此牌对击杀免疫！");
            return 0;
        }
        return 1;
    }
}
