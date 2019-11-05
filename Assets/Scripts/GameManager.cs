using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour{
    // Start is called before the first frame update
    public GameObject box;

    void Start(){
        box = GameObject.Find("box");
    }

    // Update is called once per frame
    void Update(){
        float thumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        box.transform.Rotate(0, (thumbstick * 10), 0);
    }
}
