using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 问答数据
{
    public int id;
    public string question;
    public string option;
    public string anwser;
    public 问答数据(string question, string option, string anwser, int id)
    {
        this.question = question;
        this.option = option;
        this.anwser = anwser;
        this.id = id;
    }
}