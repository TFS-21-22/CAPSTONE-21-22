using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public float waveHeight = 0f;
    public float floatheight = 0.3f;
    public float bounceDamp = 0.05f;
    public float offset = 0;
    float forceFactor;
    public bool floatingaway = false;

    // Update is called once per frame
    void Update()
    {
        waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        forceFactor = 1f - ((transform.position.y + offset) - waveHeight) / floatheight;
        if(forceFactor > 0f)
        {
            Vector3 upLift = -Physics.gravity * (forceFactor - rb.velocity.y * bounceDamp);
            rb.AddForceAtPosition(upLift, transform.position + new Vector3(0, offset, 0));
            if (floatingaway == true)
            {
                rb.AddForceAtPosition(Vector3.left * 0.01f * WaveManager.instance.speed, transform.position);
            }
        }

    }
}
