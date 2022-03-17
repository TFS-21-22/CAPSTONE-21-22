using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class Tiger : MonoBehaviour
{
    public static Tiger instance;
    Strafe strafeScript;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawnLocation;

    [SerializeField] private GameObject roarParticle;

    public Queue<GameObject> pool = new Queue<GameObject>();

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
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate projectiles on first frame
        for (int i = 0; i < 5; i++)
        {
            GameObject temp = Instantiate(projectile, projectileSpawnLocation.transform.position, Quaternion.identity);
            temp.SetActive(false);
            pool.Enqueue(temp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeCheck += Time.deltaTime;
        print(timeCheck);
        //Debug.Log(chooseLane);
        transform.LookAt(player.transform);

        //Tiger State
        switch (TigerState)
        {
            case CurrentState.WaitForButtonSequence:
                print("FINAL TIME: " + timeCheck);
                break;
            case CurrentState.Move:
                StartCoroutine(MoveTiger());
                break;
            case CurrentState.Shoot:
                if (canShoot)
                {
                    canShoot = false;
                    GetProjectile();
                }

                if(shotsFired >= 5)
                {
                    TigerState = CurrentState.WaitForButtonSequence;
                }
                break;
        }
    }

    public void ReturnProjectile(GameObject obj)
    {
        //Set projectile in-active
        obj.SetActive(false);
        //Enqueue
        pool.Enqueue(obj);
    }

    void GetProjectile()
    {
        GameObject temp = pool.Dequeue();

        temp.SetActive(true);

        temp.transform.position = projectileSpawnLocation.transform.position;
    }
    IEnumerator Shoot(float wait)
    {
        GetProjectile();
        roarParticle.SetActive(true);
        yield return new WaitForSeconds(wait);
        shotsFired++;
        canShoot = true;
        roarParticle.SetActive(false);
    }

    private int RandomLane(int minValue, int maxValue)
    {
        return Random.Range(minValue, maxValue);
    }
    IEnumerator MoveTiger()
    {

        float moveTime = 2f;
        float moveDistance;
        int chosenDirection;
        var validChoice = new int[]{ 0, 2 };

        if(previousLane == 0)
        {
            chosenDirection = RandomLane(1, 2);

            if (chosenDirection == 1)
                moveDistance = 2f;
            else
                moveDistance = 4f;
        }
        else if(previousLane == 1)
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
        while(LeanTween.isTweening(id))
        {
            yield return null;
        }
        previousLane = chosenDirection;
        LeanTween.cancel(id);
        canShoot = true;
        TigerState = CurrentState.Shoot; 
    }
    
}

   

    




