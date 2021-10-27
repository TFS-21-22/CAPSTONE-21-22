using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour
{
    public GameObject[] wayPoints = new GameObject[2];
    public int current = 0;
    public float speed;
    public float wPRadius;

    void Start()
    {
        
    }

    void Update()
    {
        WispMovement();
    }

    public void WispMovement()
    {
       if (Vector3.Distance(wayPoints[current].transform.position, transform.position) < wPRadius)
       {
            current++;
            if (current >= wayPoints.Length)
            {
                current = 0;
            }  
       }
       transform.position = Vector3.MoveTowards(transform.position, wayPoints[current].transform.position, Time.deltaTime * speed);
    }
}

