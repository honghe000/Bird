using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class b_弃牌 : MonoBehaviour, IPointerClickHandler
{
    public int is_choose = 0;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(ValueHolder.弃牌数量.text == "0")
        {
            if (is_choose == 0)
            {
                return;
            }

        }
        GameObject card = eventData.pointerClick.gameObject;
        string uid = card.GetComponent<数据显示>().卡牌数据.uid;

        GameObject 选中显示 = card.transform.Find("choose").gameObject;

        if (is_choose == 1)
        {
            Debug.Log("弃牌取消");
            选中显示.SetActive(false);
            ValueHolder.弃牌数量.text = (int.Parse(ValueHolder.弃牌数量.text) + 1).ToString();
            ValueHolder.弃牌uid.Remove(uid);
            is_choose = 0;
        }
        else
        {
            Debug.Log("弃牌确认");
            选中显示.SetActive(true);
            ValueHolder.弃牌数量.text = (int.Parse(ValueHolder.弃牌数量.text) - 1).ToString();
            ValueHolder.弃牌uid.Add(uid);
            is_choose = 1;
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
