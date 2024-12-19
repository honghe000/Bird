using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class b_listen : MonoBehaviour
{
    public GameObject commoncard;
    public TextMeshProUGUI 体力;
    public TextMeshProUGUI 回合数;
    public Button 下个回合;
    public GameObject 手牌区;
    public HintManager hintManager;
    void Start()
    {
        StartCoroutine(GetMessageCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GetMessageCoroutine()
    {
        while (true)
        {

            if (ValueHolder.receiveQueue.Count == 0)
            {
                yield return new WaitForSeconds(0.2f); // 等待0.1秒
                continue;
            }
            else
            {
                // 假设 ValueHolder.ResieveMessages 是一个消息队列或存储最新消息的地方
                string mesString = ValueHolder.receiveQueue.Dequeue();
                try
                {
                    // 将 mesString 按 "}{“ 进行分割，得到一个字符串数组，每个元素代表一条消息
                    string[] messages = mesString.Split(new string[] { "}{" }, System.StringSplitOptions.None);

                    foreach (string msg in messages)
                    {
                        // 重新加上花括号，保证每条消息完整
                        string completeMessage = "{" + msg.Trim('{', '}') + "}";

                        // 使用 JsonUtility 解析每条消息
                        Message mes = JsonUtility.FromJson<Message>(completeMessage);
                        mes.is_used = 1;

                        // 调用修改接收消息的方法
                        mainfunction.ChangeRecieveMessage("is_used", 1);

                        // 处理每一条消息
                        process(mes);
                    }

                }
                catch (ArgumentException e)
                {
                    Debug.LogError("JSON Parsing Error: " + e.Message);
                    Debug.LogError("JSON String: " + mesString);
                }

                yield return new WaitForSeconds(0.2f); // 等待0.1秒
            }
        }
    }

    void change_card_color(GameObject card, string color)
    {
        foreach (Transform child in card.transform)
        {
            TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                if (color == "red")
                {
                    text.color = Color.red;
                }
                if (color == "blue")
                {
                    text.color = Color.blue;
                }
            }
        }
    }

    void Winmove(GameObject card)
    {
        List<int> winpos = card.GetComponent<MoveController>().GetWinpos();

        if (winpos.Contains(int.Parse(card.transform.parent.name)))
        {
            mainfunction.卡牌摧毁(card);
            Debug.Log("win");
        }
    }

    IEnumerator RotateAndScaleCoroutine(GameObject card)
    {
        Vector3 originalScale = card.transform.localScale;
        float elapsedTime = 0f;

        float rotationSpeed = 360f;
        float fadeDuration = 2f; // 持续时间

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration; // 时间比例 [0, 1]

            // 计算缩放
            float scale = Mathf.Lerp(1f, 0f, t);
            card.transform.localScale = new Vector3(scale, scale, 1f);

            // 计算旋转
            float rotation = rotationSpeed * Time.deltaTime;
            card.transform.Rotate(Vector3.forward, rotation);

            yield return null;
        }

        // 确保最终状态
        card.transform.localScale = Vector3.zero;
        Destroy(card); // 可选择禁用物体
    }

    void process(Message mes)
    {
        Debug.Log(JsonUtility.ToJson(mes));
        if (mes.Action == 5)
        {
            level_room(mes);
        }
        else if (mes.Action == 9)
        {
            summon_card(mes);
        }
        else if (mes.Action == 10) { 
            move_card(mes);
        }else if (mes.Action == 12)
        {
            nextTurn();
        }else if (mes.Action == 13)
        {
            destroy_card(mes);
        }else if (mes.Action == 14)
        {
            中立法术创建(mes);
        }else if (mes.Action == 15)
        {
            中立法术销毁(mes);
        }else if (mes.Action == 16)
        {
            法术禁用(mes);
        }else if (mes.Action == 17)
        {
            法术禁用取消(mes);
        }else if (mes.Action == 18)
        {
            人物位置禁用(mes);
        }else if(mes.Action == 19)
        {
            人物位置禁用取消(mes);
        }else if (mes.Action == 20)
        {
            我方红牌法术创建(mes);
        }else if (mes.Action == 21)
        {
            我方红牌法术销毁(mes);
        }else if (mes.Action == 22)
        {
            人物禁用(mes);
        }else if (mes.Action == 23)
        {
            人物禁用取消(mes);
        }else if (mes.Action == 24)
        {
            敌方红牌法术创建(mes);
        }else if (mes.Action == 25){
            敌方红牌法术销毁(mes);
        }else if (mes.Action == 26)
        {
            效果挂载(mes);
        }else if (mes.Action == 27){
            效果卸载(mes);
        }else if (mes.Action == 28)
        {
            卡牌摧毁(mes);
        }else if (mes.Action == 29)
        {
            Attack(mes);
        }else if (mes.Action == 30){
            血量上限改变(mes);
        }else if (mes.Action == 31)
        {
            攻击力改变(mes);
        }else if (mes.Action == 32)
        {
            倒计时(mes);
        }else if (mes.Action == 33)
        {

        }else if (mes.Action == 34)
        {
            回合结束弃牌(mes);
        }
        else if (mes.Action == 37)
        {
            摸牌(mes);
        }
        else if (mes.Action == 38)
        {
            我方暂停();
        } else if (mes.Action == 39)
        {
            我方继续();
        }else if (mes.Action == 40)
        {
            技能释放申请(mes);
        }else if (mes.Action == 41)
        {
            技能释放申请同意(mes);
        }else if (mes.Action == 42)
        {
            弃牌堆更新(mes);
        }else if (mes.Action == 43)
        {
            治疗(mes);
        }else if (mes.Action == 44)
        {
            扣血(mes);
        }else if (mes.Action == 45)
        {
            攻击申请(mes);
        }else if (mes.Action == 46)
        {
            攻击申请同意(mes);
        }else if (mes.Action == 47)
        {
            扣发思考中(mes);
        }else if (mes.Action == 48)
        {
            扣发思考结束(mes);
        }
    }

    void destroy_card(Message mes)
    {
        string end_index = mainfunction.Convertposition(mes.end_index).ToString();
        Debug.Log(end_index);
        if (ValueHolder.棋盘[end_index].transform.childCount != 0)
        {
            GameObject card_to_del = ValueHolder.棋盘[end_index].transform.GetChild(0).gameObject;
            StartCoroutine(RotateAndScaleCoroutine(card_to_del));
        }

    }

    void Attack(Message mes)
    {
        GameObject mycard = mainfunction.uid找卡(mes.uid);
        GameObject hecard = mainfunction.uid找卡(mes.uid1);

        mainfunction.cardAttack(mes.uid, mes.uid1);
    }

    void level_room(Message mes)
    {
        hintManager.AddHint(mes.SendUser + "已离开房间！");
    }

    void summon_card(Message mes)
    {
        GameObject cardone = Instantiate(commoncard);
        cardone = summon_one(cardone, mes.cardID);
        cardone.transform.SetParent(ValueHolder.棋盘[mes.start_index.ToString()].transform, false);
        change_card_color(cardone, "red");
        cardone.GetComponent<MoveController>().cardType = 1;
        cardone.GetComponent<b_enemy>().enabled = true;
        cardone.GetComponent<b_cardaction>().enabled = false;
        cardone.transform.Rotate(0, 0, 180);

        cardone.GetComponent<数据显示>().卡牌数据.uid = mes.uid;
    }

    void move_card(Message mes)
    {
        int start_index = mes.start_index;
        int end_index = mes.end_index;
        GameObject cardone = ValueHolder.棋盘[start_index.ToString()].transform.GetChild(0).gameObject;
        cardone.transform.SetParent(ValueHolder.棋盘[end_index.ToString()].transform, false);
        Debug.Log("move");
        Winmove(cardone);
        mainfunction.DestroyAllChildren(ValueHolder.放大展示区1);
        mainfunction.DestroyAllChildren(ValueHolder.放大展示区2);

    }

    void nextTurn()
    {

        nextTurn_First();
        nextTurn_Second();
        StartCoroutine(nextTurn_Third_Coroutine());

    }

    private IEnumerator nextTurn_Third_Coroutine()
    {

        while (SkillExecutor.skillQueue.Count > 0 || SkillExecutor.currentRunningSkillUid != null)
        {
            yield return new WaitForSeconds(0.2f);
        }

        nextTurn_Third();
    }
    void nextTurn_First()
    {
        ValueHolder.point = 1;
        ValueHolder.is_myturn = 1;
        体力.text = ValueHolder.point.ToString();
        hintManager.AddHint("我的回合！");
        activae_action();

        ValueHolder.turn += 0.5f;
        回合数.text = ((int)Mathf.Floor(ValueHolder.turn)).ToString();

        mainfunction.效果卸载遍历();
        mainfunction.倒计时回合变化();
        mainfunction.灵力回合更新();
    }

    void nextTurn_Second()
    {
        skillturn();
    }

    void nextTurn_Third()
    {
        summonHandcard(1);
    }

     void summonHandcard(int num)
    {
        foreach (int id in mainfunction.SumomonHandCardID(num))
        {
            GameObject cardone = Instantiate(commoncard);
            cardone = summon_one(cardone, id);
            cardone.transform.SetParent(手牌区.transform, false);
        }
    }

    public GameObject summon_one(GameObject commoncard, int id)
    {
        Texture2D texture = Resources.Load<Texture2D>("card/" + id.ToString());
        卡牌数据 原卡牌数据 = ValueHolder.gloabCaedData[id];
        // 深拷贝
        卡牌数据 新卡牌数据 = new 卡牌数据(原卡牌数据);
        commoncard.GetComponent<数据显示>().卡牌数据 = 新卡牌数据;
        commoncard.GetComponent<数据显示>().enabled = true;
        commoncard.GetComponent<数据显示>().卡牌数据.uid = System.Guid.NewGuid().ToString();
        if (texture != null)
        {
            commoncard.GetComponentsInChildren<Image>()[3].sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        System.Type scriptType = System.Type.GetType(ValueHolder.gloabCaedData[id].名字);
        if (scriptType != null)
        {
            commoncard.AddComponent(scriptType);
        }

        return commoncard;
    }

    void skillturn()
    {
        List<string> skillDelete = new List<string>();
        foreach (KeyValuePair<string,BaseSkill> item in ValueHolder.SkillAction)
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

                if (skill.己方回合开始时触发 == 1)
                {
                    mainfunction.运行己方回合开始时触发技能阶段(skill);
                }
            }
            mainfunction.DestroyAllChildren(ValueHolder.放大展示区1);
            mainfunction.DestroyAllChildren(ValueHolder.放大展示区2);

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

    void activae_action()
    {
        下个回合.interactable = true;
        下个回合.image.color = Color.white;
        foreach (GameObject item in ValueHolder.棋盘.Values)
        {
            if (item.transform.gameObject.name == "棋盘")
            {
                continue;
            }
            if (item.transform.childCount > 0 && item.transform.GetChild(0).gameObject.GetComponent<MoveController>().cardType == 0)
            {
                GameObject cardone = item.transform.GetChild(0).gameObject;
                cardone.GetComponent<MoveController>().point = 1;
                cardone.GetComponent<MoveController>().is_myturn = 1;
                cardone.GetComponent<b_moveca>().enabled = true;

            }
        }
        mainfunction.启用手牌物件代码("b_cardaction");

    }

    void 中立法术创建(Message mes)
    {
        卡牌数据 card_data = ValueHolder.gloabCaedData[mes.cardID];
        GameObject cardone = Instantiate(ValueHolder.延时法术牌);
        GameObject card_summon = summon_one(cardone, card_data.id);
        mainfunction.缩放调整(card_summon);
        card_summon.transform.SetParent(ValueHolder.中立延时法术框.transform);
    }

    void 中立法术销毁(Message mes)
    {
        卡牌数据 card_data = ValueHolder.gloabCaedData[mes.cardID];
        foreach (Transform child in ValueHolder.中立延时法术框.transform)
        {
            if (child.gameObject.GetComponent<数据显示>().卡牌数据.id == card_data.id)
            {
                Destroy(child.gameObject);
            }
        }
    }

    void 我方红牌法术创建(Message mes)
    {
        卡牌数据 card_data = ValueHolder.gloabCaedData[mes.cardID];
        GameObject cardone = Instantiate(ValueHolder.延时法术牌);
        GameObject card_summon = summon_one(cardone, card_data.id);
        mainfunction.缩放调整(card_summon);
        change_card_color(card_summon, "red");
        card_summon.transform.SetParent(ValueHolder.我方延时法术框.transform);

        card_summon.GetComponent<数据显示>().卡牌数据.uid = mes.uid;
        Debug.Log(mes.uid);
    }

    void 我方红牌法术销毁(Message mes)
    {
        卡牌数据 card_data = ValueHolder.gloabCaedData[mes.cardID];
        foreach (Transform child in ValueHolder.我方延时法术框.transform)
        {
            Debug.Log(mes.uid);
            if (child.gameObject.GetComponent<数据显示>().卡牌数据.uid == mes.uid)
            {
                Destroy(child.gameObject);
            }
        }
    }

    void 敌方红牌法术创建(Message mes)
    {
        卡牌数据 card_data = ValueHolder.gloabCaedData[mes.cardID];
        GameObject cardone = Instantiate(ValueHolder.延时法术牌);
        GameObject card_summon = summon_one(cardone, card_data.id);
        mainfunction.缩放调整(card_summon);
        change_card_color(card_summon, "red");
        card_summon.transform.SetParent(ValueHolder.敌方延时法术框.transform);

        card_summon.GetComponent<数据显示>().卡牌数据.uid = mes.uid;
    }

    void 敌方红牌法术销毁(Message mes)
    {
        卡牌数据 card_data = ValueHolder.gloabCaedData[mes.cardID];
        foreach (Transform child in ValueHolder.敌方延时法术框.transform)
        {
            if (child.gameObject.GetComponent<数据显示>().卡牌数据.uid == mes.uid)
            {
                Destroy(child.gameObject);
            }
        }
    }

    void 法术禁用(Message mes)
    {
        string name = ValueHolder.gloabCaedData[mes.cardID].名字;
        ValueHolder.法术禁用.Add(name, mes.turn);
    }

    void 法术禁用取消(Message mes)
    {
        string name = ValueHolder.gloabCaedData[mes.cardID].名字;
        ValueHolder.法术禁用.Remove(name);
    }

    void 人物禁用(Message mes)
    {
        ValueHolder.人物禁用.Add(mes.uid, mes.turn);
        ValueHolder.uid_to_name.Add(mes.uid, ValueHolder.gloabCaedData[mes.cardID].名字);
    }

    void 人物禁用取消(Message mes)
    {
        ValueHolder.人物禁用.Remove(mes.uid);
        ValueHolder.uid_to_name.Remove(mes.uid);
        Debug.Log("人物禁用取消:" + ValueHolder.人物禁用.Count);
    }

    void 人物位置禁用(Message mes)
    {
        List<int> list1 = new List<int>();
        string cardname = ValueHolder.gloabCaedData[mes.cardID].名字;
        list1.Add(mes.start_index);
        mainfunction.位置禁用("角色", cardname, list1);
    }

    void 人物位置禁用取消(Message mes)
    {
        string cardname = ValueHolder.gloabCaedData[mes.cardID].名字;
        mainfunction.位置禁用取消("角色", cardname);
    }

    void 效果挂载(Message mes)
    {
        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card = grid.transform.GetChild(0).gameObject;
                if (card.GetComponent<数据显示>().卡牌数据.uid == mes.uid)
                {
                    if (mes.effect == 1) //击杀免疫
                    {
                        card.GetComponent<MoveController>().击杀免疫 = 1;
                    }
                    else if (mes.effect == 2) //消灭免疫
                    {
                        card.GetComponent<MoveController>().消灭免疫 = 1;
                    }
                    else if (mes.effect == 3) //眩晕
                    {
                        card.GetComponent<MoveController>().眩晕 = 1;
                    }
                    else if (mes.effect == 4)//法术不可作用
                    {
                        card.GetComponent<MoveController>().法术可作用 = 0;
                    }
                    else if(mes.effect == 5)//眩晕免疫
                    {
                        card.GetComponent<MoveController>().眩晕免疫 = 1;
                    }
                    else if (mes.effect == 6)//识水
                    {
                        card.GetComponent<MoveController>().识水 = 1;
                    }else if (mes.effect == 7)//无双
                    {
                        card.GetComponent<MoveController>().无双 = 1;
                    }else if (mes.effect == 8)//免疫攻击一次
                    {
                        card.GetComponent<MoveController>().攻击免疫次数 += 1;
                    }
                }
            }
        }
        Effect 效果卸载 = new Effect
        {
            effectID = mes.effect,
            uid = mes.uid,
            turn = mes.turn
        };
        ValueHolder.效果卸载队列.Add(效果卸载);
    }

    void 效果卸载(Message mes)
    {
        foreach (KeyValuePair<string, GameObject> grids in ValueHolder.棋盘)
        {
            GameObject grid = grids.Value;
            if (grid.transform.childCount != 0 && grids.Key != "0")
            {
                GameObject card = grid.transform.GetChild(0).gameObject;
                if (card.GetComponent<数据显示>().卡牌数据.uid == mes.uid)
                {
                    if (mes.effect == 1) //击杀免疫
                    {
                        card.GetComponent<MoveController>().击杀免疫 = 0;
                    }else if (mes.effect == 2) //消灭免疫
                    {
                        card.GetComponent<MoveController>().消灭免疫 = 0;
                    }
                    else if (mes.effect == 3) //眩晕
                    {
                        card.GetComponent<MoveController>().眩晕 = 0;
                    }
                    else if (mes.effect == 4)//法术不可作用
                    {
                        card.GetComponent<MoveController>().法术可作用 = 1;
                    }
                    else if (mes.effect == 5)//眩晕免疫
                    {
                        card.GetComponent<MoveController>().眩晕免疫 = 0;
                    }
                    else if (mes.effect == 6)//识水
                    {
                        card.GetComponent<MoveController>().识水 = 1;
                    }
                    else if (mes.effect == 7)//无双
                    {
                        card.GetComponent<MoveController>().无双 = 1;
                    }
                }
            }
        }
    }

    void 卡牌摧毁(Message mes)
    {
        GameObject card = mainfunction.uid找卡(mes.uid);
        if (card != null)
        {
            mainfunction.卡牌摧毁(card);
        }



        mainfunction.DestroyAllChildren(ValueHolder.放大展示区1);
        mainfunction.DestroyAllChildren(ValueHolder.放大展示区2);
    }

    void 血量上限改变(Message mes)
    {
        GameObject card = mainfunction.uid找卡(mes.uid);
        if (card != null)
        {
            mainfunction.血量上限改变(card, mes.num);

        }
    }

    void 攻击力改变(Message mes)
    {
        GameObject card = mainfunction.uid找卡(mes.uid);
        if (card != null)
        {
            mainfunction.攻击力改变(card, mes.num,0);
        }
    }

    void 倒计时(Message mes)
    {
        ValueHolder.倒计时储存.Add(mes.uid, (float)mes.num);
    }

    void 回合结束弃牌(Message mes)
    {
        ValueHolder.回合结束弃牌 = 1;
        ValueHolder.弃牌数量限制 = mes.num;
    }
    void 摸牌(Message mes)
    {
        summonHandcard(mes.num);
    }

    void 我方暂停()
    {
        mainfunction.禁用棋盘物件代码("b_moveca", 0);
        mainfunction.禁用手牌物件代码("b_cardaction");
        ValueHolder.下个回合.interactable = false;
        ValueHolder.下个回合.image.color = Color.gray;
        ValueHolder.hintManager.AddHint("等待对方响应");

        SkillExecutor.currentRunningSkillUid = "我方暂停";
    }

    void 我方继续()
    {
        mainfunction.启用棋盘物件代码("b_moveca", 0);
        mainfunction.启用手牌物件代码("b_cardaction");
        mainfunction.禁用棋盘物件代码("b_cardaction", 0);
        ValueHolder.下个回合.interactable = true;
        ValueHolder.下个回合.image.color = Color.white;

        SkillExecutor.currentRunningSkillUid = null;
    }

    void 技能释放申请(Message mes)
    {
        Dictionary<string, int> effect1 = new Dictionary<string, int>();
        effect1.Add(mes.uid, mes.num);
        ValueHolder.申请释放技能队列.Enqueue(effect1);
    }

    void 技能释放申请同意(Message mes)
    {
        mainfunction.Send对方暂停();
        BaseSkill skill = ValueHolder.SkillAction[mes.uid];
        int skill_type = mes.num;
        ValueHolder.敌方回合运行我方技能 = 1;


        //0 : 普通运行下个技能阶段
        //1 : 亡语
        //2 : 场上角色死亡触发
        //3 : 场上我方角色死亡触发
        //4 : 场上敌方角色死亡触发
        //5 : 己方回合开始时触发
        //6 : 己方回合结束时触发
        //7 : 杀人后触发
        //8 : 血恨
        //9 : 血量增加时触发
        //10 : 血量降低时触发

        if (skill_type == 0)
        {
            mainfunction.运行下个技能阶段(skill);
        }else if (skill_type == 1)
        {
            mainfunction.运行亡语技能阶段(skill);
        }else if (skill_type == 2)
        {
            mainfunction.运行场上角色死亡触发技能阶段(skill);
        }else if (skill_type == 3)
        {
            mainfunction.运行场上我方角色死亡触发技能阶段(skill);
        }else if (skill_type == 4)
        {
            mainfunction.运行场上敌方角色死亡触发技能阶段(skill);
        }else if (skill_type == 5)
        {
            mainfunction.运行己方回合开始时触发技能阶段(skill);
        }else if (skill_type == 6)
        {
            mainfunction.运行己方回合结束时触发技能阶段(skill);
        }else if (skill_type == 7)
        {
            mainfunction.运行杀人后触发技能阶段(skill);
        }else if (skill_type == 8)
        {
            mainfunction.运行血恨技能阶段(skill);
        }else if (skill_type == 9)
        {
            mainfunction.运行血量增加时触发技能阶段(skill);
        }else if (skill_type == 10)
        {
            mainfunction.运行血量降低时触发技能阶段(skill);
        }

    }


    void 弃牌堆更新(Message mes)
    {
        int cardID = mes.cardID;
        mainfunction.弃牌堆更新(cardID,0);
    }


    void 治疗(Message mes)
    {
        GameObject card = mainfunction.uid找卡(mes.uid);
        if (card != null)
        {
            mainfunction.治疗(card, mes.num,0);

        }
    }

    void 扣血(Message mes)
    {
        GameObject card = mainfunction.uid找卡(mes.uid);
        if (card != null)
        {
            mainfunction.扣血(card, mes.num,0);
        }
    }


    
    void 攻击申请(Message mes)
    {
        ValueHolder.Listen_主动攻击uid = mes.uid;
        ValueHolder.Listen_承受攻击uid = mes.uid1;

        int temp = 0;
        foreach (KeyValuePair<string,BaseSkill > kvp in ValueHolder.扣发技能)
        {
            BaseSkill skill = kvp.Value;

            if (skill.扣发触发条件 == "敌方攻击")
            {
                if (temp == 0)
                {
                    temp = 1;
                    mainfunction.Send扣发思考中();
                }

                ValueHolder.等待攻击扣发技能数量 += 1;
                mainfunction.运行下个技能阶段(skill);
            }

        }

        StartCoroutine(等待攻击同意(mes));
    }

    IEnumerator 等待攻击同意(Message mes)
    {
        while (ValueHolder.等待攻击扣发技能数量 >= 1 && SkillExecutor.skillQueue.Count >= 1)
        {
            yield return new WaitForSeconds(0.1f);
        }
        mainfunction.Send扣发思考结束();
        mainfunction.cardAttack(mes.uid, mes.uid1);
        mainfunction.Send攻击同意(mes.uid, mes.uid1);

    } 

    void 攻击申请同意(Message mes)
    {
        mainfunction.cardAttack(mes.uid, mes.uid1);
    }

    void 扣发思考中(Message mes)
    {
        ValueHolder.幕布.SetActive(true);
        ValueHolder.hintManager.AddHint("敌方扣发思考中。。。");
    }

    void 扣发思考结束(Message mes)
    {
        ValueHolder.幕布.SetActive(false);
    }










}
