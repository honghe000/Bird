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

public class 张三封大师 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 3f;
    public 张三封大师(GameObject Card, MonoBehaviour monoBehaviour)
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

        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();


    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "张三封大师");
        }
        ValueHolder.倒计时储存.Add(uid, delay);
        mainfunction.Send倒计时(uid,(int)delay);
    }

    public override void Action_1()
    {
        if (card == null)
        {
            skill_end = 1;
            return;
        }
        int card_id = int.Parse(card.transform.parent.name);
        List<int> killGrid = Grids.GetNeighbors_九(card_id);
        Dictionary<int, GameObject> killCards = mainfunction.消灭_destroy(killGrid);


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

public class 相安无事: BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 2f;
    public 相安无事(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = ValueHolder.turn + delay;
        activateTurn_3 = -1;
        activateTurn_4 = -1;
        this.monoBehaviour = monoBehaviour;

        card_data = card.GetComponent<数据显示>().卡牌数据;


        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();


    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "相安无事");
        }
        ValueHolder.倒计时储存.Add(uid, delay);
        mainfunction.Send倒计时(card_data.uid, (int)delay);
    }
    public override void Action_1()
    {
        GameObject cardone = Instantiate(ValueHolder.延时法术牌);
        GameObject card_summon = summon_one(cardone, card_data.id);
        card_summon.GetComponent<数据显示>().卡牌数据.uid = uid;
        Debug.Log(uid);

        mainfunction.缩放调整(card_summon);

        card_summon.transform.SetParent(ValueHolder.中立延时法术框.transform);
        ValueHolder.法术禁用.Add(card_data.名字, activateTurn_2);

        mainfunction.Send中立法术创建(card_data.id);
        mainfunction.Send法术禁用(card_data.id, activateTurn_2);
        activateTurn_1_finish = 1;
    }

    public override void Action_2()
    {
        Debug.Log("相安无事");
        foreach (Transform child in ValueHolder.中立延时法术框.transform)
        {
            Debug.Log(child.gameObject.GetComponent<数据显示>().卡牌数据.uid);
            if (child.gameObject.GetComponent<数据显示>().卡牌数据.uid == uid)
            {
                ValueHolder.法术禁用.Remove(child.gameObject.GetComponent<数据显示>().卡牌数据.名字);
                Destroy(child.gameObject);

                mainfunction.Send中立法术销毁(card_data.id);
                mainfunction.Send法术禁用取消(card_data.id);
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

public class 鬼屋 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    //private 卡牌数据 card_data;
    private int card_grid;
    public 鬼屋(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = -1;
        activateTurn_4 = -1;
        this.monoBehaviour = monoBehaviour;
        card_data = card.GetComponent<数据显示>().卡牌数据;


        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();


    }


    private void initialization()
    {
        card_grid = int.Parse(card.transform.parent.name);
        亡语 = 1;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "鬼屋");
        }
    }
    public override void Action_1()
    {
        List<int> 禁止放置 = Grids.GetColumnIndices(card_grid);
        mainfunction.Send人物位置禁用(card_data.id,禁止放置);
        activateTurn_1_finish = 1;
    }

    public override void Action_2()
    {
        mainfunction.Send人物位置禁用取消(card_data.id);

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

public class 鬼将 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 鬼将(GameObject Card, MonoBehaviour monoBehaviour)
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

        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();


    }

    private void initialization()
    {
        亡语 = 1;

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "鬼将");
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

public class 龙首之玉 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 2f;
    public 龙首之玉(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = ValueHolder.turn + delay;
        activateTurn_3 = -1;
        activateTurn_4 = -1;

        card_data = card.GetComponent<数据显示>().卡牌数据;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;


        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();


    }
    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "龙首之玉");
        }
        ValueHolder.倒计时储存.Add(uid, delay);
        mainfunction.Send倒计时(card_data.uid, (int)delay);
    }
    public override void Action_1()
    {
        GameObject cardone = Instantiate(ValueHolder.延时法术牌);

        GameObject card_summon = summon_one(cardone, card_data.id);
        card_summon.GetComponent<数据显示>().卡牌数据.uid = uid;


        mainfunction.缩放调整(card_summon);
        change_card_color(card_summon, "blue");


        card_summon.transform.SetParent(ValueHolder.敌方延时法术框.transform);


        mainfunction.Send我方红牌法术创建(card_data.id, uid);
        mainfunction.Send人物禁用(uid, activateTurn_2);

        Debug.Log(uid);
        Debug.Log(card_summon.gameObject.GetComponent<数据显示>().卡牌数据.uid);

        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        Debug.Log("摧毁");
        foreach (Transform child in ValueHolder.敌方延时法术框.transform)
        {
            //Debug.Log(uid);
            //Debug.Log(child.gameObject.GetComponent<数据显示>().卡牌数据.uid);
            if (child.gameObject.GetComponent<数据显示>().卡牌数据.uid == uid)
            {
                Destroy(child.gameObject);

                mainfunction.Send我方红牌法术销毁(card_data.id,uid);
                mainfunction.Send人物禁用取消(uid);
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


public class 佛光 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 1f;
    public 佛光(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;

        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = ValueHolder.turn + delay;
        activateTurn_4 = -1;

        card_data = card.GetComponent<数据显示>().卡牌数据;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();


    }

    private void initialization()
    {
        card.GetComponent<MoveController>().场上我方人数要求 = 1;

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "佛光");
        }
    }
    public override void Action_1()
    {
        ValueHolder.法术作用敌我类型 = 0;
        mainfunction.禁用棋盘物件代码("b_moveca",0);
        mainfunction.禁用手牌物件代码("b_cardaction");
        mainfunction.ShowCardchoose(0);
        mainfunction.启用棋盘物件代码("b_choose_fa",0);
        ValueHolder.法术选择取消.gameObject.SetActive(true);
        Debug.Log("选择");
        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        GameObject cardone = Instantiate(ValueHolder.延时法术牌);
        GameObject card_summon = summon_one(cardone, card_data.id);
        card_summon.GetComponent<数据显示>().卡牌数据.uid = uid;


        mainfunction.缩放调整(card_summon);
        change_card_color(card_summon, "blue");

        card_summon.transform.SetParent(ValueHolder.我方延时法术框.transform);



        卡牌数据 作用目标卡牌数据 = 作用目标卡牌.GetComponent<数据显示>().卡牌数据;

        作用目标卡牌.GetComponent<MoveController>().击杀免疫 = 1;
        作用目标卡牌.GetComponent<MoveController>().消灭免疫 = 1;

        mainfunction.Send敌方红牌法术创建(card_data.id, uid);
        mainfunction.Send效果挂载(作用目标卡牌数据.uid, 1);
        mainfunction.Send效果挂载(作用目标卡牌数据.uid, 2);

        ValueHolder.倒计时储存.Add(uid, delay);
        mainfunction.Send倒计时(card_data.uid, (int)delay);

        activateTurn_2_finish = 1;
    }

    public override void Action_3()
    {
        foreach (Transform child in ValueHolder.我方延时法术框.transform)
        {
            if (child.gameObject.GetComponent<数据显示>().卡牌数据.uid == uid)
            {
                Destroy(child.gameObject);

                卡牌数据 作用目标卡牌数据 = 作用目标卡牌.GetComponent<数据显示>().卡牌数据;
                mainfunction.Send敌方红牌法术销毁(card_data.id, uid);
                mainfunction.Send效果卸载(作用目标卡牌数据.uid, 1);
                mainfunction.Send效果卸载(作用目标卡牌数据.uid, 2);
            }
        }

        作用目标卡牌.GetComponent<MoveController>().击杀免疫 = 0;
        作用目标卡牌.GetComponent<MoveController>().消灭免疫 = 0;

        activateTurn_3_finish = 1;
        skill_end = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}

public class 判官笔 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 判官笔(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;



        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = -1;
        activateTurn_4 = -1;

        card_data = card.GetComponent<数据显示>().卡牌数据;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();

    }

    private void initialization()
    {
        card.GetComponent<MoveController>().场上敌方人数要求 = 1;
        效果 = "消灭";

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "判官笔");
        }
    }
    public override void Action_1()
    {
        ValueHolder.法术作用敌我类型 = 1;
        mainfunction.禁用棋盘物件代码("b_moveca", 0);
        mainfunction.禁用手牌物件代码("b_cardaction");
        mainfunction.ShowCardchoose(1);
        mainfunction.启用棋盘物件代码("b_choose_fa", 1);
        ValueHolder.法术选择取消.gameObject.SetActive(true);
        Debug.Log("选择");



        activateTurn_1_finish = 1;


    }

    public override void Action_2()
    {
        ValueHolder.手牌对战牌.GetComponent<MonoBehaviour>().StartCoroutine(RotateAndScaleCoroutine(作用目标卡牌));
        mainfunction.Send卡牌摧毁(作用目标卡牌.GetComponent<数据显示>().卡牌数据.uid);


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

public class 巨灵神 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 巨灵神(GameObject Card, MonoBehaviour monoBehaviour)
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

        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();



    }

    private void initialization()
    {
        card_data = card.GetComponent<数据显示>().卡牌数据;
        己方回合结束时触发 = 1;

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "巨灵神");
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
        card.GetComponent<数据显示>().更新数据();

        mainfunction.Send血量改变(card_data.uid, 1);
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


