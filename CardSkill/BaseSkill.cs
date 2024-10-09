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

    public int �����غϿ�ʼʱ���� = 0;
    public int �����غϽ���ʱ���� = 0;


    public �������� card_data { get; protected set; } // ���ܶ�Ӧ�Ŀ�������
    public GameObject ����Ŀ�꿨�� { get; set; } // �������õ�Ŀ�꿨��

    public string Ч�� = ""; // ����Ч��

    public int ���� = 0; // ����

    public abstract void Action_1();
    public abstract void Action_2();
    public abstract void Action_3();
    public abstract void Action_4();



    // �������Թ�������Ժͷ���
    public IEnumerator RotateAndScaleCoroutine(GameObject card)
    {
        Vector3 originalScale = card.transform.localScale;
        float elapsedTime = 0f;

        float rotationSpeed = 360f;
        float fadeDuration = 2f; // ����ʱ��

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration; // ʱ����� [0, 1]

            // ��������
            float scale = Mathf.Lerp(1f, 0f, t);
            card.transform.localScale = new Vector3(scale, scale, 1f);

            // ������ת
            float rotation = rotationSpeed * Time.deltaTime;
            card.transform.Rotate(Vector3.forward, rotation);

            yield return null;
        }

        // ȷ������״̬
        card.transform.localScale = Vector3.zero;
        Destroy(card); // ��ѡ���������
    }

    public GameObject summon_one(GameObject commoncard, int id)
    {
        Texture2D texture = Resources.Load<Texture2D>("card/" + id.ToString());
        �������� ԭ�������� = ValueHolder.gloabCaedData[id];
        // ���
        �������� �¿������� = new ��������(ԭ��������);
        commoncard.GetComponent<������ʾ>().�������� = �¿�������;
        commoncard.GetComponent<������ʾ>().��������.uid = "0";
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

    public void summonHandcard(int num)
    {
        foreach (int id in mainfunction.SumomonHandCardID(num))
        {
            Debug.Log(id);
            GameObject cardone = Instantiate(ValueHolder.���ƶ�ս��);
            cardone = summon_one(cardone, id);
            cardone.transform.SetParent(ValueHolder.������.transform, false);
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