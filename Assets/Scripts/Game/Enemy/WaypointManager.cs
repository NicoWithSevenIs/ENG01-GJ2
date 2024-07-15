using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class WaypointManager : MonoBehaviour
{

    [SerializeField] internal Transform[,] _waypoints;

    public Transform[,] Waypoints { get { return _waypoints; } }

   
    public Transform getNearestWaypoint(Vector3 position)
    {
        Transform nearest = _waypoints[0,0];

        for(int i =0; i < _waypoints.GetLength(0); i++)
        {

            for(int j =0; j < _waypoints.GetLength(1); j++)
            {

                if (Vector3.Distance(position, _waypoints[i,j].position) < Vector3.Distance(position, nearest.position) )
                    nearest = _waypoints[i,j];

            }

        }

        return nearest;

    }

    public List<Transform> getAdjacentWaypoints(Transform waypoint)
    {
        List<Transform> adjacentWaypoints = new List<Transform>();

        for (int i = 0; i < _waypoints.GetLength(0); i++)
        {
            for (int j = 0; j < _waypoints.GetLength(1); j++)
            {
                if (_waypoints[i,j] == waypoint)
                {

                    if(i > 0)
                        adjacentWaypoints.Add(_waypoints[i - 1, j]);

                    if(i < _waypoints.GetLength(0) - 1)
                        adjacentWaypoints.Add(_waypoints[i + 1, j]);

                    if(j > 0)
                        adjacentWaypoints.Add(_waypoints[i, j - 1]);

                    if (j > _waypoints.GetLength(1) - 1)
                        adjacentWaypoints.Add(_waypoints[i, j + 1]);

                    break;

                }
            }
        }

        return adjacentWaypoints;
    }
}



