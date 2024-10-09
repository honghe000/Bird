using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class b_al_ac : MonoBehaviour,IPointerExitHandler,IPointerEnterHandler
{

    private float scale_x;
    private GameObject copycard;
    public GameObject canvas;
    public GameObject commoncard;
    public GameObject 卡牌放大展示;
    private UnityEngine.Color color1;

    public Image 倒计时;

    private void Start()
    {
        scale_x = canvas.transform.localScale.x;
    }

    public  GameObject Load_img(GameObject commoncard, int id)
    {
        Texture2D texture = Resources.Load<Texture2D>("card/" + id.ToString());
        if (texture != null)
        {
            foreach (Image image in commoncard.GetComponentsInChildren<Image>())
            {
                if (image.ToString() == "pics (UnityEngine.UI.Image)")
                {
                    image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                }
            }

        }
        return commoncard;
    }
    
    public void 倒计时显示(GameObject card)
    {
        卡牌数据 card_data = new 卡牌数据(card.GetComponent<数据显示>().卡牌数据);
        MoveController moveController = card.GetComponent<MoveController>();
        TextMeshProUGUI time = 倒计时.transform.Find("time").GetComponent<TextMeshProUGUI>();


        if (ValueHolder.倒计时储存.ContainsKey(card_data.uid))
        {
            Debug.Log(ValueHolder.倒计时储存[card_data.uid]);
            int t = (int)Mathf.Ceil(ValueHolder.倒计时储存[card_data.uid]);
            Debug.Log(t);
            if (t > 0)
            {
                time.text = t.ToString();
                倒计时.gameObject.SetActive(true);
            }

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
            倒计时显示(transform.gameObject);

            // 生成卡牌复制品
            copycard = Instantiate(commoncard);

            // 复制卡牌数据
            copycard.GetComponent<数据显示>().卡牌数据 = transform.gameObject.GetComponent<数据显示>().卡牌数据;
            copycard.GetComponent<数据显示>().enabled = true;

        //Debug.Log(transform.gameObject.GetComponent<数据显示>().卡牌数据.uid);
            Load_img(copycard, transform.gameObject.GetComponent<数据显示>().卡牌数据.id);


            // 设置复制品的父物体
            copycard.transform.SetParent(卡牌放大展示.transform, false);

            foreach (Transform child in transform)
            {
                TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
                if (text != null)
                {
                    color1 = text.color;
                    break;
                }
            }

            foreach (Transform child in copycard.transform)
            {
                TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
                if (text != null)
                {
                    text.color = color1;
                }
            }


        ValueHolder.copyed_object = copycard;


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 删除实例化的物件
        if (copycard != null)
        {
            Destroy(copycard);
        }
        倒计时.gameObject.SetActive(false);
    }
}
