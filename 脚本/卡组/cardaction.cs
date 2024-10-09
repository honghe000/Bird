using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;



public class ChangeSize : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IDragHandler,IEndDragHandler,IBeginDragHandler
{
    public float Size;
    public float MinSize;
    public Vector2 orinalpos;
    public bool orinalbool = true;
    public GameObject layoutgroup;
    public GameObject 卡牌;
    public GameObject content;
    public GameObject Canvas;
    public CanvasGroup c;


    public int indexCard;



    private void Start()
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("ok");
        transform.position = Input.mousePosition;
    }

    //public GameObject layout;
    //public GameObject cardClone;

    public void OnPointerEnter(PointerEventData eventData)
    {

        //获取卡牌在仓库中的顺序
        indexCard = transform.GetSiblingIndex();
        //Debug.Log(indexCard);

        //Debug.Log("3");
        if (orinalbool)
        {
            orinalpos = transform.position;
            orinalbool = false;
        }
        MinSize = transform.localScale.x;
        //Debug.Log(MinSize);
        transform.localScale = new Vector3(Size*MinSize, Size*MinSize, 1.0f);
        mainfunction.DestroyAllChildren(layoutgroup);
        GameObject card = Instantiate(卡牌, layoutgroup.transform);
        card.GetComponent<MonoBehaviour>().enabled = true;
        card.transform.localScale = new Vector3(2.8f, 2.8f, 2.8f);
        card.GetComponent<数据显示>().卡牌数据 = GetComponent<cardShow>().卡牌数据;
        GetComponent<mainfunction>().Loadimages(card, 3,card.GetComponent<数据显示>().卡牌数据.id);



    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mainfunction.DestroyAllChildren(layoutgroup);
        transform.localScale = new Vector3(MinSize,MinSize,1.0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ValueHolder.isplace)
        {
            //判断已经放进卡组



            //重新设置变量
            ValueHolder.isplace = false;
            eventData.pointerDrag.GetComponent<cardShow>().numt.text = (eventData.pointerDrag.GetComponent<cardShow>().num - 1).ToString();
            eventData.pointerDrag.GetComponent<cardShow>().num -= 1;


            if (int.Parse(eventData.pointerDrag.GetComponent<cardShow>().numt.text) != 0)
            {
                transform.SetParent(content.transform);
                transform.SetSiblingIndex(indexCard);

                //Debug.Log(transform.GetSiblingIndex());
                c.blocksRaycasts = true;
            }
            else
            {
                Destroy(eventData.pointerDrag);
            }
        }
        else
        {
            //transform.position = orinalpos;
            //Debug.Log(orinalpos);
            transform.SetParent(content.transform);
            transform.SetSiblingIndex(indexCard);
            c.blocksRaycasts = true;
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {



        transform.SetParent(Canvas.transform);
        c.blocksRaycasts = false;


    }
}
