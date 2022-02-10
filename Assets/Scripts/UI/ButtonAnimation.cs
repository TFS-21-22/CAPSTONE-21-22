using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    public void Shake()
    {
        StartCoroutine(ShakeAnimation());
    }

    IEnumerator ShakeAnimation()
    {
        int id = LeanTween.rotate(this.gameObject, new Vector3(0,0,20), 1f).id;

        while (LeanTween.isTweening(id))
        {
            yield return null;
        }

        int id2 = LeanTween.rotate(this.gameObject, new Vector3(0, 0, -20), 1f).id;

        while (LeanTween.isTweening(id2))
        {
            yield return null;
        }
    }

}
