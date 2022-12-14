using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NoteEmitter : MonoBehaviour
{
    static int update_difficulty = 1;
    static bool change_difficulty = false;
    static bool increment_difficulty = false;
    // Declaleft the controller, notes and dictionary here so they can be used in functions
    public MicrogameJamController controller;
    public GameObject upNote;  // Up
    public GameObject leftNote;   // Left
    public GameObject rightNote; // Right
    public GameObject downNote;  // Down
    public IDictionary<int, GameObject> numberNotes = new Dictionary<int, GameObject>();

    // math for notes
    float xEmit;
    float yEmit;

    // Set Note Detector Location
    float xDetect;
    float yDetect;

    // Calculates necessary note velocity
    float xDist;
    float xVel;

    // Sets time for note to get from emitter to detector (in seconds)
    public float noteTime;

    // Set milliseconds before each note (needs to be read in from map)
    public List<float> timeBeforeNotes = new List<float>();

    // List of notes to be used by the note detector
    public List<GameObject> allNotes = new List<GameObject>();
    // List of notes to be used for fading out
    public List<GameObject> cloneNotes = new List<GameObject>();
  
    // Declares note Detector
    public GameObject detector;
    // distance for input to be valid
    public const float validInputDistance = 1.25f;
    // distance for input to be too early or late
    public const float offDistance = 0.75f;
    // distance to pop note from list
    public const float tooFarDistance = 1f;
    // number of notes deleted
    private int noteCounter;
    //number of notes shot
    private int notesShot = 0;
    // total number of notes
    private int numOfNotes;
    // if game over or not
    public bool game_over = false;

    // lmao
    private bool lmao = true;

    // sfx
    public AudioSource hitSfx;
    public AudioSource missSfx;
    public AudioSource gameOverSfx;
    public AudioSource winSfx;
    public AudioSource music;

    // hand and paper objs
    public GameObject hands;
    public GameObject paper;
    //animators
    public Animator handAnim;
    public Animator paperAnim;

    GameObject hearts;
    GameObject heartsBox;

    private int difficulty;
    public int health;

    //timers
    private float noteTimer;

    [HideInInspector] public float bpm;
    private float bpmTimer;

    // Start is called before the first frame update
    void Start()
    {
        // seconds per beat
        bpm = 6f/13f;
        bpmTimer = 0f;
        noteCounter = 0;
        noteTimer = 0f;
        notesShot = 0;
        health = 3;
        // Gets note Detector
        detector = GameObject.Find("NotePress");
        
        // Initialize Game Jam Controller
        controller = GameObject.Find("Jam Controller").GetComponent<MicrogameJamController>();

        // Initialize Animations
        hands = GameObject.Find("Hands");
        handAnim = hands.GetComponent<Animator>();
        paper = GameObject.Find("Paper");
        paperAnim = paper.GetComponent<Animator>();
        hearts = GameObject.Find("Hearts");
        heartsBox = GameObject.Find("HeartBox");

        // Gets current difficulty (1, 2, or 3)
        difficulty = controller.GetDifficulty();
        if (change_difficulty) {
            difficulty = update_difficulty;
        }
        print($"current difficulty: {difficulty}");

        // Reads the given csv files
        //StreamReader reader = File.OpenText("Assets/Imports/TextFiles/chart.csv");
        TextAsset fin = Resources.Load<TextAsset>("chart");
        // Initializes float for holding the times of notes that are passed over due to difficulty
        float addedMilliseconds = 0f;
        // Toss away the header line of the csv file
        //string line = reader.ReadLine();
        string[] data = fin.text.Split('\n');
        //while ((line = reader.ReadLine()) != null)
        for (int i = 1; i < data.Length; i++)
        {
            string line = data[i];
            string[] words = line.Split(',');

            // Get the difficulty of the note
            int noteDifficulty = Int32.Parse(words[1]);

            // If the note is within difficulty, add it to the list
            if (noteDifficulty <= difficulty)
            {
                float millisecondCount = float.Parse(words[0]);
                timeBeforeNotes.Add(millisecondCount + addedMilliseconds);

                // Reset the added milliseconds
                addedMilliseconds = 0f;
            }

            // If it's not, add the milliseconds left over
            else {
                addedMilliseconds += float.Parse(words[0]);
            }
        }
        numOfNotes = timeBeforeNotes.Count;

        // Find and add all of the types of notes to a dictionary for randomization purposes
        upNote = GameObject.Find("UpNote");
        leftNote = GameObject.Find("LeftNote");
        rightNote = GameObject.Find("RightNote");
        downNote = GameObject.Find("DownNote");
        numberNotes.Add(0, upNote);
        numberNotes.Add(1, leftNote);
        numberNotes.Add(2, rightNote);
        numberNotes.Add(3, downNote);

        // Set Note Spawn Location
        xEmit = 9f;
        yEmit = detector.transform.position.y;

        // Set Note Detector Location
        xDetect = detector.transform.position.x;
        yDetect = detector.transform.position.y;

        // Set rate of note emitting
        // float noteRate = 1f;

        // Calculates necessary note velocity
        xDist = xDetect - xEmit;
        xVel = xDist / noteTime;
    }

    void bpmAnimation() {
        bpmTimer += Time.deltaTime;
        if (bpmTimer >= bpm) {
            bpmTimer = 0f;
            LeanTween.moveY(hearts, hearts.transform.position.y + 0.1f, 0.3f).setEaseShake();
            LeanTween.moveY(heartsBox, heartsBox.transform.position.y + 0.1f, 0.3f).setEaseShake();
            hands.transform.position = new Vector3(0f, 0f, 0f);
            if (handAnim.GetCurrentAnimatorStateInfo(0).IsName("default")) {
                hands.transform.position = new Vector3(0f, 0f, 0f);
                LeanTween.moveY(hands, hands.transform.position.y + 0.3f, 0.3f).setEaseShake();
            }
        }
    }

    void checkUpdateDifficulty() {
        if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Return)) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                increment_difficulty = !increment_difficulty;
                change_difficulty = increment_difficulty;
                print($"incremement difficulty on win: {increment_difficulty}");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1)) {
                increment_difficulty = false;
                change_difficulty = true;
                update_difficulty = 1;
                print("Next difficulty set to 1");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                increment_difficulty = false;
                change_difficulty = true;
                update_difficulty = 2;
                print("Next difficulty set to 2");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                increment_difficulty = false;
                change_difficulty = true;
                update_difficulty = 3;
                print("Next difficulty set to 3");
            }
        }
    }
    
    // Returns true if distance between first note and noteDetector is close enough to process input
    bool isValidInput()
    {
        if (Mathf.Abs(detector.transform.position.x - allNotes[0].transform.position.x) < validInputDistance)
        {
            return true;
        }
        return false;
    }

    // Returns true if button press is too early
    bool earlyCheck()
    {
        if ((allNotes[0].transform.position.x - detector.transform.position.x) > offDistance)
        {
            return true;
        }
        return false;
    }

    // Returns true if button press is too late
    bool lateCheck()
    {
        if ((detector.transform.position.x - allNotes[0].transform.position.x) > offDistance)
        {
            return true;
        }
        return false;
    }

    // Returns true if correct button is pressed
    int successCheck()
    {
        Note note = allNotes[0].GetComponent<Note>();
        if ((note.type == 0 && Input.GetKeyDown("up")) || (note.type == 1 && Input.GetKeyDown("left")) ||
            (note.type == 2 && Input.GetKeyDown("right")) || (note.type == 3 && Input.GetKeyDown("down")))
        {
            return note.type;
        }
        return -1;
    }

    // Takes in note spawn location and speed as arguments
    void EmitNote(float x, float y, float xV)
    {
        if (game_over) return;
        // Chooses a random note out of the four to use
        GameObject emittingNote = numberNotes[UnityEngine.Random.Range(0, 4)];
        // GameObject emittingNote = numberNotes[PickRandomNote()];
        
        // Instantiates the note at the given spawn location with no rotation
        GameObject emittedNote = (GameObject)Instantiate(emittingNote, new Vector3(x, y, 0), Quaternion.identity);
        notesShot++;
        allNotes.Add(emittedNote);

        // Give the note the specified speed
        Rigidbody2D rb = emittedNote.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(xV, 0, 0);
    }

    void shootNote() {
        if (notesShot >= numOfNotes) {
            return;
        }
        // timeBeforeNotes[] is the milliseconds between notes
        float note_interval = timeBeforeNotes[notesShot]/1000f;
        noteTimer += Time.deltaTime;
        //print($"constant dT: {Time.deltaTime}");
        if (noteTimer >= note_interval) {
            noteTimer = 0f;
            EmitNote(xEmit, yEmit, xVel);
        }
    }

    // updates notes
    void updateNotes() {
        bool success = false;
        // set sprite to incoming sprite, and lower opacity.
        detector.GetComponent<SpriteRenderer>().sprite = allNotes[0].GetComponent<SpriteRenderer>().sprite;
        detector.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        detector.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,.5f);
        
        int successState = 0;
        if (Input.GetKeyDown("left") || Input.GetKeyDown("down") || 
            Input.GetKeyDown("right") || Input.GetKeyDown("up"))
        {
            if (isValidInput()) {
                successState = successCheck();
                //earlyCheck
                if (earlyCheck()) {
                    //print("TOO EARLY, LOSE");
                    success = false;               
                }
                else if (lateCheck()) {
                    //print("TOO LATE, LOSE");
                    success = false;
                }
                else if (successState == -1) {
                    //print("WRONG BUTTON, LOSE");
                    success = false;  
                }
                else {
                    success = true;
                    //print("SUCCESS!!!");
                    // Success Sound and Visual
                }

                // play animation and sfx depending on if success or not
                if (success) {
                    // fade out cloned note

                    // tween a direction
                    hitSfx.PlayOneShot(hitSfx.clip, 0.7f);
                    playAnimation(successState);
                }
                else {
                    missSfx.PlayOneShot(missSfx.clip, 1f);
                    health--;
                    Animator heartsAnim = GameObject.Find("Hearts").GetComponent<Animator>();
                    heartsAnim.Play($"{health}_hearts", -1, 0f);
                    playAnimation(-1);
                }
                
                if (health <= 0) {
                    gameOver();
                    return;
                }
                // add to note counter
                noteCounter++;
                DeleteFrontNote();
                return;
            }
        }
        // Check if note is too far past detector and add to fail condition
        if ((detector.transform.position.x - allNotes[0].transform.position.x) > tooFarDistance)
        {
            //print("TOO FAR!");
            // Lose function or health - 1
            missSfx.PlayOneShot(missSfx.clip, 1f);
            health--;
            Animator heartsAnim = GameObject.Find("Hearts").GetComponent<Animator>();
            heartsAnim.Play($"{health}_hearts", -1, 0f);
            playAnimation(-1);
            if (health <= 0) {
                gameOver();
                return;
            }
            noteCounter++;
            DeleteFrontNote();
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        shootNote();
        if (allNotes.Count > 0) {
            updateNotes();
        }
        bpmAnimation();
        checkUpdateDifficulty();
    }

    // deletes the current front note
    void DeleteFrontNote() {
        cloneNotes.Insert(0, allNotes[0]);
        // Destroy(allNotes[0]);
        Rigidbody2D rb = allNotes[0].GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(0, 0, 0);
        allNotes.RemoveAt(0);
        LeanTween.alpha(cloneNotes[0], 0f, 0.15f).setOnComplete(DestroyNote);
        // you win!
        if (noteCounter >= numOfNotes && health > 0) {
            gameWin();
        }
    }

    void DestroyNote()
    {
        Destroy(cloneNotes[cloneNotes.Count - 1]);
        cloneNotes.RemoveAt(cloneNotes.Count - 1);
        return;
    }

    // Play hand and paper animation
    // 0 = up, 1 = left, 2 = right, 3 = down
    void playAnimation(int type) {
        if (game_over) return;
        Dictionary<int, string> noteDir = new Dictionary<int, string>() {
            {-1, "fail"}, {0, "up"}, {1, "left"}, {2, "right"}, {3, "down"}
        };
        float notesElapsed = (float)noteCounter / (float)numOfNotes;
        int currentState = 0;
        if (notesElapsed >= 2f/3f) currentState = 2;
        else if (notesElapsed >= 1f/3f) currentState = 1;
        //print($"play {currentState}_{noteDir[type]}");

        if (type == -1) {
            handAnim.Play("fail_anim", -1, 0f);
        }
        else {
            handAnim.Play($"{currentState}_{noteDir[type]}", -1, 0f);
            paperAnim.Play($"{currentState}_{noteDir[type]}", -1, 0f);
            LeanTween.cancel(hands);
            hands.transform.position = new Vector3(0f, 0f, 0f);
            switch (type) {
                case 0:
                    LeanTween.moveY(hands, hands.transform.position.y + 0.3f, 0.3f).setEaseShake();
                    break;
                case 1:
                    LeanTween.moveX(hands, hands.transform.position.x - 0.3f, 0.3f).setEaseShake();
                    break;
                case 2:
                    LeanTween.moveX(hands, hands.transform.position.x + 0.3f, 0.3f).setEaseShake();
                    break;
                case 3:
                    LeanTween.moveY(hands, hands.transform.position.y - 0.3f, 0.3f).setEaseShake();
                    break;
                default:
                    break;
            }
        }
    }

    // should make it so all notes dissapear, hand is upset and little animation happens for like 1.5 seconds ish
    void gameOver() {
        game_over = true;
        handAnim.SetBool("gameOver", true);
        for (int i = 0; i < allNotes.Count; i++) {
            Destroy(allNotes[i]);
        }
        allNotes.Clear();
        // CHANGE THIS TO FALL FROM THE TOP
        GameObject.Find("FailBg").GetComponent<SpriteRenderer>().enabled = true;

        handAnim.Play("fail_anim");
        paperAnim.Play("failure");
        gameOverSfx.PlayOneShot(gameOverSfx.clip, 1f);
        music.Stop();
        StartCoroutine("Lose");
    }
    IEnumerator Lose() {
        yield return new WaitForSeconds(2f);
        controller.LoseGame();
    }

    void gameWin() {
        game_over = true;
        for (int i = 0; i < allNotes.Count; i++) {
            Destroy(allNotes[i]);
        }
        allNotes.Clear();
        // play animation where hands move apart
        GameObject.Find("Hands").GetComponent<SpriteRenderer>().enabled = false;
        GameObject handL = GameObject.Find("EndHands").transform.GetChild(0).gameObject;
        GameObject handR = GameObject.Find("EndHands").transform.GetChild(1).gameObject;
        handL.GetComponent<SpriteRenderer>().enabled = true;
        handR.GetComponent<SpriteRenderer>().enabled = true;

        //LeanTween.moveX(hands, hands.transform.position.x + 0.3f, 0.3f).setEaseShake();
        LeanTween.moveX(handL, handL.transform.position.x - 3f, 0.5f);
        LeanTween.moveX(handR, handR.transform.position.x + 3f, 0.5f);

        paperAnim.Play($"win_{difficulty}");
        winSfx.PlayOneShot(winSfx.clip, 1f);
        if (increment_difficulty) {
            update_difficulty = (update_difficulty % 3) + 1;
        }
        StartCoroutine("Win");
    }
    IEnumerator Win() {
        yield return new WaitForSeconds(2.2f);
        controller.WinGame();
    }
}
