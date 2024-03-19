using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class calculator : MonoBehaviour
{
    public Text result;
    
    public InputField entry1;
    public InputField entry2;

    public void Add()
    {
    result.text = "" +(int.Parse(entry1.text) + int.Parse(entry2.text));
    }

    public void Subtract()
    {
        result.text = "" + (int.Parse(entry1.text) - int.Parse(entry2.text));
    }
    public void Multiplication()
    { 
         result.text = "" +(int.Parse(entry1.text) * int.Parse(entry2.text));
    }
    public void Division()
    {
        result.text = "" + (int.Parse(entry1.text) / int.Parse(entry2.text));
    }
}   