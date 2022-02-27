using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerProjectile : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Return"))
        {
            Tiger.instance.pool.Enqueue(collision.gameObject);
        }
    }
}
