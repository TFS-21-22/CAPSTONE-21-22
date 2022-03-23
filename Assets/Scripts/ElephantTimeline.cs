using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class ElephantTimeline : MonoBehaviour
{
    public PlayableDirector playableDirector;

    // Start is called before the first frame update
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();

        float CPBeatOne = 0;
        float CPBeatTwo = 45;
        float CPBeatThree = 278;

        if (CPManager.instance.checkPoint == 0)
        {
            playableDirector.time = CPBeatOne;
        }
        if (CPManager.instance.checkPoint == 1)
        {
            playableDirector.time = CPBeatTwo;
        }
        if (CPManager.instance.checkPoint == 2)
        {
            playableDirector.time = CPBeatThree;
        }
    }
}
