using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob_effect : MonoBehaviour
{
    public float speed = 1f;
    public float offset = 0;
    public float input;
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
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
            if(input < 0)
            {
                transform.eulerAngles = new Vector3(-3, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            else {
                transform.eulerAngles = new Vector3(3, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }
        else
        {
            offset += Time.deltaTime * speed;
            transform.eulerAngles = new Vector3(3 * Mathf.Sin(transform.position.x + offset), transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
