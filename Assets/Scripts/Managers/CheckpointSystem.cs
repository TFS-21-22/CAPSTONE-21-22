using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    public Transform pathFollowerTransform;
    public static CheckpointSystem instance;
    public Vector3 currentCheckpoint;
    private Vector3 pathFollowerPosition;
    public AudioSource mainSong;
    private float audioCheckPointTime = 0;

    int currentLives = 3;
    int maxLives = 3;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance != null)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        pathFollowerPosition = pathFollowerTransform.position;

        if(currentLives == 0)
        {
            PlayerCheckpoint();
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            //Store follower position
            currentCheckpoint = pathFollowerPosition;
            //Store audio time at checkpoint hit time
            audioCheckPointTime = mainSong.time;
        }
    }

    public void PlayerCheckpoint()
    {
        //Reset song
        mainSong.time = audioCheckPointTime;
        //Reset lives
        currentLives = maxLives;
        //Move player to checkpoint
    }
}
