using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Strafe : MonoBehaviour
{
    [SerializeField] Image xCricle;
    [SerializeField] Image xButton;
    [SerializeField] GameObject tiger;

    public float boundx = 2.25f;
    public float speed = 3.0f;
    public Transform follow;
    public GameObject enemy;
    public Canvas rhythmCanvas;
    public bool enemySequence = false;

    public SmoothCameraScript camera;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        tiger.SetActive(false);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BeatMaster.beatCount == 8 && !enemySequence)
        {
            tiger.SetActive(true);
            //EnemySequence();
        }

        if (BeatMaster.beatCount == 52 && !enemySequence)
        {
            tiger.SetActive(true);
        }

        if (BeatMaster.beatCount == 74 && !enemySequence)
        {
            tiger.SetActive(true);
        }

        if (!enemy.activeSelf)
            enemySequence = false;

        if (!tiger.activeSelf)
            enemySequence = false;

        float h = Input.GetAxis("Horizontal") * speed;
        transform.Translate(h * Time.deltaTime, 0, 0);
    }

    private void EnemySequence()
    {
        //Set enemy true
        enemy.SetActive(true);
        //Set Rythm Cavas Active
        rhythmCanvas.gameObject.SetActive(true);
        //Camera Movement
        camera.cameraPosition = SmoothCameraScript.ECameraPosition.OffsetRight;
        //Camera Switch
        camera.StartCoroutine(camera.CameraSwitch(3));
        //Enemy sqeuence true
        enemySequence = true;
    }

    public void BossButtonSeuqence()
    {
        //Set Button Squence Active
        rhythmCanvas.gameObject.SetActive(true);

        //Camera Movement
        camera.cameraPosition = SmoothCameraScript.ECameraPosition.OffsetLeft;

        //Camera Switch
        camera.StartCoroutine(camera.CameraSwitch(3));

        //Set sqeuence true
        enemySequence = true;
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Lily"))
        {
            Destroy(other.gameObject);
            rb.AddForce(transform.up * 8);
        }
    }

    
}