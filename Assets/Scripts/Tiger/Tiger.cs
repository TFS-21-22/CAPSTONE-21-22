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
    Vector3 startPosition;
    //Projectile
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnLocation;
    //Particles
    [SerializeField] private ParticleSystem roarParticle;
    //Claw positions
    [SerializeField] private Transform[] tigerDamgeZones;
    [SerializeField] private Transform[] clawPositions;
    private int clawPosIndex;
    //Audio
    public AudioSource roarSound;
    //Bool
    public bool attacking = false;
    private bool shooting = false;
    private int bulletsFired = 0;
    private bool chooseClawLane = true;
    //Pool
    Queue<GameObject> pool;

    int shotsFired = 0;
    public int currentLane = 0;
    public float currentHealth = 100f;

    private enum enTigerState
    {
        Claw,
        Move,
        Shoot,
    }

    private enTigerState TigerState;

    private void OnEnable()
    {
        startPosition = transform.position;
        TigerState = enTigerState.Move;
    }

    void Start()
    {
        tigerAnim = GetComponent<Animator>();
    }

    private IEnumerator GetProjectile()
    {
        yield return new WaitForSeconds(2f);
        roarParticle.gameObject.SetActive(true);
        roarSound.Play();
        Instantiate(projectilePrefab, projectileSpawnLocation.transform.position, projectileSpawnLocation.transform.rotation);
        roarParticle.gameObject.SetActive(false);
        
    }

    private void Update()
    {
        switch(TigerState)
        {
            case enTigerState.Claw:

                ChooseClawPosition(tigerParent.localPosition);

                break;

            case enTigerState.Move:

                //Moves tiger left or right
                bool moving = false;
                if(!moving)
                {
                    moving = true;
                    StartCoroutine(MoveTiger());

                }

                break;

            case enTigerState.Shoot:

                if(!shooting)
                {
                    if(bulletsFired < 3)
                    {
                        print("Fire count: " + bulletsFired);
                        shooting = true;
                        bulletsFired++;
                        StartCoroutine(GetProjectile());
                        TigerState = enTigerState.Move;
                    }
                    else
                    {
                        TigerState = enTigerState.Claw;
                    }
                }

                break;
        }

        //Animator
        tigerAnim.SetBool("Attack", attacking);

        //Health bar
        if(healthSlider)
        {
            healthSlider.value = currentHealth;
        }
    }


    public void ChooseClawPosition(Vector3 _currentPosition)
    {
        if(chooseClawLane)
        {
            clawPosIndex = Random.Range(0, clawPositions.Length);
            chooseClawLane = false;
        }

        Vector3 positionChosen = clawPositions[clawPosIndex].localPosition;
        float boxDuration = 0.5f;
        float speed = 100f;
        float step = speed * Time.deltaTime; // calculate distance to move
        _currentPosition = Vector3.MoveTowards(_currentPosition, positionChosen, step);
        

        //Distance between current position and start position
        float distance = Vector3.Distance(_currentPosition, positionChosen);
        print("Distance - " + distance);
        if (distance < 0.001f)
        {
            StartCoroutine(ChooseDamageBox(boxDuration));
        }
    }

    private IEnumerator ChooseDamageBox(float _wait)
    {
        print("MOVING");
        if (!roarSound.isPlaying)
            roarSound.Play();
        roarParticle.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(_wait);
        tigerDamgeZones[clawPosIndex].gameObject.SetActive(true);
        yield return new WaitForSeconds(_wait);
        tigerDamgeZones[clawPosIndex].gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void ResetPosition(Vector3 _startPosition, Vector3 _currentPosition)
    {
        float speed = 4f;
        float step = speed * Time.deltaTime; // calculate distance to move
        _currentPosition = Vector3.MoveTowards(_currentPosition, _startPosition, step);
        //Distance between current position and start position
        float distance = Vector3.Distance(_currentPosition, _startPosition);
        if (distance < 0.001f)
        {
            TigerState = enTigerState.Move;
        }
        else
        {
            _startPosition *= -1f;
        }

    }

    private int RandomLane(int minOffsetValue, int maxOffsetValue)
    {
        return Random.Range(minOffsetValue, maxOffsetValue);
    }
    public IEnumerator MoveTiger()
    {

        float moveTime = 2f;
        float moveDistance; //moves you along the x-axis
        
        //keeps track of the lanes // current and chosen
        int chosenDirection;
        int[] centerLaneChoices = new int[] { 0, 2 };

        if (currentLane == 0)
        {
            //Choses lane to move to
            chosenDirection = RandomLane(1, 2);

            if(chosenDirection == 1)
            {
                moveDistance = 5f;
            }
            else
            {
                moveDistance = 10f;
            }

           
        }
        else if (currentLane == 1)
        {
            //Choses a valid lane to move to
            int index = Random.Range(0, centerLaneChoices.Length);
            chosenDirection = centerLaneChoices[index];

            if (chosenDirection == 0)
            {
                moveDistance = -5f;
            }
            else
            {
                moveDistance = 5f;
            }

        }
        else
        {
            int index = Random.Range(0, 1);
            chosenDirection = centerLaneChoices[index];

            if (chosenDirection == 0)
            {
                moveDistance = -10f;
            }
            else
            {
                moveDistance = -5f;
            }

        }

        int id = LeanTween.moveLocalX(this.gameObject, moveDistance, moveTime).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }
        currentLane = chosenDirection;
        LeanTween.cancel(id);
        TigerState = enTigerState.Shoot;
        shooting = false;
    }



}








