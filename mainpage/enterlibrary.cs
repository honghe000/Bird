using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class enterlibrary : MonoBehaviour
{
    public UnityEngine.UI.Button groupbutton;
    // Start is called before the first frame update
    void Start()
    {
        groupbutton.onClick.AddListener(() => ChangeScene1("卡组"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeScene1(string scene)
    {
        string cardgroupname = groupbutton.GetComponentInChildren<TextMeshProUGUI>().text;
        ValueHolder.cardgroupName = cardgroupname;
        ValueHolder.cardgroupchoose = ValueHolder.cardgroupdata[cardgroupname]; 
        SceneManager.LoadScene(scene);

    }
}
