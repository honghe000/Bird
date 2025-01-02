using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class b_summon : MonoBehaviour
{
    public GameObject commoncard;
    public LayoutGroup 手牌;
    public GameObject canvas;
    public TextMeshProUGUI subtitleText;
    public Button 下个回合;
    public Button leave;
    public TextMeshProUGUI 体力;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ChangeSubtitle(ValueHolder.initiative));
        //foreach (int id in ValueHolder.random_card)
        //{
        //    GameObject cardone = Instantiate(commoncard);
        //    cardone = summon_one(cardone, id);
        //    cardone.transform.SetParent(手牌.transform,false);
        //}
        StartCoroutine(Begingame());




    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject summon_one(GameObject commoncard,int id,int index=3)
    {
        Texture2D texture = Resources.Load<Texture2D>("card/" + id.ToString());
        卡牌数据 原卡牌数据 = ValueHolder.gloabCaedData[id];
        // 深拷贝
        卡牌数据 新卡牌数据 = new 卡牌数据(原卡牌数据);
        commoncard.GetComponent<数据显示>().卡牌数据 = 新卡牌数据;
        commoncard.GetComponent<数据显示>().卡牌数据.uid = System.Guid.NewGuid().ToString();
        commoncard.GetComponent<数据显示>().enabled = true;
        if (texture != null)
        {
           foreach (Image image in commoncard.GetComponentsInChildren<Image>()) { 
                if(image.ToString() == "pics (UnityEngine.UI.Image)")
                {
                    image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                }
            }

        }

        //System.Type scriptType = System.Type.GetType(ValueHolder.gloabCaedData[id].名字);
        //if (scriptType != null)
        //{
        //    commoncard.AddComponent(scriptType);
        //}
        return commoncard;
    }

    IEnumerator Begingame()
    {
        下个回合.interactable = false;
        下个回合.image.color = Color.gray;

        leave.interactable = false;
        leave.image.color = Color.gray;
        // 等待 ChangeSubtitle 完成
        yield return StartCoroutine(ChangeSubtitle(ValueHolder.initiative));
        //体力.text = ValueHolder.point.ToString();
        StartCoroutine(FadeOut());
        if (ValueHolder.initiative != 0)
        {
            下个回合.interactable = true;
            下个回合.image.color = Color.white;
        }
        leave.interactable = true;
        leave.image.color = Color.white;

        // 在 ChangeSubtitle 执行完之后再进行循环
        foreach (int id in mainfunction.SumomonHandCardID(5))
        {
            GameObject cardone = Instantiate(commoncard);
            cardone = summon_one(cardone, id);
            cardone.transform.SetParent(手牌.transform, false);
        }
    }
    //协程函数
    IEnumerator ChangeSubtitle(int initiative)
    {
        string l_text = "先手";
        int times = 3;
        float waitTime = 0.1f;
        string end_text = "先手";
        while (times > 0)
        {
            subtitleText.text = l_text;
            l_text = (l_text == "先手") ? "后手" : "先手";
            yield return new WaitForSeconds(waitTime); // 随机等待一段时间
            times--;
        }

        while (waitTime < 2f)
        {
            subtitleText.text = l_text;
            l_text = (l_text == "先手") ? "后手" : "先手";
            yield return new WaitForSeconds(waitTime); // 随机等待一段时间
            waitTime *= 1.5f;
        }
        if (initiative == 1)
        {
            end_text = "先手";
        }
        else
        {
            end_text = "后手";
        }
        subtitleText.text = end_text;
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        float fadeDuration = 3f;
        Color originalColor = subtitleText.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            subtitleText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        subtitleText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // 最终变为透明
    }
}

