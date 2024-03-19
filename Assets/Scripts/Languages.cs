using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Languages : MonoBehaviour
{
    public static Languages instence;
    TextData[] textData;
    public Action OnLanguageChangeHandler = () => { };
    private void Awake()
    {
        if (instence == null)
        {
            instence = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        TextAsset targetFile = Resources.Load<TextAsset>("data");
        string jsonString = fixJson(targetFile.text);
        Debug.Log(jsonString);
        textData = JsonHelper.FromJson<TextData>(jsonString);
    }
    public void SelectEnglish()
    {
        persistantmanager.instence.lang = persistantmanager.Lang.English;
        startController.instance.switchPanels(6);
        Languages.instence.OnLanguageChangeHandler.Invoke();
        DataBaseHandler.Instance.checkWhenStart();
    }
    public void SelectDanish()
    {
        persistantmanager.instence.lang = persistantmanager.Lang.Danish;
        startController.instance.switchPanels(6);
        Languages.instence.OnLanguageChangeHandler.Invoke();
        DataBaseHandler.Instance.checkWhenStart();
    }
    public string GetText(string code)
    {
        foreach (var data in textData)
        {
            if (CheckIfCodeCorrect(data.Code , code)) {
                if (persistantmanager.instence.lang == persistantmanager.Lang.English)
                {
                    return data.English; 
                }
                else
                {
                    return data.Danish; 
                }
            }
        }
        return null;
    }
    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }
    public bool CheckIfCodeCorrect(string to, string from)
    {
        if (CalcLevenshteinDistance(to, from) < 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private static int CalcLevenshteinDistance(string a, string b)
    {
        if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b))
        {
            return 0;
        }
        if (String.IsNullOrEmpty(a))
        {
            return b.Length;
        }
        if (String.IsNullOrEmpty(b))
        {
            return a.Length;
        }
        int lengthA = a.Length;
        int lengthB = b.Length;
        var distances = new int[lengthA + 1, lengthB + 1];
        for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
        for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

        for (int i = 1; i <= lengthA; i++)
            for (int j = 1; j <= lengthB; j++)
            {
                int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                distances[i, j] = Math.Min
                    (
                    Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                    distances[i - 1, j - 1] + cost
                    );
            }
        return distances[lengthA, lengthB];
    }
    [System.Serializable]
    public class TextData
    {
        public string Code; 
        public string English;
        public string Danish;
    }
}
