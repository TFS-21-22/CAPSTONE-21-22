using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public float score;
    public float scoreMulitplyer = 1;

    bool transitionActive;

    public float time;
    public float timer = 5.0f;

    public transition2 transition;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        transitionActive = false;
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    IEnumerator ScoreAdder()
    {
        score *= scoreMulitplyer;
        yield return new WaitForSeconds(timer);
        scoreMulitplyer = 1;
        transitionActive = false;
        time = 0f;
        timer = 0f;
        while (time < timer)
        {
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lily"))
        {
            transitionActive = true;
            score += 1;
            scoreMulitplyer += 1;
            timer += 5.0f;
            StartCoroutine(ScoreAdder());
        }
    }
}
