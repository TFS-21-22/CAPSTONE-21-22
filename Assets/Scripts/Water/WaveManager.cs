using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    static WaveManager _instance = null;
    public static WaveManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
    public float amplitude = 1f;
    public float speed = 1f;
    public float offset = 0;
    public float length = 2f;
    bool maxOffset;
    public float direction = 1;
    public float heightoffset = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(this);
        }
        maxOffset = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        offset += Time.deltaTime * speed;
    }

    
    public float GetWaveHeight(float xCordinate)
    {
        return amplitude * Mathf.Sin(xCordinate / length + offset) + heightoffset;
    }
    public float GetfloatHeight()
    {
        return amplitude;
    }
}
