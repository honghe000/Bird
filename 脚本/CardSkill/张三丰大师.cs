using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 张三封大师1 : MonoBehaviour
{
    private int delay = 3;
    private CardTurn put_turn;
    private int skill_turn;
    private GameObject card;
    public event Action DelaySkill;

    public struct CardTurn
    {
        public int start_turn;
        public GameObject card;
    }
    public CardTurn PUT_TURN
    {
        get { return put_turn; }
        set {
            skill_turn = value.start_turn + delay;
            put_turn = value;
            card = value.card;
            Debug.Log("技能将在" +  skill_turn.ToString() + "回合触发");
        }
    }

    public int NOW_TURN
    {
        set
        {
            if (value == skill_turn)
            {
                Debug.Log("张三封大师技能触发");
                DelaySkill?.Invoke();

            }
        }
    }

    private void OnEnable()
    {
        DelaySkill += TriggerSkill;
    }

    private void OnDisable()
    {
        DelaySkill -= TriggerSkill;
    }

    private void TriggerSkill()
    {
    }

    IEnumerator RotateAndScaleCoroutine(GameObject card)
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

}
