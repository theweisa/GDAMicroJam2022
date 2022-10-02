using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NoteEmitter : MonoBehaviour
{
    // Declared the notes and dictionary here so they can be used in functions
    public MicrogameJamController controller;
    public GameObject blueNote;
    public GameObject redNote;
    public GameObject greenNote;
    public GameObject yellowNote;
    public IDictionary<int, GameObject> numberNotes = new Dictionary<int, GameObject>();

    // Set milliseconds before each note (needs to be read in from map)
    public List<float> timeBeforeNotes = new List<float>();

    // List of notes to be used by the note detector
    public List<GameObject> allNotes = new List<GameObject>();

    // Takes in note spawn location and speed as arguments
    void EmitNote(float x, float y, float xV)
    {
        // Chooses a random note out of the four to use
        GameObject emittingNote = numberNotes[UnityEngine.Random.Range(0, 4)];
        
        // Instantiates the note at the given spawn location with no rotation
        GameObject emittedNote = (GameObject)Instantiate(emittingNote, new Vector3(x, y, 0), Quaternion.identity);

        allNotes.Add(emittedNote);

        // Give the note the specified speed
        Rigidbody2D rb = emittedNote.GetComponent<Rigidbody2D>();
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
        // float noteRate = 1f;

        // Sets time for note to get from emitter to detector (in seconds)
        float noteTime = 2f;

        // Calculates necessary note velocity
        float xDist = xDetect - xEmit;
        float xVel = xDist / noteTime;

        int numOfNotes = timeBeforeNotes.Count;

        // Calls the note emitter every x second(s)
        for(int i=0; i<numOfNotes; i++)
        {
            // yield return new WaitForSeconds(noteRate);
            print(timeBeforeNotes[i]);
            yield return new WaitForSeconds(timeBeforeNotes[i]/1000f);
            EmitNote(xEmit, yEmit, xVel);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Jam Controller").GetComponent<MicrogameJamController>();

        // int difficulty = controller.GetDifficulty();
        int difficulty = 3;

        StreamReader reader = File.OpenText("Assets/Imports/TextFiles/chart.csv");
        float addedMilliseconds = 0f;
        string line = reader.ReadLine();
        while ((line = reader.ReadLine()) != null)
        {
            string[] words = line.Split(',');
            int noteDifficulty = Int32.Parse(words[1]);
            if (noteDifficulty <= difficulty)
            {
                float millisecondCount = float.Parse(words[0]);
                timeBeforeNotes.Add(millisecondCount + addedMilliseconds);
                addedMilliseconds = 0f;
            }
            else {
                addedMilliseconds += float.Parse(words[0]);
            }
        }

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

    }
}
