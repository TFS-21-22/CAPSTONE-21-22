using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Strafe : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] private Image xCricle;
    [SerializeField] private Image xButton;
    [SerializeField] private GameObject tiger;
    [SerializeField] private Transform tigerSpawnPos;

    //Audio
    [Header("Audio")]
    //Forest Ambience
    [SerializeField] private AudioSource forestAmbienceSource;
    //Fire Ambience
    [SerializeField] private AudioSource fireAmbienceSource;
    //Lily pad audio
    [SerializeField] private AudioSource lilyImpactAudioSource;
    [SerializeField] private AudioClip lilyImpactAudioClip;
    //Impact Audio (Obstacles)
    [SerializeField] private AudioSource waterImpactAudioSource;



    //VFX
    [Header("VFX")]
    [SerializeField] private VisualEffect obstacleCollisionParticle;

    //Vectors
    [Header("Vectors")]
    Vector3 camPos;
    Vector2 input;
    public Vector3 jump;

    [Header("Floats")]
    public float boundx = 2.25f;
    public float speed = 3.0f;
    public float h;
    public float jumpForce = 2.0f;

    [Header("Variables")]
    public Transform follow;
    public GameObject enemy;
    public GameObject fireFly;
    public Canvas rhythmCanvas;

    //bools
    [Header("Bool")]
    public bool activeQTE = false;
    public bool bossSequence = false;
    public bool stopperL;
    public bool stopperR;
    public bool canHurt = true;
    public bool isGrounded;

    public SmoothCameraScript camera;

    private Rigidbody rb;

    //Scripts
    public ScoreSystem scoresystem;
    public GameObject quickTimeEvent;
    public HUD hud;

    public ResultsScreen resultsScreen;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        canHurt = true;
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        //fireFly.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        QuickTimeEvent();

        //Movement
        Movement();
        Jump();
        Duck();
       
        //Audio Source
        if(scoresystem && scoresystem.transitionEnding)
        {
            //waterImpactAudioSource.Play();
        }
    }

    public void TigerButtonSequence()
    {
        rhythmCanvas.gameObject.SetActive(true);                                //Set Button Squence Active
        camera.cameraPosition = SmoothCameraScript.ECameraPosition.OffsetLeft;  //Camera Movement
        //camera.StartCoroutine(camera.CameraSwitch(3));                        //Camera Switch      //CAUSES CAMERA JITTER, what is the point of this if we already have tiger sequence?
        bossSequence = true;                                                   //Set sqeuence true
        //RhythmCanvas.instance.pulsing = true;
        RhythmCanvas.instance.currentEnemyQTE = tiger.gameObject;
    }

    void TigerEnable()
    {
        print("TIGER CALLED");
        tiger.SetActive(true);
        camera.cameraPosition = SmoothCameraScript.ECameraPosition.OffsetLeft;
        camera.StartCoroutine(camera.CameraSwitch(3));
        activeQTE = true;
    }

    
    IEnumerator Collision(float waitTime)
    {
        if(canHurt)
        {
            GameManager.instance.health--;
            canHurt = false;
           if(GameManager.instance.health <= 0)
            {
                anim.SetTrigger("Death");
            }
        }
        else
        {
            yield return new WaitForSeconds(waitTime);
            canHurt = true;
        }
    }

    private void Movement()
    {
        var leftInput = Input.GetKey(KeyCode.A);
        var rightInput = Input.GetKey(KeyCode.D);

        Vector3 leftForce = -Vector3.right * Time.deltaTime * speed;
        Vector3 rightForce = Vector3.right * Time.deltaTime * speed;

        if (leftForce.x < 0 && stopperL)
        {
            leftForce = Vector3.zero; 
        }
        else
        {
            leftForce = -Vector3.right * Time.deltaTime * speed;
        }

        if(rightForce.x > 0 && stopperR)
        {
            rightForce = Vector3.zero;
        }
        else if(!stopperR)
        {
            rightForce = Vector3.right * Time.deltaTime * speed;
        }

        if (leftInput)
        {
            transform.Translate(leftForce);
            anim.SetFloat("Direction", -1);
        }
        if(!leftInput && !rightInput)
        {
            print("Direction");
            anim.SetFloat("Direction", 0);
        }

        if (rightInput)
        {
            transform.Translate(rightForce);
            anim.SetFloat("Direction", 1);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetBool("Jump", true);
            isGrounded = false;
        }
    }

    private void ResetJump()
    {
        if(!isGrounded)
        {
            anim.SetBool("Jump", false);
            isGrounded = true;
        }
    }

  

    private void Duck()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetTrigger("Duck");
            anim.SetBool("Ducking", false);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            anim.ResetTrigger("Duck");
            anim.SetBool("Ducking", true);
        }
    }

    private void QuickTimeEvent()
    {
        float beatCount = BeatMaster.instance.beatCount;
        if(beatCount == 10)
        {
            quickTimeEvent.SetActive(true);
        }

        if (beatCount == 30)
        {
            quickTimeEvent.SetActive(true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("TigerProjectile"))
        {
            GameManager.instance.health--;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //AUDIO
        if (other.gameObject.CompareTag("ForestAmbience"))
        {
            forestAmbienceSource.Play();
        }

        if (other.gameObject.CompareTag("ForestAmbience"))
        {
            fireAmbienceSource.Play();
        }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            waterImpactAudioSource.Play();
            other.gameObject.SetActive(false);
            anim.SetTrigger("High Collision");
            anim.SetTrigger("Low Collision");
        }

        if (other.gameObject.CompareTag("Lily"))
        {
            //lilyImpactAudioSource.PlayOneShot(lilyImpactAudioClip);
            other.gameObject.SetActive(false);
        }



        if (other.gameObject.CompareTag("Collectable"))
        {
            for (int i = 0; i < 5; i++)
            {
                if (other.gameObject.name == "Col_" + (i + 1).ToString())
                    CPManager.instance.collectables[i] = true;

                hud.CollectGet(other.name);
            }
            other.gameObject.SetActive(false);

        }

        if (other.gameObject.CompareTag("Log"))
        {
            StartCoroutine(Collision(2.0f));
            //logCollisionSFX.Play();
            anim.SetTrigger("High Collision");
            anim.SetTrigger("Low Collision");
        }

        if (other.gameObject.CompareTag("Tiger"))
        {
            GameManager.instance.health--;
        }

        if (other.gameObject.CompareTag("End"))
        {
            if (resultsScreen)
            {
                resultsScreen.endHit = true;
            }
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

        if (other.gameObject.CompareTag("Log"))
        {
            if (!camera.hit)
                camPos = Camera.main.transform.localPosition;

            obstacleCollisionParticle.Play();
            camera.hit = true;
            camera.InduceStress(1);
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

        if (other.gameObject.CompareTag("Log") || other.gameObject.CompareTag("Obstacle"))
        {
            camera.hit = false;
            camera.InduceStress(0);
            // Debug.Log("hit");
            Camera.main.transform.localPosition = camPos;
            anim.ResetTrigger("High Collision");
            anim.ResetTrigger("Low Collision");
        }

        if (other.gameObject.CompareTag("Heart"))
        {
            if (GameManager.instance.health < 3)
                GameManager.instance.health++;

            if (GameManager.instance.health >= 3)
            {
                GameManager.instance.health = 3;
            }
        }
    }



}