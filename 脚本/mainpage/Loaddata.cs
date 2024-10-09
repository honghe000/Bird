using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loaddata : MonoBehaviour
{

    public TextMeshProUGUI name1;
    public TextMeshProUGUI coin1;
    public Canvas mainpage;
    public Image chooseCardGroup;
    public Image introduce;
    // Start is called before the first frame update
    void Start()
    {
        name1.text = ValueHolder.username;
        coin1.text = ValueHolder.coin;
        chooseCardGroup.gameObject.SetActive(false);
        introduce.gameObject.SetActive(false);

        ValueHolder.scale_x = mainpage.transform.localScale.x;
        ValueHolder.scale_y = mainpage.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
