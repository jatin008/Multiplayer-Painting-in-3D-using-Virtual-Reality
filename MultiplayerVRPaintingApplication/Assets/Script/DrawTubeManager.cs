using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class DrawTubeManager : MonoBehaviour {

    public Material Imat;
    public SteamVR_TrackedObject trackedObj;
    private MeshLineRenderer[] currLines;
    private int numClicks = 0;
    public float width = 0.02f;
    public float rotation = 0f;
    public float widthIncreasingRate = 0.01f;
    
    //when the loop appear
    public Transform[] appearPlaces;
    private Transform appearPlaceHolder;
    public Transform DrawingsHolder;
    public LoopColorPointer loopColorPointer;

    private PhotonView PV;
    void Start()
    {
        currLines = new MeshLineRenderer[6];
        appearPlaceHolder = appearPlaces[0].parent;
    }
    
    void Update()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);
        if(PV.IsMine){

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            currLines = new MeshLineRenderer[6];
            GameObject goHolder = new GameObject();
            goHolder.transform.SetParent(DrawingsHolder);
            goHolder.transform.SetAsFirstSibling();
            for (int i = 0; i < appearPlaces.Length; i++)
            {
                //what to do after click the triger
                GameObject go = new GameObject();
                go.transform.SetParent(goHolder.transform);
                go.AddComponent<MeshFilter>();
                go.AddComponent<MeshRenderer>();
                currLines[i] = go.AddComponent<MeshLineRenderer>();
                currLines[i].lmat = new Material(loopColorPointer.getMat);
                currLines[i].setWidth(width);
               
            }
            numClicks = 0;
        }
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger) )
        {
            //what to do when holding the triger
            for (int i = 0; i < currLines.Length; i++)
            {
                if (currLines[i]!=null&&appearPlaces[i]!=null&&appearPlaces[i].gameObject.activeSelf)
                {
                    currLines[i].AddPoint(appearPlaces[i].position, appearPlaces[i].up);
                    currLines[i].setWidth(width);
                    
                }
            }
            numClicks++;
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            //what to do when holding the triger
            currLines = null;
            numClicks = 0;

        }
        // for rotate brush and change width of brush 
        if (device.GetAxis().x > 0.5)
        {
            if (width < 0.055f)
                width += widthIncreasingRate * Time.deltaTime * (device.GetAxis().x ) ;
            appearPlaceHolder.localScale = new Vector3(20f*width, 20f* width, appearPlaceHolder.localScale.z);
        }
        if (device.GetAxis().x < -0.5)
        {
            if (width > 0.005f)
                width -= widthIncreasingRate * Time.deltaTime * (Mathf.Abs(device.GetAxis().x) )  ;
            appearPlaceHolder.localScale = new Vector3(20f* width, 20f*width, appearPlaceHolder.localScale.z);
        }


        // undo function
        if (DrawingsHolder.childCount > 0 && device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            Destroy(DrawingsHolder.GetChild(0).gameObject);


        }
        // PhotonNetwork.InstantiateSceneObject()

    }}
}
