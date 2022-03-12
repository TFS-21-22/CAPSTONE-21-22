using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using SonicBloom.Koreo;

public class MusicalAnimTrigger : MonoBehaviour
{
    public Animation animCom;


    [EventID]
    public string eventID;

    public int poseCount;

    public Animator anim;

    QuickTimeEvent qte;

    void Awake()
    {
        Koreographer.Instance.RegisterForEvents(eventID, OnAnimationTrigger);
    }

    private void Update()
    {
        if (anim)
        {
            anim.SetInteger("POSECOUNT", poseCount);
        }
    }

    void OnAnimationTrigger(KoreographyEvent evt)
    {
        //animCom.Stop();
        

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            poseCount = 1;
           // animCom.Play();
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            poseCount = 2;
           // animCom.Play();
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            poseCount = 3;
          //  animCom.Play();
        }
     
        
    }






}