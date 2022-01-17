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
    public GameObject[] CheckPoints;
    public int Current;
    // Start is called before the first frame update
    void Start()
    {
        Current = 0;
        if (!instance)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void revive()
    {
        Player.transform.position = CheckPoints[Current].transform.position;
    }

}
