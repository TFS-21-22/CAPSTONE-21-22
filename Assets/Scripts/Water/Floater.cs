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
    public float offset;
    public bool floatingaway = false;
    public float floatingawaySpeed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        if (transform.position.y < waveHeight)//Checks weather the boat is under water
        {
            displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y - offset) / depthBeforeSubmerge) * displacementAmount;//Checks if the boat has gone under water, adds a force of 3x the amount of force it went under
            rb.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration); //if the boat it sumbmerged add a force to the (rb.y * displacementAmount) <Math.Abs returns the absolute of that value>
        }
        if (floatingaway == true)
        {
            if (WaveManager.instance.direction == 1)
            {
                rb.AddForceAtPosition(Vector3.left * floatingawaySpeed * WaveManager.instance.speed, transform.position);
            }
            else
            {
                rb.AddForceAtPosition(Vector3.right * floatingawaySpeed * WaveManager.instance.speed, transform.position);
            }
        }
    }
}
