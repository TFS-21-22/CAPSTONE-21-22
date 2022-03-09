using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob_effect : MonoBehaviour
{
    public float speed = 1f;
    public float offset = 0;
    public float input;
    public float Amplitude = 3;

    [SerializeField] private Transform rayTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //The rotation here should be done using forces instead of just rotating the character. This will make the rotation relative and will fix the issue when the water angle changes


        input = Input.GetAxisRaw("Horizontal");
        if (input != 0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            Vector3 addAmplitude = new Vector3(transform.eulerAngles.z, transform.eulerAngles.y, Amplitude);
            Vector3 reduceAmplitude = new Vector3(transform.eulerAngles.z, transform.eulerAngles.y, -Amplitude);
            transform.eulerAngles = input < 0 ? addAmplitude : reduceAmplitude; //Ternary operator //pretty much a if else statement in 1 line

        }
        else
        {
            offset += Time.deltaTime * speed;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Amplitude * Mathf.Sin(transform.position.z/2 + offset));
        }
    }
}
