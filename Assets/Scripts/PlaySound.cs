using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{

    public GameObject NoteEmitter;

    IEnumerator PlaySong(float time)
    {
        // This needs to be the NoteTime var from NoteEmitter.cs (possible export?)
        yield return new WaitForSeconds(time);
        audioSource.Play();
    }

    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        float time = GameObject.Find("NoteEmitter").GetComponent<NoteEmitter>().noteTime;
        StartCoroutine(PlaySong(time));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
