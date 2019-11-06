using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour{
    // Start is called before the first frame update
    public GameObject puzzleLeft;
    public GameObject puzzleRight;
    string[] puzzleList = {"puzzle1","puzzle2", "puzzle3"}; 
    int currentPuzzle = 0;
    Vector3 originLeft = new Vector3(1.14f,1.3f,1.84f);
    Vector3 originRight= new Vector3(1.14f,1.3f,-2.5f);

    void Start(){
        loadNewPuzzle();
    }

    // Update is called once per frame
    void Update(){
        float thumbstickX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        float thumbstickY = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
        
        // if(OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown)){
            // go.transform.Rotate(0, 0, (thumbstickY * 3));
        // } else {
            // box.transform.Rotate(0, (thumbstickX * 3), 0);
        // }
    }

    void loadNewPuzzle(){
        GameObject puzzleModel = Resources.Load(puzzleList[currentPuzzle]) as GameObject;
        puzzleLeft =  GameObject.Instantiate(puzzleModel, originLeft, transform.rotation);
        puzzleRight =  GameObject.Instantiate(puzzleModel, originRight, transform.rotation);
        currentPuzzle++;
    }
}
