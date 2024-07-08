using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Variable
    public static NetworkManager Instance { get; private set; }

    #endregion

    #region Unity_Function
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        JoinServer();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            JoinRandomOrCreateRoom();
        }
        else if(Input.GetKeyDown(KeyCode.S)) LeaveRoom();
    }

    #endregion

    #region Photon_Function

    public static void JoinServer() => PhotonNetwork.ConnectUsingSettings();
    public static void JoinRandomOrCreateRoom() => PhotonNetwork.JoinRandomOrCreateRoom();
    public static void JoinLobby() => PhotonNetwork.JoinLobby();
    public static void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        Debug.Log("방 참가 : " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("방 탈퇴");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 참가");
    }

    

    public override void OnJoinedLobby()
    {


    }

    #endregion
}

