using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strafe : MonoBehaviour
{
    public float boundx = 2.25f;
    public float speed = 3.0f;
    public Transform follow;
    public GameObject enemy;
    public Canvas rhythmCanvas;

    public SmoothCameraScript camera;

    private Rigidbody rb;

    //public float xMove;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //var pos = transform.position;
        //pos.y = Mathf.Clamp(follow.transform.position.y, -2.0f, 2.0f);
        //transform.position = pos;
        


        float h = Input.GetAxis("Horizontal") * speed;
        //h = Mathf.Clamp(h, -2, 2);

        transform.Translate(h * Time.deltaTime, 0, 0);

        //Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //position.x = Mathf.Clamp(position.x, -5, 5);
        //transform.position = position;

        //transform.Translate(Mathf.Clamp(h, -1, 1), 0, 0);
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(name + ": OnTriggerEnter - " + other.gameObject.name);
        if (other.gameObject.CompareTag("Obstacle"))
        {
            //Destroy(gameObject);

            Destroy(other.gameObject);
            rb.AddForce(transform.up * 8);
        }

        if (other.gameObject.CompareTag("enemyTrigger"))
        {
            enemy.SetActive(true);
            rhythmCanvas.gameObject.SetActive(true);
            camera.cameraPosition = SmoothCameraScript.ECameraPosition.OffsetRight;
            camera.StartCoroutine(camera.CameraSwitch(4));
        }

        if(other.gameObject.CompareTag("reset"))
            camera.cameraPosition = SmoothCameraScript.ECameraPosition.Normal;
        camera.StartCoroutine(camera.CameraSwitch(4));





    }
    void OnTriggerStay(Collider other)
    {
        //Debug.Log(name + ": OnTriggerStay - " + other.gameObject.name);


    }
    void OnTriggerExit(Collider other)
    {
        //Debug.Log(name + ": OnTriggerExit - " + other.gameObject.name);


    }
}