public class 雅典娜 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 雅典娜(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;

        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = -1;
        activateTurn_4 = -1;

        card_data = card.GetComponent<数据显示>().卡牌数据;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();


    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "雅典娜");
        }
    }
    public override void Action_1()
    {
        if (mainfunction.我方人物数量() == 0)
        {
            skill_end = 1;
            return;
        }
        ValueHolder.释放法术uid = uid;
        ValueHolder.法术作用敌我类型 = 0;
        mainfunction.禁用棋盘物件代码("b_moveca", 0);
        mainfunction.禁用手牌物件代码("b_cardaction");
        mainfunction.ShowCardchoose(0);
        mainfunction.启用棋盘物件代码("b_choose_fa", 0);

        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        卡牌数据 作用目标卡牌数据 = 作用目标卡牌.GetComponent<数据显示>().卡牌数据;

        作用目标卡牌数据.maxAttack += 2;
        作用目标卡牌数据.nowAttack += 2;
        作用目标卡牌.GetComponent<数据显示>().更新数据();
        mainfunction.Send攻击力改变(作用目标卡牌数据.uid, 2);

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




public class 蝎尾毒 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 3f;
    public 蝎尾毒(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = -1;
        activateTurn_4 = -1;

        card_data = card.GetComponent<数据显示>().卡牌数据;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;


        uid = card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();


    }
    private void initialization()
    {
        己方回合结束时触发 = 1;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "蝎尾毒");
        }
        ValueHolder.倒计时储存.Add(uid, delay);
        mainfunction.Send倒计时(card_data.uid, (int)delay);
    }
    public override void Action_1()
    {
        GameObject cardone = Instantiate(ValueHolder.延时法术牌);

        GameObject card_summon = summon_one(cardone, card_data.id);
        card_summon.GetComponent<数据显示>().卡牌数据.uid = uid;


        mainfunction.缩放调整(card_summon);
        change_card_color(card_summon, "blue");


        card_summon.transform.SetParent(ValueHolder.敌方延时法术框.transform);


        mainfunction.Send我方红牌法术创建(card_data.id, uid);


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
        mainfunction.Send回合结束弃牌(1);
        delay -= 1;
        if (delay == 0)
        {
            己方回合开始时触发 = 1;
        }
    }

    public override void Action_3()
    {
        foreach (Transform child in ValueHolder.敌方延时法术框.transform)
        {
            if (child.gameObject.GetComponent<数据显示>().卡牌数据.uid == uid)
            {
                Destroy(child.gameObject);

                mainfunction.Send我方红牌法术销毁(card_data.id, uid);
            }
        }
        activateTurn_3_finish = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}



