using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class DataBaseHandler : MonoBehaviour
{
    public static DataBaseHandler Instance;
    public Text errorText;
    public GameObject loading;

    public TextMeshProUGUI userNameError;
    public GameObject userNamePanel;
    public TMP_InputField userNameInput;

    string BaseUrl = "http://178.62.227.209/Project/public/api";
  
    string json;
    JSONNode result;

    public delegate void EventHandler();
    public event EventHandler OnSuchEvent;
    private void Awake()
    {
        loading.SetActive(false);
        userNamePanel.SetActive(false);
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        
    }

    public void checkWhenStart()
    {
        switch (PlayerPrefs.GetInt("Guest", 3))
        {
            case 0:
                GetPlayer();
                break;
            case 1:
                GetPlayer();
                break;
            case 2:
                //facebookSetup.instence.FacebookLoginNow();
                break;
        }
    }


    //--------------------------------------GetPlayer-----------------------------------------


    public void GetPlayer()
    {
        OnSuchEvent += GetPlayerCallBack;
        StartCoroutine(APIcall(BaseUrl + "/user/"+ Player1.instance.id, "Get", errorText, loading , null, false, false));
    }
    public void GetPlayerFacebook()
    {
        PlayerPrefs.SetInt("Guest", 0);
        Debug.Log("facebook player created");
        OnSuchEvent += GetPlayerCallBack;
        WWWForm form1 = new WWWForm();
        form1.AddField("key", "facebook");
        form1.AddField("value", Player1.instance.fb_id);
        StartCoroutine(APIcall(BaseUrl + "/getUser" , "Post", errorText, loading, form1, true, false));
    }
    
    public void GetPlayerGoogle(string name , string id, string email )
    {
        PlayerPrefs.SetInt("Guest", 0);
        PlayerPrefs.SetString("PlayerNickName", name);
        Player1.instance.id = id; 
        Player1.instance.email = email;
        Player1.instance.name = name; 

        OnSuchEvent += GetPlayerCallBack;
        WWWForm form1 = new WWWForm();
        form1.AddField("key", "facebook");
        form1.AddField("value", Player1.instance.email);
        StartCoroutine(APIcall(BaseUrl + "/getUser", "Post", errorText, loading, form1, true, false));
    }
    public void GetPlayerCallBack()
    {
        userNamePanel.SetActive(false);
        Player1.instance.id = result["data"]["id"];
        PlayerPrefs.SetString("id", Player1.instance.id);
        Player1.instance.Name = result["data"]["name"];
        Player1.instance.user_name = result["data"]["user_name"];
        Player1.instance.fb_id = result["data"]["fb_id"];
        if (result["data"]["photon_id"] != null) {
            Player1.instance.photon_id = int.Parse(result["data"]["photon_id"]); 
        }
        Player1.instance.email = result["data"]["email"];
        Player1.instance.pendingRequests = new Player1.otherPlayer[result["data"]["pendingRequests"].Count];
        Player1.instance.friendRequest = new Player1.otherPlayer[result["data"]["pendingRequests"].Count];
        Player1.instance.friends = new Player1.otherPlayer[result["data"]["pendingRequests"].Count];
        for (int x = 0; x < result["data"]["pendingRequests"].Count; x++)
        {
            Debug.Log("check");
            Player1.instance.pendingRequests[x] = new Player1.otherPlayer();
            Player1.instance.pendingRequests[x].id = result["data"]["pendingRequests"][x]["id"];
            Player1.instance.pendingRequests[x].Name = result["data"]["pendingRequests"][x]["name"];
            Player1.instance.pendingRequests[x].user_name = result["data"]["pendingRequests"][x]["user_name"];
            Player1.instance.pendingRequests[x].fb_id = result["data"]["pendingRequests"][x]["fb_id"];
            Player1.instance.pendingRequests[x].photon_id = result["data"]["pendingRequests"][x]["photon_id"];
            Player1.instance.pendingRequests[x].email = result["data"]["pendingRequests"][x]["email"];
            
        }
        for (int x = 0; x < result["data"]["friendRequest"].Count; x++)
        {
            Player1.instance.friendRequest[x] = new Player1.otherPlayer();
            Player1.instance.friendRequest[x].id = result["data"]["friendRequest"][x]["id"];
            Player1.instance.friendRequest[x].Name = result["data"]["friendRequest"][x]["name"];
            Player1.instance.friendRequest[x].user_name = result["data"]["friendRequest"][x]["user_name"];
            Player1.instance.friendRequest[x].fb_id = result["data"]["friendRequest"][x]["fb_id"];
            Player1.instance.friendRequest[x].photon_id = result["data"]["friendRequest"][x]["photon_id"];
            Player1.instance.friendRequest[x].email = result["data"]["friendRequest"][x]["email"];
            
        }
        for (int x = 0; x < result["data"]["friends"].Count; x++)
        {
            Player1.instance.friends[x] = new Player1.otherPlayer();
            Player1.instance.friends[x].id = result["data"]["friends"][x]["id"];
            Player1.instance.friends[x].Name = result["data"]["friends"][x]["name"];
            Player1.instance.friends[x].user_name = result["data"]["friends"][x]["user_name"];
            Player1.instance.friends[x].fb_id = result["data"]["friends"][x]["fb_id"];
            Player1.instance.friends[x].photon_id = result["data"]["friends"][x]["photon_id"];
            Player1.instance.friends[x].email = result["data"]["friends"][x]["email"];
            
        }
        startController.instance.switchPanels(0);
        PlayerPrefs.SetString("id", Player1.instance.id);
        //facebookSetup.instence.UpdateStats();
        OnSuchEvent -= GetPlayerCallBack;
        persistantmanager.instence.logedIn = true;
        facebookSetup.instence.UpdateStats();
        startController.instance.SelectPicture();
        
    }
    // ----------------------------------Update Player-----------------------------------
    public void UpdatePlayer()
    {
        OnSuchEvent += SuccessCallBack;
        WWWForm form1 = new WWWForm();
        form1.AddField("user_name", Player1.instance.user_name);
        form1.AddField("id", Player1.instance.id);
        //form1.AddField("fb_id", Player1.instance.fb_id);
        form1.AddField("photon_id", Player1.instance.photon_id);
        //form1.AddField("email", Player1.instance.email);
        form1.AddField("name", Player1.instance.name);

        StartCoroutine(APIcall(BaseUrl+ "/user", "Put", errorText, loading, form1, false, false));
    }
    // ----------------------------------Friends-----------------------------------

    public void removeFriendRequest(string friendid)
    {
        OnSuchEvent += SuccessCallBack;
        WWWForm form1 = new WWWForm();
        form1.AddField("user_id", Player1.instance.id);
        form1.AddField("friend_id", friendid);
        StartCoroutine(APIcall(BaseUrl + "/removeFriendRequest", "Post", errorText, loading, form1, false, false));
    }
    public void aproveFriendRequest(string friendid)
    {
        OnSuchEvent += SuccessCallBack;
        WWWForm form1 = new WWWForm();
        form1.AddField("user_id", Player1.instance.id);
        form1.AddField("friend_id", friendid);
        StartCoroutine(APIcall(BaseUrl + "/approveFriendRequest", "Post", errorText, loading, form1, false, false));
    }
    public void addFriends(string friendid)
    {
        OnSuchEvent += SuccessCallBack;
        WWWForm form1 = new WWWForm();
        form1.AddField("user_id", Player1.instance.id);
        form1.AddField("friend_id", friendid);
        StartCoroutine(APIcall(BaseUrl + "/friendRequest", "Post", errorText, loading, form1, false, false));
    }
    public void removeFriend(string friendid)
    {
        OnSuchEvent += SuccessCallBack;
        WWWForm form1 = new WWWForm();
        form1.AddField("user_id", Player1.instance.id);
        form1.AddField("friend_id", friendid);
        StartCoroutine(APIcall(BaseUrl + "/removeFriend", "Post", errorText, loading , form1, false, false));
    }
    public void SuccessCallBack()
    {
        errorText.text = "Request Success";
        Debug.Log("Success");
        OnSuchEvent -= SuccessCallBack;
    }

    // ----------------------------------Create Player-----------------------------------

    public void CreatePlayer()
    {
        WWWForm form = new WWWForm();
        OnSuchEvent += GetPlayerCallBack;
        //OneUser Cu = new OneUser();
        string userName = userNameInput.text; 
        //Cu.user_name = "Player"+Cu.id;
        form.AddField("user_name" , userName);
        form.AddField("name" ,  Player1.instance.Name);
        form.AddField("email" , Player1.instance.email);
        form.AddField("fb_id", Player1.instance.fb_id);
        //form.AddField("photon_id", "0");
        //json = JsonUtility.ToJson(Cu);
        //Debug.Log(json);
        StartCoroutine(APIcall(BaseUrl+ "/user", "Post", errorText, loading , form , false , true));
        PlayerPrefs.SetInt("Guest" , 0 );
    }

    public void OpenUserNamePanel()
    {
        userNamePanel.SetActive(true);
    }
    // -------------------------------------Api Call-----------------------------------------
    IEnumerator APIcall(string url, string type, Text error, GameObject loadingPanel , WWWForm form , bool login , bool userName)
    {
        loadingPanel.SetActive(true);
        var uwr = new UnityWebRequest(url, type);
        switch (type)
        {
            case "Get":
                uwr.SetRequestHeader("Content-Type", "application/json");
                break;

            case "Post":
                UploadHandler uploader = new UploadHandlerRaw(form.data);
                uwr.uploadHandler = uploader;
                uwr.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                break;

            case "Put":
                UploadHandler uploader1 = new UploadHandlerRaw(form.data);
                uwr.uploadHandler = uploader1;
                uwr.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                break;

            case "Delete":
                uwr.SetRequestHeader("Content-Type", "application/json");
                break;
        }

        uwr.SetRequestHeader("Accept", "application/json");
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        yield return uwr.SendWebRequest();
        Debug.Log(uwr.downloadHandler.text);
        JSONNode metadata = JSON.Parse(uwr.downloadHandler.text);

        //Debug.Log(uwr.downloadHandler.text);

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            error.text = uwr.error;
        }
        else
        {
            if (metadata["status"] == 1)
            {
                result = metadata;
                //callbackFct = callback; 
                if (OnSuchEvent != null)
                    OnSuchEvent();
            }
            else
            {
                error.text = metadata["message"];
                if (login)
                {
                    userNamePanel.SetActive(true);
                }
                if (userName)
                {
                    userNameError.text = "User Name Already Taken , Please try another";
                }

            }
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
        loadingPanel.SetActive(false);
    }
    public class OneUser
    {
        public string user_name;
        public string id;
        public string name;
        public string fb_id;
        public string photon_id;
        public string email;
    }
}
