using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public float score;
    public float scoreMulitplyer = 1;

    public transition2 transition;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    IEnumerator ScoreAdder()
    {
        score *= scoreMulitplyer;
        yield return new WaitForSeconds(5);
        scoreMulitplyer = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lily"))
        {
            score += 1;
            scoreMulitplyer += 1;
            StartCoroutine(ScoreAdder());
        }
    }
}
