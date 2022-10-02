using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// detector = circle that pixels will hit
// note = note that will reach circle
// Use transform/vector3 to check distance b/n x positions
// Make max distance allowed
// Figure out how notes will work
// 

public class pixelDetector : MonoBehaviour
{
    // Start is called before the first frame update
    bool Start()
    {
        bool success = false;
        float maxDistance = 2.0F;
        GameObject detector = GameObject.Find("NotePress");
        GameObject note = GameObject.Find("Note");
        Debug.Log(Mathf.Abs(detector.transform.position.x - note.transform.position.x));
        if (Mathf.Abs(detector.transform.position.x - note.transform.position.x) < maxDistance)
        {
            Debug.Log("SUCCESS");
            success = true;
            return success;
        }
        Debug.Log("FAILURE");
        success = false;
        return success;
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
