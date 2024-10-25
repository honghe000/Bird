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

    public TextMeshProUGUI 灵力;

    public Image 背景人;
    public Image 背景法;
    public Image 背景筑;

    public Image HP背景;
    public Image ATK背景;

    public Image 黄;
    public Image 绿;
    public Image 蓝;
    public Image 紫;

    public 卡牌数据 卡牌数据;
    public int 灵力消耗等级;
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



        黄.gameObject.SetActive(false);
        绿.gameObject.SetActive(false);
        蓝.gameObject.SetActive(false);
        紫.gameObject.SetActive(false);

        if(卡牌数据.黄 >= 0)
        {
            黄.gameObject.SetActive(true);
            灵力.text = 卡牌数据.黄.ToString();
            灵力消耗等级 = 1;
        }else if (卡牌数据.绿 >= 0)
        {
            绿.gameObject.SetActive(true);
            灵力.text = 卡牌数据.绿.ToString();
            灵力消耗等级 = 2;
        }else if (卡牌数据.蓝 >= 0)
        {
            蓝.gameObject.SetActive(true);
            灵力.text = 卡牌数据.蓝.ToString();
            灵力消耗等级 = 3;
        }else if (卡牌数据.紫 >= 0)
        {
            紫.gameObject.SetActive(true);
            灵力.text = 卡牌数据.紫.ToString();
            灵力消耗等级 = 4;
        }
        else
        {
            灵力.text = "";
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

        黄.gameObject.SetActive(false);
        绿.gameObject.SetActive(false);
        蓝.gameObject.SetActive(false);
        紫.gameObject.SetActive(false);

        if (卡牌数据.黄 >= 0)
        {
            黄.gameObject.SetActive(true);
            灵力.text = 卡牌数据.黄.ToString();
            灵力消耗等级 = 1;
        }
        else if (卡牌数据.绿 >= 0)
        {
            绿.gameObject.SetActive(true);
            灵力.text = 卡牌数据.绿.ToString();
            灵力消耗等级 = 2;
        }
        else if (卡牌数据.蓝 >= 0)
        {
            蓝.gameObject.SetActive(true);
            灵力.text = 卡牌数据.蓝.ToString();
            灵力消耗等级 = 3;
        }
        else if (卡牌数据.紫 >= 0)
        {
            紫.gameObject.SetActive(true);
            灵力.text = 卡牌数据.紫.ToString();
            灵力消耗等级 = 4;
        }
        else
        {
            灵力.text = "";
        }
    }


}
