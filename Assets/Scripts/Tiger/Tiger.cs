using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger : MonoBehaviour
{
    public static Tiger instance;
    Strafe strafeScript;
    [SerializeField] private Transform[] lanePositions = new Transform[3]; //0 = Left lane, 1 = middle lane 2 = Right Lane
    [SerializeField] private Transform player;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawnLocation;

    public Queue<GameObject> pool = new Queue<GameObject>();

    int shotsFired = 0;
    bool canShoot = true;
    public bool chooseLane = true;
    bool chooseInt = false;
    int chosenLane = 0;

    float lerpDuration = 4f;
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    public enum CurrentState
    {
        Idle,
        Move,
        Shoot,
        ButtonSquence,
    }

    public CurrentState BossState;


    // Start is called before the first frame update
    void Start()
    {
        chosenLane = RandomLane();

        gameObject.SetActive(false);

        strafeScript = FindObjectOfType<Strafe>();

        BossState = CurrentState.Idle;

        //Instantiate projectiles on first frame
        for (int i = 0; i < 10; i++)
        {
            GameObject temp = Instantiate(projectile, projectileSpawnLocation.transform.position, Quaternion.identity);
            temp.SetActive(false);
            pool.Enqueue(temp);
        }


    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(chooseLane);
        transform.LookAt(player.transform);
        if (shotsFired >= 5 && canShoot)
        {
            BossState = CurrentState.ButtonSquence;
            shotsFired = 0;
            canShoot = false;
        }

        switch (BossState)
        {
            case CurrentState.Idle:
                
                break;
            case CurrentState.ButtonSquence:
                strafeScript.BossButtonSeuqence();
                break;
            case CurrentState.Move:
                StartCoroutine(MoveTiger(1f));
                break;
            case CurrentState.Shoot:
                if (shotsFired < 5 && canShoot)
                {
                    canShoot = false;
                    StartCoroutine(Shoot(1f));
                    shotsFired++;
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
        shotsFired++;
        GetProjectile();
        yield return new WaitForSeconds(wait);
        canShoot = true;
    }
    IEnumerator MoveTiger(float wait)
    {
        
        if (chosenLane == 1)
        {
            
            int id = LeanTween.moveLocalX(this.gameObject, 2f, 1).id;

            while (LeanTween.isTweening(id))
            {
                yield return null;
            }

            BossState = CurrentState.Shoot;
        }
        else if(chosenLane == 0)
        {

            int id = LeanTween.moveLocalX(this.gameObject, -2f, 1).id;

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

}

   

    




