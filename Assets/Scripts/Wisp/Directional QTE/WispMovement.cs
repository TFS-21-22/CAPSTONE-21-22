using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public GameObject player;
    public QuickTimeEvent QTE;

    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        timer += Time.deltaTime;
        if (timer >= 1.1 && timer <= 2 && QTE.correctButtonSwitch == true)
        {
            Destroy(gameObject);
        }
        else if (timer >= 2.2)
        {
            GameManager.instance.health--;
            Destroy(gameObject);            
        }
    }
}
