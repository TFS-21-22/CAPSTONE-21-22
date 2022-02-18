using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BeatMaster : MonoBehaviour
{
    public static BeatMaster instance;
    public AudioSource source;
    [Range(0.01f, 0.2f)] public float beatGate = 0.1f;
    public float BPM = 0f;
    public int currentBeat = -1;
    public int timeSignature = 4;
    public float beatFeel = 0.1f;
    public static int temp;
    public static float beatRealTime;
    public int beatCount = 0;

    public ScoreSystem scoreSystem;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    public void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        //broadcast BPS of track...
        BPS?.Invoke(BPM / 60f);

        this.Invoke("PlayThing", 1);

       if (CPManager.instance)
       {
            if (CPManager.instance.checkPoint == 0)
            {
                scoreSystem.score = CPManager.instance.scoreSaved;
                collectLoad();
                beatCount = 0;
                source.time = 0;               
            }
            if (CPManager.instance.checkPoint == 1)
            {
                scoreSystem.score = CPManager.instance.scoreSaved;
                collectLoad();
                beatCount = 100;
                source.time = 46f;
            }
            if (CPManager.instance.checkPoint == 2)
            {
                scoreSystem.score = CPManager.instance.scoreSaved;
                collectLoad();
                beatCount = 278;
                source.time = 129f;
            }
        }
        
        
        

        //Debug.Log("Source.Time: " + source.time);
        //Debug.Log("Source.Clip.Length: " + source.clip.length);
        //Debug.Log("BPM: " + BPM);
        //Debug.Log("BPS: " + BPM / 60f);
    }

    public static event Action<float> BPS;
    //broadcast beat
    public static event Action<int> Beat;



    public void PlayThing() => source.Play();

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(beatCount);
        scoreSystem = FindObjectOfType<ScoreSystem>();
        //the source audio starts immediately on a beat, which means we can use the beats per minute to get the beats per second and increment the beats
        //based on the source time.
        temp = (int)Mathf.Ceil(((source.time - beatFeel) * (BPM / 60f)) % timeSignature);

        beatRealTime = (((source.time - beatFeel) * (BPM / 60f)) % timeSignature);
      
        if(currentBeat != temp)
        {
            currentBeat = temp;
            Beat?.Invoke(currentBeat);
            beatCount++;
        }

        if (beatCount >= 0 && CPManager.instance.checkPoint < 0)
        {
            CPManager.instance.checkPoint = 0;
            CPManager.instance.scoreSaved = scoreSystem.score;
            collectSave();
        }
        if (beatCount >= 100 && CPManager.instance.checkPoint < 1)
        {
            CPManager.instance.checkPoint = 1;
            CPManager.instance.scoreSaved = scoreSystem.score;
            collectSave();
        }
        if (beatCount >= 278 && CPManager.instance.checkPoint < 2)
        {
            CPManager.instance.checkPoint = 2;
            CPManager.instance.scoreSaved = scoreSystem.score;
            collectSave();
        }
    }

    public void collectSave()
    {
        print("SAVING");
        ///TRUE
        if (CPManager.instance.collectables[0] == true)
        {
            CPManager.instance.collectablesSaved[0] = true;
        }
        if (CPManager.instance.collectables[1] == true)
        {
            CPManager.instance.collectablesSaved[1] = true;
        }
        if (CPManager.instance.collectables[2] == true)
        {
            CPManager.instance.collectablesSaved[2] = true;
        }
        if (CPManager.instance.collectables[3] == true)
        {
            CPManager.instance.collectablesSaved[3] = true;
        }
        if (CPManager.instance.collectables[4] == true)
        {
            CPManager.instance.collectablesSaved[4] = true;
        }
        if (CPManager.instance.collectables[5] == true)
        {
            CPManager.instance.collectablesSaved[5] = true;
        }
    }

    public void collectLoad()
    {
        print("LOADING");
        ///TRUE
        if (CPManager.instance.collectablesSaved[0] == true)
        {
            CPManager.instance.collectables[0] = true;
        }
        if (CPManager.instance.collectablesSaved[1] == true)
        {
            CPManager.instance.collectables[1] = true;
        }
        if (CPManager.instance.collectablesSaved[2] == true)
        {
            CPManager.instance.collectables[2] = true;
        }
        if (CPManager.instance.collectablesSaved[3] == true)
        {
            CPManager.instance.collectables[3] = true;
        }
        if (CPManager.instance.collectablesSaved[4] == true)
        {
            CPManager.instance.collectables[4] = true;
        }
        if (CPManager.instance.collectablesSaved[5] == true)
        {
            CPManager.instance.collectables[5] = true;
        }

        ///FALSE
        if (CPManager.instance.collectablesSaved[0] == false)
        {
            CPManager.instance.collectables[0] = false;
        }
        if (CPManager.instance.collectablesSaved[1] == false)
        {
            CPManager.instance.collectables[1] = false;
        }
        if (CPManager.instance.collectablesSaved[2] == false)
        {
            CPManager.instance.collectables[2] = false;
        }
        if (CPManager.instance.collectablesSaved[3] == false)
        {
            CPManager.instance.collectables[3] = false;
        }
        if (CPManager.instance.collectablesSaved[4] == false)
        {
            CPManager.instance.collectables[4] = false;
        }
        if (CPManager.instance.collectablesSaved[5] == false)
        {
            CPManager.instance.collectables[5] = false;
        }
    }
}
