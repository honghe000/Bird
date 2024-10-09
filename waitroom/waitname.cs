using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class waitname : MonoBehaviour
{
    public TextMeshProUGUI myname;
    public TextMeshProUGUI hename;
    public TextMeshProUGUI roomname;
    // Start is called before the first frame update
    void Start()
    {
        myname.text = ValueHolder.username;
        roomname.text = ValueHolder.Room;
        Message mes = JsonUtility.FromJson<Message>(ValueHolder.ResieveMessages);
        hename.text = ValueHolder.hename;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
