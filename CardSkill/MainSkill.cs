using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;


//涨跌my king

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
        card_data = card.GetComponent<数据显示>().卡牌数据;
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

        string uid_temp = "";

        foreach (KeyValuePair<int, GameObject> kvp in killCards)
        {
            uid_temp = kvp.Value.GetComponent<数据显示>().卡牌数据.uid;
            mainfunction.卡牌摧毁(kvp.Value);
            mainfunction.Send卡牌摧毁(uid_temp);
        }

        uid_temp = card.GetComponent<数据显示>().卡牌数据.uid;
        mainfunction.卡牌摧毁(card);
        mainfunction.Send卡牌摧毁(uid_temp);


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

        


        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        card_data = card.GetComponent<数据显示>().卡牌数据;
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
        


        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        card_data = card.GetComponent<数据显示>().卡牌数据;
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
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

        
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;


        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        card_data = card.GetComponent<数据显示>().卡牌数据;
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

        activateTurn_1_finish = 1;


    }

    public override void Action_2()
    {
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

        
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();


    }

    private void initialization()
    {
        card.GetComponent<MoveController>().场上我方人数要求 = 1;

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "佛光");
        }

        ValueHolder.倒计时储存.Add(uid, delay);
        mainfunction.Send倒计时(card_data.uid, (int)delay);
    }
    public override void Action_1()
    {
        mainfunction.选择我方卡牌施放(card_data, 1);
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
        mainfunction.Send效果挂载(作用目标卡牌数据.uid, 1,delay);
        mainfunction.Send效果挂载(作用目标卡牌数据.uid, 2, delay);




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
                //mainfunction.Send效果卸载(作用目标卡牌数据.uid, 1);
                //mainfunction.Send效果卸载(作用目标卡牌数据.uid, 2);
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

        
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        card_data = card.GetComponent<数据显示>().卡牌数据;
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
        mainfunction.选择敌方卡牌施放(card_data, 1);


        activateTurn_1_finish = 1;


    }

    public override void Action_2()
    {
        string uid_temp = 作用目标卡牌.GetComponent<数据显示>().卡牌数据.uid;
        mainfunction.卡牌摧毁(作用目标卡牌);
        mainfunction.Send卡牌摧毁(uid_temp);



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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();



    }

    private void initialization()
    {
        
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

        
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid =  card.GetComponent<数据显示>().卡牌数据.uid;
        card_data = card.GetComponent<数据显示>().卡牌数据;
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
        mainfunction.选择我方卡牌施放(card_data, 0);

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

        
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;


        uid = card.GetComponent<数据显示>().卡牌数据.uid;
        card_data = card.GetComponent<数据显示>().卡牌数据;
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

        
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;


        uid = card.GetComponent<数据显示>().卡牌数据.uid;
        card_data = card.GetComponent<数据显示>().卡牌数据;
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
        card_summon.transform.SetParent(ValueHolder.中立延时法术框.transform);
        mainfunction.Send中立法术创建(card_data.id);


        foreach (GameObject card in mainfunction.获取桥上卡牌())
        {
            if( card.GetComponent<MoveController>().法术可作用 == 0)
            {
                continue;
            }
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
            card.GetComponent<数据显示>().卡牌数据.nowAttack += kvp.Value;
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

        
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid = card.GetComponent<数据显示>().卡牌数据.uid;
        card_data = card.GetComponent<数据显示>().卡牌数据;
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
        mainfunction.选择我方卡牌施放(card_data, 1);
        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        string uid_temp = 作用目标卡牌.GetComponent<数据显示>().卡牌数据.uid;

        mainfunction.卡牌摧毁(作用目标卡牌);
        mainfunction.Send卡牌摧毁(uid_temp);
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

        
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid = card.GetComponent<数据显示>().卡牌数据.uid;
        card_data = card.GetComponent<数据显示>().卡牌数据;
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

        mainfunction.Send效果挂载(card_data.uid, 1,delay);
        mainfunction.Send效果挂载(card_data.uid, 2,delay);

        activateTurn_1_finish = 1;


    }

    public override void Action_2()
    {
        //mainfunction.Send效果卸载(card_data.uid, 1);
        //mainfunction.Send效果卸载(card_data.uid, 2);
        if (card == null)
        {
            return;
        }
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
        card_data =  card.GetComponent<数据显示>().卡牌数据;
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
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

public class 雷电 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 雷电(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;



        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = ValueHolder.turn + 1;
        activateTurn_4 = -1;

        
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid = card.GetComponent<数据显示>().卡牌数据.uid;
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        card.GetComponent<MoveController>().场上敌方人数要求 = 1;
        效果 = "眩晕";

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "雷电");
        }
        ValueHolder.倒计时储存.Add(uid, 1);
        mainfunction.Send倒计时(card_data.uid, 1);
    }
    public override void Action_1()
    {
        mainfunction.选择敌方卡牌施放(card_data, 1);



        activateTurn_1_finish = 1;


    }

    public override void Action_2()
    {
        GameObject cardone = Instantiate(ValueHolder.延时法术牌);

        GameObject card_summon = summon_one(cardone, card_data.id);
        card_summon.GetComponent<数据显示>().卡牌数据.uid = uid;


        mainfunction.缩放调整(card_summon);
        change_card_color(card_summon, "blue");


        card_summon.transform.SetParent(ValueHolder.敌方延时法术框.transform);
        mainfunction.Send我方红牌法术创建(card_data.id, uid);
        作用目标卡牌.GetComponent<MoveController>().眩晕 = 1;
        mainfunction.Send效果挂载(作用目标卡牌.GetComponent<数据显示>().卡牌数据.uid, 3,1);

        activateTurn_2_finish = 1;


    }

    public override void Action_3()
    {
        if (作用目标卡牌 != null)
        {
            作用目标卡牌.GetComponent<MoveController>().眩晕 = 0;
            //mainfunction.Send效果卸载(作用目标卡牌.GetComponent<数据显示>().卡牌数据.uid, 3);
        }

        foreach (Transform child in ValueHolder.敌方延时法术框.transform)
        {
            if (child.gameObject.GetComponent<数据显示>().卡牌数据.uid == uid)
            {
                Destroy(child.gameObject);

                mainfunction.Send我方红牌法术销毁(card_data.id, uid);
            }
        }


        activateTurn_3_finish = 1;
        skill_end = 1;

    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}
