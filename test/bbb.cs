using UnityEngine;

public class GameController : MonoBehaviour
{
    public HintManager hintManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hintManager.AddHint("����һ����ʾ��");
        }
    }
}
