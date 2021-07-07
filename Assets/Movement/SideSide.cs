using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideSide : MonoBehaviour
{
    private float xMin = -1.0f, xMax = 1.0f;
    public float speed = 5.0f;

    //public float xMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xMove = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        transform.Translate(0f, xMove, 0f);
        // initially, the temporary vector should equal the player's position
        Vector3 clampedPosition = transform.position;
        // Now we can manipulte it to clamp the y element
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -4.1f, 4.1f);
        // re-assigning the transform's position will clamp it
        transform.position = clampedPosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(name + ": OnCollisionEnter - " + collision.gameObject.name);
    }

    // - Called as long as there is collision between two GameObjects
    void OnCollisionStay(Collision collision)
    {
        Debug.Log(name + ": OnCollisionStay - " + collision.gameObject.name);
    }

    // - Called once on collision stopping between two GameObjects
    void OnCollisionExit(Collision collision)
    {
        Debug.Log(name + ": OnCollisionExit - " + collision.gameObject.name);
    }

    // Collision Usage Rules:
    // - GameObject needs a CharacterController component
    // - One of the GameObjects need a Collider
    // - Behaves like OnCollisionStay
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(name + ": OnControllerColliderHit - " + hit.gameObject.name);
    }


    // Trigger Usage Rules:
    // - Both GameObjects need colliders
    // - Set one of the Collider components to "Is Trigger"
    // - Called once on first overlap
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(name + ": OnTriggerEnter - " + other.gameObject.name);

       
    }

    // - Called as long as there is overlap between two GameObjects
    void OnTriggerStay(Collider other)
    {
        Debug.Log(name + ": OnTriggerStay - " + other.gameObject.name);

       
    }



    // - Called once on first overlap
    void OnTriggerExit(Collider other)
    {
        Debug.Log(name + ": OnTriggerExit - " + other.gameObject.name);

     
    }
}
