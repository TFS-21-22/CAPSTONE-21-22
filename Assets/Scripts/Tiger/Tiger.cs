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
    bool isShooting = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance != null)
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
        for(int i = 0; i < 10; i++)
        {
            GameObject temp = Instantiate(projectile, projectileSpawnLocation.transform.position, Quaternion.identity) as GameObject;
            temp.SetActive(false);
            pool.Enqueue(temp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(player);
        Debug.Log(pool.Count);

       switch(BossState)
        {
            case CurrentState.ButtonSquence:
                strafeScript.BossButtonSeuqence();
                break;
            case CurrentState.Move:
                StartCoroutine(Delay(3f));
                break;
            case CurrentState.Shoot:
                //Shoot Projectiles at a random lane
                if(!isShooting)
                ShootingState(Random.Range(0,3));
                break;
        }
    }

    public void GetProjectile()
    {
        if(pool.Count > 0)
        {
            //Get Target

            GameObject temp = pool.Dequeue();
            //Reset position
            temp.transform.position = projectileSpawnLocation.transform.position;

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

    public void ShootingState(int randomLane)
    {
        isShooting = true; 

        //Choose a lane,
        if (randomLane == 0)
        {
            //Move to lane position
            LeanTween.moveLocalX(this.gameObject, -2, Time.time);
            StartCoroutine(Shoot(1f, shotsFired));
            
        }
        if (randomLane == 1)
        {
          
            //Move to lane position
            LeanTween.moveLocalX(this.gameObject, 2, Time.time);
            StartCoroutine(Shoot(1f, shotsFired));
        }
        else
        {
            //Move to lane position
            StartCoroutine(Shoot(1f, shotsFired));
        }
    }

    IEnumerator Shoot(float wait, int projectilesFired)
    {
        while(projectilesFired < 4)
        {
            yield return new WaitForSecondsRealtime(wait);
            projectilesFired++;
            Debug.Log(projectilesFired);
            GetProjectile();
            
            
        }

        if (projectilesFired >= 4)
        {
            BossState = CurrentState.ButtonSquence;
            shotsFired = 0;
        }

        isShooting = false;
    }

    IEnumerator Delay(float wait)
    {
        //Ping Pong
        yield return new WaitForSecondsRealtime(wait);
        BossState = CurrentState.Shoot;
    }

    



}
