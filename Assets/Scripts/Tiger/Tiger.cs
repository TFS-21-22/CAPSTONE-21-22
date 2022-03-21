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
    [SerializeField] private Transform player;
    //Projectile
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnLocation;
    public Transform[] projectileHitLocations = new Transform[3];
    //Particles
    [SerializeField] private ParticleSystem roarParticle;
    [SerializeField] private ParticleSystem bulletImpactParticle;
    //Audio
    public AudioSource roarSound;
    public bool attacking = false;

    Queue<GameObject> pool;

    int shotsFired = 0;
    public int currentLane = 0;
    public float currentHealth = 100f;

    private void OnEnable()
    {
        StartCoroutine(MoveTiger());
        healthSlider.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        healthSlider.gameObject.SetActive(false);
    }

    void Start()
    {
        tigerAnim = GetComponent<Animator>();
    }

    private void GetProjectile()
    {
        roarParticle.gameObject.SetActive(true);
        roarSound.Play();
        Instantiate(projectilePrefab, projectileSpawnLocation.transform.position, projectileSpawnLocation.transform.rotation);
        roarParticle.gameObject.SetActive(false);
    }

    private void Update()
    {
        tigerAnim.SetBool("Attack", attacking);
        print(currentHealth);
        transform.LookAt(player);
        if(healthSlider)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            strafeScript.tigerAlive = false;
            Destroy(this);
        }
    }

    private int RandomLane(int minOffsetValue, int maxOffsetValue)
    {
        return Random.Range(minOffsetValue, maxOffsetValue);
    }
    public IEnumerator MoveTiger()
    {

        float moveTime = 2f;
        float moveDistance;
        int chosenDirection;
        var validChoice = new int[] { 0, 2 };

        if (currentLane == 0)
        {
            chosenDirection = RandomLane(1, 2);

            if (chosenDirection == 1)
                moveDistance = 4f;
            else
                moveDistance = 8f;
        }
        else if (currentLane == 1)
        {
            chosenDirection = RandomLane(0, 2);

            if (chosenDirection == 0)
                moveDistance = -4f;
            else
                moveDistance = 4f;
        }
        else
        {
            chosenDirection = validChoice[Random.Range(0, validChoice.Length)];

            if (chosenDirection == 1)
                moveDistance = -4f;
            else
                moveDistance = -8f;
        }


        int id = LeanTween.moveLocalX(this.gameObject, moveDistance, moveTime).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }
        currentLane = chosenDirection;
        LeanTween.cancel(id);
        GetProjectile();

    }



}








