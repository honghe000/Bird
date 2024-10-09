using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grids : MonoBehaviour
{

    //�Ź���
    public static List<int> GetNeighbors_��(int index)
    {
        List<int> neighbors = new List<int>();
        int gridSize = 5;

        // ��������
        int row = (index - 1) / gridSize;
        int col = (index - 1) % gridSize;

        // ������Χ8������
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue; // �����Լ�
                int newRow = row + i;
                int newCol = col + j;

                // ����Ƿ��ڱ߽���
                if (newRow >= 0 && newRow < gridSize && newCol >= 0 && newCol < gridSize)
                {
                    int newIndex = newRow * gridSize + newCol + 1;
                    neighbors.Add(newIndex);
                }
            }
        }

        return neighbors;
    }

    // ���������ţ����ظ��е��������(����)
    public static List<int> GetColumnIndices(int cellIndex)
    {
        List<int> columnIndices = new List<int>();
        int gridSize = 5;

        // ������������ڵڼ���
        int column = (cellIndex - 1) % gridSize + 1;

        // ѭ����ȡ���е��������
        for (int i = 0; i < gridSize; i++)
        {
            int index = (i * gridSize) + column;
            columnIndices.Add(index);
        }

        return columnIndices;
    }
}
