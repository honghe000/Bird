using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class b_nexturn : MonoBehaviour
{

    public Button �¸��غ�;
    public TextMeshProUGUI ����;
    public TextMeshProUGUI �غ���;
    public HintManager HintManager;
    // Start is called before the first frame update
    void Start()
    {
        �¸��غ�.onClick.AddListener(NextTurn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NextTurn()
    {

        NextTurn_First();

        StartCoroutine(NextTurn_Second_Coroutine());

        
    }
    private IEnumerator NextTurn_Second_Coroutine()
    {
        while (SkillExecutor.skillQueue.Count > 0)
        {
            yield return new WaitForSeconds(0.2f);
        }

        NextTurn_Second();

    }
    void NextTurn_First()
    {
        �¸��غ�.interactable = false;
        �¸��غ�.image.color = Color.gray;
        skillturn();
    }

    void NextTurn_Second()
    {
        ValueHolder.point = 0;
        ����.text = ValueHolder.point.ToString();
        ValueHolder.is_myturn = 0;
        del_action();




        ValueHolder.turn += 0.5f;
        �غ���.text = ((int)Mathf.Floor(ValueHolder.turn)).ToString();

        mainfunction.Ч��ж�ر���();

        if (ValueHolder.�غϽ������� == 1)
        {
            mainfunction.����();
        }
        else
        {
            mainfunction.ChangeSendMessage("Action", 12);
            ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
            mainfunction.����ʱ�غϱ仯();
            �ж������();
            HintManager.AddHint("�з��غϣ�");
        }
    }


    void �ж������()
    {
        foreach (GameObject card in mainfunction.��ȡ�ҷ�ȫ������())
        {
            if (card != null)
            {
                card.GetComponent<MoveController>().�ж��� = 0;
            }
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
                    SkillExecutor.EnqueueSkill(skill, skill.Action_1);
                }
                if (skill.activateTurn_2_finish == 0 && skill.activateTurn_2 == ValueHolder.turn)
                {
                    SkillExecutor.EnqueueSkill(skill, skill.Action_2);
                }
                if (skill.activateTurn_3_finish == 0 && skill.activateTurn_3 == ValueHolder.turn)
                {
                    SkillExecutor.EnqueueSkill(skill, skill.Action_3);
                }
                if (skill.activateTurn_4_finish == 0 && skill.activateTurn_4 == ValueHolder.turn)
                {
                    SkillExecutor.EnqueueSkill(skill, skill.Action_4);
                }
            }
            mainfunction.DestroyAllChildren(ValueHolder.�Ŵ�չʾ��1);
            mainfunction.DestroyAllChildren(ValueHolder.�Ŵ�չʾ��2);

            if (skill.�����غϽ���ʱ���� == 1)
            {
                mainfunction.���м����غϽ���ʱ�������ܽ׶�(skill);
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
        foreach (GameObject item in ValueHolder.����.Values)
        {
            if (item.transform.gameObject.name == "����")
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
