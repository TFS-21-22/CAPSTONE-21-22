using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour
{
    GameObject[] startSpawnGameobjects = new GameObject[3];
    GameObject[] endSpawnGameobjects = new GameObject[3];

    // Transforms to act as start and end markers for the journey.
    public Transform[] startMarker = new Transform[3];
    public Transform[] endMarker = new Transform[3];

    // Movement speed in units per second.
    public float speed = 1F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    //Random numbers
    public int startMarkerInt;
    public int endMarkerInt;

    void Start()
    {
        startSpawnGameobjects = GameObject.FindGameObjectsWithTag("StartTransform");
        endSpawnGameobjects = GameObject.FindGameObjectsWithTag("EndTransform");

        for (int i = 0; i < 3; i++)
        {
            startMarker[i] = startSpawnGameobjects[i].transform;
        }

        for (int i = 0; i < 3; i++)
        {
            endMarker[i] = endSpawnGameobjects[i].transform;
        }

        int randomStart = Random.Range(0, startMarker.Length);
        int randomEnd = Random.Range(0, endMarker.Length);

        startMarkerInt = randomStart;
        endMarkerInt = randomEnd;
        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker[randomStart].position, endMarker[randomEnd].position);
    }

    // Move to the target end position.
    void Update()
    {
        WispMovement();
    }

    public void WispMovement()
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed * 3;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startMarker[startMarkerInt].position, endMarker[endMarkerInt].position, fractionOfJourney);
    }
}