public class 河神之怒 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 1f;
    private Dictionary<string,int> atk = new Dictionary<string,int>();
    public 河神之怒(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = ValueHolder.turn + delay;
        activateTurn_3 = -1;
        activateTurn_4 = -1;

        card_data = card.GetComponent<数据显示>().卡牌数据;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;


        uid = card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();


    }
    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "河神之怒");
        }
        ValueHolder.倒计时储存.Add(uid, delay);
        mainfunction.Send倒计时(card_data.uid, (int)delay);
    }
    public override void Action_1()
    {
        GameObject cardone = Instantiate(ValueHolder.延时法术牌);
        GameObject card_summon = summon_one(cardone, card_data.id);
        card_summon.GetComponent<数据显示>().卡牌数据.uid = uid;
        mainfunction.缩放调整(card_summon);
        change_card_color(card_summon, "blue");
        card_summon.transform.SetParent(ValueHolder.敌方延时法术框.transform);
        mainfunction.Send中立法术创建(card_data.id);


        foreach (GameObject card in mainfunction.获取桥上卡牌())
        {
            string uid = card.GetComponent<数据显示>().卡牌数据.uid;
            int before_atk = card.GetComponent<数据显示>().卡牌数据.nowAttack;
            this.atk.Add(uid, before_atk);

            card.GetComponent<数据显示>().卡牌数据.nowAttack = 0;
            card.GetComponent<数据显示>().更新数据();
            mainfunction.Send攻击力改变(uid, -999);
        }

        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        foreach (Transform child in ValueHolder.中立延时法术框.transform)
        {
            if (child.gameObject.GetComponent<数据显示>().卡牌数据.uid == uid)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (KeyValuePair<string, int> kvp in atk)
        {
            GameObject card = mainfunction.uid找卡(kvp.Key);
            card.GetComponent<数据显示>().卡牌数据.nowAttack = kvp.Value;
            card.GetComponent<数据显示>().更新数据();

            mainfunction.Send攻击力改变(kvp.Key, kvp.Value);
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


public class 舍生 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 舍生(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;

        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = -1;
        activateTurn_4 = -1;

        card_data = card.GetComponent<数据显示>().卡牌数据;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid = card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();


    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "舍生");
        }
        card.GetComponent<MoveController>().场上我方人数要求 = 1;
    }
    public override void Action_1()
    {
        ValueHolder.释放法术uid = uid;
        ValueHolder.法术作用敌我类型 = 0;
        mainfunction.禁用棋盘物件代码("b_moveca", 0);
        mainfunction.禁用手牌物件代码("b_cardaction");
        mainfunction.ShowCardchoose(0);
        mainfunction.启用棋盘物件代码("b_choose_fa", 0);

        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        卡牌数据 作用目标卡牌数据 = 作用目标卡牌.GetComponent<数据显示>().卡牌数据;

        mainfunction.卡牌摧毁(作用目标卡牌);
        mainfunction.Send卡牌摧毁(作用目标卡牌数据.uid);
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




public class 神谕者 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 1f;
    public 神谕者(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = ValueHolder.turn + delay;
        activateTurn_3 = -1;
        activateTurn_4 = -1;

        card_data = card.GetComponent<数据显示>().卡牌数据;
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid = card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();


    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "神谕者");
        }
        ValueHolder.倒计时储存.Add(uid, delay);
        mainfunction.Send倒计时(uid, (int)delay);
    }

    public override void Action_1()
    {

        card.GetComponent<MoveController>().击杀免疫 = 1;
        card.GetComponent<MoveController>().消灭免疫 = 1;

        mainfunction.Send效果挂载(card_data.uid, 1);
        mainfunction.Send效果挂载(card_data.uid, 2);

        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        mainfunction.Send效果卸载(card_data.uid, 1);
        mainfunction.Send效果卸载(card_data.uid, 2);

        card.GetComponent<MoveController>().击杀免疫 = 0;
        card.GetComponent<MoveController>().消灭免疫 = 0;
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



public class 外交官 : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public 外交官(GameObject Card, MonoBehaviour monoBehaviour)
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

        uid = card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();

    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "外交官");
        }

    }

    public override void Action_1()
    {
        summonHandcard(1);
        mainfunction.Send摸牌(1);
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
public class 肥嘟嘟左卫门 : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public 肥嘟嘟左卫门(GameObject Card, MonoBehaviour monoBehaviour)
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

        uid = card.GetComponent<数据显示>().卡牌数据.uid;
        initialization();

    }

    private void initialization()
    {
        card.GetComponent<MoveController>().杀人后触发 = 1;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "肥嘟嘟左卫门");
        }

    }

    public override void Action_1()
    {
        Debug.Log("肥嘟嘟左卫门");
        summonHandcard(3);
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
