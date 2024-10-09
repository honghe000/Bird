using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class battle : MonoBehaviour
{
    public Button battleButton;
    public TextMeshProUGUI mygroup;
    // Start is called before the first frame update
    void Start()
    {
        battleButton.onClick.AddListener(bat);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void bat()
    {

        //生成选择牌组
        选择牌组();


        //进入战斗也买你
        mainfunction.ChangeSendMessage("Action", 8);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
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
}
