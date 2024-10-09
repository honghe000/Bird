using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;




/// <summary>
/// /////////////////////////////////////////////////////////////////////////
/// </summary>

public class �������ʦ : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 3f;
    public �������ʦ(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn + delay;
        activateTurn_2 = -1;
        activateTurn_3 = -1;
        activateTurn_4 = -1;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<������ʾ>().��������.uid;
        initialization();


    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "�������ʦ");
        }
        ValueHolder.����ʱ����.Add(uid, delay);
        mainfunction.Send����ʱ(uid,(int)delay);
    }

    public override void Action_1()
    {
        if (card == null)
        {
            skill_end = 1;
            return;
        }
        int card_id = int.Parse(card.transform.parent.name);
        List<int> killGrid = Grids.GetNeighbors_��(card_id);
        Dictionary<int, GameObject> killCards = mainfunction.����_destroy(killGrid);


        foreach (KeyValuePair<int, GameObject> kvp in killCards)
        {
            Debug.Log("Destroying card: " + kvp.Value.name);
            monoBehaviour.StartCoroutine(RotateAndScaleCoroutine(kvp.Value));

            mainfunction.ChangeSendMessage("Action", 13);
            mainfunction.ChangeSendMessage("end_index",kvp.Key);
            ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
        }

        monoBehaviour.StartCoroutine(RotateAndScaleCoroutine(card));

        mainfunction.ChangeSendMessage("Action", 13);
        mainfunction.ChangeSendMessage("end_index", card_id);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);

        activateTurn_1_finish = 1;
        skill_end = 1;
    }

    public override void Action_2()
    {
        activateTurn_2_finish = 1;
    }

    public override void Action_3()
    {
        activateTurn_3_finish = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}

public class �ల����: BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 2f;
    public �ల����(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = ValueHolder.turn + delay;
        activateTurn_3 = -1;
        activateTurn_4 = -1;
        this.monoBehaviour = monoBehaviour;

        card_data = card.GetComponent<������ʾ>().��������;


        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<������ʾ>().��������.uid;
        initialization();


    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "�ల����");
        }
        ValueHolder.����ʱ����.Add(uid, delay);
        mainfunction.Send����ʱ(card_data.uid, (int)delay);
    }
    public override void Action_1()
    {
        GameObject cardone = Instantiate(ValueHolder.��ʱ������);
        GameObject card_summon = summon_one(cardone, card_data.id);
        card_summon.GetComponent<������ʾ>().��������.uid = uid;
        Debug.Log(uid);

        mainfunction.���ŵ���(card_summon);

        card_summon.transform.SetParent(ValueHolder.������ʱ������.transform);
        ValueHolder.��������.Add(card_data.����, activateTurn_2);

        mainfunction.Send������������(card_data.id);
        mainfunction.Send��������(card_data.id, activateTurn_2);
        activateTurn_1_finish = 1;
    }

    public override void Action_2()
    {
        Debug.Log("�ల����");
        foreach (Transform child in ValueHolder.������ʱ������.transform)
        {
            Debug.Log(child.gameObject.GetComponent<������ʾ>().��������.uid);
            if (child.gameObject.GetComponent<������ʾ>().��������.uid == uid)
            {
                ValueHolder.��������.Remove(child.gameObject.GetComponent<������ʾ>().��������.����);
                Destroy(child.gameObject);

                mainfunction.Send������������(card_data.id);
                mainfunction.Send��������ȡ��(card_data.id);
            }
        }
        skill_end = 1;
        activateTurn_2_finish = 1;
    }

    public override void Action_3()
    {
        activateTurn_3_finish = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}

