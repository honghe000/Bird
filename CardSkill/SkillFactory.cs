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
            case "丁达尔效应":
                return new 丁达尔效应(card, monoBehaviour);
            case "钟馗":
                return new 钟馗(card, monoBehaviour);
            case "武僧":
                return new 武僧(card, monoBehaviour);
            case "虾兵蟹将":
                return new 虾兵蟹将(card, monoBehaviour);
            case "猩猩守卫":
                return new 猩猩守卫(card, monoBehaviour);
            case "火灵法师":
                return new 火灵法师(card, monoBehaviour);
            case "火焰小鬼":
                return new 火焰小鬼(card, monoBehaviour);
            case "尼斯湖水怪":
                return new 尼斯湖水怪(card, monoBehaviour);
            case "充能":
                return new 充能(card, monoBehaviour);
            case "荒骷髅":
                return new 荒骷髅(card, monoBehaviour);
            case "枉死城":
                return new 枉死城(card, monoBehaviour);
            case "青坊主":
                return new 青坊主(card, monoBehaviour);
            case "神之审判":
                return new 神之审判(card, monoBehaviour);
            case "猎人":
                return new 猎人(card, monoBehaviour);
            case "拳师":
                return new 拳师(card, monoBehaviour);
            case "吕布":
                return new 吕布(card, monoBehaviour);
            case "两面佛":
                return new 两面佛(card, monoBehaviour);
            case "大红莲":
                return new 大红莲(card, monoBehaviour);
            case "腥红之月":
                return new 腥红之月(card, monoBehaviour);
            case "摆渡人":
                return new 摆渡人(card, monoBehaviour);
            case "祈晴":
                return new 祈晴(card, monoBehaviour);
            case "山贼":
                return new 山贼(card, monoBehaviour);
            case "小雷音寺":
                return new 小雷音寺(card, monoBehaviour);
            default:
                return null;
        }
    }
}
