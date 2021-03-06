using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
public class Tiger : MonoBehaviour
{
    public Strafe strafeScript;
    Animator tigerAnim;
    public Slider healthSlider;
    //Player
    [SerializeField] private Transform tigerParent;
    //Projectile
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private Transform projectileSpawnLocation;
    //Particles
    [SerializeField] private ParticleSystem roarParticle;
    //Claw positions
    [SerializeField] private Transform tigerDamgeZones;
    [SerializeField] private Transform[] clawPositions;
    [SerializeField] private Transform[] lanes;


    [SerializeField] private Transform startPosition;

    private int clawPosIndex;
    //Audio
    public AudioSource tigerSwipeSFX;
    public AudioSource roarSound;
    //Bool
    public bool attacking = false;
    private bool shooting = false;
    private int bulletsFired = 0;
    private bool chooseClawLane = true;
    private bool inRange = false;
    private bool getLane = true;
    //Pool
    Queue<GameObject> pool;

    private bool chooseCurrentLane = true;
    public int currentLane = 0;
    public float currentHealth = 100f;
    private float moveDistance;
    private int chosenLocation;


    public bool tigerStun = false;

    void OnDisable()
    {
        healthSlider.gameObject.SetActive(false);
    }

    private enum enTigerState
    {
        ResetPosition,
        Claw,
        Move,
        Shoot,
    }

    private enTigerState TigerState;

    private void OnEnable()
    {
        TigerState = enTigerState.Move;
        shooting = false;
    }

    void Start()
    {
        tigerAnim = GetComponent<Animator>();
    }

    

    private void Update()
    {
        //print(TigerState);
        print(TigerState);
        switch (TigerState)
        {
            case enTigerState.ResetPosition:
                ResetPosition(startPosition.position);

                break;
            case enTigerState.Claw:

                if(!inRange)
                {
                    ChooseClawPosition();
                }

                break;

            case enTigerState.Move:

                //Moves tiger left or right
                if(getLane)
                {
                    getLane = false;
                    currentLane = GetLane();
                }
                else
                {
                    MoveToLanePosition();
                }

                break;

            case enTigerState.Shoot:

                if (!shooting)
                {
                    Debug.Log("Not Shooting" + bulletsFired);
                    if(bulletsFired < 3)
                    {
                        shooting = true;
                        StartCoroutine(GetProjectile());
                    }
                    else if(bulletsFired >= 3)
                    {
                        TigerState = enTigerState.Claw;
                    }
                }

              

                break;
        }
        //Health bar
        if (healthSlider)
        {
            healthSlider.value = currentHealth;
        }

        if(RhythmCanvas.instance.tigerStun)
        {
            tigerAnim.SetTrigger("Hit");
        }
        tigerAnim.SetBool("Hit 0", RhythmCanvas.instance.tigerStun);
    }

    public void MoveToLanePosition()
    {
        float distance = Vector3.Distance(tigerParent.transform.position, lanes[chosenLocation].position);
        Debug.Log("Distance:" + distance);
        if (distance < 0.001f)
        {
            if(bulletsFired <= 3)
            {
                shooting = false;
                TigerState = enTigerState.Shoot;
            }
            else
            {
                TigerState = enTigerState.Claw;
            }
        }
        else
        {
            float speed = 6f;
            float step = speed * Time.deltaTime; // calculate distance to move
            tigerParent.position = Vector3.MoveTowards(tigerParent.transform.position, lanes[chosenLocation].position, step);
        }
    }

    private IEnumerator GetProjectile()
    {
        
        yield return new WaitForSeconds(1f);

        Instantiate(projectilePrefab, projectileSpawnLocation.transform.position, projectileSpawnLocation.transform.rotation);
        bulletsFired++;
        roarParticle.gameObject.SetActive(true);
        roarSound.Play();
        roarParticle.gameObject.SetActive(false);
        getLane = true;
        TigerState = enTigerState.Move;

    }
    public void ResetPosition(Vector3 endPos)
    {
        float distance = Vector3.Distance(tigerParent.transform.position, endPos);

        if (distance < 0.001f)
        {
            bulletsFired = 0;
            currentLane = 0;
            shooting = false;
            chooseClawLane = true;
            chooseCurrentLane = true;
            getLane = true;
            TigerState = enTigerState.Move;
           
        }
        else
        {
            float speed = 6f;
            float step = speed * Time.deltaTime; // calculate distance to move
            tigerParent.position = Vector3.MoveTowards(tigerParent.transform.position, endPos, step);
        }
    }


    public void ChooseClawPosition()
    {
        if (chooseClawLane)
        {
            clawPosIndex = Random.Range(0, 3);
            chooseClawLane = false;
        }

        Vector3 positionChosen = clawPositions[clawPosIndex].position;
        float boxDuration = 2f;
        float distance = Vector3.Distance(tigerParent.transform.position, positionChosen);
        if (distance < 0.001f)
        {
            inRange = true;
            StartCoroutine(ChooseDamageBox(boxDuration));
        }
        else
        {
            float speed = 6f;
            float step = speed * Time.deltaTime; // calculate distance to move
            tigerParent.position = Vector3.MoveTowards(tigerParent.transform.position, positionChosen, step);
        }
    }

    private IEnumerator ChooseDamageBox(float _wait)
    {
        float waitForQTE = 5f;


        roarParticle.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(_wait);
        tigerAnim.SetBool("Attack", true);
        tigerDamgeZones.gameObject.SetActive(true);
        if (!tigerSwipeSFX.isPlaying)
            tigerSwipeSFX.Play();
        yield return new WaitForSecondsRealtime(_wait);
        tigerAnim.SetBool("Attack", false);
        tigerDamgeZones.gameObject.SetActive(false);
        roarParticle.gameObject.SetActive(false);
        healthSlider.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(waitForQTE);
        healthSlider.gameObject.SetActive(false);
        TigerState = enTigerState.ResetPosition;
        bulletsFired = 0;
        chooseClawLane = true;
        chooseCurrentLane = true;
        inRange = false;
        

    }

    
    private int RandomLane(int minOffsetValue, int maxOffsetValue)
    {
        return Random.Range(minOffsetValue, maxOffsetValue);
    }
    public int GetLane()
    {
        int[] centerLaneChoices = new int[] { 0, 2 };

        if (currentLane == 0)
        {
            //Choses lane to move to
            return chosenLocation = RandomLane(1, 2);
        }
        else if (currentLane == 1)
        {
            return chosenLocation = RandomLane(0, 2);
        }
        else
        {
            int index = Random.Range(0, 1);
            return chosenLocation = centerLaneChoices[index];
        }
               
    }



}








