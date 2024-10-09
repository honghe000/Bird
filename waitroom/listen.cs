using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class listen : MonoBehaviour
{
    public TextMeshProUGUI hename;
    public TextMeshProUGUI hegroup;
    public TextMeshProUGUI mygroup;
    public Image he_ready;
    public Button battle;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetMessageCoroutine());
        if (ValueHolder.room_is_host == 0)
        {
            hename.text = ValueHolder.hename;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if (mes.Action == 999)
        {
            joined_room(mes);
            Debug.Log("房主接收到其他成员");
        }
        else if (mes.Action == 6)
        {
            change_group(mes);
        }
        else if (mes.Action == 7)
        {
            if (ValueHolder.he_ready == 0)
            {
                ValueHolder.he_ready = 1;
                he_ready.gameObject.SetActive(true);
                if (ValueHolder.my_ready == 1 && ValueHolder.room_is_host == 1)
                {
                    battle.gameObject.SetActive(true);
                }
            }
            else
            {
                ValueHolder.he_ready = 0;
                he_ready.gameObject.SetActive(false);
                battle.gameObject.SetActive(false);
            }
        }
        else if (mes.Action == 8) { 
            if (ValueHolder.room_is_host == 0)
            {
                选择牌组();
                ValueHolder.initiative = mes.initiative;
                SceneManager.LoadScene("Battle 1");
            }
        }
        else if (mes.Action == 11)
        {
            ValueHolder.initiative = mes.initiative;
            SceneManager.LoadScene("Battle 1");
        }
    }

    void 选择牌组()
    {
        ValueHolder.choosegroup = mygroup.text;
        ValueHolder.random_card.Clear();
        foreach (var cardgroupdict in ValueHolder.cardgroupdata)
        {
            string groupname = cardgroupdict.Key;
            Dictionary<string, int> cardgroupData = cardgroupdict.Value;
            if (groupname == ValueHolder.choosegroup)
            {
                foreach (KeyValuePair<string, int> kvp in cardgroupData)
                {
                    for (int i = 0; i < kvp.Value; i++)
                    {
                        ValueHolder.random_card.Add(int.Parse(kvp.Key));
                    }
                }
                break;
            }
        }
    }


    void joined_room(Message mes)
    {
        mainfunction.ChangeSendMessage("SendUser", ValueHolder.username);
        mainfunction.ChangeSendMessage("ReceiveUser", mes.SendUser);
        mainfunction.ChangeRecieveMessage("ReceiveUser", ValueHolder.username);
        mainfunction.ChangeRecieveMessage("SendUser", mes.SendUser);
        hename.text = mes.SendUser;

    }


    void change_group(Message mes)
    {
        hegroup.text = mes.cardgroup;
    }
}
