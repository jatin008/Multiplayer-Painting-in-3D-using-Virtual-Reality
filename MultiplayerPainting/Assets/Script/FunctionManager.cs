using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR; 
public class FunctionManager : MonoBehaviour {

    public enum Functions {
        DrawSurface,
        DrawTube,
        Teleport,
        PlaceModel

    }
    public SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    public bool drawSurfaceOn = true;
    public bool drawTubeOn = true;
    public bool TeleportOn = true;
    public bool placeModelOn = true;
    [Header("drawSurface")]
    public GameObject colorPanel;
    public GameObject colorPicker;
    public DrawSurfaceManager drawSurfaceManager;
    [Header("drawTube")]
    public GameObject loopColorPicker;
    public DrawTubeManager drawTubeManager;
    [Header("Teleport")]
    public SteamVR_Teleporter teleporter;
    public SteamVR_LaserPointer lazerPointer;
    [Header("PlaceModel")]
    public GameObject modelPanel;
    public GameObject modelPicker;
    public ModelPlacingManager loadModelManager;

    private Functions currentFunction = Functions.DrawSurface;

    private Dictionary<int, Functions> functionsDic;
    private int funcIndex = 0;
	// Use this for initialization
	void Start () {

        //NOTE change when add new functions
        functionsDic = new Dictionary<int, Functions>();
        int index = 0;
        if (drawSurfaceOn)
        functionsDic.Add(index++, Functions.DrawSurface);
        if(drawTubeOn)
        functionsDic.Add(index++, Functions.DrawTube);
        if(placeModelOn)
        functionsDic.Add(index++, Functions.PlaceModel);
        if(TeleportOn)
        functionsDic.Add(index++, Functions.Teleport);
       

        OnFunctionChange();
    }

    // Update is called once per frame
    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if(device.GetAxis().x > 0)
            { funcIndex += 1;
                if (funcIndex >= functionsDic.Count)
                    funcIndex = 0;
            }
            if (device.GetAxis().x < 0)
            {
                funcIndex -= 1;
                if (funcIndex <= -1)
                    funcIndex = functionsDic.Count-1;
            }
            currentFunction = functionsDic[funcIndex];
            OnFunctionChange();
        }
    }
    private void OnFunctionChange() {
        switch (currentFunction){
            case Functions.DrawSurface:
                ChangeToDrawSurface();
                break;
            case Functions.DrawTube:
                ChangeToDrawTube();
                break;
            case Functions.Teleport:
                ChangeToTeleport();
                break;
            case Functions.PlaceModel:
                ChangeToPlaceModel();
                break;
            
            }

    }
    private void DisableAll() {
       // disable drawing surface
        colorPanel.SetActive(false);
        colorPicker.SetActive(false);
        drawSurfaceManager.enabled = false;

        // disable drawing tube
        colorPanel.SetActive(false);
        loopColorPicker.SetActive(false);
        drawTubeManager.enabled = false;
        // disable tleport
        teleporter.teleportOnClick = false;

        lazerPointer.enabled = false;
        if (lazerPointer.holder)
            lazerPointer.holder.SetActive(false);

        // diable  model placing
        modelPanel.SetActive(false);
        modelPicker.SetActive(false);
        loadModelManager.enabled = false;
    }
    



    private void ChangeToDrawSurface() {
        DisableAll();
        colorPicker.SetActive(true);
        colorPanel.SetActive(true);
        drawSurfaceManager.enabled = true;
        currentFunction = Functions.DrawSurface;
    }

    private void ChangeToDrawTube() {
        DisableAll();
        loopColorPicker.SetActive(true);
        colorPanel.SetActive(true);
        drawTubeManager.enabled = true;
        currentFunction = Functions.DrawTube;

    }

    private void ChangeToTeleport() {
        DisableAll();
        teleporter.teleportOnClick = true;

        if (lazerPointer.holder)
        lazerPointer.holder.SetActive(true);
        lazerPointer.enabled = true;
       
        currentFunction = Functions.Teleport;
    }

    private void ChangeToPlaceModel() {
        DisableAll();
        //TODO  
        modelPanel.SetActive(true);
        modelPicker.SetActive(true);
        loadModelManager.enabled = true;
        currentFunction = Functions.PlaceModel;
    }
 
}
