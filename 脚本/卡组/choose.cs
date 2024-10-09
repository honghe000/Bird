using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class choose : MonoBehaviour
{
    public TMP_Dropdown level;
    public TMP_Dropdown typ;

    private void Start()
    {
        level.onValueChanged.AddListener(delegate {
            DropdownValueChanged(level, typ);
        });

        typ.onValueChanged.AddListener(delegate {
            DropdownValueChanged(level, typ);
        });

    }

    // 下拉列表值变化时的回调函数
    void DropdownValueChanged(TMP_Dropdown dropdown, TMP_Dropdown otherDropdown)
    {
        int selectedIndex1 = dropdown.value;
        string selectedOption1 = dropdown.options[selectedIndex1].text;
        int selectedIndex2 = otherDropdown.value;
        string selectedOption2 = otherDropdown.options[selectedIndex2].text;
        GetComponent<libary>().InstantinateCard(selectedOption1, selectedOption2);
    }
}
