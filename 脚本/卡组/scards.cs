using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class scards : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    public float Size;
    public GameObject commonCard;
    public GameObject layone;
    public GameObject allcontent;
    public GameObject storecard;

    public TextMeshProUGUI rennum;
    public TextMeshProUGUI fanum;
    public TextMeshProUGUI zhunum;


    public TextMeshProUGUI SSnum;
    public TextMeshProUGUI Snum;
    public TextMeshProUGUI Anum;
    public TextMeshProUGUI Bnum;
    public TextMeshProUGUI CDnum;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            Transform parentTransform = allcontent.transform;
            bool isexist = false;
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                GameObject childobject = parentTransform.GetChild(i).gameObject;
                if (childobject.GetComponent<cardShow>().名字.text == eventData.pointerClick.GetComponent<smallCard>().名字.text)
                {
                    childobject.GetComponent<cardShow>().numt.text = (int.Parse(childobject.GetComponent<cardShow>().numt.text) + 1).ToString();
                    childobject.GetComponent<cardShow>().num = int.Parse(childobject.GetComponent<cardShow>().numt.text);
                    ValueHolder.cardData[childobject.GetComponent<cardShow>().卡牌数据.id] = childobject.GetComponent<cardShow>().num;
                    ValueHolder.cardDataexsist[childobject.GetComponent<cardShow>().卡牌数据.id] -= 1;
                    Destroy(eventData.pointerClick);
                    isexist = true;


                    if (childobject.GetComponent<cardShow>().卡牌数据.类别 == "角色")
                    {
                        rennum.text = (int.Parse(rennum.text) - 1).ToString();
                    }
                    else if (childobject.GetComponent<cardShow>().卡牌数据.类别 == "法术")
                    {
                        fanum.text = (int.Parse(fanum.text) - 1).ToString();
                    }
                    else
                    {
                        zhunum.text = (int.Parse(zhunum.text) - 1).ToString();
                    }

                    break;
                }

            }
            if (!isexist)
            {
                GameObject card = Instantiate(storecard, allcontent.transform);
                card.GetComponent<cardShow>().卡牌数据 = eventData.pointerClick.GetComponent<smallCard>().卡牌数据;
                card.GetComponent<cardShow>().num = 1;
                card.GetComponent<MonoBehaviour>().enabled = true;
                GetComponent<mainfunction>().Loadimages(card, 4, eventData.pointerClick.GetComponent<smallCard>().卡牌数据.id);
                Destroy(eventData.pointerClick);
                ValueHolder.cardData[eventData.pointerClick.GetComponent<smallCard>().卡牌数据.id] =1;
                ValueHolder.cardDataexsist[eventData.pointerClick.GetComponent<smallCard>().卡牌数据.id] -= 1;

                if (card.GetComponent<cardShow>().卡牌数据.类别 == "角色")
                {
                    rennum.text = (int.Parse(rennum.text) - 1).ToString();
                }
                else if (card.GetComponent<cardShow>().卡牌数据.类别 == "法术")
                {
                    fanum.text = (int.Parse(fanum.text) - 1).ToString();
                }
                else
                {
                    zhunum.text = (int.Parse(zhunum.text) - 1).ToString();
                }


            }


            
            //改变该等级牌组内的数量显示
            string level = eventData.pointerClick.GetComponent<smallCard>().卡牌数据.等级.ToString();
            if (level == "SS" || level == "SSS")
            {
                SSnum.text = (int.Parse(SSnum.text) - 1).ToString();
            }
            else if (level == "S")
            {
                Snum.text = (int.Parse(Snum.text) - 1).ToString();
            }
            else if (level == "A")
            {
                Anum.text = (int.Parse(Anum.text) - 1).ToString();
            }
            else if (level == "B")
            {
                Bnum.text = (int.Parse(Bnum.text) - 1).ToString();
            }
            else
            {
                CDnum.text = (int.Parse(CDnum.text) - 1).ToString();
            }


        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(Size, Size, 1.0f);
        mainfunction.DestroyAllChildren(layone);
        GameObject summonCard = Instantiate(commonCard, layone.transform);
        summonCard.GetComponent<MonoBehaviour>().enabled = true;
        summonCard.GetComponent<数据显示>().卡牌数据 = GetComponent<smallCard>().卡牌数据;
        summonCard.transform.localScale = new Vector3(2.8f, 2.8f, 2.8f);
        GetComponent<mainfunction>().Loadimages(summonCard, 3, summonCard.GetComponent<数据显示>().卡牌数据.id);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mainfunction.DestroyAllChildren(layone);
        transform.localScale = Vector3.one;
    }
}
