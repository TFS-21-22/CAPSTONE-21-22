using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public float score;
    public float scoreMulitplyer = 1;

    bool collectableHit;

    public float time;
    public float timer = 5.0f;

    public transition2 transition;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        collectableHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!collectableHit && timer < 5.0f)
        {
            timer = 5.0f;
            //StartCoroutine(ScoreAdder());
        }

        if(time >= timer)
        {
            time = 0.0f;
            timer = 5.0f;

            scoreMulitplyer = 1.0f;

            collectableHit = false;
        }
    }


    IEnumerator ScoreAdder()
    {
        while (time < timer && collectableHit)
        {
            time += Time.fixedDeltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(timer);
      
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lily"))
        {
            collectableHit = true;
            score += 1;
            scoreMulitplyer += 1;
            timer += 2.0f;
            time = 0.0f;
            StartCoroutine(ScoreAdder());

            if(scoreMulitplyer > 30.0f)
            {
                scoreMulitplyer = 30.0f;
            }
           
        }
    }
}
