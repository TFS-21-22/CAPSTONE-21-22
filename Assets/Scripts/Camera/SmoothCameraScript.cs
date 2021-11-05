using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SmoothCameraScript : MonoBehaviour
{
    [SerializeField] private Transform cameraLookPoint;

    public Transform target;

    public Transform targetDeath;

    public Transform targetPlayer;

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


    public enum ECameraPosition
    {
        Normal,
        OffsetRight,
        OffsetLeft
    }
    public ECameraPosition cameraPosition = ECameraPosition.Normal;

    private void Update()
    {
        transform.LookAt(cameraLookPoint);

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

    IEnumerator DeathWaitTime(float wait)
    {
        yield return new WaitForSeconds(wait);
        QuitGame();
    }


    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

}
