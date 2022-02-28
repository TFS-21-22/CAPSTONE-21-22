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
    [SerializeField] private AudioClip logCollisionSFX;
    [SerializeField] private AudioClip transitionSFX;
    [SerializeField] private AudioClip wispSFX;
    [SerializeField] private AudioClip tigerSFX;

    [SerializeField] private AudioSource waterImpactAudioSource;
    [SerializeField] private AudioSource lilyImpactAudioSource;

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
    public GameObject newQTE;
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
        print(tiger.activeSelf);
        //print(BeatMaster.instance.beatCount);
        //QTE
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

    public void BossButtonSeuqence()
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

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("TigerProjectile"))
        {
            GameManager.instance.health--;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            //waterImpactAudioSource.Play();
            Destroy(other.gameObject);
            anim.SetTrigger("High Collision");
            anim.SetTrigger("Low Collision");
            //rb.AddForce(transform.up * 8);
        }

        if(other.gameObject.CompareTag("Lily"))
        {
            Destroy(other.gameObject);
            //lilyImpactAudioSource.Play();
        }

        if (other.gameObject.CompareTag("Collectable"))
        {
            for(int i = 0; i < 5; i++)
            {
                if (other.gameObject.name == "Col_" + (i + 1).ToString()) 
                    CPManager.instance.collectables[i] = true;

                hud.CollectGet(other.name);
            }
            
            //waterImpactAudioSource.PlayOneShot(transitionSFX);
            Destroy(other.gameObject);
    
            //rb.AddForce(transform.up * 8);
        }

        if (other.gameObject.CompareTag("Log"))
        {
            //waterImpactAudioSource.PlayOneShot(logCollisionSFX);
            //GameManager.instance.health--;
            StartCoroutine(Collision(2.0f));
            //logCollisionSFX.Play();
            anim.SetTrigger("High Collision");
            anim.SetTrigger("Low Collision");
        }

        if (other.gameObject.CompareTag("Tiger"))
        {
            //waterImpactAudioSource.PlayOneShot(tigerSFX);
            //logCollisionSFX.Play();
            GameManager.instance.health--;
        }

        if(other.gameObject.CompareTag("End"))
        {
            if(resultsScreen)
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

        if (other.gameObject.CompareTag("Heart") )
        {
            if (GameManager.instance.health < 3)
                 GameManager.instance.health++;

            if (GameManager.instance.health >= 3)
            {
                GameManager.instance.health = 3;
            }
        }
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
    /// <summary>
    /// FIX LEAN ANIMATION IN MOVEMENT METHOD
    /// </summary>
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
      

        //Fix aniimation to work with new input

    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
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
        if (beatCount == 18 && !activeQTE)
        {
            newQTE.SetActive(true);
        }

        if (beatCount == 66 && !activeQTE)
        {
            newQTE.SetActive(true);
        }

        if (beatCount == 82 && !activeQTE)
        {
            newQTE.SetActive(true);
        }
        
        if (beatCount == 101 && !activeQTE)
        {
            newQTE.SetActive(true);
        }

        if (beatCount == 139 && !activeQTE)
        {
            newQTE.SetActive(true);
        }


        if (beatCount == 205 && !activeQTE)
        {
            newQTE.SetActive(true);
        }
        
        if (beatCount == 272 && !activeQTE)
        {
            newQTE.SetActive(true);
        }

        if (beatCount == 296 && !activeQTE)
        {
            newQTE.SetActive(true);
        }

        if (beatCount == 320 && !activeQTE)
        {
            newQTE.SetActive(true);
        }

        if (beatCount == 352 && !activeQTE)
        {
            TigerEnable();
        }

        if (beatCount == 370 && !activeQTE)
        {
            newQTE.SetActive(true);
        }

        if (beatCount == 390 && !activeQTE)
        {
            TigerEnable();
        }

        if (beatCount == 444 && !activeQTE)
        {
            newQTE.SetActive(true);
        }

        if (beatCount == 468 && !activeQTE)
        {
            newQTE.SetActive(true);
        }
    }

                                                                                                                                                                                                                                                                     
}