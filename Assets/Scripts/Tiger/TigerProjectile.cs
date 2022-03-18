using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerProjectile : MonoBehaviour
{
    GameObject collisionParticle;
    // Movement speed in units per second.
    public float speed;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    void Start()
    {
        collisionParticle = GetComponentInChildren<GameObject>();
        //Ignore colliders
        GameObject[] playerWalls = GameObject.FindGameObjectsWithTag("Colliders");

        for (int i = 0; i < playerWalls.Length; i++)
        {
            Physics.IgnoreCollision(playerWalls[i].GetComponent<Collider>(), GetComponent<Collider>());
        }

        Destroy(this, 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            collisionParticle.gameObject.SetActive(true);
            Destroy(this);

        }
    }
}