public class 阿尔卡祭坛 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 阿尔卡祭坛(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "阿尔卡祭坛");
        }
    }
    public override void Action_1()
    {
        
        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card = grid.transform.GetChild(0).gameObject;
                if (card.GetComponent<MoveController>().cardType == 0 && card.GetComponent<数据显示>().卡牌数据.类别 == "角色")
                {
                    卡牌数据 作用目标卡牌数据 = card.GetComponent<数据显示>().卡牌数据;

                    作用目标卡牌数据.maxAttack += 1;
                    作用目标卡牌数据.nowAttack += 1;
                    card.GetComponent<数据显示>().更新数据();
                    mainfunction.Send攻击力改变(作用目标卡牌数据.uid, 1);

                }
            }
        }

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

public class 太乙真人 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 太乙真人(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();


    }

    private void initialization()
    {
        亡语 = 1;
        用户操作型技能 = 1;

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "太乙真人");
        }
    }
    public override void Action_1()
    {
        if (mainfunction.我方人物数量() == 1)
        {
            skill_end = 1;
            return;
        }
        mainfunction.选择我方卡牌施放(card_data, 0);

        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {

        卡牌数据 作用目标卡牌数据 = 作用目标卡牌.GetComponent<数据显示>().卡牌数据;

        作用目标卡牌数据.maxAttack += 3;
        作用目标卡牌数据.nowAttack += 3;
        作用目标卡牌数据.maxHp += 3;
        作用目标卡牌数据.nowHp += 3;
        作用目标卡牌.GetComponent<数据显示>().更新数据();
        mainfunction.Send攻击力改变(作用目标卡牌数据.uid, 3);
        mainfunction.Send血量改变(作用目标卡牌数据.uid, 3);


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
public class 关羽 : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public 关羽(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        card.GetComponent<MoveController>().杀人后触发 = 1;
        效果 = "消灭";
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "关羽");
        }

    }

    public override void Action_1()
    {
       if (mainfunction.敌方人物数量() < 0)
        {
            return;
        }
        mainfunction.选择敌方卡牌施放(card_data, 0);
        activateTurn_1_finish = 1;


    }

    public override void Action_2()
    {
        string uid_temp = 作用目标卡牌.GetComponent<数据显示>().卡牌数据.uid;
        mainfunction.卡牌摧毁(作用目标卡牌);
        mainfunction.Send卡牌摧毁(uid_temp);
        activateTurn_2_finish = 1;
        activateTurn_1_finish = 0;

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
public class 困兽之斗 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 2f;
    public 困兽之斗(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = ValueHolder.turn + delay;
        activateTurn_3 = -1;
        activateTurn_4 = -1;
        this.monoBehaviour = monoBehaviour;

        


        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid = card.GetComponent<数据显示>().卡牌数据.uid;
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();


    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "困兽之斗");
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
        foreach (GameObject card in mainfunction.获取全部人物())
        {
            if( card.GetComponent<MoveController>().法术可作用 == 0 || card.GetComponent<MoveController>().眩晕免疫 == 1)
            {
                continue;
            }
           card.GetComponent<MoveController>().眩晕 = 1;
            mainfunction.Send效果挂载(card.GetComponent<数据显示>().卡牌数据.uid, 3,delay);
        }
        mainfunction.Send中立法术创建(card_data.id);
       
        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        Debug.Log("困兽之斗");
        foreach (GameObject card in mainfunction.获取全部人物())
        {
            if (card != null)
            {
                card.GetComponent<MoveController>().眩晕 = 0;
                //mainfunction.Send效果卸载(card.GetComponent<数据显示>().卡牌数据.uid, 3);
            }
        }
        foreach (Transform child in ValueHolder.中立延时法术框.transform)
        {
           
            if (child.gameObject.GetComponent<数据显示>().卡牌数据.uid == uid)
            {
                Destroy(child.gameObject);
                mainfunction.Send中立法术销毁(card_data.id);
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
public class 牛头马面 : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public 牛头马面(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        
        initialization();

    }

    private void initialization()
    {
        场上角色死亡触发 = 1;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "牛头马面");
        }

    }

    public override void Action_1()
    {
        if (card == null)
        {
            skill_end = 1;
            return;
        }

        card_data.maxAttack += 1;
        card_data.nowAttack += 1;
        card_data.maxHp += 1;
        card_data.nowHp += 1;
        card.GetComponent<数据显示>().更新数据();
        mainfunction.Send攻击力改变(uid, 1);
        mainfunction.Send血量改变(uid, 1);


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
public class 迅雷的崩玉 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private float delay = 2f;
    public 迅雷的崩玉(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;
        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = ValueHolder.turn + delay;
        activateTurn_3 = -1;
        activateTurn_4 = -1;
        this.monoBehaviour = monoBehaviour;

        


        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid = card.GetComponent<数据显示>().卡牌数据.uid;
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();


    }

    private void initialization()
    {
        card.GetComponent<MoveController>().场上敌方人数要求 = 1;
        效果 = "眩晕";

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "迅雷的崩玉");
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

        card_summon.transform.SetParent(ValueHolder.敌方延时法术框.transform);
        mainfunction.Send我方红牌法术创建(card_data.id,uid);

        foreach (GameObject card in mainfunction.获取敌方全部人物())
        {
            if( card.GetComponent<MoveController>().法术可作用 == 0 || card.GetComponent<MoveController>().眩晕免疫==1 )
            {
                continue;
            }
            card.GetComponent<MoveController>().眩晕 = 1;
            mainfunction.Send效果挂载(card.GetComponent<数据显示>().卡牌数据.uid, 3, delay);
        }
       

        activateTurn_1_finish = 1;
    }

    public override void Action_2()
    {
        foreach (GameObject card in mainfunction.获取全部人物())
        {
            if (card != null)
            {
                card.GetComponent<MoveController>().眩晕 = 0;
                //mainfunction.Send效果卸载(card.GetComponent<数据显示>().卡牌数据.uid, 3);
            }
        }
        foreach (Transform child in ValueHolder.敌方延时法术框.transform)
        {

            if (child.gameObject.GetComponent<数据显示>().卡牌数据.uid == uid)
            {
                Destroy(child.gameObject);
                mainfunction.Send我方红牌法术销毁(card_data.id, uid);
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

public class 鬼琵琶 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 鬼琵琶(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();


    }

    private void initialization()
    {
        己方回合结束时触发 = 1;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "鬼琵琶");
        }
        card.GetComponent<MoveController>().场上我方人数要求 = 1;
    }
    public override void Action_1()
    {
        mainfunction.选择我方卡牌施放(card_data, 1);
        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {

        卡牌数据 作用目标卡牌数据 = 作用目标卡牌.GetComponent<数据显示>().卡牌数据;

        作用目标卡牌数据.maxAttack += 5;
        作用目标卡牌数据.nowAttack += 5;
        作用目标卡牌.GetComponent<数据显示>().更新数据();
        mainfunction.Send攻击力改变(作用目标卡牌数据.uid, 5);


        activateTurn_2_finish = 1;

    }

    public override void Action_3()
    {
        string uid_temp = 作用目标卡牌.GetComponent<数据显示>().卡牌数据.uid;
        mainfunction.卡牌摧毁(作用目标卡牌);
        mainfunction.Send卡牌摧毁(uid_temp);
        skill_end = 1;
        activateTurn_3_finish = 1;

    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}


public class 毒雾 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 毒雾(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();


    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "毒雾");
        }

    }
    public override void Action_1()
    {

        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card_temp = grid.transform.GetChild(0).gameObject;
                if (card_temp.GetComponent<MoveController>().cardType == 1 && card_temp.GetComponent<数据显示>().卡牌数据.类别 == "角色"&& card_temp.GetComponent<MoveController>().法术可作用 == 1)
                {
                    卡牌数据 card_temp_data = card_temp.GetComponent<数据显示>().卡牌数据;


                    card_temp_data.nowHp -= 2;
                    if (card_temp_data.nowHp <= 0)
                    {
                        mainfunction.卡牌摧毁(card_temp);
                        mainfunction.Send卡牌摧毁(card_temp_data.uid);
                    }
                    card_temp.GetComponent<数据显示>().更新数据();
                    mainfunction.Send血量改变(card_temp_data.uid, -2);

                }
            }
        }

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

