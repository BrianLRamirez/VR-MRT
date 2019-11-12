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
    AudioClip winSound;
    bool showedParticle = false;

    void Start(){
        debugger = GameObject.Find("Debugger");
        consoleDisplay = GameObject.Find("ConsoleDisplay").GetComponent<TextMeshPro>();;
        isGameOver = false;
        isPuzzleSolved = false;
        gameHasStarted = false;
        audioSource = gameObject.GetComponent<AudioSource>();
        successSound = (AudioClip) Resources.Load("Sounds/success");
        winSound = (AudioClip) Resources.Load("Sounds/win");
    }

    // Update is called once per frame
    void Update(){
        isGameOver = isPuzzleSolved && (currentPuzzle == puzzleList.Length-1);
        if (gameHasStarted) {
            checkForInputAfterGameStarts();
        } else {
           checkForInputBeforeGameStarts();
        }
    }

    void checkForInputAfterGameStarts() {
        checkForWinningCondition();
        updateDebugger();
        if (isPuzzleSolved){
           checkForStateAfterPuzzleSolved();
        } else {
            consoleDisplay.SetText("Use the right thumbstick to rotate along the X and Y axis.\n\nUse the left thumbstick to rotate along the Z axis.");
        }
    }

    void checkForInputBeforeGameStarts() {
         if(OVRInput.GetDown(OVRInput.Button.One)){
            loadPuzzle();
            gameHasStarted = true;
        }
    }

    void checkForStateAfterPuzzleSolved(){
        consoleDisplay.SetText("Great Job!\n Press 'A' to try the next puzzle!");
        if (isGameOver){
            consoleDisplay.SetText("Great Job! You successfully completed the test! Click 'A' to reset the game." );
            if(!showedParticle) {StartCoroutine(displayWinningAnimation());}
                stopAmbientMusic();
                audioSource.volume = 0.045f;
                audioSource.PlayOneShot(winSound);
                if (OVRInput.GetDown(OVRInput.Button.One)){
                    resetGame();
                }
        } else {
            if (OVRInput.GetDown(OVRInput.Button.One)){
                loadPuzzle();
            }
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

    IEnumerator displayWinningAnimation(){
        showedParticle = true;
        Destroy(puzzleLeft);
        Destroy(puzzleRight);
        for(int i = 0; i < 4; i++){
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

    void resetGame(){
        isPuzzleSolved = false;
        gameHasStarted = false;
        showedParticle = false;
        isGameOver = false;
        currentPuzzle = 0;
        consoleDisplay.SetText("Welcome!\nPress the 'A' button to get started wit the test.");
        audioSource.volume = 0.08f;
        playAmbientMusic();
    }

    void stopAmbientMusic(){
        AudioSource speaker = GameObject.Find("Speaker").GetComponent<AudioSource>();
        speaker.Pause();
    }

    void playAmbientMusic(){
        AudioSource speaker = GameObject.Find("Speaker").GetComponent<AudioSource>();
        speaker.Play();
    }

}
