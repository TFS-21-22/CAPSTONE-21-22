using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EBeatScore
{
    missed,
    early,
    ok,
    good,
    perfect
}


public class RhythmCanvas : MonoBehaviour
{
    public Image xButton;
    public Image xCircle;
    Vector3 xScale = new Vector3(3, 3, 3);
    Vector3 xScaleBig = new Vector3(3.5f, 3.5f, 3.5f);
    Vector3 bigCircle = new Vector3(6, 6, 6);
    bool scaling;
    bool pulsing = false;
    float timeBetweenBeats = 3.8f;
    float flux = 1.32f;
    public GameObject enemy;
    Vector3 enemyScale;
    public EBeatScore beatScore;
    float beatTime = 0.0f;
    int scaleCount = 0;
    int tempBeat = 0;


    // Start is called before the first frame update
    void Start()
    {
        enemyScale = enemy.transform.localScale;
        scaling = false;
        BeatMaster.Beat += BeatCheck;
        BeatMaster.Beat += BeatX;
    }

    // Update is called once per frame
    void Update()
    {
        if(pulsing && Input.GetButtonDown("Jump"))
        {
            StartCoroutine(DestroyEnemy());
            LeanTween.alpha(enemy, 0, 6);

            if (beatTime <= 0.15f)
                beatScore = EBeatScore.perfect;

            if (beatTime > 0.15f && beatTime <= 0.3f)
                beatScore = EBeatScore.good;

            if (beatTime > 0.3f && beatTime <= 0.5f)
                beatScore = EBeatScore.ok;

            if (beatTime > 0.5f)
                beatScore = EBeatScore.missed;
        }

        if (pulsing)
            beatTime += Time.deltaTime;
        else
            beatTime = 0;
            
    }

    public void BeatCheck(int beat)
    {
    
        if ((beat+3) % 4 == 0 && !pulsing && !scaling)
        {
            StartCoroutine(Scale());
        }

        if (scaling)
        {
            if(beat != tempBeat)
            {
                scaleCount++;
            }
            tempBeat = beat;
        }
        else
        {
            scaleCount = 0;
            tempBeat = 0;
        }
       


    }

    IEnumerator Scale()
    {
        Debug.Log("beat");
        //xCircle.gameObject.SetActive(true);
        LeanTween.scale(xCircle.gameObject, bigCircle, 0.15f);
        scaling = true;
        //float count = 0f;
        while (scaleCount < 9)
        {
            xCircle.transform.localScale -= new Vector3(flux * Time.deltaTime, flux * Time.deltaTime, flux * Time.deltaTime);
            //count += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(XPulse());
        pulsing = true;
    }


    public void BeatX(int beat)
    {
        if(pulsing == true)
        StartCoroutine(XPulse());
    }

    IEnumerator XPulse()
    {
        int id  = LeanTween.scale(xButton.gameObject, xScaleBig, 0.15f).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }
        xButton.transform.localScale = xScale;
    }

    IEnumerator DestroyEnemy()
    {
        Debug.Log(beatScore);
        int id = LeanTween.scale(enemy, xScale/1.5f, 0.4f).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }
        int id2 = LeanTween.alpha(enemy, 0, 10f).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }
        pulsing = false;
        scaling = false;
        enemy.transform.localScale = enemyScale;
        enemy.SetActive(false);
        gameObject.SetActive(false);
    }


   


}
