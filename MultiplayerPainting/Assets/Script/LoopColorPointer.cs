using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopColorPointer : MonoBehaviour {

    public  MeshRenderer render;
    private Material Imat;

    public Material getMat
    {
        get
        {
            return render.material;
        }
    }
    void Start()
    {
        Imat = render.material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ColorBall")
        {
            if (other.gameObject.name == "glass")//this time we change the material
            { render.material = other.GetComponent<MeshRenderer>().material;
                
            }
            else
                render.material = Imat;

            Imat.color = other.gameObject.GetComponent<MeshRenderer>().material.color;
            changeLoopColor(Imat.color);
        }
    }

    void changeLoopColor(Color color) {
     
            render.material.color = color;

        


    }
}
