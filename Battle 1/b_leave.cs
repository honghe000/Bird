using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class b_leave : MonoBehaviour
{
    public Button leave;
    // Start is called before the first frame update
    void Start()
    {
        leave.onClick.AddListener(leaveBattle);
    }

    void leaveBattle()
    {
        mainfunction.ChangeSendMessage("Action", 5);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
        ValueHolder.Room = "";
        ValueHolder.my_ready = 0;
        ValueHolder.he_ready = 0;
        ValueHolder.room_is_host = 0;
        ValueHolder.hename = "";


        ValueHolder.��ʱ������ = null;
        ValueHolder.���ƶ�ս�� = null;
        ValueHolder.initiative = 1;
        ValueHolder.random_card = new List<int> { 1, 2, 3, 4, 76 };
        ValueHolder.Discard_pile = new List<int>();
        ValueHolder.choosegroup = "666";
        ValueHolder.choosed_object = null;
        ValueHolder.copyed_object = null;
        ValueHolder.is_choose = 0;
        ValueHolder.���� = new Dictionary<string, GameObject>();
        ValueHolder.������ = null;
        ValueHolder.��ק��� = 0;
        ValueHolder.��ͼ = null;
        ValueHolder.turn = 1;
        ValueHolder.����ѡ��ȡ�� = null;

        ValueHolder.is_myturn = 1;
        ValueHolder.point = 1;

        ValueHolder.hintManager = null;
        ValueHolder.SkillAction = new Dictionary<string, BaseSkill>();

        ValueHolder.����ɷ���λ�� = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        ValueHolder.�����ɷ���λ�� = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        ValueHolder.�����ֹ����λ�� = new Dictionary<string, List<int>>();
        ValueHolder.������ֹ����λ�� = new Dictionary<string, List<int>>();

        ValueHolder.������ʱ������ = null;
        ValueHolder.�з���ʱ������ = null;
        ValueHolder.�ҷ���ʱ������ = null;

        ValueHolder.�������� = new Dictionary<string, float>();
        ValueHolder.������� = new Dictionary<string, float>();

        ValueHolder.�ͷŷ���uid = null;
        ValueHolder.uid_to_name = new Dictionary<string, string>();
        ValueHolder.�������õ������� = 0;
        ValueHolder.�Ŵ�չʾ��1 = null;
        ValueHolder.�Ŵ�չʾ��2 = null;

        ValueHolder.����ʱ���� = new Dictionary<string, float>();
        ValueHolder.����ʱ��ʾ1 = null;
        ValueHolder.����ʱ��ʾ2 = null;

        ValueHolder.���� = null;
        ValueHolder.������ = null;
        ValueHolder.�������� = null;
        ValueHolder.������ʾ = null;
        ValueHolder.����uid = new List<string>();
        ValueHolder.������������ = 0;
        ValueHolder.�غϽ������� = 0;
        SceneManager.LoadScene("mainpage");
    }
}
