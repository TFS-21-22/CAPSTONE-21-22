using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Strafe : MonoBehaviour
{
    [SerializeField] private Image xCricle;
    [SerializeField] private Image xButton;
    [SerializeField] private GameObject tiger;

    //Audio
    [SerializeField] private AudioSource logCollisionSFX;

    //VFX
    [SerializeField] private VisualEffect obstacleCollisionParticle;


    Vector2 input;

    public float boundx = 2.25f;
    public float speed = 3.0f;

    public bool stopperL;
    public bool stopperR;
    public float h;

    public Transform follow;
    public GameObject enemy;
    public Canvas rhythmCanvas;
    public bool enemySequence = false;
    public bool bossSequence = false;

    public SmoothCameraScript camera;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (BeatMaster.beatCount == 8 && !enemySequence && !bossSequence)
        {
            TigerSequence();
        }

        if (BeatMaster.beatCount == 52 && !enemySequence && !bossSequence)
        {

        }

        if (BeatMaster.beatCount == 74 && !enemySequence && !bossSequence)
        {

        }

        if (!enemy.activeSelf)
            enemySequence = false;

        if (!tiger.activeSelf)
            bossSequence = false;


        //Movement
        h = Input.GetAxis("Horizontal") * speed;
        //h = Mathf.Clamp(h, -2, 2);

        if (stopperL && h < 0)
        {
            h = 0;
        }
        if (stopperR && h > 0)
        {
            h = 0;
        }
       
        transform.Translate(h * Time.deltaTime, 0, 0);
    }

    private void EnemySequence()
    {
        enemy.SetActive(true);                                                  //Set enemy true
        rhythmCanvas.gameObject.SetActive(true);                                //Set Rythm Cavas Active
        camera.cameraPosition = SmoothCameraScript.ECameraPosition.OffsetRight; //Camera Movement
        //camera.StartCoroutine(camera.CameraSwitch(3));                          //Camera Switch
        enemySequence = true;                                                   //Enemy sqeuence true
    }

    public void BossButtonSeuqence()
    {
        rhythmCanvas.gameObject.SetActive(true);                                //Set Button Squence Active
        camera.cameraPosition = SmoothCameraScript.ECameraPosition.OffsetLeft;  //Camera Movement
        //camera.StartCoroutine(camera.CameraSwitch(3));                        //Camera Switch      //CAUSES CAMERA JITTER, what is the point of this if we already have tiger sequence?
        bossSequence = true;                                                   //Set sqeuence true
        //RhythmCanvas.instance.pulsing = true;
    }

    void TigerSequence()
    {
        Tiger.instance.gameObject.SetActive(true);
        camera.cameraPosition = SmoothCameraScript.ECameraPosition.OffsetLeft;
        camera.StartCoroutine(camera.CameraSwitch(3));
        enemySequence = true;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Lily"))
        {
            Destroy(other.gameObject);
            //rb.AddForce(transform.up * 8);
        }
    }
    void OnTriggerStay(Collider other)
    {
        //Debug.Log("AAAAA");
        if (other.gameObject.name == "Wall_1")
        {
            stopperL = true;
        }
        if (other.gameObject.name == "Wall_2")
        {
            stopperR = true;
        }

        if (other.gameObject.tag == "Log")
        {
            logCollisionSFX.Play();
            obstacleCollisionParticle.Play();
        }
    }
    void OnTriggerExit(Collider other)
    {
        //Debug.Log("AAAAA");
        if (other.gameObject.name == "Wall_1")
        {
            stopperL = false;
        }
        if (other.gameObject.name == "Wall_2")
        {
            stopperR = false;
        }
    }



}