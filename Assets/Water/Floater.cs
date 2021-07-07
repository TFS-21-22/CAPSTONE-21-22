using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    float depthBeforeSubmerge = 0.3f;
    float displacementAmount = 1.8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(transform.position.y < 1)//Checks weather the boat is under water
        {
            float displacementMultiplier = Mathf.Clamp01(-transform.position.y / depthBeforeSubmerge) * displacementAmount;//Checks if the boat has gone under water, adds a force of 3x the amount of force it went under
            rb.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y * displacementMultiplier), 0f), ForceMode.Acceleration); //if the boat it sumbmerged add a force to the (rb.y * displacementAmount) <Math.Abs returns the absolute of that value>
        }
    }
}
