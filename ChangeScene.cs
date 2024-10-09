using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public Button button;
    public string scene;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() => ChangeScene1(scene));
    }

    // Update is called once per frame
    void Update()
    {

    }


    void ChangeScene1(string scene)
    {
        SceneManager.LoadScene(scene);

    }
}
