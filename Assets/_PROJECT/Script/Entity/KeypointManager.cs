using UnityEngine;

public class KeypointManager : MonoBehaviour
{

    [SerializeField] private Transform[] keypoints;
    [SerializeField] private Transform[] keypoints2;

    public Transform GetNextWaypoint(int waypointIndex)
    {
        return keypoints[waypointIndex];
    }
    public Transform GetNextWaypoint2(int waypointIndex)
    {
        return keypoints2[waypointIndex];
    }

    public bool CheckEnd(int waypointIndex)
    {
        if (waypointIndex >= keypoints.Length)
            return true;

        return false;
    }
    public bool CheckEnd2(int waypointIndex)
    {
        if (waypointIndex >= keypoints2.Length)
            return true;

        return false;
    }
}