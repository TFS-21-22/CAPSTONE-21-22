using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BeatMaster : MonoBehaviour
{
    public AudioSource source;
    [Range(0.01f, 0.2f)] public float beatGate = 0.1f;
    public float BPM = 0f;
    public int currentBeat = -1;
    public int timeSignature = 4;
    public float beatFeel = 0.1f;
    public static int temp;
    public static float beatRealTime;
    public static int beatCount = 0;


    private void Start()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        //broadcast BPS of track...
        BPS?.Invoke(BPM / 60f);

        this.Invoke("PlayThing", 1);

        Debug.Log("Source.Time: " + source.time);
        Debug.Log("Source.Clip.Length: " + source.clip.length);
        Debug.Log("BPM: " + BPM);
        Debug.Log("BPS: " + BPM / 60f);
    }

    public static event Action<float> BPS;
    //broadcast beat
    public static event Action<int> Beat;


    public void PlayThing() => source.Play();

    // Update is called once per frame
    void Update()
    {
        //how long by total time 2/60
        //float temp = Mathf.Ceil(((source.time / source.clip.length) * (source.clip.length * (BPM / 60f))) % 4);
        temp = (int)Mathf.Ceil(((source.time - beatFeel) * (BPM / 60f)) % timeSignature);

        beatRealTime = (((source.time - beatFeel) * (BPM / 60f)) % timeSignature);
      
      
        if(currentBeat != temp)
        {
            currentBeat = temp;
            Beat?.Invoke(currentBeat);
            beatCount++;
        }
    }    
}
