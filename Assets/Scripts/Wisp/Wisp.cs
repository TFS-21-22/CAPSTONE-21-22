using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour
{
    public GameObject[] wayPoints;
    public int current = 0;
    public float speed;
    public float wPRadius;
    public Rigidbody attackBall;
    private int counter = 0;

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
                Destroy(gameObject);
            }  
       }

       transform.position = Vector3.MoveTowards(transform.position, wayPoints[current].transform.position, Time.deltaTime * speed);

        Rigidbody clone;
        if (counter < 2)
        {
            clone = Instantiate(attackBall,wayPoints[current].transform.position,wayPoints[current].transform.rotation);
            clone.AddForce(Vector3.forward * 10);
            counter++;
        }
    }
}

