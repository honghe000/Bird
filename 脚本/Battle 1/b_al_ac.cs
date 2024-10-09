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
    public GameObject ���ƷŴ�չʾ;
    private UnityEngine.Color color1;

    public Image ����ʱ;

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
    
    public void ����ʱ��ʾ(GameObject card)
    {
        �������� card_data = new ��������(card.GetComponent<������ʾ>().��������);
        MoveController moveController = card.GetComponent<MoveController>();
        TextMeshProUGUI time = ����ʱ.transform.Find("time").GetComponent<TextMeshProUGUI>();


        if (ValueHolder.����ʱ����.ContainsKey(card_data.uid))
        {
            Debug.Log(ValueHolder.����ʱ����[card_data.uid]);
            int t = (int)Mathf.Ceil(ValueHolder.����ʱ����[card_data.uid]);
            Debug.Log(t);
            if (t > 0)
            {
                time.text = t.ToString();
                ����ʱ.gameObject.SetActive(true);
            }

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
            ����ʱ��ʾ(transform.gameObject);

            // ���ɿ��Ƹ���Ʒ
            copycard = Instantiate(commoncard);

            // ���ƿ�������
            copycard.GetComponent<������ʾ>().�������� = transform.gameObject.GetComponent<������ʾ>().��������;
            copycard.GetComponent<������ʾ>().enabled = true;

        //Debug.Log(transform.gameObject.GetComponent<������ʾ>().��������.uid);
            Load_img(copycard, transform.gameObject.GetComponent<������ʾ>().��������.id);


            // ���ø���Ʒ�ĸ�����
            copycard.transform.SetParent(���ƷŴ�չʾ.transform, false);

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
        // ɾ��ʵ���������
        if (copycard != null)
        {
            Destroy(copycard);
        }
        ����ʱ.gameObject.SetActive(false);
    }
}
