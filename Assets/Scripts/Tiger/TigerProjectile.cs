using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerProjectile : MonoBehaviour
{
    void Start()
    {
        GameObject[] playerWalls = GameObject.FindGameObjectsWithTag("Colliders");

        for (int i = 0; i < playerWalls.Length; i++)
        {
            Physics.IgnoreCollision(playerWalls[i].GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}