public class ���� : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    //private �������� card_data;
    private int card_grid;
    public ����(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = -1;
        activateTurn_4 = -1;
        this.monoBehaviour = monoBehaviour;
        card_data = card.GetComponent<������ʾ>().��������;


        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<������ʾ>().��������.uid;
        initialization();


    }


    private void initialization()
    {
        card_grid = int.Parse(card.transform.parent.name);
        ���� = 1;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "����");
        }
    }
    public override void Action_1()
    {
        List<int> ��ֹ���� = Grids.GetColumnIndices(card_grid);
        mainfunction.Send����λ�ý���(card_data.id,��ֹ����);
        activateTurn_1_finish = 1;
    }

    public override void Action_2()
    {
        mainfunction.Send����λ�ý���ȡ��(card_data.id);

        activateTurn_2_finish = 1;
        skill_end = 1;
    }

    public override void Action_3()
    {
        activateTurn_3_finish = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}

public class �� : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public ��(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = -1;
        activateTurn_2 = -1;
        activateTurn_3 = -1;
        activateTurn_4 = -1;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<������ʾ>().��������.uid;
        initialization();


    }

    private void initialization()
    {
        ���� = 1;

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "��");
        }
    }
    public override void Action_1()
    {
        summonHandcard(1);
        activateTurn_1_finish = 1;
        skill_end = 1;
    }

    public override void Action_2()
    {
        activateTurn_2_finish = 1;
    }

    public override void Action_3()
    {
        activateTurn_3_finish = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}

public class ����֮�� : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 2f;
    public ����֮��(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = ValueHolder.turn + delay;
        activateTurn_3 = -1;
        activateTurn_4 = -1;

        card_data = card.GetComponent<������ʾ>().��������;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;


        uid =  card.GetComponent<������ʾ>().��������.uid;
        initialization();


    }
    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "����֮��");
        }
        ValueHolder.����ʱ����.Add(uid, delay);
        mainfunction.Send����ʱ(card_data.uid, (int)delay);
    }
    public override void Action_1()
    {
        GameObject cardone = Instantiate(ValueHolder.��ʱ������);

        GameObject card_summon = summon_one(cardone, card_data.id);
        card_summon.GetComponent<������ʾ>().��������.uid = uid;


        mainfunction.���ŵ���(card_summon);
        change_card_color(card_summon, "blue");


        card_summon.transform.SetParent(ValueHolder.�з���ʱ������.transform);


        mainfunction.Send�ҷ����Ʒ�������(card_data.id, uid);
        mainfunction.Send�������(uid, activateTurn_2);

        Debug.Log(uid);
        Debug.Log(card_summon.gameObject.GetComponent<������ʾ>().��������.uid);

        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        Debug.Log("�ݻ�");
        foreach (Transform child in ValueHolder.�з���ʱ������.transform)
        {
            //Debug.Log(uid);
            //Debug.Log(child.gameObject.GetComponent<������ʾ>().��������.uid);
            if (child.gameObject.GetComponent<������ʾ>().��������.uid == uid)
            {
                Destroy(child.gameObject);

                mainfunction.Send�ҷ����Ʒ�������(card_data.id,uid);
                mainfunction.Send�������ȡ��(uid);
            }
        }
        activateTurn_2_finish = 1;
        skill_end = 1;
    }

    public override void Action_3()
    {
        activateTurn_3_finish = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}


