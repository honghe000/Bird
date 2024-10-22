using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class b_choose_fa : MonoBehaviour, IPointerClickHandler
{
    private void Start()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        int cardType = eventData.pointerClick.gameObject.GetComponent<MoveController>().cardType;
        BaseSkill skill = ValueHolder.SkillAction[ValueHolder.释放法术uid];
        skill.作用目标卡牌 = eventData.pointerClick.gameObject;

        if (skill.效果 == "消灭" && eventData.pointerClick.gameObject.GetComponent<MoveController>().消灭免疫 == 1)
        {
            ValueHolder.hintManager.AddHint("此牌无法被消灭！");
            return;
        }else if (skill.效果 == "眩晕" && eventData.pointerClick.gameObject.GetComponent<MoveController>().眩晕免疫 == 1)
        {
            ValueHolder.hintManager.AddHint("此牌无法被眩晕！");
            return;
        }


        mainfunction.启用棋盘物件代码("b_moveca", 0);
        mainfunction.启用手牌物件代码("b_cardaction");
        mainfunction.HideCardchoose();
        ValueHolder.法术选择取消.gameObject.SetActive(false);
        mainfunction.禁用棋盘物件代码("b_choose_fa", cardType);
        mainfunction.运行下个技能阶段(skill);

    }
}
