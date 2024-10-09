using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButton : MonoBehaviour
{
    public Button ready;
    public Image readyImage;
    public Button battle;
    public TextMeshProUGUI cardgroup;
    public TextMeshProUGUI error;
    // Start is called before the first frame update
    void Start()
    {
        cardgroup.text = "等待选择卡组...";
        ready.GetComponentInChildren<TextMeshProUGUI>().text = "准备";
        ready.onClick.AddListener(readys);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void readys()
    {
        if (cardgroup.text == "等待选择卡组...")
        {
            error.gameObject.SetActive(true);
            return;
        }
        error.gameObject.SetActive(false);
        Debug.Log(ready.GetComponentInChildren<TextMeshProUGUI>().text);
        if (ready.GetComponentInChildren<TextMeshProUGUI>().text == "准备")
        {
            ready.GetComponentInChildren<TextMeshProUGUI>().text = "取消准备";
            readyImage.gameObject.SetActive(true);
            mainfunction.ChangeSendMessage("Action", 7);
            ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
            ValueHolder.my_ready = 1;
            if (ValueHolder.he_ready == 1 && ValueHolder.room_is_host == 1)
            {
                battle.gameObject.SetActive(true);
            }
        }
        else
        {
            ready.GetComponentInChildren<TextMeshProUGUI>().text = "准备";
            readyImage.gameObject.SetActive(false);
            mainfunction.ChangeSendMessage("Action", 7);
            ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
            ValueHolder.my_ready = 0;
            battle.gameObject.SetActive(false);
        }
    }
}