public class ��� : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 1f;
    public ���(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;

        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = ValueHolder.turn + delay;
        activateTurn_4 = -1;

        card_data = card.GetComponent<������ʾ>().��������;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<������ʾ>().��������.uid;
        initialization();


    }

    private void initialization()
    {
        card.GetComponent<MoveController>().�����ҷ�����Ҫ�� = 1;

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "���");
        }
    }
    public override void Action_1()
    {
        ValueHolder.�������õ������� = 0;
        mainfunction.���������������("b_moveca",0);
        mainfunction.���������������("b_cardaction");
        mainfunction.ShowCardchoose(0);
        mainfunction.���������������("b_choose_fa",0);
        ValueHolder.����ѡ��ȡ��.gameObject.SetActive(true);
        Debug.Log("ѡ��");
        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        GameObject cardone = Instantiate(ValueHolder.��ʱ������);
        GameObject card_summon = summon_one(cardone, card_data.id);
        card_summon.GetComponent<������ʾ>().��������.uid = uid;


        mainfunction.���ŵ���(card_summon);
        change_card_color(card_summon, "blue");

        card_summon.transform.SetParent(ValueHolder.�ҷ���ʱ������.transform);



        �������� ����Ŀ�꿨������ = ����Ŀ�꿨��.GetComponent<������ʾ>().��������;

        ����Ŀ�꿨��.GetComponent<MoveController>().��ɱ���� = 1;
        ����Ŀ�꿨��.GetComponent<MoveController>().�������� = 1;

        mainfunction.Send�з����Ʒ�������(card_data.id, uid);
        mainfunction.SendЧ������(����Ŀ�꿨������.uid, 1);
        mainfunction.SendЧ������(����Ŀ�꿨������.uid, 2);

        ValueHolder.����ʱ����.Add(uid, delay);
        mainfunction.Send����ʱ(card_data.uid, (int)delay);

        activateTurn_2_finish = 1;
    }

    public override void Action_3()
    {
        foreach (Transform child in ValueHolder.�ҷ���ʱ������.transform)
        {
            if (child.gameObject.GetComponent<������ʾ>().��������.uid == uid)
            {
                Destroy(child.gameObject);

                �������� ����Ŀ�꿨������ = ����Ŀ�꿨��.GetComponent<������ʾ>().��������;
                mainfunction.Send�з����Ʒ�������(card_data.id, uid);
                mainfunction.SendЧ��ж��(����Ŀ�꿨������.uid, 1);
                mainfunction.SendЧ��ж��(����Ŀ�꿨������.uid, 2);
            }
        }

        ����Ŀ�꿨��.GetComponent<MoveController>().��ɱ���� = 0;
        ����Ŀ�꿨��.GetComponent<MoveController>().�������� = 0;

        activateTurn_3_finish = 1;
        skill_end = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}

public class �йٱ� : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public �йٱ�(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;



        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = -1;
        activateTurn_4 = -1;

        card_data = card.GetComponent<������ʾ>().��������;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<������ʾ>().��������.uid;
        initialization();

    }

    private void initialization()
    {
        card.GetComponent<MoveController>().���ϵз�����Ҫ�� = 1;
        Ч�� = "����";

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "�йٱ�");
        }
    }
    public override void Action_1()
    {
        ValueHolder.�������õ������� = 1;
        mainfunction.���������������("b_moveca", 0);
        mainfunction.���������������("b_cardaction");
        mainfunction.ShowCardchoose(1);
        mainfunction.���������������("b_choose_fa", 1);
        ValueHolder.����ѡ��ȡ��.gameObject.SetActive(true);
        Debug.Log("ѡ��");



        activateTurn_1_finish = 1;


    }

    public override void Action_2()
    {
        ValueHolder.���ƶ�ս��.GetComponent<MonoBehaviour>().StartCoroutine(RotateAndScaleCoroutine(����Ŀ�꿨��));
        mainfunction.Send���ƴݻ�(����Ŀ�꿨��.GetComponent<������ʾ>().��������.uid);


        activateTurn_2_finish = 1;
        skill_end = 1;
    }

    public override void Action_3()
    {
        activateTurn_3_finish = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}

public class ������ : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public ������(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = -1;
        activateTurn_2 = -1;
        activateTurn_3 = -1;
        activateTurn_4 = -1;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<������ʾ>().��������.uid;
        initialization();



    }

    private void initialization()
    {
        card_data = card.GetComponent<������ʾ>().��������;
        �����غϽ���ʱ���� = 1;

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "������");
        }
    }

    public override void Action_1()
    {
        if (card ==  null)
        {
            skill_end = 1;
            return;
        }
        card_data.nowHp += 1;
        card_data.maxHp += 1;
        card.GetComponent<������ʾ>().��������();

        mainfunction.SendѪ���ı�(card_data.uid, 1);
    }

    public override void Action_2()
    {
        activateTurn_2_finish = 1;
    }

    public override void Action_3()
    {
        activateTurn_3_finish = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}


