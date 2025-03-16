using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAudio : MonoBehaviour
{
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying)
            Destroy(gameObject);
    }
}
