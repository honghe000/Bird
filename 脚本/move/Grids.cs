using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grids : MonoBehaviour
{

    //九宫格
    public static List<int> GetNeighbors_九(int index)
    {
        List<int> neighbors = new List<int>();
        int gridSize = 5;

        // 计算行列
        int row = (index - 1) / gridSize;
        int col = (index - 1) % gridSize;

        // 遍历周围8个格子
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue; // 跳过自己
                int newRow = row + i;
                int newCol = col + j;

                // 检查是否在边界内
                if (newRow >= 0 && newRow < gridSize && newCol >= 0 && newCol < gridSize)
                {
                    int newIndex = newRow * gridSize + newCol + 1;
                    neighbors.Add(newIndex);
                }
            }
        }

        return neighbors;
    }

    // 输入格子序号，返回该列的所有序号(鬼屋)
    public static List<int> GetColumnIndices(int cellIndex)
    {
        List<int> columnIndices = new List<int>();
        int gridSize = 5;

        // 计算输入格子在第几列
        int column = (cellIndex - 1) % gridSize + 1;

        // 循环获取该列的所有序号
        for (int i = 0; i < gridSize; i++)
        {
            int index = (i * gridSize) + column;
            columnIndices.Add(index);
        }

        return columnIndices;
    }
}
