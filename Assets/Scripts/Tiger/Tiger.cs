using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class Tiger : MonoBehaviour
{

    Strafe strafeScript;
    //Player
    [SerializeField] private Transform player;
    //Projectile
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawnLocation;
    public Transform[] projectileHitLocations = new Transform[3];
    //Particles
    [SerializeField] private ParticleSystem roarParticle;
    [SerializeField] private ParticleSystem bulletImpactParticle;

    int shotsFired = 0;
    bool canShoot = true;
    public bool chooseLane = true;
    bool chooseInt = false;
    public int previousLane = 0;
    float lerpDuration = 4f;
    float timeCheck;

   
    void OnEnable()
    {
        StartCoroutine(MoveTiger());
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
    }

    private void GetProjectile()
    {
        Instantiate(projectile, projectileSpawnLocation.transform.position, projectileHitLocations[previousLane].transform.rotation);
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
        GetProjectile();
        canShoot = true;
    }



}








