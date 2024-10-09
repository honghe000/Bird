using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    void Start()
    {

        // 添加下拉框选择事件监听器
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }

    // 下拉框选择值变化时的回调函数
    void DropdownValueChanged(TMP_Dropdown dropdown)
    {
        int selectedIndex = dropdown.value;
        string selectedOption = dropdown.options[selectedIndex].text;
        Debug.Log("Selected Option: " + selectedOption);
    }
}

