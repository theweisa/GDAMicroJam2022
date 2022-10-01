using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEmitter : MonoBehaviour
{
    public GameObject note;


    void EmitNote(float x, float y, float xV)
    {
        // Debug.Log("X0: " + x + ";Y1: " + y);
        GameObject EmittedNote = (GameObject)Instantiate(note, new Vector3(x, y, 0), Quaternion.identity);
        // Debug.Log("X1: " + EmittedNote.transform.position.x + ";Y1: " + EmittedNote.transform.position.y);
        Rigidbody2D rb = EmittedNote.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(xV, 0, 0);
        Debug.Log("Velocity: " + rb.velocity + "Wanted Velocity: " + xV);
        // Debug.Log("X2: " + EmittedNote.transform.position.x + ";Y2: " + EmittedNote.transform.position.y);
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
        note = GameObject.Find("Note");
        StartCoroutine("SpitNotes");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
