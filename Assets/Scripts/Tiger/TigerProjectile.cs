using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerProjectile : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Colliders"))
        {
            Tiger.instance.ReturnProjectile(this.gameObject);
        }
    }
}
