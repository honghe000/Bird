using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class 数据显示 : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI 名字;
    public TextMeshProUGUI 技能;
    public TextMeshProUGUI 等级;
    public TextMeshProUGUI 类别;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI ATK;

    public Image 背景人;
    public Image 背景法;
    public Image 背景筑;

    public Image HP背景;
    public Image ATK背景;

    public 卡牌数据 卡牌数据;
    void Start()
    {
        展示数据();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void 展示数据()
    {
        名字.text = 卡牌数据.名字;
        技能.text = 卡牌数据.技能;
        等级.text = 卡牌数据.等级;
        类别.text = 卡牌数据.类别;
        if (卡牌数据.nowHp == -1)
        {
            HP.gameObject.SetActive(false);
            ATK.gameObject.SetActive(false);
            HP背景.gameObject.SetActive(false);
            ATK背景.gameObject.SetActive(false);
        }
        else
        {
            HP.text = 卡牌数据.nowHp.ToString();
            ATK.text = 卡牌数据.nowAttack.ToString();
        }

        if (类别.text == "角色")
        {
            背景法.gameObject.SetActive(false);
            背景筑.gameObject.SetActive(false);
        }
        else if (类别.text == "法术")
        {
            背景人.gameObject.SetActive(false);
            背景筑.gameObject.SetActive(false);
        }
        else
        {
            背景法.gameObject.SetActive(false);
            背景人.gameObject.SetActive(false);
        }

    }

    public void 更新数据()
    {
        名字.text = 卡牌数据.名字;
        技能.text = 卡牌数据.技能;
        等级.text = 卡牌数据.等级;
        类别.text = 卡牌数据.类别;
        if (卡牌数据.nowHp == -1)
        {
            HP.gameObject.SetActive(false);
            ATK.gameObject.SetActive(false);
            HP背景.gameObject.SetActive(false);
            ATK背景.gameObject.SetActive(false);
        }
        else
        {
            HP.text = 卡牌数据.nowHp.ToString();
            ATK.text = 卡牌数据.nowAttack.ToString();
        }

        if (类别.text == "角色")
        {
            背景法.gameObject.SetActive(false);
            背景筑.gameObject.SetActive(false);
        }
        else if (类别.text == "法术")
        {
            背景人.gameObject.SetActive(false);
            背景筑.gameObject.SetActive(false);
        }
        else
        {
            背景法.gameObject.SetActive(false);
            背景人.gameObject.SetActive(false);
        }
    }


}
