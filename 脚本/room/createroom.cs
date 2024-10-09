using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class createroom : MonoBehaviour
{
    public Button createButton;
    public TMP_InputField roomName_c;
    public TMP_InputField roomName_j;
    public Button join;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetMessageCoroutine());
        createButton.onClick.AddListener(create_room);
        join.onClick.AddListener(Search_Room);
    }


    void create_room()
    {
        mainfunction.ChangeSendMessage("Action", 4);
        mainfunction.ChangeSendMessage("RoomName", roomName_c.text);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    void Search_Room()
    {
        if (roomName_j.text == "") return;
        mainfunction.ChangeSendMessage("Action", 3);
        mainfunction.ChangeSendMessage("RoomName", roomName_j.text);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    IEnumerator GetMessageCoroutine()
    {
        while (true)
        {

            if (ValueHolder.receiveQueue.Count == 0)
            {
                yield return new WaitForSeconds(0.2f); // 等待0.1秒
                continue;
            }
            else
            {
                // 假设 ValueHolder.ResieveMessages 是一个消息队列或存储最新消息的地方
                Message mes = JsonUtility.FromJson<Message>(ValueHolder.receiveQueue.Dequeue());
                mes.is_used = 1;
                mainfunction.ChangeRecieveMessage("is_used", 1);
                process(mes); // 处理消息
                yield return new WaitForSeconds(0.2f); // 等待0.1秒
            }
        }
    }

    void process(Message mes)
    {
        if (mes.Action == 998)
        {
            ValueHolder.Room = roomName_c.text;
            ValueHolder.room_is_host = 1;
            SceneManager.LoadScene("waitRoom");

        }else if (mes.Action == 999)
        {
            ValueHolder.Room = roomName_j.text;


            mainfunction.ChangeSendMessage("SendUser", ValueHolder.username);
            mainfunction.ChangeSendMessage("ReceiveUser", mes.ReceiveUser);
            mainfunction.ChangeRecieveMessage("ReceiveUser", ValueHolder.username);
            mainfunction.ChangeRecieveMessage("SendUser", mes.ReceiveUser);
            ValueHolder.hename = mes.ReceiveUser;

            SceneManager.LoadScene("waitRoom");
        }
    }
}
