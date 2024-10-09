using UnityEngine;

public class GameController : MonoBehaviour
{
    public HintManager hintManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hintManager.AddHint("这是一个提示！");
        }
    }
}
