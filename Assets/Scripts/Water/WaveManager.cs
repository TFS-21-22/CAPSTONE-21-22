using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public float amplitude = 1f;
    public float speed = 1f;
    public float offset = 0;
    public float length = 2f;
    bool maxOffset;
    // Start is called before the first frame update
    void Start()
    {
        maxOffset = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        offset += Time.deltaTime * speed;
    }

    
    public float WaveHeight(float xCordinate, float zCordinate)
    {
        return amplitude * Mathf.Sin(xCordinate / length + offset) + amplitude * Mathf.Sin(zCordinate / length + offset);
        ;
    }
    
}
