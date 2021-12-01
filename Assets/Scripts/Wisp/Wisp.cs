using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public float speed;
    private float startTime;
    private float journeyLength;
    public Rigidbody attackBall;
    private int counter = 0;

    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }

    void Update()
    {
        WispMovement();
    }

    public void WispMovement()
    {
        //if (Vector3.Distance(wayPoints[current].transform.position, transform.position) < wPRadius)
        //{
        //current++;
        //if (current >= wayPoints.Length)
        //{
        //Destroy(gameObject);
        //}  
        //}

        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);

        Rigidbody clone;
        if (counter < 2)
        {
            clone = Instantiate(attackBall,startMarker.transform.position,startMarker.transform.rotation);
            clone.AddForce(Vector3.forward * 10);
            counter++;
        }
    }
}

