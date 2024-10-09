using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Threading;
using System.Xml.Linq;
public class joinroom : MonoBehaviour
{
    public TMP_InputField roomName;
    public Button join;
    public int status;

    private void Start()
    {
        status = 0;
        join.onClick.AddListener(Search_Room_t);
    }

    void Search_Room_t()
    {
        Thread newThread = new Thread(Search_Room);
        newThread.Start();

    }

    private void Update()
    {
        if (status == 1)
        {
            SceneManager.LoadScene("waitRoom");
        }
    }

    void Search_Room()
    {
        if (roomName.text == "") return;
        mainfunction.ChangeSendMessage("Action", 3);
        mainfunction.ChangeSendMessage("RoomName", roomName.text);
        mainfunction.SendMessages();
        Message mes =  GetMessage();
        if (mes.Action == 999)
        {
            ValueHolder.Room = roomName.text;
            status = 1;


            mainfunction.ChangeSendMessage("SendUser", ValueHolder.username);
            mainfunction.ChangeSendMessage("ReceiveUser", mes.ReceiveUser);
            mainfunction.ChangeRecieveMessage("ReceiveUser", ValueHolder.username);
            mainfunction.ChangeRecieveMessage("SendUser", mes.ReceiveUser);
            ValueHolder.hename = mes.ReceiveUser;
        }
    } 

    Message GetMessage()
    {
        while (true)
        {
            Message mes = JsonUtility.FromJson<Message>(ValueHolder.ResieveMessages);
            if (mes.is_used == 1)
            {
                continue;
            }
            else
            {
                mes.is_used = 1;
                mainfunction.ChangeRecieveMessage("is_used", 1);
                return mes;
            }
        }
    } 
}
