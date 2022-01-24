using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    static CheckPointManager _instance = null;
    public static CheckPointManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
    public GameObject Player;
    public GameObject[] Checkpoint;
    public int current;

    // Start is called before the first frame update
    void Start()
    {
        current = 0;
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void revive()
    {
        if (Player)
        {
            if (Checkpoint[current])
            {
                Player.transform.position = Checkpoint[current].transform.position;
            }
        }
    }
}