public class �ŵ��� : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public �ŵ���(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;

        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = -1;
        activateTurn_4 = -1;

        card_data = card.GetComponent<������ʾ>().��������;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<������ʾ>().��������.uid;
        initialization();


    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "�ŵ���");
        }
    }
    public override void Action_1()
    {
        if (mainfunction.�ҷ���������() == 0)
        {
            skill_end = 1;
            return;
        }
        ValueHolder.�ͷŷ���uid = uid;
        ValueHolder.�������õ������� = 0;
        mainfunction.���������������("b_moveca", 0);
        mainfunction.���������������("b_cardaction");
        mainfunction.ShowCardchoose(0);
        mainfunction.���������������("b_choose_fa", 0);

        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        �������� ����Ŀ�꿨������ = ����Ŀ�꿨��.GetComponent<������ʾ>().��������;

        ����Ŀ�꿨������.maxAttack += 2;
        ����Ŀ�꿨������.nowAttack += 2;
        ����Ŀ�꿨��.GetComponent<������ʾ>().��������();
        mainfunction.Send�������ı�(����Ŀ�꿨������.uid, 2);

        activateTurn_2_finish = 1;
        skill_end = 1;
    }

    public override void Action_3()
    {
        activateTurn_3_finish = 1;

    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}




public class Ыβ�� : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 3f;
    public Ыβ��(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = -1;
        activateTurn_4 = -1;

        card_data = card.GetComponent<������ʾ>().��������;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;


        uid = card.GetComponent<������ʾ>().��������.uid;
        initialization();


    }
    private void initialization()
    {
        �����غϽ���ʱ���� = 1;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "Ыβ��");
        }
        ValueHolder.����ʱ����.Add(uid, delay);
        mainfunction.Send����ʱ(card_data.uid, (int)delay);
    }
    public override void Action_1()
    {
        GameObject cardone = Instantiate(ValueHolder.��ʱ������);

        GameObject card_summon = summon_one(cardone, card_data.id);
        card_summon.GetComponent<������ʾ>().��������.uid = uid;


        mainfunction.���ŵ���(card_summon);
        change_card_color(card_summon, "blue");


        card_summon.transform.SetParent(ValueHolder.�з���ʱ������.transform);


        mainfunction.Send�ҷ����Ʒ�������(card_data.id, uid);


        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        if (delay <= 0)
        {
            activateTurn_2_finish = 1;
            Action_3();
            skill_end = 1;
            return;
        }
        mainfunction.Send�غϽ�������(1);
        delay -= 1;
        if (delay == 0)
        {
            �����غϿ�ʼʱ���� = 1;
        }
    }

    public override void Action_3()
    {
        foreach (Transform child in ValueHolder.�з���ʱ������.transform)
        {
            if (child.gameObject.GetComponent<������ʾ>().��������.uid == uid)
            {
                Destroy(child.gameObject);

                mainfunction.Send�ҷ����Ʒ�������(card_data.id, uid);
            }
        }
        activateTurn_3_finish = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}



