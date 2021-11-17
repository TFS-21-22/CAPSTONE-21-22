using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class MovementCalculation : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform endPoint;

    public float time;
    public float speed;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        //speed = PathFollower.instance.speed;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, endPoint.transform.position);
        speed = distance / (229 - Time.time);
        PathFollower.instance.speed = speed;
    }
}
