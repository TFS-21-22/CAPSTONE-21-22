using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp2 : MonoBehaviour
{
    public Transform startMarker;
    public Transform qtePos;
    public bool active;
    public bool attacking;
    public bool wispSequence;

    public RhythmCanvas rhythmCanvas;
    public SmoothCameraScript camera;

    // Movement speed in units per second.
    public float speed;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;


    // Start is called before the first frame update
    void Start()
    {
        startMarker = gameObject.transform;
        active = false;
        attacking = false;
        speed = 0.3F;
        //qtePos.position += new Vector3(0, 1, -1);
    }

    // Update is called once per frame
    void Update()
    {
        journeyLength = Vector3.Distance(transform.position, qtePos.position);

        float dist = Vector3.Distance(qtePos.position, transform.position);

        if (active == true)
        {
            startTime += Time.deltaTime;
            //transform.position = qtePos.position + new Vector3(0,1,-1);

            float distCovered = (Time.time - startTime) * speed;

            float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(startMarker.position, qtePos.position, fractionOfJourney);


            if (dist <= 0.5)
            {

                speed = 1F;
                attacking = true;
                WispButtonSequence();
                //Destroy(gameObject);
            }
        }
    }

    public void WispButtonSequence()
    {
        rhythmCanvas.gameObject.SetActive(true);                                //Set Button Squence Active
        rhythmCanvas.wisp = gameObject;
        camera.cameraPosition = SmoothCameraScript.ECameraPosition.OffsetLeft;  //Camera Movement
        camera.StartCoroutine(camera.CameraSwitch(3));                        //Camera Switch      //CAUSES CAMERA JITTER, what is the point of this if we already have tiger sequence?
        wispSequence = true;                                                 //Set sqeuence true
        rhythmCanvas.currentEnemyQTE = this.gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            active = true;
        }
    }
}
