using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class Tiger : MonoBehaviour
{

    public static Tiger instance;
    Strafe strafeScript;
    //Player
    [SerializeField] private Transform player;
    //Projectile
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawnLocation;
    [SerializeField] private Transform[] projectileHitLocations = new Transform[3];
    //Particles
    [SerializeField] private ParticleSystem roarParticle;
    [SerializeField] private ParticleSystem bulletImpactParticle;


    int shotsFired = 0;
    bool canShoot = true;
    public bool chooseLane = true;
    bool chooseInt = false;
    int previousLane = 0;
    float lerpDuration = 4f;
    float timeCheck;


    public enum CurrentState
    {
        Move,
        Shoot,
        WaitForButtonSequence,
    }

    public CurrentState TigerState;

    void OnEnable()
    {
        TigerState = CurrentState.Move;
    }

    // Update is called once per frame
    void Update()
    {
        //timeCheck += Time.deltaTime;
        //print(timeCheck);
        //Debug.Log(chooseLane);
        transform.LookAt(player.transform);

        //Tiger State
        switch (TigerState)
        {
            case CurrentState.WaitForButtonSequence:
                //print("FINAL TIME: " + timeCheck);
                break;
            case CurrentState.Move:
                StartCoroutine(MoveTiger());
                break;
            case CurrentState.Shoot:
                if (shotsFired >= 5)
                {
                    TigerState = CurrentState.WaitForButtonSequence;
                }

                if (canShoot)
                {
                    canShoot = false;
                    StartCoroutine(Shoot());
                    //Shoot
                }

                
                break;
        }
    }

    private IEnumerator Shoot()
    {
        float speed = 5f;
        GameObject bullet = projectile;

        //Enable
        bullet.SetActive(true);

        //Reset Bullet Position
        bullet.transform.position = projectileSpawnLocation.transform.position;

        //Shoot particle enable
        //roarParticle.Play();

        float step = speed * Time.deltaTime; // calculate distance to move
        projectile.transform.position = Vector3.MoveTowards(bullet.transform.position, projectileHitLocations[previousLane].transform.position, step);

        // Check if the position of the projectile and ground are approximately equal.
        float distance = Vector3.Distance(transform.position, bullet.transform.position);
        while (distance > 0.0001f)
        {
            // Move the position
            bullet.transform.position *= -1.0f;
            yield return null;
        }
        //bulletImpactParticle.Play();
        shotsFired++;
        canShoot = true;
    }

    private int RandomLane(int minValue, int maxValue)
    {
        return Random.Range(minValue, maxValue);
    }
    private IEnumerator MoveTiger()
    {

        float moveTime = 2f;
        float moveDistance;
        int chosenDirection;
        var validChoice = new int[] { 0, 2 };

        if (previousLane == 0)
        {
            chosenDirection = RandomLane(1, 2);

            if (chosenDirection == 1)
                moveDistance = 2f;
            else
                moveDistance = 4f;
        }
        else if (previousLane == 1)
        {
            chosenDirection = RandomLane(0, 2);

            if (chosenDirection == 0)
                moveDistance = -2f;
            else
                moveDistance = 2f;
        }
        else
        {
            chosenDirection = validChoice[Random.Range(0, validChoice.Length)];

            if (chosenDirection == 1)
                moveDistance = -2f;
            else
                moveDistance = -4f;
        }


        int id = LeanTween.moveLocalX(this.gameObject, moveDistance, moveTime).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }
        previousLane = chosenDirection;
        LeanTween.cancel(id);
        canShoot = true;
        TigerState = CurrentState.Shoot;
    }



}








