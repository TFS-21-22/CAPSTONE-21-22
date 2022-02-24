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
    Vector3 camPos;
    Vector2 input;
    public Vector3 jump;

    [Header("Floats")]
    public float boundx = 2.25f;
    public float speed = 3.0f;
    public float h;
    public float jumpForce = 2.0f;


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

    public ResultsScreen resultsScreen;
    public Animator anim;

    //Temp
    int currentQTENum = 0;



   // private int lastHeartInstanceID;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        canHurt = true;
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        fireFly.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (BeatMaster.instance.beatCount == 10 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 32 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 56 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 80 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 104 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 128 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 152 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 176 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 200 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 224 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 248 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 272 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 296 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 320 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 352 && !activeQTE)
        {
            TigerEnable();
        }

        if (BeatMaster.instance.beatCount == 370 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 320 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 344 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 368 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 392 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 416 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 440 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        if (BeatMaster.instance.beatCount == 464 && !activeQTE)
        {
            newQTE.SetActive(true);
            print(currentQTENum);
            currentQTENum++;
        }

        
        



        //Movement
        h = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.S))
        {
            anim.SetTrigger("Duck");  
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            anim.ResetTrigger("Duck");
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            //fireFly.SetActive(true);
        }

        if(Input.GetKeyUp(KeyCode.W) && fireFly)
        {
            fireFly.SetActive(false);
        }
        //h = Mathf.Clamp(h, -2, 2);
        anim.SetFloat("Direction", h);
        h *= speed;
        if (stopperL && h < 0)
        {
            h = 0;
        }
        if (stopperR && h > 0)
        {
            h = 0;
        }
       
        
        transform.Translate(h * Time.deltaTime, 0, 0);

       
        if(scoresystem && scoresystem.transitionEnding)
        {
            waterImpactAudioSource.PlayOneShot(transitionSFX);
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
        Instantiate<GameObject>(tiger, tigerSpawnPos.transform.position, Quaternion.identity);
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
            waterImpactAudioSource.Play();
            Destroy(other.gameObject);
            anim.SetTrigger("Low Collision");
            //rb.AddForce(transform.up * 8);
        }

        if(other.gameObject.CompareTag("Lily"))
        {
            lilyImpactAudioSource.Play();
        }

        if (other.gameObject.CompareTag("Collectable"))
        {
            
            for(int i = 0; i < 5; i++)
            {
                if (other.gameObject.name == "Col_" + (i + 1).ToString()) 
                    CPManager.instance.collectables[i] = true;
            }
            
            waterImpactAudioSource.PlayOneShot(transitionSFX);
            Destroy(other.gameObject);
            anim.SetTrigger("Low Collision");
            //rb.AddForce(transform.up * 8);
        }

        if (other.gameObject.CompareTag("Log"))
        {
            waterImpactAudioSource.PlayOneShot(logCollisionSFX);
            //GameManager.instance.health--;
            StartCoroutine(Collision(2.0f));
            //logCollisionSFX.Play();
            anim.SetTrigger("High Collision");

        }

        if (other.gameObject.CompareTag("Tiger"))
        {
            waterImpactAudioSource.PlayOneShot(tigerSFX);
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

                                                                                                                                                                                                                                                                     
}