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
        if (CPManager.instance.checkPoint == 0)
        {
            playableDirector.time = 0;
        }
        if (CPManager.instance.checkPoint == 1)
        {
            playableDirector.time = 45;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
