using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUp : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;
    public void ClosePopUp()
    {
        LeanTween.scale(this.gameObject, new Vector3(0f, 0f, 0f), 0.4f);
    }
}
