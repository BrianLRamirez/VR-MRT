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
    Vector3 originRight = new Vector3(1.05f, 2.75f, -1.60f);
    Vector3 originLeft= new Vector3(1.05f, 2.75f, 1.25f);
    GameObject debugger;
    AudioSource audioSource;
    AudioClip successSound;
    AudioClip failureSound;
    AudioClip winSound;
    bool showedParticle = false;
    float timer = 0f;
    bool timerEnabled = false;
    float[] puzzleSolvedTimes;

    void Start(){
        debugger = GameObject.Find("Debugger");
        consoleDisplay = GameObject.Find("ConsoleDisplay").GetComponent<TextMeshPro>();;
        isGameOver = false;
        isPuzzleSolved = false;
        gameHasStarted = false;
        audioSource = gameObject.GetComponent<AudioSource>();
        successSound = (AudioClip) Resources.Load("Sounds/success");
        failureSound = (AudioClip) Resources.Load("Sounds/failure");
        winSound = (AudioClip) Resources.Load("Sounds/win");
        puzzleSolvedTimes = new float[puzzleList.Length];
    }

    // Update is called once per frame
    void Update(){
        isGameOver = isPuzzleSolved && (currentPuzzle == puzzleList.Length);
        if (gameHasStarted) {
            checkForInputAfterGameStarts();
        } else {
           checkForInputBeforeGameStarts();
        }
        updateTimer();
    }

    void checkForInputAfterGameStarts() {
        if (OVRInput.GetDown(OVRInput.Button.Two)){
             checkForWinningCondition(); //Only when button down!
        }
        updateDebugger();
        if (isPuzzleSolved){
            timerEnabled = false;
            savePuzzledSolveTime();
            checkForStateAfterPuzzleSolved();
        } 
    }

    void checkForInputBeforeGameStarts() {
         if(OVRInput.GetDown(OVRInput.Button.One)){
            loadPuzzle();
            gameHasStarted = true;
        }
    }

    void checkForStateAfterPuzzleSolved(){
        consoleDisplay.SetText("<#2ecc71>Great Job!</color>\n Press the <#3498db>'A'</color> button to try the next puzzle!");
        if (isGameOver){
            consoleDisplay.SetText("<b>CONGRATULATIONS!</b>\n\nYou successfully completed this VR experience. You may take off the headset now.\n\n <b>Thanks for playing!</b>");
            if(!showedParticle) {StartCoroutine(displayWinningAnimation());}
                stopAmbientMusic();
                audioSource.volume = 0.5f;
                audioSource.PlayOneShot(winSound);
        } else {
            if (OVRInput.GetDown(OVRInput.Button.One)){
                loadPuzzle();
            }
        } 
    }


    void loadPuzzle(){
        float sizeScale = 0.8f;
        Destroy(puzzleLeft);
        Destroy(puzzleRight);
        isPuzzleSolved = false;
        GameObject puzzleModel = Resources.Load(puzzleList[currentPuzzle]) as GameObject;
        puzzleLeft =  GameObject.Instantiate(puzzleModel, originLeft, transform.rotation);
        puzzleLeft.transform.Rotate(Random.Range(45f, 270f), Random.Range(45f, 270.0f), 0);
        puzzleRight =  GameObject.Instantiate(puzzleModel, originRight, transform.rotation);
        puzzleRight.AddComponent<Rotateable>();
        puzzleLeft.transform.localScale = new Vector3(sizeScale, sizeScale, sizeScale);
        puzzleRight.transform.localScale = new Vector3(sizeScale, sizeScale, sizeScale);
        currentPuzzle++;
        consoleDisplay.SetText("Use the right thumbstick to rotate along the X and Y axis.\n\nUse the left thumbstick to rotate along the Z axis.\n\nPress the <#3498db>'B'</color> button to check your solution!");
        startTimer();
    }

    void updateDebugger(){
        bool xMatches = isWithinTolerance(puzzleLeft.transform.localEulerAngles.x, puzzleRight.transform.localEulerAngles.x);
        bool yMatches = isWithinTolerance(puzzleLeft.transform.localEulerAngles.y, puzzleRight.transform.localEulerAngles.y);
        bool zMatches = isWithinTolerance(puzzleLeft.transform.localEulerAngles.z, puzzleRight.transform.localEulerAngles.z);


        TextMeshPro debuggerText = GameObject.Find("DebuggerText").GetComponent<TextMeshPro>();;
        string debugText = "";
        for(int i =0; i < puzzleSolvedTimes.Length; i++){
            debugText += ("Puzzle " + (i+1) + ": " + ((int)puzzleSolvedTimes[i]) + " seconds\n");
        }
        // string debugText = "Puzzle Left: \n" + 
        //                     "<#00ff00>X: " + puzzleLeft.transform.rotation.x + "(" + puzzleLeft.transform.localEulerAngles.x + ")" + "</color>\n" + 
        //                     "<#3498db>Y: " + puzzleLeft.transform.rotation.y + "(" + puzzleLeft.transform.localEulerAngles.y + ")" + "</color>\n" +
        //                     "<#ff46a5>Z: " + puzzleLeft.transform.rotation.z + "(" + puzzleLeft.transform.localEulerAngles.z + ")" + "</color>\n" +
        //                     "Puzzle Right: \n" + 
        //                     "<#00ff00>X: " + puzzleRight.transform.rotation.x + "(" + puzzleRight.transform.localEulerAngles.x + ")" + "</color>\n" + 
        //                     "<#3498db>Y: " + puzzleRight.transform.rotation.y + "(" + puzzleRight.transform.localEulerAngles.y + ")" + "</color>\n" +
        //                     "<#ff46a5>Z: " + puzzleRight.transform.rotation.z + "(" + puzzleRight.transform.localEulerAngles.z + ")" + "</color>\n" +
        //                     "<#00ff00>X Matches: " + xMatches + "</color>\n" +
        //                     "<#3498db>Y Matches: " + yMatches + "</color>\n" +
        //                     "<#ff46a5>Z Matches: " + zMatches + "</color>\n";
        debuggerText.SetText(debugText);
    }

    void checkForWinningCondition(){
        Debug.Log("Checking for winning condition ---------------------------------------");
        bool xMatches = isWithinTolerance(puzzleLeft.transform.localEulerAngles.x, puzzleRight.transform.localEulerAngles.x);
        bool yMatches = isWithinTolerance(puzzleLeft.transform.localEulerAngles.y, puzzleRight.transform.localEulerAngles.y);
        bool zMatches = isWithinTolerance(puzzleLeft.transform.localEulerAngles.z, puzzleRight.transform.localEulerAngles.z);

        if(xMatches && yMatches && zMatches) {
            if(!isPuzzleSolved){
                audioSource.PlayOneShot(successSound);
            }
            isPuzzleSolved = true;
            Destroy(puzzleRight.GetComponent<Rotateable>());
        } else {
            Debug.Log("Win condition false! Playing sad sound! ---------------------------------------");
            consoleDisplay.SetText("<#e74c3c>Wrong!</color>\nIt seems like your rotation is off. Rotate the object and press the <#3498db>'B'</color> button to try again!");
            audioSource.PlayOneShot(failureSound);
        }
    }

    bool isWithinTolerance(float value, float compareTo){
        float tolerance = 55;
        float ceiling = value + tolerance;
        float floor = value - tolerance;

        return (compareTo >= floor && compareTo <= ceiling);
    }

    IEnumerator displayWinningAnimation(){
        showedParticle = true;
        Destroy(puzzleLeft);
        Destroy(puzzleRight);
        for(int i = 0; i < 5; i++){
            createParticleSystemAndPlay();
            yield return new WaitForSeconds(2);
        }
    }

    void createParticleSystemAndPlay() {
        GameObject particleResource = Resources.Load("CFX_Firework_Trails_Gravity") as GameObject;
        GameObject particleObject = GameObject.Instantiate(particleResource, new Vector3(0.86f, 3.1f, 0.17f), transform.rotation);
        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
        particleSystem.Play();
    }

    void stopAmbientMusic(){
        AudioSource speaker = GameObject.Find("Speaker").GetComponent<AudioSource>();
        speaker.Pause();
    }

    void startTimer(){
        timer = 0;
        timerEnabled = true;
    }

    void updateTimer(){
        if (timerEnabled){
            timer += Time.deltaTime;
        }
    }

    void savePuzzledSolveTime(){
        puzzleSolvedTimes[currentPuzzle-1] = timer;
    }

}
