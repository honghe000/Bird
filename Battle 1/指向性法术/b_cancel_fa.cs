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
        mainfunction.���������������("b_moveca", 0);
        mainfunction.���������������("b_cardaction");
        mainfunction.HideCardchoose();
        ValueHolder.����ѡ��ȡ��.gameObject.SetActive(false);
        mainfunction.���������������("b_choose_fa", ValueHolder.�������õ�������);

        if (mainfunction.uid�ҿ�(ValueHolder.�ͷŷ���uid) != null)
        {
            if(ValueHolder.is_myturn == 0)
            {
                if (SkillExecutor.skillQueue.Count > 0 || SkillExecutor.currentRunningSkillUid != null)
                {
                    return;
                }
                else
                {
                    mainfunction.Send�Է�����();
                    return;
                }
            }
            mainfunction.�����ͷŽ���();
            return;
        }

        BaseSkill skill = ValueHolder.SkillAction[ValueHolder.�ͷŷ���uid];
        Debug.Log("ȡ������" + skill.card_data.����);
        GameObject cardone = Instantiate(ValueHolder.���ƶ�ս��);
        cardone = summon_one(cardone, skill.card_data.id);
        cardone.GetComponent<������ʾ>().��������.uid = skill.card_data.uid;
        cardone.transform.SetParent(ValueHolder.������.transform, false);
        cardone.transform.SetSiblingIndex(ValueHolder.��ק���);

        ValueHolder.SkillAction.Remove(ValueHolder.�ͷŷ���uid);

        //��������
        mainfunction.��������(skill.card_data.�������ĵȼ�, skill.card_data.������������);
        mainfunction.�����ͷŽ���();

    }

    public GameObject summon_one(GameObject commoncard, int id)
    {
        Texture2D texture = Resources.Load<Texture2D>("card/" + id.ToString());
        commoncard.GetComponent<������ʾ>().�������� = ValueHolder.gloabCaedData[id];
        commoncard.GetComponent<������ʾ>().enabled = true;
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
