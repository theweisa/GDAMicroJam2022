using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEmitter : MonoBehaviour
{
    // Declared the notes and dictionary here so they can be used in functions
    public GameObject blueNote;   // Up
    public GameObject redNote;    // Down
    public GameObject greenNote;  // Left
    public GameObject yellowNote; // Right
    public IDictionary<int, GameObject> numberNotes = new Dictionary<int, GameObject>();

    // List of notes to be used by the note detector
    public List<GameObject> allNotes = new List<GameObject>();
  
    // Declares note Detector
    public GameObject detector;
    // distance for input to be valid
    public const float validInputDistance = 3f;
    // distance for input to be too early or late
    public const float offDistance = 1f;

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
    bool successCheck()
    {
        return true;
        //if (allNotes[0] blue)
    }

    // Takes in note spawn location and speed as arguments
    void EmitNote(float x, float y, float xV)
    {
        // Chooses a random note out of the four to use
        GameObject emittingNote = numberNotes[Random.Range(0, 4)];
        
        // Instantiates the note at the given spawn location with no rotation
        GameObject EmittedNote = (GameObject)Instantiate(emittingNote, new Vector3(x, y, 0), Quaternion.identity);

        allNotes.Add(EmittedNote);

        // Give the note the specified speed
        Rigidbody2D rb = EmittedNote.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(xV, 0, 0);
    }

    IEnumerator SpitNotes()
    {
        // Set Note Spawn Location
        float xEmit = 9f;
        float yEmit = -3f;

        // Set Note Detector Location
        float xDetect = -2f;
        // float yDetect = -3f;

        // Set rate of note emitting
        float noteRate = 1f;

        // Sets time for note to get from emitter to detector)
        float noteTime = 5f;

        // Calculates necessary note velocity
        float xDist = xDetect - xEmit;
        float xVel = xDist / noteTime;

        // Calls the note emitter every x second(s)
        for(;;)
        {
            EmitNote(xEmit, yEmit, xVel);
            yield return new WaitForSeconds(noteRate);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Gets note Detector
        detector = GameObject.Find("NotePress");

        // Find and add all of the types of notes to a dictionary for randomization purposes
        blueNote = GameObject.Find("BlueNote");
        redNote = GameObject.Find("RedNote");
        greenNote = GameObject.Find("GreenNote");
        yellowNote = GameObject.Find("YellowNote");
        numberNotes.Add(0, blueNote);
        numberNotes.Add(1, redNote);
        numberNotes.Add(2, greenNote);
        numberNotes.Add(3, yellowNote);

        // Actually starts the note emitting
        StartCoroutine("SpitNotes");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left") || Input.GetKeyDown("down") || 
            Input.GetKeyDown("right") || Input.GetKeyDown("up"))
        {
            //Debug.Log(Mathf.Abs(detector.transform.position.x - allNotes[0].transform.position.x));
            if (isValidInput()) {
                if (earlyCheck())
                {
                    Debug.Log("TOO EARLY, LOSE");
                    // Lose function
                    return;
                }
                if (lateCheck())
                {
                    Debug.Log("TOO LATE, LOSE");
                    // Lose function
                    return;
                }
                if (!successCheck())
                {
                    Debug.Log("WRONG BUTTON, LOSE");
                    // Lose function
                    return;
                }
                else
                {
                    Debug.Log("SUCCESS!!!");
                    // Delete note here?
                    return;
                }

            }
        }
    }
}
