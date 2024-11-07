using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent; // ���Ƶ�ԭ������
    public Transform otherParent; // �������Ƶĸ�����
    private Transform tempParent;
    private int is_chaged = 0;
    private int originalSiblingIndex; // ���Ƴ�ʼ��SiblingIndex
    private Vector2 originalPosition; // ���Ƴ�ʼλ��
    private RectTransform rectTransform; // ���Ƶ� RectTransform
    private GameObject placeholder; // ռλ������

    private float swapThreshold = 70f; // ����λ�õ���С������ֵ

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ��¼���Ƶ�ԭ������λ�ú�����
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
        originalPosition = rectTransform.anchoredPosition;

        // ����ռλ�������뵽ԭλ��
        placeholder = new GameObject("Placeholder");
        var layoutElement = placeholder.AddComponent<LayoutElement>();
        layoutElement.preferredWidth = rectTransform.rect.width;
        layoutElement.preferredHeight = rectTransform.rect.height;

        placeholder.transform.SetParent(originalParent);
        placeholder.transform.SetSiblingIndex(originalSiblingIndex);

        // ��ʱ�Ƴ�GridLayoutGroup�Ŀ���
        transform.SetParent(originalParent.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ���¿��Ƹ������λ��
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

            // ֻ�е�����С����ֵ�����ұȵ�ǰ��С�����ʱ������ռλ����λ��
            if (distance < swapThreshold)
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

                // ֻ�е�����С����ֵ�����ұȵ�ǰ��С�����ʱ������ռλ����λ��
                if (distance < swapThreshold)
                {
                    is_chaged = 1;
                    newSiblingIndex = i;
                    closestDistance = distance;
                    foundNearbyCard = true;
                }
            }
        }


        // ����Y��ļ����ֵ
        float yThreshold = 50f; // ���Ը�����Ҫ����

        // ���û���ҵ��㹻�ӽ��Ŀ��ƣ������Ƿ񿿽���Եλ��
        if (!foundNearbyCard)
        {
            RectTransform firstChild = originalParent.GetChild(0) as RectTransform;
            RectTransform lastChild = originalParent.GetChild(originalParent.childCount - 1) as RectTransform;

            // ��ȡ��ǰ�϶��������һ���������Y�����
            float distanceToFirstY = Mathf.Abs(rectTransform.position.y - firstChild.position.y);
            float distanceToLastY = Mathf.Abs(rectTransform.position.y - lastChild.position.y);

            // �ж��Ƿ��ϵ���ͷλ�� (x����������������y������ֵ��Χ��)
            if (rectTransform.position.x < firstChild.position.x - firstChild.rect.width / 2 && distanceToFirstY < yThreshold)
            {
                is_chaged = 0;
                newSiblingIndex = 0;

            }
            // �ж��Ƿ��ϵ�ĩβλ�� (x����������������y������ֵ��Χ��)
            else if (rectTransform.position.x > lastChild.position.x + lastChild.rect.width / 2 && distanceToLastY < yThreshold)
            {
                is_chaged = 0;
                newSiblingIndex = originalParent.childCount - 1;
            }
        }

        // ���û���ҵ��㹻�ӽ��Ŀ��ƣ������Ƿ񿿽���Եλ��
        if (!foundNearbyCard && otherParent != null)
        {
            RectTransform firstChild = otherParent.GetChild(0) as RectTransform;
            RectTransform lastChild = otherParent.GetChild(otherParent.childCount - 1) as RectTransform;

            // ��ȡ��ǰ�϶��������һ���������Y�����
            float distanceToFirstY = Mathf.Abs(rectTransform.position.y - firstChild.position.y);
            float distanceToLastY = Mathf.Abs(rectTransform.position.y - lastChild.position.y);

            // �ж��Ƿ��ϵ���ͷλ�� (x����������������y������ֵ��Χ��)
            if (rectTransform.position.x < firstChild.position.x - firstChild.rect.width / 2 && distanceToFirstY < yThreshold)
            {
                is_chaged = 1;
                newSiblingIndex = 0;

            }
            // �ж��Ƿ��ϵ�ĩβλ�� (x����������������y������ֵ��Χ��)
            else if (rectTransform.position.x > lastChild.position.x + lastChild.rect.width / 2 && distanceToLastY < yThreshold)
            {
                is_chaged = 1;
                newSiblingIndex = otherParent.childCount - 1;
            }
        }



        if (newSiblingIndex != originalSiblingIndex)
        {
            if (is_chaged == 1)
            {
                // ��ռλ���ƶ����µ�λ��
                placeholder.transform.SetParent(otherParent);
            }
            else
            {
                // ��ռλ���ƶ����µ�λ��
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

        // ɾ��ռλ��
        Destroy(placeholder);

        // ����λ�����õ�ê��
        rectTransform.anchoredPosition = originalPosition;
    }


}
