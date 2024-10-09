using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    public GameObject hintPrefab; // 提示的预制体
    public Transform hintContainer; // 用于放置提示的容器（Vertical Layout Group）
    public int maxHintCount = 5; // 最大提示数量
    public float hintDuration = 0.5f; // 提示持续时间
    public float fadeDuration = 2f; // 淡出时间

    private Queue<GameObject> activeHints = new Queue<GameObject>();

    // 向提示系统添加一条提示
    public void AddHint(string message)
    {
        // 超出最大提示数量，删除最早的提示
        if (activeHints.Count >= maxHintCount)
        {
            RemoveOldestHint();
        }

        // 创建新提示
        GameObject newHint = Instantiate(hintPrefab, hintContainer);
        TextMeshProUGUI hintText = newHint.GetComponent<TextMeshProUGUI>();
        hintText.text = message;

        // 将提示加入队列并显示
        activeHints.Enqueue(newHint);
        StartCoroutine(FadeOutHint(newHint, hintDuration));
    }

    // 协程：提示在延迟后逐渐淡出
    private IEnumerator FadeOutHint(GameObject hint, float delay)
    {
        yield return new WaitForSeconds(delay);

        CanvasGroup canvasGroup = hint.GetComponent<CanvasGroup>();
        float startAlpha = canvasGroup.alpha;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, elapsed / fadeDuration);
            yield return null;
        }

        activeHints.Dequeue();
        Destroy(hint);
    }

    // 立即删除最早的提示
    private void RemoveOldestHint()
    {
        if (activeHints.Count > 0)
        {
            GameObject oldestHint = activeHints.Dequeue();
            Destroy(oldestHint);
        }
    }
}