public class ����֮ŭ : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 1f;
    private Dictionary<string,int> atk = new Dictionary<string,int>();
    public ����֮ŭ(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = ValueHolder.turn + delay;
        activateTurn_3 = -1;
        activateTurn_4 = -1;

        card_data = card.GetComponent<������ʾ>().��������;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;


        uid = card.GetComponent<������ʾ>().��������.uid;
        initialization();


    }
    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "����֮ŭ");
        }
        ValueHolder.����ʱ����.Add(uid, delay);
        mainfunction.Send����ʱ(card_data.uid, (int)delay);
    }
    public override void Action_1()
    {
        GameObject cardone = Instantiate(ValueHolder.��ʱ������);
        GameObject card_summon = summon_one(cardone, card_data.id);
        card_summon.GetComponent<������ʾ>().��������.uid = uid;
        mainfunction.���ŵ���(card_summon);
        change_card_color(card_summon, "blue");
        card_summon.transform.SetParent(ValueHolder.�з���ʱ������.transform);
        mainfunction.Send������������(card_data.id);


        foreach (GameObject card in mainfunction.��ȡ���Ͽ���())
        {
            string uid = card.GetComponent<������ʾ>().��������.uid;
            int before_atk = card.GetComponent<������ʾ>().��������.nowAttack;
            this.atk.Add(uid, before_atk);

            card.GetComponent<������ʾ>().��������.nowAttack = 0;
            card.GetComponent<������ʾ>().��������();
            mainfunction.Send�������ı�(uid, -999);
        }

        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        foreach (Transform child in ValueHolder.������ʱ������.transform)
        {
            if (child.gameObject.GetComponent<������ʾ>().��������.uid == uid)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (KeyValuePair<string, int> kvp in atk)
        {
            GameObject card = mainfunction.uid�ҿ�(kvp.Key);
            card.GetComponent<������ʾ>().��������.nowAttack = kvp.Value;
            card.GetComponent<������ʾ>().��������();

            mainfunction.Send�������ı�(kvp.Key, kvp.Value);
        }

        activateTurn_2_finish = 1;
        skill_end = 1;
    }

    public override void Action_3()
    {
        activateTurn_3_finish = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}


public class ���� : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public ����(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;

        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = -1;
        activateTurn_4 = -1;

        card_data = card.GetComponent<������ʾ>().��������;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid = card.GetComponent<������ʾ>().��������.uid;
        initialization();


    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "����");
        }
        card.GetComponent<MoveController>().�����ҷ�����Ҫ�� = 1;
    }
    public override void Action_1()
    {
        ValueHolder.�ͷŷ���uid = uid;
        ValueHolder.�������õ������� = 0;
        mainfunction.���������������("b_moveca", 0);
        mainfunction.���������������("b_cardaction");
        mainfunction.ShowCardchoose(0);
        mainfunction.���������������("b_choose_fa", 0);

        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        �������� ����Ŀ�꿨������ = ����Ŀ�꿨��.GetComponent<������ʾ>().��������;

        mainfunction.���ƴݻ�(����Ŀ�꿨��);
        mainfunction.Send���ƴݻ�(����Ŀ�꿨������.uid);
        summonHandcard(2);

        activateTurn_2_finish = 1;
        skill_end = 1;
    }

    public override void Action_3()
    {
        activateTurn_3_finish = 1;

    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}




public class ������ : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 1f;
    public ������(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = ValueHolder.turn + delay;
        activateTurn_3 = -1;
        activateTurn_4 = -1;

        card_data = card.GetComponent<������ʾ>().��������;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid = card.GetComponent<������ʾ>().��������.uid;
        initialization();


    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "������");
        }
        ValueHolder.����ʱ����.Add(uid, delay);
        mainfunction.Send����ʱ(uid, (int)delay);
    }

    public override void Action_1()
    {

        card.GetComponent<MoveController>().��ɱ���� = 1;
        card.GetComponent<MoveController>().�������� = 1;

        mainfunction.SendЧ������(card_data.uid, 1);
        mainfunction.SendЧ������(card_data.uid, 2);

        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        mainfunction.SendЧ��ж��(card_data.uid, 1);
        mainfunction.SendЧ��ж��(card_data.uid, 2);

        card.GetComponent<MoveController>().��ɱ���� = 0;
        card.GetComponent<MoveController>().�������� = 0;
        activateTurn_2_finish = 1;
        skill_end = 1;
    }

    public override void Action_3()
    {
        activateTurn_3_finish = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}



public class �⽻�� : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public �⽻��(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = -1;
        activateTurn_4 = -1;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid = card.GetComponent<������ʾ>().��������.uid;
        initialization();

    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "�⽻��");
        }

    }

    public override void Action_1()
    {
        summonHandcard(1);
        mainfunction.Send����(1);
        activateTurn_1_finish = 1;
        skill_end = 1;
    }

    public override void Action_2()
    {
        activateTurn_2_finish = 1;
    }

    public override void Action_3()
    {
        activateTurn_3_finish = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}
