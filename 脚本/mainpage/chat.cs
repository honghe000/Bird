//using System.Collections;
//using System.Collections.Generic;
//using System.Globalization;
//using TMPro;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using System.Threading;
//using System.Threading.Tasks;
//public class chat : MonoBehaviour
//{
//    public Button 发送;
//    public TMP_InputField 输入框;
//    public GameObject 对话框;
//    public TextMeshProUGUI textone;
//    public ScrollRect scrollRect;
//    public Canvas canvas;

//    // Start is called before the first frame update
//    void Start()
//    {
//        StartCoroutine(ReceiveOtherCoroutine());
//        发送.onClick.AddListener(sendMy);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Return))
//        {
//            sendMy();
//        }
//    }

//    void sendMy()
//    {
//        if(输入框.text == "")
//        {
//            //光标移回输入框
//            EventSystem.current.SetSelectedGameObject(输入框.gameObject, null);
//            输入框.OnPointerClick(new PointerEventData(EventSystem.current)); // 需要重置光标位置
//            return;
//        }
//        string text = ValueHolder.username + ":" + 输入框.text;
//        TextMeshProUGUI content = Instantiate(textone, 对话框.transform);
//        content.text = text;
//        content.color = Color.green;
//        输入框.text = "";
//        EventSystem.current.SetSelectedGameObject(输入框.gameObject, null);
//        输入框.OnPointerClick(new PointerEventData(EventSystem.current)); // 需要重置光标位置
//        Canvas.ForceUpdateCanvases();
//        scrollRect.verticalNormalizedPosition = 0f;
//    }

//    // 接收消息的协程
//    IEnumerator ReceiveOtherCoroutine()
//    {
//        while (true)
//        {
//            // 调用异步方法并等待其完成
//            yield return ReceiveOtherAsync();
//        }
//    }

//    // 异步方法，处理接收消息
//    async Task ReceiveOtherAsync()
//    {
//        string text = await Task.Run(() => mainfunction.ReciveMassage());
//        Debug.Log(text);
//        if (!string.IsNullOrEmpty(text))
//        {
//            TextMeshProUGUI content = Instantiate(textone, 对话框.transform);
//            content.text = text;
//            content.color = Color.red;
//        }
//    }
//}
