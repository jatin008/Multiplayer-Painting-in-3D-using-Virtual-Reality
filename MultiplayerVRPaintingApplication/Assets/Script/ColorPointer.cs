using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPointer : MonoBehaviour {

    private MeshRenderer render;
    private Material Imat;
    public Material getMat {
        get {
            return render.material;
                }
    }
    void Start () {
        render = this.GetComponent<MeshRenderer>();
        Imat = render.material;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ColorBall") {

            render.material.color = other.gameObject.GetComponent<MeshRenderer>().material.color;

        }


        if (other.gameObject.tag =="material")
        {
            render.material = other.GetComponent<MeshRenderer>().material;
            Debug.Log("Material Changed");
        }
    }
}
