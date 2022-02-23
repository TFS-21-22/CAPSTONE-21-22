using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public float depthBeforeSubmerge = 0.3f;
    public float displacementAmount = 1.8f;
    public float waveHeight;
    public float displacementMultiplier;
    public bool floatingaway = false;
    public float floatingawaySpeed = 0.01f;
    public float positiony;

    public float amplitude = 1f;
    public float speed = 1f;
    public float offset = 0;
    public float length = 2f;
    bool maxOffset;
    public float direction = 1;
    public float heightoffset = 0f;
    public float floatingspeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        offset += Time.deltaTime * speed;
        positiony = transform.position.y;
        waveHeight = amplitude * Mathf.Sin(transform.position.x / length + offset) + 1;
        if (0 < waveHeight)//Checks weather the boat is under water
        {
            displacementMultiplier = Mathf.Clamp01(waveHeight/ depthBeforeSubmerge) * displacementAmount;//Checks if the boat has gone under water, adds a force of 3x the amount of force it went under
            rb.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration); //if the boat it sumbmerged add a force to the (rb.y * displacementAmount) <Math.Abs returns the absolute of that value>
        }
        if (floatingaway == true)
        {
            if (direction == 1)
            {
                rb.AddForceAtPosition(Vector3.left * floatingawaySpeed * floatingspeed, transform.position);
            }
            else
            {
                rb.AddForceAtPosition(Vector3.right * floatingawaySpeed * floatingspeed, transform.position);
            }
        }
    }
}
