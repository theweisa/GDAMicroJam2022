// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// // detector = circle that pixels will hit
// // note = note that will reach circle
// // Use transform/vector3 to check distance b/n x positions
// // Make max distance allowed
// // Figure out how notes will work
// // 

// public class pixelDetector : MonoBehaviour
// {


//     // Start is called before the first frame update
//     void Start()
//     {
//         bool success = false;
//         float maxDistance = 2.0F;
//         GameObject emitter = GameObject.Find("NoteEmitter");
//         List<GameObject> notes = emitter.GetComponent<NoteEmitter>().allNotes;
//         GameObject detector = GameObject.Find("NotePress");
//         GameObject note = GameObject.Find("Note");
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (Input.GetKeyDown("space") || Input.GetKeyDown("left") ||
//             Input.GetKeyDown("down") || Input.GetKeyDown("right") || Input.GetKeyDown("up"))
//         {
//             print(allNotes[0]);
//         }
//     }
// }
