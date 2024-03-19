using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    public static Player1 instance;

    public string user_name;
    public string id;
    public string Name;
    public string fb_id;
    public int photon_id;
    public string email;
    public otherPlayer[] pendingRequests;
    public otherPlayer[] friendRequest;
    public otherPlayer[] friends;

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }
        id = PlayerPrefs.GetString("id", "");
        instance = this;
        DontDestroyOnLoad(this.gameObject);

    }
    public class otherPlayer{
        public string user_name;
        public string id;
        public string Name;
        public string fb_id;
        public int photon_id;
        public string email;
        
    }
}
