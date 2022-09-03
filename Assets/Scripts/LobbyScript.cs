using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScript : MonoBehaviourPunCallbacks
{
    [SerializeField] Button joinButton;
    [SerializeField] Button createButton;

    void Start()
    {
        joinButton.onClick.AddListener(JoinRoom);
        createButton.onClick.AddListener(CreateRoom);
    }

    public void JoinRoom()
    {
        string roomName = joinButton.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text;
        PhotonNetwork.JoinRoom(roomName);
    }
    public void CreateRoom()
    {
        string roomName = createButton.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text;

        PhotonNetwork.CreateRoom(roomName);

    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(2);
    }
}
