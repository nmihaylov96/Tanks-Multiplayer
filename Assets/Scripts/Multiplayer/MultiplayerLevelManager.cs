using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;


public class MultiplayerLevelManager : MonoBehaviourPunCallbacks
{
    public int maxKills = 3;
    public GameObject gameOverPopup;
    public Text winnerText;
    void Start()
    {
        PhotonNetwork.Instantiate("Multiplayer Player", Vector3.zero, Quaternion.identity);
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer.GetScore() == maxKills)
        {
            winnerText.text = targetPlayer.NickName;
            gameOverPopup.SetActive(true);
            StorePersonalBest();
        }
    }

    void StorePersonalBest()
    {
        int currentScore = PhotonNetwork.LocalPlayer.GetScore();
        PlayerData playerData = GameManager.instance.playerData;

        if (currentScore > playerData.bestScore)
        {
            playerData.username = PhotonNetwork.LocalPlayer.NickName;
            playerData.bestScore = currentScore;
            playerData.bestScoreDate = DateTime.UtcNow.ToString();
            playerData.totalPlayersInGame = PhotonNetwork.CurrentRoom.Players.Count;
            playerData.roomName = PhotonNetwork.CurrentRoom.Name;
            GameManager.instance.SavePlayerData();
        }
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("GameScene_Multiplayer");
    }
}
