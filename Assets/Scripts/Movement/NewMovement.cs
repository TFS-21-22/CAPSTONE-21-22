using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector2 input;
    public float maxSpeed = 5f;
    public float timeZeroToMax = 2f;
    public float acelRatePerSecond;
    public float velocity;
    float gravity;

    // Start is called before the first frame update
    void Start()
    {
        if (!rb)
            rb = GetComponent<Rigidbody>();

        acelRatePerSecond = maxSpeed / timeZeroToMax;
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2();
        input.x = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        
    }
}
