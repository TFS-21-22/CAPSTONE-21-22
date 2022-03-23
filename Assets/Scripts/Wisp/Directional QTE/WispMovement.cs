using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public GameObject player;
    public QuickTimeEvent QTE;

    public ParticleSystem wispPoof;
    public GameObject birds;

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

        if (timer >= 0 && timer <= 0.4)
        {
            wispPoof.Play();
        }

        if (timer >= 1.1 && timer <= 2 && QTE.correctButtonSwitch == true)
        {
            StartCoroutine(DestroyWisp());
        }
        else if (timer >= 2.2)
        {            
            //GameManager.instance.health--;
            StartCoroutine(DestroyWisp());
        }
    }

    IEnumerator DestroyWisp()
    {
        force = 0;
        wispPoof.Play();
        birds.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        wispPoof.Stop();
        Destroy(gameObject);
    }
}
