using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TcpClientExample : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;

    void Start()
    {
        // 创建 TcpClient 实例
        client = new TcpClient();
        client.Connect("127.0.0.1", 12345); // IP 地址和端口号
        stream = client.GetStream();

        // 启动接收线程
        receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start();

        // 发送消息
        SendMessage("Hello, Server!");
    }

    void SendMessages(string message)
    {
        if (stream != null)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Debug.Log("Sent: " + message);
        }
    }

    void ReceiveMessages()
    {
        if (stream != null)
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Debug.Log("Received: " + response);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error receiving data: " + e.Message);
                    break;
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        if (stream != null)
        {
            stream.Close();
        }
        if (client != null)
        {
            client.Close();
        }
        if (receiveThread != null)
        {
            receiveThread.Abort();
        }
    }
}
