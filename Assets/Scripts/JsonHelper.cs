using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    public static Languages.TextData[] FromJson<T>(string json)
    {
        Wrapper<Languages.TextData> wrapper = JsonUtility.FromJson<Wrapper<Languages.TextData>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
