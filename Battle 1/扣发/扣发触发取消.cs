using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 扣发触发取消 : MonoBehaviour
{
    public Button 取消;

    void Start()
    {
        取消.onClick.AddListener(取消扣发); //取消扣发
    }

    public void 取消扣发()
    {
        ValueHolder.幕布.SetActive(false);
        ValueHolder.扣发显示.SetActive(false);
        mainfunction.技能释放结束();
    }

}
