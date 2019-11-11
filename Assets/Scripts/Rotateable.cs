using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotateable : MonoBehaviour {
    
    float speed = 2f;
    void Update() {
        bool isThumbstickUp = OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp);
        bool isThumbstickDown = OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown);
        bool isThumbstickLeft = OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft);
        bool isThumbstickRight = OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight);

        bool isLeftThumbstickLeft = OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft);
        bool isLeftThumbstickRight = OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight);
        

        if (isThumbstickUp){
            gameObject.transform.Rotate(0,0,-speed,Space.World);
        } else if(isThumbstickDown) {
            gameObject.transform.Rotate(0,0,speed,Space.World);
        } else if(isThumbstickLeft){
            gameObject.transform.Rotate(0,speed,0,Space.World);
        } else if(isThumbstickRight) {
            gameObject.transform.Rotate(0,-speed,0,Space.World);
        }

        if (isLeftThumbstickRight) {
            gameObject.transform.Rotate(-speed,0,0,Space.World);
        } else if (isLeftThumbstickLeft){
            gameObject.transform.Rotate(speed,0,0,Space.World);
        }

    }
}
