using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public abstract class BaseSkill : MonoBehaviour
{
    public int skill_end { get; protected set; }
    public GameObject card { get; protected set; }
    public float activateTurn_1 { get; protected set; } 
    public float activateTurn_2 { get; protected set; }
    public float activateTurn_3 { get; protected set; }
    public float activateTurn_4 { get; protected set; }

    public int activateTurn_1_finish { get; protected set; }
    public int activateTurn_2_finish { get; protected set; }
    public int activateTurn_3_finish { get; protected set; }
    public int activateTurn_4_finish { get; protected set; }

    public string uid { get; protected set; }

    public int 己方回合开始时触发 = 0;
    public int 己方回合结束时触发 = 0;


    public 卡牌数据 card_data { get; protected set; } // 技能对应的卡牌数据
    public GameObject 作用目标卡牌 { get; set; } // 技能作用的目标卡牌

    public string 效果 = ""; // 技能效果

    public int 亡语 = 0; // 亡语

    public abstract void Action_1();
    public abstract void Action_2();
    public abstract void Action_3();
    public abstract void Action_4();



    // 其他可以共享的属性和方法
    public IEnumerator RotateAndScaleCoroutine(GameObject card)
    {
        Vector3 originalScale = card.transform.localScale;
        float elapsedTime = 0f;

        float rotationSpeed = 360f;
        float fadeDuration = 2f; // 持续时间

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration; // 时间比例 [0, 1]

            // 计算缩放
            float scale = Mathf.Lerp(1f, 0f, t);
            card.transform.localScale = new Vector3(scale, scale, 1f);

            // 计算旋转
            float rotation = rotationSpeed * Time.deltaTime;
            card.transform.Rotate(Vector3.forward, rotation);

            yield return null;
        }

        // 确保最终状态
        card.transform.localScale = Vector3.zero;
        Destroy(card); // 可选择禁用物体
    }

    public GameObject summon_one(GameObject commoncard, int id)
    {
        Texture2D texture = Resources.Load<Texture2D>("card/" + id.ToString());
        卡牌数据 原卡牌数据 = ValueHolder.gloabCaedData[id];
        // 深拷贝
        卡牌数据 新卡牌数据 = new 卡牌数据(原卡牌数据);
        commoncard.GetComponent<数据显示>().卡牌数据 = 新卡牌数据;
        commoncard.GetComponent<数据显示>().卡牌数据.uid = "0";
        commoncard.GetComponent<数据显示>().enabled = true;
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

    public void summonHandcard(int num)
    {
        foreach (int id in mainfunction.SumomonHandCardID(num))
        {
            Debug.Log(id);
            GameObject cardone = Instantiate(ValueHolder.手牌对战牌);
            cardone = summon_one(cardone, id);
            cardone.transform.SetParent(ValueHolder.手牌区.transform, false);
        }
    }

    public void change_card_color(GameObject card, string color)
    {
        foreach (Transform child in card.transform)
        {
            TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                if (color == "red")
                {
                    text.color = Color.red;
                }
                if (color == "blue")
                {
                    text.color = Color.blue;
                }
            }
        }
    }

}