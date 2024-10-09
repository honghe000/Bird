
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class smallCard : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI 名字;
    public TextMeshProUGUI 等级;

    public Image 背景;
    public Image 背景人;
    public Image 背景法;
    public Image 背景筑;

    public 卡牌数据 卡牌数据;
    void Start()
    {
        展示数据small();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void 展示数据small()
    {
        名字.text = 卡牌数据.名字;
        等级.text = 卡牌数据.等级;
        string 类别 = 卡牌数据.类别;

        if (类别 == "角色")
        {
            背景法.gameObject.SetActive(false);
            背景筑.gameObject.SetActive(false);
        }
        else if (类别 == "法术")
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
