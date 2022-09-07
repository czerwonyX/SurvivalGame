using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectScript : MonoBehaviourPunCallbacks
{
    Dictionary<string, string> regions = new Dictionary<string, string>()
    {
        {"Europe", "eu" },
        {"Asia", "asia" },
        {"USA", "us" },
        {"Russia", "ru" },
        {"Turkey", "tr" }
    };
    [SerializeField] Button connectButton;
    [SerializeField] TMP_Dropdown regionDropDown;
    public void Start()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = 300;

        PhotonNetwork.SendRate = 40;
        PhotonNetwork.SerializationRate = 10;

        connectButton.onClick.AddListener(Connect);
    }

    private void Connect()
    {
        string selectedRegion = regionDropDown.options[regionDropDown.value].text;
        selectedRegion = regions[selectedRegion];
        print(selectedRegion);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(selectedRegion);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene(1);
    }
}
