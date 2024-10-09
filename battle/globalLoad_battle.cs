//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using System.Linq;
//using UnityEngine.UI;
//using System;
//using System.Net.Sockets;
//using System.Net;

//public class globalLoad_battle : MonoBehaviour
//{

//    public Transform parentTransform;
//    public Camera mainCamera;
//    public TextMeshProUGUI pos;
//    public TextMeshProUGUI pos1;

//    public TextMeshProUGUI 体力;





//    // Start is called before the first frame update
//    void Start()
//    {
//        hideCard();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // 如果用户点击了鼠标左键
//        if (Input.GetMouseButtonDown(0))
//        {
//            // 获取鼠标点击的屏幕位置
//            Vector3 mousePosition = Input.mousePosition;

//            // 输出鼠标位置信息
//            //Debug.Log("鼠标点击位置：" + mousePosition); 
//            pos.text = mousePosition.ToString();

//            int index = FindClosestVectors(ValueHolder.vector3s, mousePosition, 100);
//            //Debug.Log($"{index}");

//            if (index != -1 && ValueHolder.isChoose == 1 && index != int.Parse(ValueHolder.isChooseCard.transform.parent.name) - 1 && ValueHolder.tishiInt.Contains(index) && int.Parse(体力.text) > 0)
//            {
//                //Debug.Log(ValueHolder.isChooseCard.transform.parent.name);
//                if (!ValueHolder.movegameobjects[index].activeSelf)
//                {
//                    mainfunction.SendMessageToServer("SEND " + ValueHolder.isChooseCard.transform.parent.name + ";" + (index + 1).ToString());
//                    //替换移动后的元素下标
//                    foreach (int xianxain in ValueHolder.xianxain)
//                    {
//                        if (xianxain == int.Parse(ValueHolder.isChooseCard.transform.parent.name))
//                        {
//                            ValueHolder.xianxain[ValueHolder.xianxain.IndexOf(xianxain)] = index + 1;
//                            //Debug.Log("*" + int.Parse(ValueHolder.isChooseCard.transform.parent.name).ToString() + "*" + xianxain.ToString() + "*");
//                            break;
//                        }
//                    }
//                }
//                if (!ValueHolder.xianxain.Contains(index + 1))
//                {
//                    ValueHolder.xianxain.Add(index + 1);
//                }

//                if (ValueHolder.isChooseCard.transform.Find("biankuang(Clone)"))
//                {
//                    Destroy(ValueHolder.isChooseCard.transform.Find("biankuang(Clone)").gameObject);
//                }
//                ValueHolder.isChooseCard.SetActive(false);
//                ValueHolder.isChooseCard = null;
//                ValueHolder.isChoose = 0;

//                ValueHolder.movegameobjects[index].SetActive(true);

//                foreach (GameObject tishi in ValueHolder.tishi)
//                {
//                    Destroy(tishi);
//                }
//                ValueHolder.tishi.Clear();


//                ValueHolder.tishiInt.Clear();

//                体力.text = (int.Parse(体力.text) - 1).ToString();

//            }
//        }
//    }


//    private void OnApplicationQuit()
//    {
//        ValueHolder.socket.Close();

//    }
//    void hideCard()
//    {
//        int a = 1;
//        int index = 1;
//        // 遍历父物体的每个子物体
//        foreach (Transform childTransform in parentTransform)
//        {
//            // 在当前子物体中查找名为"commonCard"的孙物体
//            Transform grandchildTransform = childTransform.Find("commonCard");

//            if (grandchildTransform != null)
//            {
//                grandchildTransform.gameObject.GetComponent<数据显示Battle>().卡牌数据 = ValueHolder.gloabCaedData[1];

//                grandchildTransform.gameObject.GetComponent<数据显示Battle>().positionindex = index;

//                index++;

//                ValueHolder.movegameobjects.Add(grandchildTransform.gameObject);

//                // 获取目标物体的渲染器组件
//                //SpriteRenderer spriteRenderer = grandchildTransform.GetComponent<SpriteRenderer>();


//                //Vector3 screenCenter = mainCamera.WorldToScreenPoint(spriteRenderer.bounds.center);
//                //Vector3 screenCenter = mainCamera.WorldToScreenPoint(grandchildTransform.position);
//                Vector3 screenCenter = grandchildTransform.position;
//                screenCenter.x += 70;
//                screenCenter.y += 95;
//                screenCenter.z -= 10;
//                ValueHolder.vector3s.Add(screenCenter);

//                if (a == 1)
//                {
//                    pos1.text = grandchildTransform.position.ToString();
//                }

//                //Debug.Log(screenCenter.x + "  " + screenCenter.y + "  " +screenCenter.z);
//                if (a == 1 || a == 2)
//                {
//                    ValueHolder.xianxain.Add(a);
//                    a++;
//                    continue;
//                }
//                grandchildTransform.gameObject.SetActive(false);
//            }
//        }
//    }


//    public int FindClosestVectors(List<Vector3> vectorList, Vector3 mousePosition, float minDistance)
//    {
//        for (int i = 0; i < vectorList.Count; i++)
//        {
//            float distance = Vector3.Distance(vectorList[i], mousePosition);
//            if (distance < minDistance)
//            {
//                return i;
//            }
//        }
//        return -1;
//    }

//    public static Vector3 AdaptPosition(Vector3 originalPosition)
//    {
//        float currentScreenWidth = Screen.width;
//        float scaleRatio = currentScreenWidth / 1920f;

//        // 调整坐标，确保在不同分辨率下位置相对一致
//        Vector3 adaptedPosition = new Vector3(
//            originalPosition.x * scaleRatio,
//            originalPosition.y * scaleRatio,   // 这里假设高度也按相同的比例调整
//            originalPosition.z
//        );

//        return adaptedPosition;
//    }

//    void AdjustOrthographicSize()
//    {
//        float targetAspect = 16f / 9f; // 目标宽高比，例如16:9
//        float currentAspect = (float)Screen.width / Screen.height;

//        float size = mainCamera.orthographicSize;

//        // 计算新的正交大小以适应当前屏幕的宽高比
//        if (currentAspect > targetAspect)
//        {
//            // 屏幕更宽，基于高度调整
//            size = (Screen.height / 2f) / Mathf.Tan(Mathf.PI * mainCamera.fieldOfView / 360f);
//        }
//        else
//        {
//            // 屏幕更窄或相等，基于宽度调整
//            float targetHeight = (Screen.width / targetAspect) / 2f;
//            size = targetHeight / Mathf.Tan(Mathf.PI * mainCamera.fieldOfView / 360f);
//        }

//        mainCamera.orthographicSize = size;
//    }


//}
