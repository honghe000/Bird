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
        BaseSkill skill = ValueHolder.SkillAction[ValueHolder.�ͷŷ���uid];
        skill.����Ŀ�꿨�� = eventData.pointerClick.gameObject;

        if (skill.Ч�� == "����" && eventData.pointerClick.gameObject.GetComponent<MoveController>().�������� == 1)
        {
            ValueHolder.hintManager.AddHint("�����޷�������");
            return;
        }else if (skill.Ч�� == "ѣ��" && eventData.pointerClick.gameObject.GetComponent<MoveController>().ѣ������ == 1)
        {
            ValueHolder.hintManager.AddHint("�����޷���ѣ�Σ�");
            return;
        }


        mainfunction.���������������("b_moveca", 0);
        mainfunction.���������������("b_cardaction");
        mainfunction.HideCardchoose();
        ValueHolder.����ѡ��ȡ��.gameObject.SetActive(false);
        mainfunction.���������������("b_choose_fa", cardType);
        mainfunction.�����¸����ܽ׶�(skill);

    }
}
