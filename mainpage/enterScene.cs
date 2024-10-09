using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class enterScene : MonoBehaviour
{
    public TMP_InputField input;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeScene()
    {

        //设置所配卡组的名字
        ValueHolder.cardgroupName = input.text;
        SceneManager.LoadScene("卡组");

    }
}
