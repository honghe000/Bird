//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Net.Sockets;
//using System.Reflection;
//using System.Text;
//using System.Threading;
//using UnityEngine;


//public class socketMove : MonoBehaviour
//{
//    private Thread clientThread;

//    private byte[] receiveBuffer = new byte[1024];
//    // Start is called before the first frame update
//    private string previousValue = "1;1";
//    void Start()
//    {
//        clientThread = new Thread(ReceiveData);
//        clientThread.Start();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (ValueHolder.moveString != previousValue)
//        {
//            Debug.Log(ValueHolder.moveString);
//            if (!ValueHolder.moveString.Contains(";"))
//            {
//                previousValue = ValueHolder.moveString;
//                return;
//            }
//            string[] command = ValueHolder.moveString.Split(';');
//            previousValue = ValueHolder.moveString;
//            moveSocket(int.Parse(command[0]), int.Parse(command[1]));
            
//        }
//    }

//    //private void ReceiveData()
//    //{
//    //    while (true)
//    //    {
//    //            int bytesRead = ValueHolder.socket.Receive(receiveBuffer);
//    //            if (bytesRead > 0)
//    //            {
//    //                ValueHolder.moveString = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);
//    //                Debug.Log(ValueHolder.moveString);

//    //            }
//    //    }
//    //}

//    public void moveSocket(int start,int end)
//    {
//        if (ValueHolder.xianxain.Contains(end) && !ValueHolder.xianxain.Contains(start))
//        {
//            return;
//        }
//        if (!ValueHolder.xianxain.Contains(start) && !ValueHolder.xianxain.Contains(end)) { return; }
//        if (ValueHolder.xianxain.Contains(start) && ValueHolder.xianxain.Contains(end)) { return; }
//        if (!ValueHolder.movegameobjects[end-1].activeSelf)
//        {
//            //替换移动后的元素下标
//            foreach (int xianxain in ValueHolder.xianxain)
//            {
//                if (xianxain == start)
//                {
//                    ValueHolder.xianxain[ValueHolder.xianxain.IndexOf(xianxain)] = end;
//                    break;
//                }
//            }
//        }
//        if (!ValueHolder.xianxain.Contains(end))
//        {
//            ValueHolder.xianxain.Add(end);
//        }
//        foreach(int i in ValueHolder.xianxain)
//        {
//            Debug.Log(i);
//        }

//        ValueHolder.movegameobjects[start-1].SetActive(false);

//        ValueHolder.movegameobjects[end-1].SetActive(true);


//    }
//}
