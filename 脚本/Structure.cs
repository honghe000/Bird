using System.Collections.Generic;
using System;

[Serializable]     //序列化
public class Message
{
    public string SendUser;
    public string ReceiveUser;
    public string chat;
    public string RoomName;
    public int Action;
    public int is_used;
    public string cardgroup;
    public int cardID;
    public int start_index;
    public int end_index;
    public int initiative;
    public float turn;
    public string uid;
    public string uid1;
    public int effect;
    public int num;
}