using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class b_cancel_fa : MonoBehaviour
{
    public Button cancel;
    void Start()
    {
        cancel.onClick.AddListener(cancel_f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void cancel_f()
    {
        mainfunction.启用棋盘物件代码("b_moveca", 0);
        mainfunction.启用手牌物件代码("b_cardaction");
        mainfunction.HideCardchoose();
        ValueHolder.法术选择取消.gameObject.SetActive(false);
        mainfunction.禁用棋盘物件代码("b_choose_fa", ValueHolder.法术作用敌我类型);

        if (mainfunction.uid找卡(ValueHolder.释放法术uid) != null)
        {
            if(ValueHolder.is_myturn == 0)
            {
                if (SkillExecutor.skillQueue.Count > 0 || SkillExecutor.currentRunningSkillUid != null)
                {
                    return;
                }
                else
                {
                    mainfunction.Send对方继续();
                    return;
                }
            }
            mainfunction.技能释放结束();
            return;
        }

        BaseSkill skill = ValueHolder.SkillAction[ValueHolder.释放法术uid];
        Debug.Log("取消法术" + skill.card_data.名字);
        GameObject cardone = Instantiate(ValueHolder.手牌对战牌);
        cardone = summon_one(cardone, skill.card_data.id);
        cardone.GetComponent<数据显示>().卡牌数据.uid = skill.card_data.uid;
        cardone.transform.SetParent(ValueHolder.手牌区.transform, false);
        cardone.transform.SetSiblingIndex(ValueHolder.拖拽序号);

        ValueHolder.SkillAction.Remove(ValueHolder.释放法术uid);

        //返还灵力
        mainfunction.灵力增加(skill.card_data.灵力消耗等级, skill.card_data.灵力消耗数量);
        mainfunction.技能释放结束();

    }

    public GameObject summon_one(GameObject commoncard, int id)
    {
        Texture2D texture = Resources.Load<Texture2D>("card/" + id.ToString());
        commoncard.GetComponent<数据显示>().卡牌数据 = ValueHolder.gloabCaedData[id];
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
}
