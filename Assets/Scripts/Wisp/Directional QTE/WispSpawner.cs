using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispSpawner : MonoBehaviour
{
    public int direction;
    public Transform[] spawns;

    public Rigidbody wispPrefab;
    public GameObject player;
    public QuickTimeEvent QTE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Spawn(0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Spawn(1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Spawn(1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Spawn(2);
        }
        */
    }

    public void Spawn(KeyCode dir)
    {
        Rigidbody temp;
        print(dir);
        if (dir == KeyCode.LeftArrow)
        {
            temp = Instantiate(wispPrefab, spawns[0].position, spawns[0].rotation);
            temp.GetComponent<WispMovement>().player = player;
            temp.GetComponent<WispMovement>().QTE = QTE;          
        }
        else if (dir == KeyCode.UpArrow)
        {
            temp = Instantiate(wispPrefab, spawns[1].position, spawns[1].rotation);
            temp.GetComponent<WispMovement>().player = player;
            temp.GetComponent<WispMovement>().QTE = QTE;
        }
        else if (dir == KeyCode.DownArrow)
        {
            temp = Instantiate(wispPrefab, spawns[1].position, spawns[1].rotation);
            temp.GetComponent<WispMovement>().player = player;
            temp.GetComponent<WispMovement>().QTE = QTE;
        }
        else if (dir == KeyCode.RightArrow)
        {
            temp = Instantiate(wispPrefab, spawns[2].position, spawns[2].rotation);
            temp.GetComponent<WispMovement>().player = player;
            temp.GetComponent<WispMovement>().QTE = QTE;
        }



        //to destroy wisp, if ANY arrow is pressed and succesful, destroy wisp that has been alive for blank amount of time (or is in certain range window)
        //this will accurately destroy the corresponding wisp without requiring any references

        // Destroy wisp, this is if the wisp reachs the player or has been alive for a certain amount of time

    }
}
