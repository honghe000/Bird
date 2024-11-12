using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class b_关闭摸牌堆 : MonoBehaviour
{
    public Button 关闭;
    void Start()
    {
        关闭.onClick.AddListener(关闭摸牌堆);
    }

    void 关闭摸牌堆()
    {
        mainfunction.DestroyAllChildren(ValueHolder.摸牌堆显示区);

        ValueHolder.幕布.SetActive(false);
        ValueHolder.摸牌堆.SetActive(false);
    }

}
