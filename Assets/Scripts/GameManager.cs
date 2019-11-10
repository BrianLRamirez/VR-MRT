using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour{
    // Start is called before the first frame update
    public GameObject puzzleLeft;
    public GameObject puzzleRight;
    string[] puzzleList = {"puzzle1","puzzle2", "puzzle3"}; 
    int currentPuzzle = 0;
    Vector3 originRight = new Vector3(1.05f, 2.75f, -2.27f);
    Vector3 originLeft= new Vector3(1.05f, 2.75f, 1.45f);

    GameObject debugger;

    float thumbstickX;
    float thumbstickY;

    void Start(){
        loadNewPuzzle();
        debugger = GameObject.Find("Debugger");
    }

    // Update is called once per frame
    void Update(){
        thumbstickX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
        thumbstickY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;
        updateDebugger();

        handleRotation();
        
        if(OVRInput.GetDown(OVRInput.Button.One)){
            loadNewPuzzle();
        } else if (OVRInput.Get(OVRInput.Button.Two)){
        //    Do something else
        }
    }

    void loadNewPuzzle(){
        Destroy(puzzleLeft);
        Destroy(puzzleRight);
        GameObject puzzleModel = Resources.Load(puzzleList[currentPuzzle]) as GameObject;
        puzzleLeft =  GameObject.Instantiate(puzzleModel, originLeft, transform.rotation);
        puzzleRight =  GameObject.Instantiate(puzzleModel, originRight, transform.rotation);
        currentPuzzle++;
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
             puzzleRight.transform.Rotate(0, (thumbstickX * 3), 0);
         } else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp)|| OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown)) {
             puzzleRight.transform.Rotate((thumbstickY * 3), 0 , 0);
           
         }
     }


}
