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


        ValueHolder.延时法术牌 = null;
        ValueHolder.手牌对战牌 = null;
        ValueHolder.initiative = 1;
        ValueHolder.random_card = new List<int> { 1, 2, 3, 4, 76 };
        ValueHolder.Discard_pile.Clear();
        ValueHolder.choosegroup = "666";
        ValueHolder.choosed_object = null;
        ValueHolder.copyed_object = null;
        ValueHolder.is_choose = 0;
        ValueHolder.棋盘.Clear();
        ValueHolder.手牌区 = null;
        ValueHolder.拖拽序号 = 0;
        ValueHolder.视图 = null;
        ValueHolder.turn = 1;
        ValueHolder.法术选择取消 = null;

        ValueHolder.is_myturn = 1;
        ValueHolder.point = 1;

        ValueHolder.hintManager = null;
        ValueHolder.SkillAction.Clear();

        ValueHolder.人物可放置位置 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        ValueHolder.建筑可放置位置 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        ValueHolder.人物禁止放置位置.Clear();
        ValueHolder.建筑禁止放置位置.Clear();

        ValueHolder.中立延时法术框 = null;
        ValueHolder.敌方延时法术框 = null;
        ValueHolder.我方延时法术框 = null;

        ValueHolder.法术禁用.Clear();
        ValueHolder.人物禁用.Clear();

        ValueHolder.释放法术uid = null;
        ValueHolder.uid_to_name.Clear();
        ValueHolder.法术作用敌我类型 = 0;
        ValueHolder.放大展示区1 = null;
        ValueHolder.放大展示区2 = null;

        ValueHolder.倒计时储存.Clear();
        ValueHolder.倒计时显示1 = null;
        ValueHolder.倒计时显示2 = null;
        ValueHolder.体力 = null;

        ValueHolder.弃牌 = null;
        ValueHolder.弃牌区 = null;
        ValueHolder.弃牌数量 = null;
        ValueHolder.弃牌显示 = null;
        ValueHolder.弃牌uid.Clear();
        ValueHolder.弃牌数量限制 = 0;
        ValueHolder.回合结束弃牌 = 0;
        ValueHolder.下个回合 = null;

        ValueHolder.黄 = null;
        ValueHolder.绿 = null;
        ValueHolder.蓝 = null;
        ValueHolder.紫 = null;
        ValueHolder.灵力栏 = null;
        ValueHolder.灵力当前状态 = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 } };
        ValueHolder.灵力当前上限 = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 } };

        ValueHolder.启用点选格子 = 0;
        ValueHolder.点击格子编号 = 0;
        ValueHolder.点选技能uid = null;
        ValueHolder.效果卸载队列.Clear();
        ValueHolder.申请释放技能队列.Clear();
        //ValueHolder.技能释放中数量 = 0;

        ValueHolder.占卜区 = null;
        ValueHolder.占卜牌 = null;
        ValueHolder.占卜牌堆底 = null;
        ValueHolder.占卜牌堆顶 = null;
        ValueHolder.幕布 = null;
        ValueHolder.占卜数量 = 0;

        ValueHolder.弃牌堆 = null;
        ValueHolder.弃牌堆卡牌 = null;
        ValueHolder.弃牌堆关闭 = null;
        ValueHolder.我方弃牌堆卡牌编号.Clear();
        ValueHolder.敌方弃牌堆卡牌编号.Clear();

        SceneManager.LoadScene("mainpage");
    }
}
