using UnityEngine;

public class ClickDebugger : MonoBehaviour
{
    void Update()
    {
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            // 使用 Raycast 射线检测点击的对象
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 如果射线碰撞到对象
            if (Physics.Raycast(ray, out hit))
            {
                // 输出被点击对象的名字
                Debug.Log("Clicked on object: " + hit.collider.gameObject.name);
            }
            else
            {
                // 如果射线没有碰撞到对象，输出提示信息
                Debug.Log("Clicked on nothing");
            }
        }
    }
}
