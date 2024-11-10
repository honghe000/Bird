using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class ValueHolder : MonoBehaviour
{

    //mysql
    public static string mysql_ip = "z9098198c6.goho.co";
    
    
    public static string mysql_port = "59073";
    public static string mysql_username = "root";
    public static string mysql_pwd = "Zhanghehan123";
    public static string mysql_database = "card";

    public static float scale_x = 1.0f;
    public static float scale_y = 1.0f;

    //socket
    //public static string socket_ip = "127.0.0.1";
    //public static int socket_port = 12345;

    // public static string socket_ip = "cn-hb-sy-2.starryfrp.com";
    // public static int socket_port = 55655;

    public static string socket_ip = "222.211.75.240";
    public static int socket_port = 43185;

    public static TcpClient client = null;
    public static readonly object sendLock = new object();
    public static string SendMessages = "{\"SendUser\":\"\",\"ReceiveUser\":\"\",\"chat\":\"\",\"RoomName\":\"\",\"Action\":0,\"is_used\":1,\"cradgroup\":\"\",\"cardID\":1,\"start_index\":1,\"end_index\":1,\"initiative\":1,\"turn\":0.5,\"uid\":\"\",\"effect\":0,\"num\":0}";
    public static string ResieveMessages = "{\"SendUser\":\"\",\"ReceiveUser\":\"\",\"chat\":\"\",\"RoomName\":\"\",\"Action\":0,\"is_used\":1,\"cradgroup\":\"\",\"cardID\":1,\"start_index\":1,\"end_index\":1,\"initiative\":1,\"turn\":0.5,\"uid\":\"\",\"effect\":0,\"num\":0}";

    public static Queue<string> sendQueue = new Queue<string>();
    public static Queue<string> receiveQueue = new Queue<string>();

    //cardstore
    public static string username = "test";
    public static string coin = "2000";
    public static Dictionary<int,int> cardData = new Dictionary<int, int> { { 1,2},{ 2,2},{ 3,3},{ 4,4},{ 5,5} };



    //cardstore
    public static bool isplace = false;
    public static Dictionary<int, int> cardDataexsist = new Dictionary<int, int>();


    //globalLoadCard
    public static Dictionary<int, 卡牌数据> gloabCaedData = new Dictionary<int, 卡牌数据>();


    //cardgroup
    public static string cardgroupName = "test";
    public static Dictionary<string, Dictionary<string, int>> cardgroupdata = new Dictionary<string, Dictionary<string, int>>();
    public static Dictionary<string, int> cardgroupchoose = new Dictionary<string, int>();



    //cardMove-battle
    public static 卡牌数据 MovecardData = null;

    public static List<Vector3> vector3s = new List<Vector3>();

    public static List<GameObject> movegameobjects = new List<GameObject>();

    public static int isChoose = 0;
    public static GameObject isChooseCard = null;
    public static int isChooseCardindex = 0;

    //提示边框
    public static List<GameObject> tishi = new List<GameObject>();
    public static List<int> tishiInt = new List<int>();
    public static List<GameObject> tishilist = new List<GameObject>();

    //卡牌显现下标
    public static List<int> xianxain = new List<int>();

    //socketmove
    public static string moveString = "1;1";


    //socketroom
    public static string socketroom = "";

    //Room
    public static string Room = "";
    public static int my_ready = 0;
    public static int he_ready = 0;
    public static int room_is_host = 0;
    public static string hename = "";


    //questionSystem
    public static bool isTrue = false;
    public static List<问答数据> questionList;
    public static int isRuning = 0;//防止多次点击确定按钮
    public static int isRuning1 = 0;//防止多次点击每日答题

    //battle1
    public static GameObject 延时法术牌 = null;
    public static GameObject 手牌对战牌 = null;
    public static GameObject 弃牌 = null;
    public static int initiative = 1;
    public static List<int> random_card = new List<int> { 1,2,2,4,168};
    public static List<int> Discard_pile = new List<int>();
    public static string choosegroup = "666";
    public static GameObject choosed_object = null;
    public static GameObject copyed_object = null;
    public static int is_choose = 0;
    public static Dictionary<string, GameObject> 棋盘 = new Dictionary<string, GameObject>();
    public static GameObject 手牌区 = null;
    public static int 拖拽序号 = 0;
    public static GameObject 视图 = null;
    public static float turn = 1;
    public static Button 法术选择取消 = null;

    public static int is_myturn = 1;
    public static int point = 1;

    public static HintManager hintManager = null;
    public static Dictionary<string, BaseSkill> SkillAction = new Dictionary<string, BaseSkill>();

    public static List<int> 人物可放置位置 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    public static List<int> 建筑可放置位置 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    public static Dictionary<string,List<int>> 人物禁止放置位置 = new Dictionary<string, List<int>>();
    public static Dictionary<string,List<int>> 建筑禁止放置位置 = new Dictionary<string, List<int>>();

    public static GameObject 中立延时法术框 = null;
    public static GameObject 敌方延时法术框 = null;
    public static GameObject 我方延时法术框 = null;

    public static Dictionary<string, float> 法术禁用 = new Dictionary<string, float>();
    public static Dictionary<string, float> 人物禁用 = new Dictionary<string, float>();

    public static string 释放法术uid = null;
    public static TextMeshProUGUI 体力 = null;

    public static Dictionary<string,string> uid_to_name = new Dictionary<string, string>();
    public static int 法术作用敌我类型 = 0;
    public static GameObject 放大展示区1 = null;
    public static GameObject 放大展示区2 = null;

    public static Dictionary <string,float> 倒计时储存 = new Dictionary<string, float>();
    public static Image 倒计时显示1 = null;
    public static Image 倒计时显示2 = null;

    public static Button 下个回合 = null;

    public static TextMeshProUGUI 弃牌数量 = null;
    public static GameObject 弃牌显示 = null;
    public static GameObject 弃牌区 = null;
    public static List<string> 弃牌uid = new List<string>();
    public static int 弃牌数量限制 = 0;
    public static int 回合结束弃牌 = 0;

    //灵力
    public static GameObject 黄 = null;
    public static GameObject 绿 = null;
    public static GameObject 蓝 = null;
    public static GameObject 紫 = null;
    public static GameObject 灵力栏 = null;


    public static Dictionary<int,int> 灵力当前状态 = new Dictionary<int,int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }};
    public static Dictionary<int,int> 灵力当前上限 = new Dictionary<int,int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }};

    //点选格子
    public static int 启用点选格子 = 0;
    public static int 点击格子编号 = 0;
    public static string 点选技能uid = null;

    //效果卸载
    public static List<Effect> 效果卸载队列 = new List<Effect>();

    ////技能执行先后级临时存储
    public static Queue<string> 申请释放技能队列 = new Queue<string>();
    //public static int 技能释放中数量 = 0;
    //public static string 正在运行技能uid = null;

    public static int 敌方回合运行我方技能 = 0;


    //占卜
    public static GameObject 占卜牌 = null;
    public static GameObject 占卜区 = null;
    public static GameObject 占卜牌堆顶= null;
    public static GameObject 占卜牌堆底 = null;
    public static GameObject 占卜确认按钮 = null;

    public static GameObject 幕布 = null;

    public static int 占卜数量 = 0;
    public static int 占卜后立即执行技能 = 0;

    //弃牌堆
    public static GameObject 弃牌堆 = null;
    public static GameObject 弃牌堆显示区 = null;
    public static GameObject 弃牌堆卡牌 = null;
    public static Button 弃牌堆关闭 = null;
    public static List<int> 我方弃牌堆卡牌编号 = new List<int>();
    public static List<int> 敌方弃牌堆卡牌编号 = new List<int>();




}
