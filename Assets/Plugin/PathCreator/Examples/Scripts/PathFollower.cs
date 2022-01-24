using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public static PathFollower instance;
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;
        float time;

        public CPManager cpManager;
        public PathCreator[] otherPaths;

        public int checkPoint;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else if(instance != null)
            {
                Destroy(this);
            }
        }

        void Start() 
        {
            cpManager = GameObject.FindObjectOfType<CPManager>();
            checkPoint = cpManager.checkPoint;
            pathCreator = otherPaths[cpManager.checkPoint];

            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            checkPoint = cpManager.checkPoint;
            //Debug.Log("Player speed: " + speed);

            if (pathCreator != null && !PauseMenuManager.instance.isPaused)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}