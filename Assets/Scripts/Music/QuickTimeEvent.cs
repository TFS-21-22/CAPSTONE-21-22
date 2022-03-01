using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuickTimeEvent : MonoBehaviour
{
    private int poseCount;
    private float beatTempo;
    public Queue<GameObject> pool = new Queue<GameObject>();
    [SerializeField] private GameObject[] directionalButtons;
    [SerializeField] private RectTransform buttonStartPosition;
    [SerializeField] private Canvas canvas;

    void Awake()
    {
        foreach (GameObject button in directionalButtons)
        {
            GameObject temp = Instantiate(button, buttonStartPosition) as GameObject;
            temp.AddComponent<Image>();
            temp.transform.parent = canvas.gameObject.transform;
            RectTransform tempTransform = temp.GetComponent<RectTransform>();
            tempTransform = buttonStartPosition;
            pool.Enqueue(temp);
            //temp.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        BeatMaster.Beat += BeatCheckQTE;
        beatTempo = BeatMaster.instance.BPM / 60;
    }

    private void BeatCheckQTE(int beat)
    {
        if ((beat + 3) % 4 == 0)
        {
            //StartCoroutine(StartQuickTimeEvent());
        }
    }
    
    IEnumerator StartQuickTimeEvent()
    {
        GameObject getButton = pool.Dequeue();

        getButton.SetActive(true);
        getButton.transform.localPosition -= new Vector3(beatTempo * Time.deltaTime, 0f, 0f);
        return null;
    }
    
}
