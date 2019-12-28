using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveModelManager : MonoBehaviour {

    
    private string path = "Assets/Resources/Prefabs/";
    private string objName = "obj.prefab";

    public GameObject drawingHolder;
    public SteamVR_TrackedObject trackedObj;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            CreateNew(drawingHolder, path+objName);
            Destroy(drawingHolder);

        }

    }

    void CreateNew(GameObject obj, string localPath)
    {
        //Create a new prefab at the path given
        // Object prefab = PrefabUtility.CreatePrefab(localPath, obj);
        // PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
    }
}
