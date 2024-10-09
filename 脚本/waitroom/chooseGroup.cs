using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class chooseGroup : MonoBehaviour
{
    public Button choose;
    public TextMeshProUGUI groupname;
    public GameObject layout;
    // Start is called before the first frame update
    void Start()
    {
        choose.onClick.AddListener(choose_act);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void choose_act()
    {
        groupname.text = choose.GetComponentInChildren<TextMeshProUGUI>().text;
        mainfunction.ChangeSendMessage("Action", 6);
        mainfunction.ChangeSendMessage ("cardgroup", groupname.text);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
        DeleteAllChildren(layout);
    }

    void DeleteAllChildren(GameObject parent)
    {
        // 获取父对象的 Transform
        Transform parentTransform = parent.transform;

        // 使用反向循环来删除所有子对象
        for (int i = parentTransform.childCount - 1; i >= 0; i--)
        {
            // 获取子对象
            Transform child = parentTransform.GetChild(i);

            // 销毁子对象
            Destroy(child.gameObject);
        }
    }
}
