using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob_effect : MonoBehaviour
{
    public float speed = 1f;
    public float offset = 0;
    public float input;
    public float Amplitude = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");
        if (input != 0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            Vector3 addAmplitude = new Vector3(transform.eulerAngles.z, transform.eulerAngles.y, Amplitude);
            Vector3 reduceAmplitude = new Vector3(transform.eulerAngles.z, transform.eulerAngles.y, -Amplitude);
            transform.eulerAngles = input < 0 ? addAmplitude : reduceAmplitude; //Ternary operator //pretty much a if else statement in 1 line
            /*
            if (input < 0)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.z, transform.eulerAngles.y, Amplitude);
            }
            else {
                transform.eulerAngles = new Vector3(transform.eulerAngles.z, transform.eulerAngles.y, - Amplitude);
            }
            */
        }
        else
        {
            offset += Time.deltaTime * speed;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Amplitude * Mathf.Sin(transform.position.z + offset));
        }
    }
}
