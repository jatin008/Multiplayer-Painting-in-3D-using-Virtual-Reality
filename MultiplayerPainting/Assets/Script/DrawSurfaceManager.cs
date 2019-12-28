using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;
using System.IO;
using System;

public class DrawSurfaceManager : MonoBehaviour,IPunInstantiateMagicCallback {
    // attach to the hand which need to draw 
    public Material Imat;
    public SteamVR_TrackedObject  trackedObj;
    private MeshLineRenderer currLine;
    private int numClicks = 0;
    public float width = 0.02f;
    public float rotation = 0f;
    public float widthIncreasingRate = 0.01f;
    public float rotationChangeRate = 0.5f;
    //when the surface appear

    public Transform appearPlace;
    public Transform DrawingsHolder;
    private PhotonView PV;

    public GameObject go;
     void Start()
    {
       
    }
    
    void Update () {
        var counter=0;
        PV=GetComponent<PhotonView>();
        if(PV.IsMine){
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            //what to do after click the triger
            // newGameObject = new GameObject();
            // var name=newGameObject.name;
            // myObj = GameObject.Find("DrawingHolder");
            go =PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","GameObject"), new Vector3(0, -1, 0),Quaternion.Euler(0, 0,0),0,null) as GameObject;
            
            // PhotonView photonView=Get<PhotonView>(go);
            
            // go.tag="DrawingObject";
            // go.name="DrawingObject"+counter;
            // counter=counter+1;
            // PhotonView photonView = this.GetComponent<PhotonView>();
            // photonView.RPC("SpawnOnNetwork", PhotonTargets.AllBuffered, transform.position, transform.rotation, id1);
            // photonView.RPC("crouch",PhotonTargets.AllBuffered,null);


            // go.transform.SetParent(DrawingsHolder);
            go.transform.SetAsFirstSibling();

            go.GetComponent<MeshFilter>();
            go.GetComponent<MeshRenderer>();
            currLine = go.GetComponent<MeshLineRenderer>();
            // Debug.Log("PhotonView Id is as shown below"+ go.GetPhotonView().ViewID);
            currLine.lmat = new Material( appearPlace.gameObject.GetComponent<ColorPointer>().getMat);
            Debug.Log("*********************************");
            Debug.Log(appearPlace.gameObject.GetComponent<ColorPointer>().getMat.color);
            Debug.Log("*********************************");
            currLine.setWidth(width);
            numClicks = 0;
            go.AddComponent<PhotonView>();
            go.AddComponent<PhotonTransformView>();
        }
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)&&appearPlace&&appearPlace.gameObject.activeSelf) {
            //what to do when holding the triger
            currLine.AddPoint(appearPlace.position,appearPlace.up);
            currLine.setWidth(width);
            numClicks++;
            
            PhotonView photonView=PhotonView.Get(this);
            photonView.RPC("SomeFunction",RpcTarget.AllBuffered,JsonUtility.ToJson(currLine),appearPlace.position.ToString(),appearPlace.up.ToString(),PhotonView.Get(go).ViewID,width,appearPlace.gameObject.GetComponent<ColorPointer>().getMat.color.ToString());
            // Debug.Log("Entered in this script");
            // Debug.Log(appearPlace.position);
            // Debug.Log(appearPlace.up);
           
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            //what to do when holding the triger
            currLine = null;
            numClicks = 0;

        }
        // for rotate brush and change width of brush 
        if (device.GetAxis().x > 0.5)
        {
            if (width < 0.1f)
                width += widthIncreasingRate * Time.deltaTime* (device.GetAxis().x );
            appearPlace.localScale =new Vector3(appearPlace.localScale.x, 0.5f  * width, appearPlace.localScale.z);
        }
        if (device.GetAxis().x < -0.5)
        {
            if (width > 0.01f)
                width -= widthIncreasingRate * Time.deltaTime * (Mathf.Abs( device.GetAxis().x) ) ;
            appearPlace.localScale = new Vector3(appearPlace.localScale.x, 0.5f * width, appearPlace.localScale.z);
        }



        if (device.GetAxis().y > 0.5)
        {
            
            rotation += rotationChangeRate * Time.deltaTime* 10f*(device.GetAxis().y-0.4f);
            appearPlace.localEulerAngles = Vector3.right * rotation;
        }
        if (device.GetAxis().y < -0.5)
        {
           
            rotation -= rotationChangeRate * Time.deltaTime * 10f * (Mathf.Abs( device.GetAxis().y )- 0.4f);
            appearPlace.localEulerAngles = Vector3.right * rotation;
        }
        
        // undo function
        if (DrawingsHolder.childCount>0 && device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            Destroy(go);
          

        }

    
    }
    if(!PV.IsMine)
    {
        Debug.Log(PhotonView.Find(PV.ViewID).gameObject);
    }
    }
    
    public void OnPhotonInstantiate(Photon.Pun.PhotonMessageInfo info)
    {
        // e.g. store this gameobject as this player's charater in Player.TagObject
        Debug.Log("Is this mine?... "+info.Sender.IsLocal.ToString());
    }
    [PunRPC]
    void SomeFunction(string a, string b,string c,int d ,float e,string f)
    {
        
        if(!PV.IsMine){
        if(!string.IsNullOrEmpty(a)){
        char[] spearator = { ',' }; 
        int count = 4; 
        f=f.Replace("RGBA(","").Replace(")","");
        string[] strlist = f.Split(spearator,  
                count, StringSplitOptions.None); 
        
        
        GameObject GO=PhotonView.Find(d).gameObject;
        GO.GetComponent<MeshFilter>();
        GO.GetComponent<MeshRenderer>();
        currLine = GO.GetComponent<MeshLineRenderer>();
        b=b.Replace("(","").Replace(")","");
        int count1=3;
        string[] sArray = b.Split(spearator,  
                count1, StringSplitOptions.None); 
        Debug.Log(sArray[0]);
        Debug.Log(sArray[1]);
        Debug.Log(sArray[2]);
        Vector3 position = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));
        // Debug.Log(position);
        c=c.Replace("(","").Replace(")","");
        string[] sArray1 = c.Split(spearator,  
                count1, StringSplitOptions.None); 
        
        Vector3 up = new Vector3(
                float.Parse(sArray1[0]),
                float.Parse(sArray1[1]),
                float.Parse(sArray1[2]));
        // Debug.Log(up);
        currLine.lmat = new Material( appearPlace.gameObject.GetComponent<ColorPointer>().getMat);
        currLine.lmat.color=new Color(float.Parse(strlist[0]),float.Parse(strlist[1]),float.Parse(strlist[2]),float.Parse(strlist[3]));
        currLine.setWidth(e);
        currLine.AddPoint(position,up);
        // numClicks++;

        }

        


    }}

    }
