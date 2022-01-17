using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public int life;
    public int maxlife = 3;
    CheckPointManager CPM;
    // Start is called before the first frame update
    void Start()
    {
        if (life < 3)
        {
            life = 3;
        }
        if (!CPM)
        {
            CPM = CheckPointManager.instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "obstacle")
        {
            life--;
            if (life < 1)
            {
                if (CPM)
                {
                    CPM.revive();
                }
            }
        }
    }
}
