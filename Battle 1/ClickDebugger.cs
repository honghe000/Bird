using UnityEngine;

public class ClickDebugger : MonoBehaviour
{
    void Update()
    {
        // ������������
        if (Input.GetMouseButtonDown(0))
        {
            // ʹ�� Raycast ���߼�����Ķ���
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ���������ײ������
            if (Physics.Raycast(ray, out hit))
            {
                // �����������������
                Debug.Log("Clicked on object: " + hit.collider.gameObject.name);
            }
            else
            {
                // �������û����ײ�����������ʾ��Ϣ
                Debug.Log("Clicked on nothing");
            }
        }
    }
}
