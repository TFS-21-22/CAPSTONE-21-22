using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public GameObject player;
    public QuickTimeEvent QTE;
    public GameObject birds;
    public ParticleSystem poofParticle;

    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Poof());
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        timer += Time.deltaTime;

        if (timer >= 1.1 && timer <= 2 && QTE.correctButtonSwitch == true)
        {
            StartCoroutine(Poof());
            birds.SetActive(false);
            force = 0;
        }
        else if (timer >= 2.2)
        {
            //GameManager.instance.health--;
            StartCoroutine(Poof());
            birds.SetActive(false);
            force = 0;
        }
    }

    IEnumerator Poof()
    {
        poofParticle.Play();
        yield return new WaitForSeconds(0.3f);
        poofParticle.Pause();
        if (birds.activeSelf == false)
        {
            Destroy(gameObject);
        }
    }
}
