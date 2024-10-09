using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class cardMove1 : MonoBehaviour,IPointerClickHandler
{
    public GameObject card;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ValueHolder.MovecardData != null)
        {
            card.GetComponent<数据显示Battle>().卡牌数据 = ValueHolder.MovecardData;
            Instantiate(card, transform);
        }
        //Debug.Log("你点击了!" + eventData.pointerClick.name);
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
