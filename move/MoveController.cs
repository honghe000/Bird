using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private int[,] board = new int[5, 5];
    public HashSet<int> obstacles = new HashSet<int> { 12, 14 }; // 障碍物格子编号
    public int cardType = 1; // 0表示我方卡牌，1表示敌方卡牌
    public int is_myturn = 1; // 0表示敌方回合，1表示我方回合
    public int point = 1; // 0表示没有体力，1表示有体力

    public Dictionary<string,string> 效果 = new Dictionary<string,string>();

    public int 法术可作用 = 1; // 0表示不可作用，1表示可作用

    public int 场上我方人数要求 = 0;
    public int 场上敌方人数要求 = 0;

    public int 击杀免疫 = 0;
    public int 消灭免疫 = 0;

    public int 眩晕免疫 = 0;
    public int 眩晕 = 0;

    public int 杀人后触发 = 0;

    public int 行动点 = 0;

    public MoveController()
    {
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        int id = 1;
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                board[y, x] = id++;
            }
        }
    }

    public List<int> GetAvailableMoves(int currentPos)
    {
        List<int> availableMoves = new List<int>();
        if (gameObject.GetComponent<数据显示>().卡牌数据.类别 == "建筑")
        {
            return availableMoves;
        }
        Vector2Int currentCoords = GetCoordinates(currentPos);
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),  // 上
            new Vector2Int(0, -1), // 下
            new Vector2Int(1, 0),  // 右
            new Vector2Int(-1, 0)  // 左
        };

        foreach (var dir in directions)
        {
            Vector2Int newCoords = currentCoords + dir;
            if (IsInBounds(newCoords))
            {
                int newPos = GetPosition(newCoords);
                if (!obstacles.Contains(newPos)) // 排除障碍物
                {
                    availableMoves.Add(newPos);
                }
            }
        }

        return availableMoves;
    }

    private Vector2Int GetCoordinates(int pos)
    {
        int y = (pos - 1) / 5;
        int x = (pos - 1) % 5;
        return new Vector2Int(x, y);
    }

    private int GetPosition(Vector2Int coords)
    {
        return board[coords.y, coords.x];
    }

    private bool IsInBounds(Vector2Int coords)
    {
        return coords.x >= 0 && coords.x < 5 && coords.y >= 0 && coords.y < 5;
    }

    public struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x + b.x, a.y + b.y);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///拖拽可放置格子
    ///
    public List<int> GetAvailableDrag()
    {
        //判断是否是我的回合
        if (is_myturn == 1) {

            //建筑不消耗体力，角色消耗体力
            if (gameObject.GetComponent<数据显示>().卡牌数据.类别 == "角色" && point >= 1)
            {
                return ValueHolder.人物可放置位置;
            }
            else if (gameObject.GetComponent<数据显示>().卡牌数据.类别 == "建筑")
            {
                return ValueHolder.建筑可放置位置;
            }
            else if (gameObject.GetComponent<数据显示>().卡牌数据.类别 == "法术")
            {
                return new List<int> { 0 };
            }

        }
        return new List<int> { };
    }
    ///到达底线判断
    public List<int> GetWinpos()
    {
        if (cardType == 0)
        {
            return new List<int> { 21, 22, 23, 24, 25 };
        }else
        {
            return new List<int> { 1, 2, 3, 4, 5 };
        }
    }


}


