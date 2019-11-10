using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour{
    // Start is called before the first frame update
    public GameObject puzzleLeft;
    public GameObject puzzleRight;
    string[] puzzleList = {"puzzle1","puzzle2", "puzzle3"}; 
    int currentPuzzle = 1;
    Vector3 originLeft = new Vector3(1.14f,1.3f,1.84f);
    Vector3 originRight= new Vector3(1.14f,1.3f,-2.5f);

    GameObject debugger;

    float thumbstickX;
    float thumbstickY;

    void Start(){
        debugger = GameObject.Find("Debugger");
        loadNewPuzzle();
    }

    // Update is called once per frame
    void Update(){
        thumbstickX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
        thumbstickY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;
        updateDebugger();

        handleRotation();
        
        if(OVRInput.Get(OVRInput.Button.One)){
        //    Do something
        } else if (OVRInput.Get(OVRInput.Button.Two)){
        //    Do something else
        }
    }

    void loadNewPuzzle(){
        puzzleRight = GameObject.Find("puzzle1");
        //Resources.Load(puzzleList[currentPuzzle]) as GameObject;
        // puzzleLeft =  GameObject.Instantiate(puzzleModel, originLeft, transform.rotation);
        // puzzleRight =  GameObject.Instantiate(puzzleModel, originRight, transform.rotation);
        // currentPuzzle++;
    }

    void updateDebugger(){
        TextMeshPro debuggerText = GameObject.Find("DebuggerText").GetComponent<TextMeshPro>();;
        string debugText = "Thumbstick Data: \n" + 
                            "<#00ff00>X: " + thumbstickX + "</color>\n" + 
                            "<#3498db>Y: " + thumbstickY + "</color>\n";
        debuggerText.SetText(debugText);
    }

     void handleRotation() {
         if(OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight)|| OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft)){
            puzzleRight.transform.localEulerAngles = new Vector3(puzzleRight.transform.localEulerAngles.x, puzzleRight.transform.localEulerAngles.y + (thumbstickX * 5), puzzleRight.transform.localEulerAngles.z);
         } else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp)|| OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown)) {
             puzzleRight.transform.Rotate((thumbstickY * 5), 0 , 0);
           
         }
     }


}
