using UnityEngine;

public static class SkillFactory
{
    public static BaseSkill CreateSkill(GameObject card, MonoBehaviour monoBehaviour)
    {
        switch (card.GetComponent<数据显示>().卡牌数据.名字)
        {
            case "张三封大师":
                return new 张三封大师(card, monoBehaviour);
            case "相安无事":
                return new 相安无事(card, monoBehaviour);
            case "鬼屋":
                return new 鬼屋(card, monoBehaviour);
            case "鬼将":
                return new 鬼将(card, monoBehaviour);
            case "龙首之玉":
                return new 龙首之玉(card, monoBehaviour);
            case "佛光":
                return new 佛光(card, monoBehaviour);
            case "判官笔":
                return new 判官笔(card, monoBehaviour);
            case "巨灵神":
                return new 巨灵神(card, monoBehaviour);
            case "雅典娜":
                return new 雅典娜(card, monoBehaviour);
            case "蝎尾毒":
                return new 蝎尾毒(card, monoBehaviour);
            case "河神之怒":
                return new 河神之怒(card, monoBehaviour);
            case "舍生":
                return new 舍生(card, monoBehaviour);
            case "神谕者":
                return new 神谕者(card, monoBehaviour);
            case "外交官":
                return new 外交官(card, monoBehaviour);
            case "肥嘟嘟左卫门":
                return new 肥嘟嘟左卫门(card, monoBehaviour);
            case "雷电":
                return new 雷电(card, monoBehaviour);
            case "阿尔卡祭坛":
                return new 阿尔卡祭坛(card, monoBehaviour);
            case "太乙真人":
                return new 太乙真人(card, monoBehaviour);
            case "关羽":
                return new 关羽(card, monoBehaviour);
            case "困兽之斗":
                return new 困兽之斗(card, monoBehaviour);
            case "牛头马面":
                return new 牛头马面(card, monoBehaviour);
            case "迅雷的崩玉":
                return new 迅雷的崩玉(card, monoBehaviour);
            case "鬼琵琶":
                return new 鬼琵琶(card, monoBehaviour);
            case "毒雾":
                return new 毒雾(card, monoBehaviour);
            case "战国犀牛":
                return new 战国犀牛(card, monoBehaviour);
            case "牛魔":
                return new 牛魔(card, monoBehaviour);
            case "贩卖鸦片":
                return new 贩卖鸦片(card, monoBehaviour);
            case "漫步者":
                return new 漫步者(card, monoBehaviour);
            case "杨枝甘露":
                return new 杨枝甘露(card, monoBehaviour);
            case "远古石像鬼":
                return new 远古石像鬼(card, monoBehaviour);
            default:
                return null;
        }
    }
}
