using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ForeverExit : MonoBehaviour
{
    public GameObject 持久化存在;
    private TcpClient client;
    private NetworkStream stream;
    private CancellationTokenSource cancellationTokenSource;
    private Task receiveTask;
    private static bool _initialized = false;


    void Start()
    {
        if (_initialized) return;
        _initialized = true;
        // 初始化 TcpClient 并连接到服务器
        client = new TcpClient();
        client.Connect(ValueHolder.socket_ip, ValueHolder.socket_port); // IP 地址和端口号

        ValueHolder.client = client;

        // 获取网络流
        stream = ValueHolder.client.GetStream();

        Register_message();

        // 启动接收任务
        cancellationTokenSource = new CancellationTokenSource();
        receiveTask = ReceiveMessagesAsync(cancellationTokenSource.Token);

        // 启动发送协程
        StartCoroutine(SendMessage());

        // 防止对象在场景切换时被销毁
        DontDestroyOnLoad(持久化存在);
    }

    void Register_message()
    {
        mainfunction.ChangeSendMessage("SendUser", ValueHolder.username);
        mainfunction.ChangeSendMessage("Action", 0);
        ValueHolder.sendQueue.Enqueue(ValueHolder.SendMessages);
    }

    void OnApplicationQuit()
    {
        // 取消接收任务并关闭连接
        cancellationTokenSource.Cancel();
        stream.Close();
        client.Close();
    }


    async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
    {
        byte[] buffer = new byte[1024];

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                if (bytesRead > 0)
                {
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    ValueHolder.receiveQueue.Enqueue(response);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error receiving data: " + e.Message);
                break;
            }
        }
    }

    IEnumerator SendMessage()
    {
        while (true)
        {
            if (ValueHolder.sendQueue.Count != 0)
            {
                mainfunction.SendMessages();
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
    }


}
