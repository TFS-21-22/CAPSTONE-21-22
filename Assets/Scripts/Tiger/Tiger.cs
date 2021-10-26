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
    bool chooseLane = true;

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
        Move,
        Shoot,
        ButtonSquence,
    }

    public CurrentState BossState;


    // Start is called before the first frame update
    void Start()
    {
        strafeScript = FindObjectOfType<Strafe>();


        //Instantiate projectiles on first frame
        for (int i = 0; i < 10; i++)
        {
            GameObject temp = Instantiate(projectile, projectileSpawnLocation.transform.position, Quaternion.identity) as GameObject;
            temp.SetActive(false);
            pool.Enqueue(temp);
        }


    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Shots fired:" + shotsFired);
        if (shotsFired < 5 && canShoot)
        {
            canShoot = false;
            StartCoroutine(Shoot(1f));
            shotsFired++;
        }
            
        if (shotsFired >= 5)
        {
            BossState = CurrentState.ButtonSquence;
        }

        switch (BossState)
        {
            case CurrentState.ButtonSquence:
                strafeScript.BossButtonSeuqence();
                break;
            case CurrentState.Move:
                Move(Random.Range(0, 3));
                break;
            case CurrentState.Shoot:
                //Shoot Projectiles at a random lane
                if (chooseLane)
                    Move(Random.Range(0, 3));
                break;
        }
    }

    public void GetProjectile()
    {
        if (pool.Count > 0)
        {
            //Get Target
            GameObject temp = pool.Dequeue() as GameObject;
            //Reset position
            temp.transform.position = projectileSpawnLocation.transform.position;
            //SetActive
            temp.SetActive(true);
        }
    }

    public void ReturnProjectile(GameObject obj)
    {
        //Set projectile in-active
        obj.SetActive(false);
        //Enqueue
        pool.Enqueue(obj);
    }

    public void Move(int randomLane)
    {

        //Choose a lane,
        if (randomLane == 0)
        {
            //Move to lane position
            LeanTween.moveLocalX(this.gameObject, -1.5f, Time.time);
            chooseLane = false;
            canShoot = true;
        }
        else if (randomLane == 1)
        {
            //Move to lane position
            LeanTween.moveLocalX(this.gameObject, 1.5f, Time.time);
            chooseLane = false;
            canShoot = true;
        }
        else
        {
            //Move to lane position
            chooseLane = false;
            canShoot = true;
        }
    }

    IEnumerator Shoot(float wait)
    {
        yield return new WaitForSeconds(wait);
        GetProjectile();
        canShoot = true;
    }

}

   

    




