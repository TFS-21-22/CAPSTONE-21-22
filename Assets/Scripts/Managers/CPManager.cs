using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace PathCreation.Examples
{
    public class CPManager : MonoBehaviour
    {
        static CPManager _instance = null;
        public static CPManager instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        public PathFollower Player;
        public int checkPoint;

        //public PathCreator pathCreator;
        // Start is called before the first frame update
        void Start()
        {
            if (instance)
            {
                Destroy(gameObject);

            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            //Player.pathCreator = Player.otherPaths[0];
            checkPoint = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.instance.health <= 0)
            {
                checkPoint = 0;
                SceneManager.LoadScene("LevelDesignBlockout");
                GameManager.instance.health = 3;
            }
            if (Input.GetKeyDown("1"))
            {
                checkPoint = 0;
                SceneManager.LoadScene("LevelDesignBlockout");
            }
            if (Input.GetKeyDown("2"))
            {
                checkPoint = 1;
                SceneManager.LoadScene("LevelDesignBlockout");
            }
            if (Input.GetKeyDown("3"))
            {
                checkPoint = 2;
                SceneManager.LoadScene("LevelDesignBlockout");
            }
        }
    }
}