using System;
using System.Collections.Generic;
using UnityEngine;

class Program1 : MonoBehaviour
{
    public GameObject card;

    private void Start()
    {
        string script = "�������ʦ";
        System.Type scriptType = System.Type.GetType(script);
        card.AddComponent(scriptType);

    }
}
