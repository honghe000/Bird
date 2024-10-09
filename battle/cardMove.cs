using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class cardMove : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler, IPointerClickHandler
{
    public GameObject card;
    public GameObject borderPrefab; // 边框预制体
    private GameObject currentBorder; // 当前已实例化的边框

    public void OnPointerClick(PointerEventData eventData)
    {
        // 检查当前边框是否已经存在
        if (currentBorder == null)
        {
            if (ValueHolder.isChoose == 1)
            {
                foreach (GameObject tishi in ValueHolder.tishilist)
                {
                    Destroy(tishi);
                }
                ValueHolder.tishilist.Clear();
            }
            // 设置移动卡牌的数据到全局变量
            ValueHolder.MovecardData = eventData.pointerClick.GetComponent<数据显示Battle>().卡牌数据;

            // 实例化边框，并将其作为当前物体的子对象
            currentBorder = Instantiate(borderPrefab, transform);

            ValueHolder.tishilist.Add(currentBorder);

            // 设置边框实例的局部坐标和旋转，使其与父对象一致
            currentBorder.transform.localPosition = Vector3.zero;
            currentBorder.transform.localRotation = Quaternion.identity;

            // 存储已选择的状态和卡牌
            ValueHolder.isChoose = 1;
            ValueHolder.isChooseCard = eventData.pointerClick;

            // 获取被点击卡牌的父对象的索引
            int index = int.Parse(eventData.pointerClick.transform.parent.name);

            // 根据索引获取可到达位置的列表
            List<int> reachlist = GetReachablePositions(index);

            // 遍历可到达位置，并放置边框
            foreach (int reach in reachlist)
            {
                //Debug.Log(reach);
                if (ValueHolder.xianxain.Contains(reach))
                {
                    //Debug.Log(reach);
                    continue;
                }

                // 实例化边框，并将其作为相应游戏对象的子对象
                GameObject border = Instantiate(borderPrefab, ValueHolder.movegameobjects[reach - 1].transform.parent);

                ValueHolder.tishilist.Add(border);

                // 设置边框实例的局部坐标和旋转，使其与父对象一致
                border.transform.localPosition = Vector3.zero;
                border.transform.localRotation = Quaternion.identity;

                ValueHolder.tishi.Add(border);
                ValueHolder.tishiInt.Add(reach-1);
            }

            return;
        }

        //实现单击鼠标隐藏/显现边框的操作
        if (currentBorder.activeSelf)
        {
            currentBorder.SetActive(false);
            ValueHolder.MovecardData = null;


            ValueHolder.isChoose = 0;
            ValueHolder.isChooseCard = null;

            foreach (GameObject tishi in ValueHolder.tishi)
            {
                tishi.SetActive(false);
            }
        }
        else
        {
            currentBorder.SetActive(true);
            ValueHolder.MovecardData = eventData.pointerClick.GetComponent<数据显示Battle>().卡牌数据;

            //储存已经选中的状态以及卡牌
            ValueHolder.isChoose = 1;
            ValueHolder.isChooseCard = eventData.pointerClick;

            foreach (GameObject tishi in ValueHolder.tishi)
            {
                tishi.SetActive(true);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 放大物体
        transform.localScale = new Vector3(1.1f, 1.1f, 1f);



    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        // 如果当前边框对象不为空并且边框不显现，则销毁边框对象
        if (currentBorder != null && !currentBorder.activeSelf)
        {
            Destroy(currentBorder);
            currentBorder = null; // 将当前边框变量置为null
            ValueHolder.isChoose = 0;
            ValueHolder.isChooseCard = null;

            //清除提示边框
            foreach (GameObject tishi in ValueHolder.tishi)
            {
                Destroy(tishi);
            }
            ValueHolder.tishi.Clear();

            //清除提示下标
            ValueHolder.tishiInt.Clear();
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log(111);
        card.GetComponent<数据显示Battle>().卡牌数据 = ValueHolder.gloabCaedData[1];
        Debug.Log(card.transform.parent.name + ":" + card.GetComponent<数据显示Battle>().卡牌数据.名字);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


public List<int> GetReachablePositions(int currentPosition)
{
    List<int> reachablePositions = new List<int>();

    int[,] map = new int[,]
    {
        {21, 22, 23, 24, 25},
        {16, 17, 18, 19, 20},
        {11, 12, 13, 14, 15},
        {6, 7, 8, 9, 10},
        {1, 2, 3, 4, 5}
    };

    // 获取当前位置在地图中的行列索引
    int currentRow = 0, currentCol = 0;
    for (int i = 0; i < 5; i++)
    {
        for (int j = 0; j < 5; j++)
        {
            if (map[i, j] == currentPosition)
            {
                currentRow = i;
                currentCol = j;
                break;
            }
        }
    }

    // 向上移动
    if (currentRow > 0 && map[currentRow - 1, currentCol] != 12 && map[currentRow - 1, currentCol] != 14)
    {
        reachablePositions.Add(map[currentRow - 1, currentCol]);
    }

    // 向下移动
    if (currentRow < 4 && map[currentRow + 1, currentCol] != 12 && map[currentRow + 1, currentCol] != 14)
    {
        reachablePositions.Add(map[currentRow + 1, currentCol]);
    }

    // 向左移动
    if (currentCol > 0 && map[currentRow, currentCol - 1] != 12 && map[currentRow, currentCol - 1] != 14)
    {
        reachablePositions.Add(map[currentRow, currentCol - 1]);
    }

    // 向右移动
    if (currentCol < 4 && map[currentRow, currentCol + 1] != 12 && map[currentRow, currentCol + 1] != 14)
    {
        reachablePositions.Add(map[currentRow, currentCol + 1]);
    }

    return reachablePositions;
}
}



//21 22 23 24 25
//16 17 18 19 20
//11 12 13 14 15
//6 7 8 9 10
//1 2 3 4 5