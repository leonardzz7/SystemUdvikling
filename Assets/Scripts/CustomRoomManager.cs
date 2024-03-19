
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CustomRoomManager : MonoBehaviourPunCallbacks
{
    public static CustomRoomManager instence; 

   

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;

    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;
    public void Awake()
    {
        if (instence == null)
        {
            instence = this; 
        }

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();

    }
    void Start()
    {
    }
   
    public void CreateRoom()
    {
        persistantmanager.instence.customRoom = true;
    }
    public void RoomList()
    {
        persistantmanager.instence.customRoom = true;
        PhotonNetwork.NickName = Player1.instance.user_name;
    }

    #region PUN CALLBACKS
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
        Debug.Log(" room list update ");
    }
    public override void OnJoinedLobby()
    {
        // whenever this joins a new lobby, clear any previous room lists
        cachedRoomList.Clear();
        ClearRoomListView();
        //SwitchPanels(0);
    }

    // note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    #endregion
    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
#pragma warning disable CS0436 // Type conflicts with imported type
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);
#pragma warning restore CS0436 // Type conflicts with imported type

            roomListEntries.Add(info.Name, entry);
        }
    }
}