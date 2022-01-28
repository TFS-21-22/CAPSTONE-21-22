using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRotation : MonoBehaviour
{
    public float speed = 0f;
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    // Update is called once per frame
    void Update()
    {
        if(!PauseMenuManager.instance.isPaused)
        rectTransform.Rotate(new Vector3(0, 0, 10));
    }
}
