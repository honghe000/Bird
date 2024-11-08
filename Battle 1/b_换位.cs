using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class b_换位 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent; // 卡牌的原父物体
    private Transform otherParent; // 其他卡牌的父物体
    private Transform tempParent;
    private int is_chaged = 0;
    private int originalSiblingIndex; // 卡牌初始的SiblingIndex
    private Vector2 originalPosition; // 卡牌初始位置
    private RectTransform rectTransform; // 卡牌的 RectTransform
    private GameObject placeholder; // 占位符对象

    private float swapThreshold = 100f; // 交换位置的最小距离阈值

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        // originalParent 设置为孙物件的直接父物件
        originalParent = transform.parent;

        // 获取父物件
        Transform parentObject = originalParent.parent;

        // 找到 otherParent（父物件下的另一个子物件）
        foreach (Transform child in parentObject)
        {
            if (child != originalParent)
            {
                otherParent = child;
                break;
            }
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        swapThreshold = rectTransform.rect.width / 1.5f;
        // 记录卡牌的原父级、位置和索引
        originalParent = transform.parent;
        //if (originalParent == otherParent)
        //{
        //    tempParent = transform.parent;
        //    originalParent = transform.parent.parent;

        //}
        originalSiblingIndex = transform.GetSiblingIndex();
        originalPosition = rectTransform.anchoredPosition;

        // 创建占位符并插入到原位置
        placeholder = new GameObject("Placeholder");
        var layoutElement = placeholder.AddComponent<LayoutElement>();
        layoutElement.preferredWidth = rectTransform.rect.width;
        layoutElement.preferredHeight = rectTransform.rect.height;

        placeholder.transform.SetParent(originalParent);
        placeholder.transform.SetSiblingIndex(originalSiblingIndex);

        // 暂时移出GridLayoutGroup的控制
        transform.SetParent(originalParent.parent);
    }


    public void OnDrag(PointerEventData eventData)
    {
        // 更新卡牌跟随鼠标位置
        rectTransform.position = eventData.position;

        int newSiblingIndex = originalSiblingIndex;
        float closestDistance = float.MaxValue;
        bool foundNearbyCard = false;

        for (int i = 0; i < originalParent.childCount; i++)
        {
            if (i == newSiblingIndex)
            {
                continue;
            }

            RectTransform childRect = originalParent.GetChild(i) as RectTransform;
            float distance = Vector2.Distance(rectTransform.position, childRect.position);

            // 只有当距离小于阈值，并且比当前最小距离近时，更新占位符的位置
            if (distance < swapThreshold && distance < closestDistance)
            {
                is_chaged = 0;
                newSiblingIndex = i;
                closestDistance = distance;
                foundNearbyCard = true;
            }
        }

        if (otherParent != null)
        {
            for (int i = 0; i < otherParent.childCount; i++)
            {
                if (i == newSiblingIndex)
                {
                    continue;
                }
                RectTransform childRect = otherParent.GetChild(i) as RectTransform;
                float distance = Vector2.Distance(rectTransform.position, childRect.position);

                // 只有当距离小于阈值，并且比当前最小距离近时，更新占位符的位置
                if (distance < swapThreshold && distance < closestDistance)
                {
                    is_chaged = 1;
                    newSiblingIndex = i;
                    closestDistance = distance;
                    foundNearbyCard = true;
                }
            }
        }


        // 定义Y轴的检测阈值
        float yThreshold = 50f; // 可以根据需要调整

        // 如果没有找到足够接近的卡牌，则检测是否靠近边缘位置
        if (!foundNearbyCard)
        {
            if (originalParent.childCount > 0)
            {
                RectTransform firstChild = originalParent.GetChild(0) as RectTransform;
                RectTransform lastChild = originalParent.GetChild(originalParent.childCount - 1) as RectTransform;

                // 获取当前拖动卡牌与第一个子物体的Y轴距离
                float distanceToFirstY = Mathf.Abs(rectTransform.position.y - firstChild.position.y);
                float distanceToLastY = Mathf.Abs(rectTransform.position.y - lastChild.position.y);

                // 判断是否拖到开头位置 (x轴满足条件，并且y轴在阈值范围内)
                if (rectTransform.position.x < firstChild.position.x - firstChild.rect.width / 2 && distanceToFirstY < yThreshold)
                {
                    is_chaged = 0;
                    newSiblingIndex = 0;

                }
                // 判断是否拖到末尾位置 (x轴满足条件，并且y轴在阈值范围内)
                else if (rectTransform.position.x > lastChild.position.x + lastChild.rect.width / 2 && distanceToLastY < yThreshold)
                {
                    is_chaged = 0;
                    newSiblingIndex = originalParent.childCount - 1;
                }
            }
            //else
            //{
            //    is_chaged = 0;
            //    newSiblingIndex = 0;
            //}
        }

        // 如果没有找到足够接近的卡牌，则检测是否靠近边缘位置
        if (!foundNearbyCard && otherParent != null)
        {
            if (otherParent.childCount > 0)
            {
                RectTransform firstChild = otherParent.GetChild(0) as RectTransform;
                RectTransform lastChild = otherParent.GetChild(otherParent.childCount - 1) as RectTransform;

                // 获取当前拖动卡牌与第一个子物体的Y轴距离
                float distanceToFirstY = Mathf.Abs(rectTransform.position.y - firstChild.position.y);
                float distanceToLastY = Mathf.Abs(rectTransform.position.y - lastChild.position.y);

                // 判断是否拖到开头位置 (x轴满足条件，并且y轴在阈值范围内)
                if (rectTransform.position.x < firstChild.position.x - firstChild.rect.width / 2 && distanceToFirstY < yThreshold)
                {
                    is_chaged = 1;
                    newSiblingIndex = 0;

                }
                // 判断是否拖到末尾位置 (x轴满足条件，并且y轴在阈值范围内)
                else if (rectTransform.position.x > lastChild.position.x + lastChild.rect.width / 2 && distanceToLastY < yThreshold)
                {
                    is_chaged = 1;
                    newSiblingIndex = otherParent.childCount - 1;
                }
            }
            else
            {
                //is_chaged = 1;
                //newSiblingIndex = 0;
                RectTransform otherRectTransform = otherParent.GetComponent<RectTransform>();
                if (rectTransform.position.x < otherRectTransform.position.x + otherRectTransform.rect.width / 2 &&
                    rectTransform.position.x > otherRectTransform.position.x - otherRectTransform.rect.width / 2 &&
                    rectTransform.position.y < otherRectTransform.position.y + otherRectTransform.rect.height / 2 &&
                    rectTransform.position.y > otherRectTransform.position.y - otherRectTransform.rect.height / 2)
                {
                    is_chaged = 1;
                    newSiblingIndex = 0;
                }
            }
        }



        if (newSiblingIndex != originalSiblingIndex)
        {
            if (is_chaged == 1)
            {
                // 将占位符移动到新的位置
                placeholder.transform.SetParent(otherParent);
            }
            else
            {
                // 将占位符移动到新的位置
                placeholder.transform.SetParent(originalParent);
            }
            placeholder.transform.SetSiblingIndex(newSiblingIndex);
            originalSiblingIndex = newSiblingIndex;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (is_chaged == 1)
        {
            transform.SetParent(otherParent);
            tempParent = originalParent;
            originalParent = otherParent;
            otherParent = tempParent;
            is_chaged = 0;
        }
        else
        {
            transform.SetParent(originalParent);
        }
        transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

        // 删除占位符
        Destroy(placeholder);

        // 卡牌位置重置到锚点
        rectTransform.anchoredPosition = originalPosition;
    }
}
