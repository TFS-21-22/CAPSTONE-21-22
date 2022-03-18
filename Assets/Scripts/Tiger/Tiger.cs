using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
public class Tiger : MonoBehaviour
{

    Strafe strafeScript;

    public Image healthImage;
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
    public int currentLane = 0;
    public float currentHealth = 100f;

    void OnEnable()
    {
        StartCoroutine(MoveTiger());
        healthImage.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        healthImage.gameObject.SetActive(false);
    }

    private void GetProjectile()
    {
        Instantiate(projectile, projectileSpawnLocation.transform.position, projectileSpawnLocation.transform.rotation);
    }

    void Update()
    {
        healthImage.fillAmount = currentHealth / 100;

        if(currentHealth <= 0)
        {
            Destroy(this);
        }
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

        if (currentLane == 0)
        {
            chosenDirection = RandomLane(1, 2);

            if (chosenDirection == 1)
                moveDistance = 2f;
            else
                moveDistance = 4f;
        }
        else if (currentLane == 1)
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
        currentLane = chosenDirection;
        LeanTween.cancel(id);
        GetProjectile();
    }



}








