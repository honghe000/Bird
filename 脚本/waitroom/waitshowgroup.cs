using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class waitshowgroup : MonoBehaviour
{
    public Button waitchoose;
    public Button choice;
    public GameObject layout;

    private void Start()
    {
        waitchoose.onClick.AddListener(show);
    }
    void show()
    {
        foreach (string name in ValueHolder.cardgroupdata.Keys) { 
        
            Button choice_one = Instantiate(choice,layout.transform);
            choice_one.GetComponentInChildren<TextMeshProUGUI>().text = name;        
        }
    }
}
