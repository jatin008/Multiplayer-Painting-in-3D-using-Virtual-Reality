using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelPicker : MonoBehaviour
{

    public delegate void ModelChange(string name);
    public static event ModelChange modelChange;

    Transform appearPlace;
    public Transform modelPanel = null;
    public SteamVR_TrackedObject trackedObj;
    private Material outFrameMat = null;

    private GameObject currrentModel = null;
    private Material currentModelOldMat;



    private void OnDisable()
    {
        if (currrentModel != null)
        {
            currrentModel.GetComponentInChildren<Renderer>().material = currentModelOldMat;
            currrentModel = null;
        }

    }

    public string currentModelName
    {
        get
        {

            return currrentModel != null ? currrentModel.name : null;
        }
    }
    // Use this for initialization
    void Start()
    {
        outFrameMat = this.GetComponent<Renderer>().material;
        appearPlace = this.transform.Find("appearPlace");
    }

    void Update()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);
        if ((device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) || device.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) && currrentModel != null)
        {
            // only control the mini models on the panel
            currrentModel.GetComponentInChildren<Renderer>().material = currentModelOldMat;
            currrentModel = null;
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ModelTag" && currrentModel == null)
        {

            currentModelOldMat = other.GetComponentInChildren<Renderer>().material;
            other.GetComponentInChildren<Renderer>().material = outFrameMat;
            modelChange(other.gameObject.name);
            currrentModel = other.gameObject;

        }
    }


}
