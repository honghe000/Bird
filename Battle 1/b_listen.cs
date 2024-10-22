using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
                Message mes = JsonUtility.FromJson<Message>(ValueHolder.receiveQueue.Dequeue());
                mes.is_used = 1;
                mainfunction.ChangeRecieveMessage("is_used", 1);
                process(mes); // 处理消息
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
            血量改变(mes);
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
        else if (mes.Action == 35)
        {
            摸牌(mes);
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

        mainfunction.cardAttack(mycard, hecard,1);
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
        ValueHolder.point = 1;
        ValueHolder.is_myturn = 1;
        体力.text = ValueHolder.point.ToString();
        hintManager.AddHint("我的回合！");
        activae_action();

        ValueHolder.turn += 0.5f;
        回合数.text = ((int)Mathf.Floor(ValueHolder.turn)).ToString();

        skillturn();
        mainfunction.倒计时回合变化();

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
                    skill.Action_1();
                }
                if (skill.activateTurn_2_finish == 0 && skill.activateTurn_2 == ValueHolder.turn)
                {
                    skill.Action_2();
                }
                if (skill.activateTurn_3_finish == 0 && skill.activateTurn_3 == ValueHolder.turn)
                {
                    skill.Action_3();
                }
                if (skill.activateTurn_4_finish == 0 && skill.activateTurn_4 == ValueHolder.turn)
                {
                    skill.Action_4();
                }

                if (skill.己方回合开始时触发 == 1)
                {
                    mainfunction.运行下个技能阶段(skill);
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

                }
            }
        }
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

    void 血量改变(Message mes)
    {
        GameObject card = mainfunction.uid找卡(mes.uid);
        if (card != null)
        {
            card.GetComponent<数据显示>().卡牌数据.nowHp += mes.num;
            card.GetComponent<数据显示>().卡牌数据.maxHp += mes.num;
            card.GetComponent<数据显示>().更新数据();
        }
    }

    void 攻击力改变(Message mes)
    {
        GameObject card = mainfunction.uid找卡(mes.uid);
        if (card != null)
        {
            if (mes.num == -999)
            {
                card.GetComponent<数据显示>().卡牌数据.nowAttack = 0;
                card.GetComponent<数据显示>().卡牌数据.maxAttack = 0;
                card.GetComponent<数据显示>().更新数据();
                return;
            }
            card.GetComponent<数据显示>().卡牌数据.nowAttack += mes.num;
            card.GetComponent<数据显示>().卡牌数据.maxAttack += mes.num;
            card.GetComponent<数据显示>().更新数据();
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
}
