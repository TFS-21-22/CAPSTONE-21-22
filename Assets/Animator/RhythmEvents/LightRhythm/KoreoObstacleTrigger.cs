using UnityEngine;
using SonicBloom.Koreo;

public class KoreoObstacleTrigger : MonoBehaviour
{
    public Animation animCom;

    [EventID]
    public string eventID;

    void Awake()
    {
        Koreographer.Instance.RegisterForEvents(eventID, OnAnimationTrigger);
    }

    void OnAnimationTrigger(KoreographyEvent evt)
    {
        animCom.Stop();
        animCom.Play();
    }
}