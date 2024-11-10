using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class b_弃牌堆关闭 : MonoBehaviour
{
    public Button 关闭;
    // Start is called before the first frame update
    void Start()
    {
        关闭.onClick.AddListener(弃牌堆关闭);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void 弃牌堆关闭()
    {
        mainfunction.DestroyAllChildren(ValueHolder.弃牌堆显示区);

        ValueHolder.幕布.SetActive(false);
        ValueHolder.弃牌堆.SetActive(false);
    }
}
