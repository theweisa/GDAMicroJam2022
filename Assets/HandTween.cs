using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTween : MonoBehaviour
{

    // The amount of time between two tweens starting
    public float delayTime;

    // The amount of time one full tween takes
    public float tweenDuration;

    // The peak y value the tween goes up to
    public float tweenAmount;

    public Vector3 tweenEndLocation;
    public GameObject NoteEmitter;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Tween());
    }

    IEnumerator Tween()
    {
        // Wait until the song starts
        float time = GameObject.Find("NoteEmitter").GetComponent<NoteEmitter>().noteTime;
        // yield return new WaitForSeconds(time);

        // Run this infinitely
        for (;;)
        {

            // Prep for moving the hands up
            float timeElapsed = 0;
            Vector3 tweenStartLocation = transform.position;
            tweenEndLocation = new Vector3(transform.position.x, transform.position.y + tweenAmount, 0f);

            // Move the hands up
            while (timeElapsed < tweenDuration)
            {
                transform.position = Vector3.Lerp(tweenStartLocation, tweenEndLocation, timeElapsed / tweenDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = tweenEndLocation;

            // Prep for moving the hands down
            timeElapsed = 0;
            tweenStartLocation = transform.position;
            tweenEndLocation = new Vector3(transform.position.x, transform.position.y - tweenAmount, 0f);

            // Move the hands down
            while (timeElapsed < tweenDuration)
            {
                transform.position = Vector3.Lerp(tweenStartLocation, tweenEndLocation, timeElapsed / tweenDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = tweenEndLocation;

            // Takes into account how long the tween takes to determine how much time
            // should be waited for between one tween ending and the next one starting
            yield return new WaitForSeconds(delayTime - tweenDuration);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
