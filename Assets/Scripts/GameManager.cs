using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour{
    // Start is called before the first frame update
    public GameObject puzzleLeft;
    public GameObject puzzleRight;
    public TextMeshPro consoleDisplay;
    public bool isGameOver;
    public bool gameHasStarted;
    public bool isPuzzleSolved;
    string[] puzzleList = {"puzzle1","puzzle2","puzzle3","puzzle4","puzzle5", "puzzle6","puzzle7","puzzle8"}; 
    int currentPuzzle = 0;
    Vector3 originRight = new Vector3(1.05f, 2.75f, -2.27f);
    Vector3 originLeft= new Vector3(1.05f, 2.75f, 1.45f);
    GameObject debugger;
    AudioSource audioSource;
    AudioClip successSound;

    void Start(){
        debugger = GameObject.Find("Debugger");
        consoleDisplay = GameObject.Find("ConsoleDisplay").GetComponent<TextMeshPro>();;
        isGameOver = false;
        isPuzzleSolved = false;
        gameHasStarted = false;
        audioSource = gameObject.GetComponent<AudioSource>();
        successSound = (AudioClip) Resources.Load("Sounds/success");
    }

    // Update is called once per frame
    void Update(){
        if (gameHasStarted) {
            checkForWinningCondition();
            updateDebugger();
            if (isPuzzleSolved){
                consoleDisplay.SetText("Great Job!\n Press 'A' to try the next puzzle!");
                if (OVRInput.GetDown(OVRInput.Button.One)){
                    loadPuzzle();
                }
            } else {
                consoleDisplay.SetText("Use the right thumbstick to rotate along the X and Y axis.\n\nUse the left thumbstick to rotate along the Z axis.");
            }
        } else {
            if(OVRInput.GetDown(OVRInput.Button.One)){
                loadPuzzle();
                gameHasStarted = true;
                consoleDisplay.SetText("Great Job!");
            }
        }

        if(OVRInput.GetDown(OVRInput.Button.Two)){
              StartCoroutine(showWinningParticle());
        }
       
    }

    void loadPuzzle(){
        Destroy(puzzleLeft);
        Destroy(puzzleRight);
        isPuzzleSolved = false;
        GameObject puzzleModel = Resources.Load(puzzleList[currentPuzzle]) as GameObject;
        puzzleLeft =  GameObject.Instantiate(puzzleModel, originLeft, transform.rotation);
        puzzleLeft.transform.Rotate(Random.Range(0f, 360.0f), Random.Range(0f, 360.0f), 0);
        puzzleRight =  GameObject.Instantiate(puzzleModel, originRight, transform.rotation);
        puzzleRight.AddComponent<Rotateable>();
        currentPuzzle++;
    }

    void updateDebugger(){
        bool xMatches = isWithinTolerance(puzzleLeft.transform.rotation.x, puzzleRight.transform.rotation.x, 0.06f);
        bool yMatches = isWithinTolerance(puzzleLeft.transform.rotation.y, puzzleRight.transform.rotation.y, 0.06f);
        bool zMatches = isWithinTolerance(puzzleLeft.transform.rotation.z, puzzleRight.transform.rotation.z, 0.07f);

        TextMeshPro debuggerText = GameObject.Find("DebuggerText").GetComponent<TextMeshPro>();;
        string debugText = "Puzzle Left: \n" + 
                            "<#00ff00>X: " + puzzleLeft.transform.rotation.x + "</color>\n" + 
                            "<#3498db>Y: " + puzzleLeft.transform.rotation.y + "</color>\n" +
                            "<#ff46a5>Z: " + puzzleLeft.transform.rotation.z + "</color>\n" +
                            "Puzzle Right: \n" + 
                            "<#00ff00>X: " + puzzleRight.transform.rotation.x + "</color>\n" + 
                            "<#3498db>Y: " + puzzleRight.transform.rotation.y + "</color>\n" +
                            "<#ff46a5>Z: " + puzzleRight.transform.rotation.z + "</color>\n" +
                            "<#00ff00>X Matches: " + xMatches + "</color>\n" +
                            "<#3498db>Y Matches: " + yMatches + "</color>\n" +
                            "<#ff46a5>Z Matches: " + zMatches + "</color>\n";
        debuggerText.SetText(debugText);
    }

    void checkForWinningCondition(){
        bool xMatches = isWithinTolerance(puzzleLeft.transform.rotation.x, puzzleRight.transform.rotation.x, 0.06f);
        bool yMatches = isWithinTolerance(puzzleLeft.transform.rotation.y, puzzleRight.transform.rotation.y, 0.06f);
        bool zMatches = isWithinTolerance(puzzleLeft.transform.rotation.z, puzzleRight.transform.rotation.z, 0.07f);

        if(xMatches && yMatches && zMatches) {
            if(!isPuzzleSolved){
                audioSource.PlayOneShot(successSound);
            }
            isPuzzleSolved = true;
            Destroy(puzzleRight.GetComponent<Rotateable>());
        }
    }

    bool isWithinTolerance(float value, float compareTo, float tolerance){
        value = Mathf.Abs(value);
        compareTo = Mathf.Abs(compareTo);
        float ceiling = value + tolerance;
        float floor = value - tolerance;

        return (compareTo >= floor && compareTo <= ceiling);
    }

    IEnumerator showWinningParticle(){
        particleWin();
        yield return new WaitForSeconds(2);
        particleWin();
        yield return new WaitForSeconds(2);
        particleWin();
    }

    void particleWin() {
        GameObject particleResource = Resources.Load("CFX_Firework_Trails_Gravity") as GameObject;
        GameObject particleObject = GameObject.Instantiate(particleResource, new Vector3(0.86f, 3.1f, 0.17f), transform.rotation);
        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
        particleSystem.Play();
    }

}
