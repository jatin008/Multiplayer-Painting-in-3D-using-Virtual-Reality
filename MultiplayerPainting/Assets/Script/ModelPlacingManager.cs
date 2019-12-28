using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelPlacingManager : MonoBehaviour
{

    // Use this for initialization
    private GameObject modelInScene;

    private bool displayModel = false;
    public ModelPicker modelPicker;
    private SteamVR_TrackedObject trackedObj;

    // for rotating
    private float currentRotation;
    private float rotationChangeRate = 100f;

    // for size changing
    private float currentSize;
    private float orginalSize;
    private float sizeChangingSpeed = 2f;

    public GameObject lazer;

    void Start()
    {

        trackedObj = this.GetComponent<SteamVR_TrackedObject>();

    }



    private void OnEnable()
    {
        ModelPicker.modelChange += LoadModel;

    }
    private void OnDisable()
    {
        ModelPicker.modelChange -= LoadModel;
        Destroy(modelInScene);
        modelInScene = null;
        displayModel = false;
        lazer.SetActive(false);

    }

    void Update()
    {

        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);
        if (displayModel && modelPicker)
        {

            Ray ray = new Ray(lazer.transform.position, lazer.transform.forward);

            // Set defaults
            bool hasGroundTarget = false;


            RaycastHit hitInfo;
            LayerMask mask = 1 << 0;
            hasGroundTarget = Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask);

            if (hasGroundTarget && hitInfo.distance > 0.1f && modelInScene != null)
            {

                if (!modelInScene.activeSelf)
                    modelInScene.SetActive(true);

                // appear the model
                modelInScene.transform.position = hitInfo.point;
             

            }
        }


        // sure to place the model
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            modelInScene = null;
            displayModel = false;
            lazer.SetActive(false);


        }
        // cancel model placing
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {

            Destroy(modelInScene);
            modelInScene = null;
            displayModel = false;
            lazer.SetActive(false);
        }

        // rotating model and size changing
        if (displayModel)
        {
            // for rotate brush and change width of brush 
            if (device.GetAxis().x > 0.5)
            {
                currentRotation = modelInScene.transform.localEulerAngles.y;
                modelInScene.transform.localEulerAngles = Vector3.up * (currentRotation+ rotationChangeRate * Time.deltaTime  * device.GetAxis().x);
            }
            if (device.GetAxis().x < -0.5)
            {
                currentRotation = modelInScene.transform.localEulerAngles.y;
                modelInScene.transform.localEulerAngles = Vector3.up * (currentRotation + rotationChangeRate * Time.deltaTime *  device.GetAxis().x);
            }
            if (device.GetAxis().y > 0.5)
            {
                currentSize = modelInScene.transform.localScale.x;
                if(currentSize<orginalSize*1.8f)
                modelInScene.transform.localScale = Vector3.one * (currentSize + sizeChangingSpeed * Time.deltaTime * device.GetAxis().y* orginalSize);
            }
            if (device.GetAxis().y < -0.5)
            {
                currentSize = modelInScene.transform.localScale.x;
                if (currentSize > orginalSize * 0.2f)
                    modelInScene.transform.localScale = Vector3.one * (currentSize + sizeChangingSpeed * Time.deltaTime * device.GetAxis().y*orginalSize);
            }




        }


    }

    private void LoadModel(string name)
    {
        if (modelInScene != null)
            Destroy(modelInScene);
        displayModel = true;

        modelInScene = Instantiate(Resources.Load("Prefabs/" + name) as GameObject);
        modelInScene.SetActive(false);
        orginalSize = modelInScene.transform.localScale.x;
       
        lazer.SetActive(true);
    }



}
