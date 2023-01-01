using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource snd;

    public float DefaultPitch = 2.6f;

    Vector3 past;
    float DeltaPosition;
    // Start is called before the first frame update
    void Start()
    {
        snd = GetComponent<AudioSource>();
        past = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DeltaPosition = Vector3.Distance(transform.position, past);
        snd.pitch = Mathf.Clamp(DefaultPitch + DeltaPosition, 0, 3);
        //snd.pitch = Mathf.Clamp(DefaultPitch + Mathf.Abs((Input.GetAxis("Horizontal")) * 0.4f + Mathf.Abs(Input.GetAxis("Vertical")) * 0.4f + Mathf.Abs(Input.GetAxis("Fly")) * 0.4f), 0, 3);
        past = transform.position;
    }
}
