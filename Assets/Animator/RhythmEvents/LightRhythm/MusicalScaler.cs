using UnityEngine;
using SonicBloom.Koreo;

public class MusicalScaler : MonoBehaviour
{
    [EventID]
    public string eventID;

    void Awake()
    {
        Koreographer.Instance.RegisterForEventsWithTime(eventID, OnMusicalTrigger);
    }

    void OnMusicalTrigger(KoreographyEvent evt, int SampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        if (evt.HasCurvePayload())
        {
            float curveValue = evt.GetValueOfCurveAtTime(SampleTime);

            transform.localScale = Vector3.one * curveValue;
        }
    }
}