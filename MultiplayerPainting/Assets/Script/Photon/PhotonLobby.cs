using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;
    public GameObject battleButton;
    public GameObject cancelButton;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        lobby=this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        // PhotonNetwork.ConnectToRegionMaster("us");
        PhotonNetwork.ConnectToRegion("us");
        Debug.Log("Player is connected to master server");
    }
    public override void OnConnectedToMaster(){
        
        Debug.Log("We are now connected to the "+ PhotonNetwork.CloudRegion +" Servers!");
        PhotonNetwork.AutomaticallySyncScene=true;
        battleButton.SetActive(true);
        Debug.Log("Connected with master");
    }
    public void OnBattleButtonClicked(){
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Battle Roomed started");
    }
    public override void OnJoinRandomFailed(short returnCode,string message){
        Debug.Log("Room not found creating new room");
        CreateRoom();
        
    }
    public void CreateRoom(){
        int randomroom=Random.Range(0,10000);
        RoomOptions roomOptions=new RoomOptions(){IsVisible=true,IsOpen=true,MaxPlayers=10};
        PhotonNetwork.CreateRoom("Room1204",roomOptions);
        Debug.Log("Entered Room number "+randomroom);
        var game=new PhotonRoom();
        game.StartGame();

    }

    public override void OnCreateRoomFailed(short returnCode,string message){
        Debug.Log("New room creation failed");
        CreateRoom();
    }
    public void OnCancelButtonClicked(){
        cancelButton.SetActive(false);
        battleButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
