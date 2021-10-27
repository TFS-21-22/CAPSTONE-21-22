using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Return"))
        {
            Tiger.instance.pool.Enqueue(collision.gameObject);
        }
    }
}
