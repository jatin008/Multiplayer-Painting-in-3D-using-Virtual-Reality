using UnityEngine;
using System.Collections;

public class SteamVR_Teleporter : MonoBehaviour
{

    public GameObject arrowPrefab = null;
    public bool teleportOnClick = false;
    private GameObject arrowInScene;
    private Transform personImage=null;
   
    public SteamVR_TrackedObject trackedObj;
    public float futureRotation = 180f;
    public float rotationChangeRate = 30f;
    public Transform CameraRig;
    private float hightVairable = 0f;
    public float hightVairableChangeRate = 10f;
    Transform reference
    {
        get
        {
            var top = SteamVR_Render.Top();
            return (top != null) ? top.origin : null;
        }
    }

    void Start()
    {
       
      


    }

    private void Update()
    {
        if (teleportOnClick)
        {
            // First get the current Transform of the the reference space (i.e. the Play Area, e.g. CameraRig prefab)
            var t = reference;
            if (t == null)
                return;

            // Get the current Y position of the reference space
            float refY = t.position.y;

            // Create a plane at the Y position of the Play Area
            // Then create a Ray from the origin of the controller in the direction that the controller is pointing
            Plane plane = new Plane(Vector3.up, -refY);
            Ray ray = new Ray(this.transform.position, transform.forward);

            // Set defaults
            bool hasGroundTarget = false;
            float dist = 0f;

            RaycastHit hitInfo;
            hasGroundTarget = Physics.Raycast(ray, out hitInfo);
            dist = hitInfo.distance;
            if (hasGroundTarget)
            {
              
                // Get the current Camera (head) position on the ground relative to the world
                Vector3 headPosOnGround = new Vector3(SteamVR_Render.Top().head.position.x, refY, SteamVR_Render.Top().head.position.z);
                if (arrowInScene == null)
                {
                    arrowInScene = GameObject.Instantiate(arrowPrefab);
                    personImage =  arrowInScene.transform.Find("personImage");
                   
                    personImage.localScale = CameraRig.transform.Find("Camera (eye)").position.y* Vector3.one*0.8f;
                }
                // appear the arrow
                if (arrowInScene.activeSelf == false)
                    arrowInScene.SetActive(true);
                personImage.localScale = CameraRig.transform.Find("Camera (eye)").position.y * Vector3.one*0.8f;
                personImage.localEulerAngles = CameraRig.transform.Find("Camera (eye)").localEulerAngles.y * Vector3.up;
                arrowInScene.transform.position = t.position+hightVairable*Vector3.up + (ray.origin + (ray.direction * dist)) - headPosOnGround;

            }

        }
        else if (arrowInScene != null && arrowInScene.activeSelf == true)
        {

            arrowInScene.SetActive(false);


        }

        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);

        if (arrowInScene && arrowInScene.activeSelf)
        {
            futureRotation = arrowInScene.transform.localEulerAngles.y;
            if (device.GetAxis().x > 0.5)
            {
                futureRotation += rotationChangeRate * Time.deltaTime * 10f * (device.GetAxis().x - 0.4f);
                arrowInScene.transform.localEulerAngles = Vector3.up * futureRotation;
            }


            if (device.GetAxis().x < -0.5)
            {
                futureRotation -= rotationChangeRate * Time.deltaTime * 10f * (Mathf.Abs(device.GetAxis().x) - 0.4f);
                arrowInScene.transform.localEulerAngles = Vector3.up * futureRotation;
            }



            if (device.GetAxis().y > 0.5 && hightVairable < 1.2f)
            {
                hightVairable += hightVairableChangeRate * Time.deltaTime * (Mathf.Abs(device.GetAxis().y) - 0.4f);

            }
            if (device.GetAxis().y < -0.5 && hightVairable > 0f)
            {
                hightVairable -= hightVairableChangeRate * Time.deltaTime * (Mathf.Abs(device.GetAxis().y) - 0.4f);

            }


        }

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (teleportOnClick)
            {
                // First get the current Transform of the the reference space (i.e. the Play Area, e.g. CameraRig prefab)
                var t = reference;
                if (t == null)
                    return;

                // Get the current Y position of the reference space
                float refY = t.position.y;

                // Create a plane at the Y position of the Play Area
                // Then create a Ray from the origin of the controller in the direction that the controller is pointing
                Plane plane = new Plane(Vector3.up, -refY);
                Ray ray = new Ray(this.transform.position, transform.forward);

                // Set defaults
                bool hasGroundTarget = false;
                float dist = 0f;

                RaycastHit hitInfo;
                hasGroundTarget = Physics.Raycast(ray, out hitInfo);
                dist = hitInfo.distance;


                if (hasGroundTarget)
                {
                    // Get the current Camera (head) position on the ground relative to the world
                    Vector3 headPosOnGround = new Vector3(SteamVR_Render.Top().head.position.x, refY, SteamVR_Render.Top().head.position.z);

                    // We need to translate the reference space along the same vector
                    // that is between the head's position on the ground and the intersection point on the ground
                    // i.e. intersectionPoint - headPosOnGround = translateVector
                    // currentReferencePosition + translateVector = finalPosition
                    t.position = t.position + (ray.origin + (ray.direction * dist)) + hightVairable * Vector3.up - headPosOnGround;
                    hightVairable = 0f;
                    //give camera a new forward direction
                    CameraRig.forward = arrowInScene.transform.forward;
                    //arrowInScene.transform.forward = (CameraRig.transform.position- arrowInScene.transform.position ).normalized;
                }
            }
        }
    }
   

   
}

