using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldTween : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(1.85f*5f, 1.85f*5f, 0f);
        LeanTween.scale(gameObject, new Vector3(1.85f, 1.85f, 0f), 0.2f).setDelay(0.1f);
        LeanTween.alpha(gameObject, 0f, 0.2f).setOnComplete(destroyFold).setDelay(1f);
    }

    void destroyFold()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
