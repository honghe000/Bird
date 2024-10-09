using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class 卡牌数据
{
    public int id;
    public string 名字;
    public string 技能;
    public string 等级;
    public string 类别;
    public string uid;

    public int maxHp;
    public int maxAttack;

    public int nowHp;
    public int nowAttack;
    public 卡牌数据(string 名字, string 技能, string 等级, string 类别, int id,string uid,int maxHp,int maxAttack,int nowHp,int nowAttack)
    {
        this.名字 = 名字;
        this.技能 = 技能;
        this.等级 = 等级;
        this.类别 = 类别;
        this.id = id;
        this.uid = uid;

        this.maxHp = maxHp;
        this.maxAttack = maxAttack;
        this.nowHp = nowHp;
        this.nowAttack = nowAttack;

        
    }

    // 拷贝构造函数（用于深拷贝）
    public 卡牌数据(卡牌数据 other)
    {
        this.名字 = other.名字;
        this.技能 = other.技能;
        this.等级 = other.等级;
        this.类别 = other.类别;
        this.id = other.id;
        this.uid = other.uid;

        this.maxHp = other.maxHp;
        this.maxAttack = other.maxAttack;
        this.nowHp = other.nowHp;
        this.nowAttack = other.nowAttack;
    }
}