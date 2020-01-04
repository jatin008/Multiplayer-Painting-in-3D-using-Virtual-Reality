using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks,IInRoomCallbacks
{
    public static PhotonRoom room;
    private PhotonView PV;
    public bool isGameLoaded;
    public int currentScene;
    public int multiplayScene;
    void Awake()
    {
        Debug.Log("Code is awake");
        if(PhotonRoom.room == null){
            PhotonRoom.room = this;
        }
        else
        {
            if(PhotonRoom.room!=this){
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room=this;
            }
        }
        PV=GetComponent<PhotonView>();
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Fetched the photon view");
        // PV=GetComponent<PhotonView>();
        
    }
    public override void OnEnable() {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded+=OnSceneFinishedLoading;
    }
    public override void OnDisable() {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded-=OnSceneFinishedLoading;
    }
    public override void OnJoinedRoom(){
        base.OnJoinedRoom();
        StartGame();
    }
    public void StartGame(){
        if(!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.LoadLevel(multiplayScene); 
    }
    void OnSceneFinishedLoading(Scene scene,LoadSceneMode mode){
        Debug.Log("Entered in sceneLoading");
        currentScene=scene.buildIndex;
        if(currentScene== multiplayScene){
            CreatePlayer();
        }
    }
    private void CreatePlayer(){
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PhotonNetworkPlayer"),transform.position,Quaternion.identity,0);
    }
}
