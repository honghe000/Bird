using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    public GameObject hintPrefab; // ��ʾ��Ԥ����
    public Transform hintContainer; // ���ڷ�����ʾ��������Vertical Layout Group��
    public int maxHintCount = 5; // �����ʾ����
    public float hintDuration = 0.5f; // ��ʾ����ʱ��
    public float fadeDuration = 2f; // ����ʱ��

    private Queue<GameObject> activeHints = new Queue<GameObject>();

    // ����ʾϵͳ���һ����ʾ
    public void AddHint(string message)
    {
        // ���������ʾ������ɾ���������ʾ
        if (activeHints.Count >= maxHintCount)
        {
            RemoveOldestHint();
        }

        // ��������ʾ
        GameObject newHint = Instantiate(hintPrefab, hintContainer);
        TextMeshProUGUI hintText = newHint.GetComponent<TextMeshProUGUI>();
        hintText.text = message;

        // ����ʾ������в���ʾ
        activeHints.Enqueue(newHint);
        StartCoroutine(FadeOutHint(newHint, hintDuration));
    }

    // Э�̣���ʾ���ӳٺ��𽥵���
    private IEnumerator FadeOutHint(GameObject hint, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (hint == null) yield break; // ��� hint �Ƿ�����

        CanvasGroup canvasGroup = hint.GetComponent<CanvasGroup>();
        if (canvasGroup == null) yield break; // ��� CanvasGroup �Ƿ����

        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            if (hint == null) yield break; // ȷ����ʾû�б�����
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, elapsed / fadeDuration);
            yield return null;
        }

        if (hint != null) // �ٴ�ȷ�� hint �Դ���
        {
            activeHints.Dequeue();
            Destroy(hint);
        }
    }


    // ����ɾ���������ʾ
    private void RemoveOldestHint()
    {
        if (activeHints.Count > 0)
        {
            GameObject oldestHint = activeHints.Dequeue();
            Destroy(oldestHint);
        }
    }
}
