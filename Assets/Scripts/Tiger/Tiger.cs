using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    int currentLane = 0;
    float lerpDuration = 4f;
    

    public enum CurrentState
    {
        Idle,
        Move,
        Shoot,
        ButtonSquence,
    }

    public CurrentState BossState;

    void OnEnable()
    {
        BossState = CurrentState.Idle;
        shotsFired = 0;
        canShoot = true;
        chooseLane = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        strafeScript = FindObjectOfType<Strafe>();

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
        //Debug.Log(chooseLane);
        transform.LookAt(player.transform);

        //Tiger State
        switch (BossState)
        {
            case CurrentState.Idle:
                StartCoroutine(SwitchState(1f));
                break;
            case CurrentState.ButtonSquence:
                strafeScript.TigerButtonSequence();
                break;
            case CurrentState.Move:
                StartCoroutine(MoveTiger(1f));
                break;
            case CurrentState.Shoot:
                if (shotsFired < 5 && canShoot)
                {
                    canShoot = false;
                    StartCoroutine(Shoot(1f));
                }

                if (shotsFired >= 5)
                {
                    BossState = CurrentState.ButtonSquence;
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
    IEnumerator MoveTiger(float wait)
    {
        if(!chooseLane)
        {
            chooseLane = false;
            currentLane = RandomLane();
        }
        
        if (currentLane == 1)
        {
            int id = LeanTween.moveLocalX(this.gameObject, 2f, 4).id;

            while (LeanTween.isTweening(id))
            {
                yield return null;
            }

            BossState = CurrentState.Shoot;
        }
        else if(currentLane == 0)
        {

            int id = LeanTween.moveLocalX(this.gameObject, -2f, 4).id;

            while (LeanTween.isTweening(id))
            {
                yield return null;
            }

            BossState = CurrentState.Shoot;

        }
        else
        {
            BossState = CurrentState.Shoot;
        }
    }

    int RandomLane()
    {
        return Random.Range(0, 4);
    }

    IEnumerator SwitchState(float _wait)
    {
        yield return new WaitForSeconds(_wait);
        BossState = CurrentState.Move;
    }

}

   

    




