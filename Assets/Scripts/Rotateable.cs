using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotateable : MonoBehaviour {
    
    void Start() {
        
    }

    void Update() {
        float thumbstickX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
        float thumbstickY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;

         if(OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight)|| OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft)){
             gameObject.transform.Rotate(0, (thumbstickX * 2), 0);
         } else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp)|| OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown)) {
             gameObject.transform.Rotate((thumbstickY * 2), 0 , 0);
         }
    }
}
