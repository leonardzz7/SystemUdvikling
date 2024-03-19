using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextmeshWrapper : TextMeshProUGUI
{
    private string code;
    public void OnChange()
    {
        Invoke("OnChangeProcess", 0.01f);
    }
    public void OnChangeProcess()
    {
        if (Languages.instence != null)
        {
            if (Languages.instence.GetText(code) != null)
            {
                this.text = Languages.instence.GetText(code);
            }
        }
    }
    protected override void Start()
    {
        base.Start();
        code = text;
        OnChange();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (Languages.instence != null)
        {
            Languages.instence.OnLanguageChangeHandler += OnChange;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (Languages.instence != null)
        {
            Languages.instence.OnLanguageChangeHandler -= OnChange;
        }
    }
}
