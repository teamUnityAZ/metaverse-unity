
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberOfPlayerInroom :MonoBehaviourPunCallbacks,ILobbyCallbacks
{
    private TextMeshProUGUI counterText; // UI Text refernce.
    // Start is called before the first frame update
    void Start()
    {
        counterText = ReferrencesForDynamicMuseum.instance.totalCounter; 
      // playerStatus();
    }

    private void Update()
    {
        playerStatus();
    }

    public override void OnJoinedRoom() {
        playerStatus();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerStatus();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerStatus();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
    }

    public void playerStatus()
    {
        try
        {
            if (counterText != null)
            {
                //Debug.Log("Player count====" + PhotonNetwork.CurrentRoom.PlayerCount);
                counterText.text = PhotonNetwork.CurrentRoom.PlayerCount + "/10";
            }
        }
        catch(Exception e)
        {

        }
       

       
    }



    public override void OnDisconnected(DisconnectCause cause)
    {
        playerStatus();
    }


}
