using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SmoothCameraScript : MonoBehaviour
{

    public Transform target;

    public float smoothSpeed = 0.6f;

    public Vector3 offset;

    public Transform offsetRight;

    public Transform offsetLeft;

    public Transform originalPosition;

    public Vector3 offsetFarView;

    public Vector3 offsetHurt1;

    public Vector3 offsetHurt2;

    Vector3 desiredPosition;

    public int cameraValue;

    bool transitioning = false;

    [SerializeField]
    float frequency = 25;

    [SerializeField]
    Vector3 maximumTranslationShake = Vector3.one;

    [SerializeField]
    Vector3 maximumAngularShake = Vector3.one;

    private float seed;

    [SerializeField]
    float recoverySpeed = 1.5f;

    public float trauma = 0;

    [SerializeField]
    float traumaExponent = 2;

    public bool hit = false;

    public void InduceStress(float stress)
    {
        trauma = Mathf.Clamp01(trauma + stress);
    }

    private void Awake()
    {
        seed = Random.value;
    }


    public enum ECameraPosition
    {
        Normal,
        OffsetRight,
        OffsetLeft
    }
    public ECameraPosition cameraPosition = ECameraPosition.Normal;

    private void Update()
    {
        if (hit)
        {

            shake();

        }
        else
        {

            if (Input.GetKeyDown(KeyCode.E) && !transitioning)
            {
                transitioning = true;
                cameraPosition = ECameraPosition.Normal;
                StartCoroutine(CameraSwitch(8));
            }

            if (Input.GetKeyDown(KeyCode.R) && !transitioning)
            {
                transitioning = true;
                cameraPosition = ECameraPosition.OffsetRight;
                StartCoroutine(CameraSwitch(8));
            }

            if (Input.GetKeyDown(KeyCode.Q) && !transitioning)
            {
                transitioning = true;
                cameraPosition = ECameraPosition.OffsetLeft;
                StartCoroutine(CameraSwitch(8));
            }
        }
    }


    private void FixedUpdate()
    {
        switch (cameraPosition)
        {
            case ECameraPosition.Normal:
                desiredPosition = originalPosition.position;
                break;
            case ECameraPosition.OffsetRight:
                desiredPosition = offsetRight.position;
                break;
            case ECameraPosition.OffsetLeft:
                desiredPosition = offsetLeft.position;
                break;
        }
    }

    public IEnumerator CameraSwitch(float seconds)
    {
        float count = 0;
        while (count < seconds) 
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.LookAt(target);
            //Debug.Log("transitioning");
            count += Time.deltaTime;
            yield return null;
        }
        transitioning = false;
        //Debug.Log("done transition");
        yield return 0;
        
    }

    public void shake()
    {
        float shake = Mathf.Pow(trauma, traumaExponent);

        transform.localPosition = new Vector3(
         maximumTranslationShake.x * (Mathf.PerlinNoise(seed, Time.time * frequency) * 2 - 1),
         maximumTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.time * frequency) * 2 - 1),
         maximumTranslationShake.z * (Mathf.PerlinNoise(seed + 2, Time.time * frequency) * 2 - 1)
     ) * shake;

        transform.localRotation = Quaternion.Euler(new Vector3(
            maximumAngularShake.x * (Mathf.PerlinNoise(seed + 3, Time.time * frequency) * 2 - 1),
            maximumAngularShake.y * (Mathf.PerlinNoise(seed + 4, Time.time * frequency) * 2 - 1),
            maximumAngularShake.z * (Mathf.PerlinNoise(seed + 5, Time.time * frequency) * 2 - 1)
        ) * shake);

        trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);

    }

}
