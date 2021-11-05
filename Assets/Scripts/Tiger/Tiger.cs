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
    bool canShoot = false;
    public bool chooseLane = true;

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
            
        if (shotsFired >= 5)
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
                if(chooseLane)
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
        int lane = Random.Range(0, 4);

        Debug.Log("Move");
        if (lane == 0)
        {
            //Move to lane position
            RhythmCanvas.instance.ResetRhythmTween();
            LeanTween.moveX(this.gameObject, -1f, 1f);
            chooseLane = false;
            canShoot = true;
            yield return new WaitForSecondsRealtime(wait);
            BossState = CurrentState.Shoot;
        }
        else if (lane == 1)
        {
            //Move to lane position
            RhythmCanvas.instance.ResetRhythmTween();
            LeanTween.moveX(this.gameObject, 1f, 1f);
            chooseLane = false;
            canShoot = true;
            yield return new WaitForSecondsRealtime(wait);
            BossState = CurrentState.Shoot;

        }
        else
        {
            //Move to lane position
            chooseLane = false;
            canShoot = true;
            yield return new WaitForSecondsRealtime(wait);
            BossState = CurrentState.Shoot;

        }
    }

}

   

    