public class 战国犀牛 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 战国犀牛(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();


    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "战国犀牛");
        }

    }
    public override void Action_1()
    {

        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card_temp = grid.transform.GetChild(0).gameObject;
                if (card_temp.GetComponent<MoveController>().cardType == 1 && card_temp.GetComponent<数据显示>().卡牌数据.类别 == "角色")
                {
                    卡牌数据 card_temp_data = card_temp.GetComponent<数据显示>().卡牌数据;

                    if (card_temp_data.nowHp <= 2)
                    {
                        mainfunction.卡牌摧毁(card_temp);
                        mainfunction.Send卡牌摧毁(card_temp_data.uid);
                    }
                }
            }
        }

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
public class 牛魔 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    private Dictionary<string, int> atk = new Dictionary<string, int>();
    public 牛魔(GameObject Card, MonoBehaviour monoBehaviour)
    {
        card = Card;

        skill_end = 0;
        activateTurn_1 = ValueHolder.turn;
        activateTurn_2 = -1;
        activateTurn_3 = ValueHolder.turn+1;
        activateTurn_4 = -1;

        
        this.monoBehaviour = monoBehaviour;

        activateTurn_1_finish = 0;
        activateTurn_2_finish = 0;
        activateTurn_3_finish = 0;
        activateTurn_4_finish = 0;

        uid = card.GetComponent<数据显示>().卡牌数据.uid;
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();


    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "牛魔");
        }
    }
    public override void Action_1()
    {
        if (mainfunction.敌方人物数量() == 0)
        {
            skill_end = 1;
            return;
        }
        mainfunction.选择敌方卡牌施放(card_data, 0);
        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        
        string uid = 作用目标卡牌.GetComponent<数据显示>().卡牌数据.uid;
        int before_atk = 作用目标卡牌.GetComponent<数据显示>().卡牌数据.nowAttack;
        this.atk.Add(uid, before_atk);

        作用目标卡牌.GetComponent<数据显示>().卡牌数据.nowAttack = 0;
        作用目标卡牌.GetComponent<数据显示>().更新数据();
        mainfunction.Send攻击力改变(uid, -999);
        

        activateTurn_2_finish = 1;
        
    }

    public override void Action_3()
    {
        foreach (KeyValuePair<string, int> kvp in atk)
        {
            GameObject card = mainfunction.uid找卡(kvp.Key);
            card.GetComponent<数据显示>().卡牌数据.nowAttack += kvp.Value;
            card.GetComponent<数据显示>().更新数据();

            mainfunction.Send攻击力改变(kvp.Key, kvp.Value);
        }
        activateTurn_3_finish = 1;
        skill_end = 1;
    }

    public override void Action_4()
    {
        activateTurn_4_finish = 1;
    }

}
public class 贩卖鸦片 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 贩卖鸦片(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        card.GetComponent<MoveController>().场上敌方人数要求 = 1;
        效果 = "眩晕";

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "贩卖鸦片");
        }
    }
    public override void Action_1()
    {
        mainfunction.选择敌方卡牌施放(card_data, 1);



        activateTurn_1_finish = 1;


    }

    public override void Action_2()
    {
        GameObject cardone = Instantiate(ValueHolder.延时法术牌);

        GameObject card_summon = summon_one(cardone, card_data.id);
        card_summon.GetComponent<数据显示>().卡牌数据.uid = uid;


        mainfunction.缩放调整(card_summon);
        change_card_color(card_summon, "blue");


        card_summon.transform.SetParent(ValueHolder.敌方延时法术框.transform);
        mainfunction.Send我方红牌法术创建(card_data.id, uid);
        作用目标卡牌.GetComponent<MoveController>().眩晕 = 1;
        mainfunction.Send效果挂载(作用目标卡牌.GetComponent<数据显示>().卡牌数据.uid, 3,-1);

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
public class 漫步者 : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public 漫步者(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        card.GetComponent<MoveController>().法术可作用 = 0;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "漫步者");
        }

    }

    public override void Action_1()
    {
        mainfunction.Send效果挂载(uid,4,-1);
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
public class 杨枝甘露 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 杨枝甘露(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        card.GetComponent<MoveController>().场上我方人数要求 = 1;

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "杨枝甘露");
        }
    }
    public override void Action_1()
    {
        mainfunction.选择我方卡牌施放(card_data, 1);
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
        mainfunction.Send敌方红牌法术创建(card_data.id, uid);
        作用目标卡牌.GetComponent<MoveController>().法术可作用 = 0;
        mainfunction.Send效果挂载(作用目标卡牌.GetComponent<数据显示>().卡牌数据.uid, 4, -1);

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
public class 远古石像鬼 : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public 远古石像鬼(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        card.GetComponent<MoveController>().消灭免疫 = 1;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "远古石像鬼");
        }

    }

    public override void Action_1()
    {
        mainfunction.Send效果挂载(uid, 2, -1);
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

public class 丁达尔效应 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 丁达尔效应(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        card.GetComponent<MoveController>().场上我方人数要求 = 1;

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "丁达尔效应");
        }
    }
    public override void Action_1()
    {
        mainfunction.选择我方卡牌施放(card_data, 1);
        activateTurn_1_finish = 1;
    }

    public override void Action_2()
    {
        作用目标卡牌.GetComponent<MoveController>().行动点 += 1;
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
public class 钟馗 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 钟馗(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        card.GetComponent<MoveController>().场上我方人数要求 = 1;

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "钟馗");
        }
    }
    public override void Action_1()
    {
        if (mainfunction.我方人物数量() == 1)
        {
            skill_end = 1;
            return;
        }
        mainfunction.选择我方卡牌施放(card_data, 0);
        activateTurn_1_finish = 1;
    }

    public override void Action_2()
    {
        卡牌数据 作用目标卡牌数据 = 作用目标卡牌.GetComponent<数据显示>().卡牌数据;
        card_data.maxAttack += 作用目标卡牌数据.nowAttack;
        card_data.nowAttack += 作用目标卡牌数据.nowAttack;
        card_data.maxHp += 作用目标卡牌数据.nowHp;
        card_data.nowHp += 作用目标卡牌数据.nowHp;
        card.GetComponent<数据显示>().更新数据();
        mainfunction.Send攻击力改变(uid, 作用目标卡牌数据.nowAttack);
        mainfunction.Send血量改变(uid, 作用目标卡牌数据.nowHp);
        mainfunction.卡牌摧毁(作用目标卡牌);
        mainfunction.Send卡牌摧毁(作用目标卡牌数据.uid);
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
public class 武僧 : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public 武僧(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        card.GetComponent<MoveController>().眩晕免疫 = 1;
        效果 = "眩晕";
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "武僧");
        }

    }

    public override void Action_1()
    {
        mainfunction.Send效果挂载(uid, 5, -1);
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

public class 虾兵蟹将 : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public 虾兵蟹将(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        召唤物id = 730;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "虾兵蟹将");
        }

    }

    public override void Action_1()
    {
        //可点击范围
        List<int> clickable = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        mainfunction.点选格子(ValueHolder.gloabCaedData[召唤物id].名字,uid, clickable);
        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        mainfunction.指定位置生成卡牌(ValueHolder.点击格子编号, 召唤物id, 0);
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
public class 猩猩守卫 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 猩猩守卫(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "猩猩守卫");
        }
    }
    public override void Action_1()
    {
        card.GetComponent<MoveController>().行动点 += 1;
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
public class 火焰小鬼 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 火焰小鬼(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();


    }

    private void initialization()
    {
        亡语 = 1;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "火焰小鬼");
        }

    }
    public override void Action_1()
    {

        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card_temp = grid.transform.GetChild(0).gameObject;
                if (card_temp.GetComponent<MoveController>().cardType == 1 && card_temp.GetComponent<数据显示>().卡牌数据.类别 == "角色" )
                {
                    卡牌数据 card_temp_data = card_temp.GetComponent<数据显示>().卡牌数据;


                    card_temp_data.nowHp -= 1;
                    if (card_temp_data.nowHp <= 0)
                    {
                        mainfunction.卡牌摧毁(card_temp);
                        mainfunction.Send卡牌摧毁(card_temp_data.uid);
                    }
                    card_temp.GetComponent<数据显示>().更新数据();
                    mainfunction.Send血量改变(card_temp_data.uid, -1);

                }
            }
        }

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
public class 火灵法师 : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public 火灵法师(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        召唤物id = 343;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "火灵法师");
        }

    }

    public override void Action_1()
    {
        //可点击范围
        List<int> clickable = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        mainfunction.点选格子(ValueHolder.gloabCaedData[召唤物id].名字, uid, clickable);
        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {

        mainfunction.指定位置生成卡牌(ValueHolder.点击格子编号, 召唤物id, 1);
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



public class 尼斯湖水怪 : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public 尼斯湖水怪(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "尼斯湖水怪");
        }

    }

    public override void Action_1()
    {
        card.GetComponent<MoveController>().识水 = 1;
        mainfunction.Send效果挂载(card_data.uid, 6,-1);
        skill_end = 1;
        activateTurn_1_finish = 1;

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
public class 充能 : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public 充能(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "充能");
        }

    }

    public override void Action_1()
    {
        ValueHolder.point += 1;
        ValueHolder.体力.text = ValueHolder.point.ToString();
        skill_end = 1;
        activateTurn_1_finish = 1;

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
public class 荒骷髅 : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public 荒骷髅(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        亡语 = 1;
        用户操作型技能 = 1;
        召唤物id = 397;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "荒骷髅");
        }

    }

    public override void Action_1()
    {
        //可点击范围
        List<int> clickable = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        mainfunction.点选格子(ValueHolder.gloabCaedData[召唤物id].名字, uid, clickable);
        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        mainfunction.指定位置生成卡牌(ValueHolder.点击格子编号, 召唤物id, 1);


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
public class 枉死城 : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public 枉死城(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        场上我方角色死亡触发= 1;
        用户操作型技能 = 1;
        召唤物id = 343;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "枉死城");
        }

    }

    public override void Action_1()
    {
        //可点击范围
        List<int> clickable = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        mainfunction.点选格子(ValueHolder.gloabCaedData[召唤物id].名字, uid, clickable);
        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {
        mainfunction.指定位置生成卡牌(ValueHolder.点击格子编号, 召唤物id, 1);

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
public class 青坊主 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 青坊主(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();


    }

    private void initialization()
    {
        亡语 = 1;
        用户操作型技能 = 1;

        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "青坊主");
        }
    }
    public override void Action_1()
    {
        if (mainfunction.我方人物数量() == 1)
        {
            skill_end = 1;
            return;
        }
        mainfunction.选择我方卡牌施放(card_data, 0);

        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {

        卡牌数据 作用目标卡牌数据 = 作用目标卡牌.GetComponent<数据显示>().卡牌数据;

        作用目标卡牌.GetComponent<MoveController>().击杀免疫 = 1;
        作用目标卡牌.GetComponent<MoveController>().消灭免疫 = 1;

        mainfunction.Send效果挂载(作用目标卡牌数据.uid, 1, 1);
        mainfunction.Send效果挂载(作用目标卡牌数据.uid, 2, 1);


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
public class 神之审判 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 神之审判(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();


    }

    private void initialization()
    {
        card.GetComponent<MoveController>().场上敌方人数要求 = 1;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "神之审判");
        }
    }
    public override void Action_1()
    {
        if (mainfunction.敌方人物数量() == 0)
        {
            skill_end = 1;
            return;
        }
        mainfunction.选择敌方卡牌施放(card_data, 0);

        activateTurn_1_finish = 1;

    }

    public override void Action_2()
    {

        卡牌数据 作用目标卡牌数据 = 作用目标卡牌.GetComponent<数据显示>().卡牌数据;

        作用目标卡牌数据.maxHp -= 5;
        作用目标卡牌数据.nowHp -= 5;
        作用目标卡牌.GetComponent<数据显示>().更新数据();
        if (作用目标卡牌数据.nowHp <= 0)
        {
            
            foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
            {
                GameObject grid = grids.Value;
                if (grid.transform.childCount != 0 && grids.Key != "0")
                {
                    GameObject card_temp = grid.transform.GetChild(0).gameObject;
                    if (card_temp.GetComponent<MoveController>().cardType == 0 && card_temp.GetComponent<数据显示>().卡牌数据.类别 == "角色")
                    {
                        卡牌数据 card_temp_data = card_temp.GetComponent<数据显示>().卡牌数据;

                        card_temp_data.maxAttack += 1;
                        card_temp_data.nowAttack += 1;
                        card_temp.GetComponent<数据显示>().更新数据();
                        mainfunction.Send攻击力改变(card_temp_data.uid, 1);

                    }
                }

            }

            mainfunction.卡牌摧毁(作用目标卡牌);
            mainfunction.Send卡牌摧毁(作用目标卡牌数据.uid);

        }


        mainfunction.Send血量改变(作用目标卡牌数据.uid, -5);
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
public class 猎人 : BaseSkill
{
    private MonoBehaviour monoBehaviour;
    public 猎人(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();


    }

    private void initialization()
    {
        card.GetComponent<MoveController>().场上敌方人数要求 = 1;
        效果 = "消灭";
        亡语 = 1;
        用户操作型技能 = 1;
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "猎人");
        }
    }
    public override void Action_1()
    {
        
        mainfunction.选择敌方卡牌施放(card_data, 0);
        activateTurn_1_finish = 1;
        skill_end = 1;
    }

    public override void Action_2()
    {
        string uid_temp = 作用目标卡牌.GetComponent<数据显示>().卡牌数据.uid;
        mainfunction.卡牌摧毁(作用目标卡牌);
        mainfunction.Send卡牌摧毁(uid_temp);


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
public class 拳师 : BaseSkill
{
    private MonoBehaviour monoBehaviour;

    public 拳师(GameObject Card, MonoBehaviour monoBehaviour)
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
        card_data = card.GetComponent<数据显示>().卡牌数据;
        initialization();

    }

    private void initialization()
    {
        card.GetComponent<MoveController>().杀人后触发 = 1;
        
        if (!ValueHolder.uid_to_name.ContainsKey(uid))
        {
            ValueHolder.uid_to_name.Add(uid, "拳师");
        }

    }

    public override void Action_1()
    {

        card.GetComponent<MoveController>().行动点 += 1;
      
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