using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Strafe : MonoBehaviour
{
    [SerializeField] private Image xCricle;
    [SerializeField] private Image xButton;
    [SerializeField] private GameObject tiger;

    Vector2 input;

    public float boundx = 2.25f;
    public float speed = 3.0f;
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

        if (BeatMaster.beatCount == 8 && !enemySequence)
        {
            //TigerSequence();
        }

        if (BeatMaster.beatCount == 52 && !enemySequence)
        {
            
        }

        if (BeatMaster.beatCount == 74 && !enemySequence)
        {
            
        }

        if (!enemy.activeSelf)
            enemySequence = false;

        if (!tiger.activeSelf)
            bossSequence = false;

        
        //Movement
        float h = Input.GetAxis("Horizontal") * speed;
        transform.Translate(h * Time.deltaTime, 0, 0);
    }

    private void EnemySequence()
    {
        enemy.SetActive(true);                                                  //Set enemy true
        rhythmCanvas.gameObject.SetActive(true);                                //Set Rythm Cavas Active
        camera.cameraPosition = SmoothCameraScript.ECameraPosition.OffsetRight; //Camera Movement
        camera.StartCoroutine(camera.CameraSwitch(3));                          //Camera Switch
        enemySequence = true;                                                   //Enemy sqeuence true
    }

    public void BossButtonSeuqence()
    {
        rhythmCanvas.gameObject.SetActive(true);                                //Set Button Squence Active
        camera.cameraPosition = SmoothCameraScript.ECameraPosition.OffsetLeft;  //Camera Movement
        //camera.StartCoroutine(camera.CameraSwitch(3));                        //Camera Switch      //CAUSES CAMERA JITTER, what is the point of this if we already have tiger sequence?
        bossSequence = true;                                                   //Set sqeuence true
    }
    void TigerSequence()
    {
        Tiger.instance.gameObject.SetActive(true);
        camera.cameraPosition = SmoothCameraScript.ECameraPosition.OffsetLeft;
        Tiger.instance.BossState = Tiger.CurrentState.Move;
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

    

    
}