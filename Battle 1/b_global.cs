using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class b_global : MonoBehaviour
{
    public GameObject 棋盘;
    public TextMeshProUGUI 体力;
    public GameObject 中立延时法术框;
    public GameObject 敌方延时法术框;
    public GameObject 我方延时法术框;
    public GameObject 延时法术牌;
    public GameObject 手牌区;
    public GameObject 手牌对战牌;
    public GameObject 视图;
    public HintManager hintManager;
    public Button 法术选择取消;
    public GameObject 放大展示区1;
    public GameObject 放大展示区2;

    public Image 倒计时显示1;
    public Image 倒计时显示2;

    public GameObject 弃牌;
    public TextMeshProUGUI 弃牌数量;
    public GameObject 弃牌显示;
    public GameObject 弃牌区;
    // Start is called before the first frame update

    public GameObject 黄;
    public GameObject 绿;
    public GameObject 蓝;
    public GameObject 紫;
    public GameObject 灵力栏;

    public Button 下个回合;

    public GameObject 占卜牌;
    public GameObject 幕布;
    public GameObject 占卜区;
    public GameObject 占卜牌堆顶;
    public GameObject 占卜牌堆底;
    public GameObject 占卜确认按钮;

    //弃牌堆
    public GameObject 弃牌堆;
    public GameObject 弃牌堆卡牌;
    public Button 弃牌堆关闭;

    private void Start()
    {
        打乱牌组();
        生成棋盘();
        静态设置();
        mainfunction.灵力回合更新();
        if (ValueHolder.initiative == 1)
        {
            ValueHolder.is_myturn = 1;
            ValueHolder.point = 1;
            体力.text = "0";
        }
        else
        {
            ValueHolder.is_myturn = 0;
            ValueHolder.point = 0;
            体力.text = "0";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void 打乱牌组()
    {

        Shuffle(ValueHolder.random_card);


    }
    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    void 生成棋盘()
    {
        Dictionary<string, GameObject>  棋盘物件字典 = new Dictionary<string, GameObject>();

        for (int i = 1; i <= 25; i++)
        {
            string 编号 = i.ToString();
            GameObject 物件 = 棋盘.transform.Find(编号).gameObject;

            if (物件 != null)
            {
                棋盘物件字典.Add(编号, 物件);
            }
            else
            {
                Debug.LogError("找不到编号为 " + 编号 + " 的物件");
            }
        }
        棋盘物件字典.Add("0",棋盘);

        ValueHolder.棋盘 = 棋盘物件字典;
    }

    void 静态设置()
    {
        ValueHolder.中立延时法术框 = 中立延时法术框;
        ValueHolder.敌方延时法术框 = 敌方延时法术框;
        ValueHolder.我方延时法术框 = 我方延时法术框;
        ValueHolder.延时法术牌 = 延时法术牌;
        ValueHolder.手牌区 = 手牌区;
        ValueHolder.手牌对战牌 = 手牌对战牌;
        ValueHolder.视图 = 视图;
        ValueHolder.hintManager = hintManager;
        ValueHolder.法术选择取消 = 法术选择取消;
        ValueHolder.放大展示区1 = 放大展示区1;
        ValueHolder.放大展示区2 = 放大展示区2;

        ValueHolder.体力 = 体力;

        ValueHolder.倒计时显示1 = 倒计时显示1;
        ValueHolder.倒计时显示2 = 倒计时显示2;

        ValueHolder.弃牌数量 = 弃牌数量;
        ValueHolder.弃牌显示 = 弃牌显示;
        ValueHolder.弃牌区 = 弃牌区;
        ValueHolder.弃牌 = 弃牌;

        ValueHolder.黄 = 黄;
        ValueHolder.绿 = 绿;
        ValueHolder.蓝 = 蓝;
        ValueHolder.紫 = 紫;
        ValueHolder.灵力栏 = 灵力栏;

        ValueHolder.下个回合 = 下个回合;

        ValueHolder.占卜牌 = 占卜牌;
        ValueHolder.幕布 = 幕布;
        ValueHolder.占卜区 = 占卜区;
        ValueHolder.占卜牌堆顶 = 占卜牌堆顶;
        ValueHolder.占卜牌堆底 = 占卜牌堆底;
        ValueHolder.占卜确认按钮 = 占卜确认按钮;

        ValueHolder.弃牌堆 = 弃牌堆;
        ValueHolder.弃牌堆卡牌 = 弃牌堆卡牌;
        ValueHolder.弃牌堆关闭 = 弃牌堆关闭;
    }
}
