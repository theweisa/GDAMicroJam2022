using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEmitter : MonoBehaviour
{
    public GameObject blueNote;
    public GameObject redNote;
    public GameObject greenNote;
    public GameObject yellowNote;

    // Declared dictionary here so it can be used in a function
    public IDictionary<int, GameObject> numberNotes = new Dictionary<int, GameObject>();

    void EmitNote(float x, float y, float xV)
    {
        GameObject emittingNote = numberNotes[Random.Range(0, 4)];
        GameObject EmittedNote = (GameObject)Instantiate(emittingNote, new Vector3(x, y, 0), Quaternion.identity);
        Rigidbody2D rb = EmittedNote.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(xV, 0, 0);
    }

    IEnumerator SpitNotes()
    {
        float xEmit = 9f;
        float yEmit = -3f;
        float xVel = -2f;
        for(;;)
        {
            EmitNote(xEmit, yEmit, xVel);
            yield return new WaitForSeconds(1f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        blueNote = GameObject.Find("BlueNote");
        redNote = GameObject.Find("RedNote");
        greenNote = GameObject.Find("GreenNote");
        yellowNote = GameObject.Find("YellowNote");
        numberNotes.Add(0, blueNote);
        numberNotes.Add(1, redNote);
        numberNotes.Add(2, greenNote);
        numberNotes.Add(3, yellowNote);
        StartCoroutine("SpitNotes");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
