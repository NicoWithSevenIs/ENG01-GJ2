using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wrapper<T>
{
    [SerializeField] public List<T> arr;
}

public class WaypointManager : MonoBehaviour
{

    [SerializeField] private List<Wrapper<Transform>> list;

    private  Transform[,] _waypoints;

    public Transform[,] Waypoints { get { return _waypoints; } }

    private void Awake()
    {
        _waypoints = new Transform[list.Count, list[0].arr.Count];

        for(int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list[i].arr.Count; j++)
            {
                _waypoints[i,j] = list[i].arr[j];
            }
        }


    }
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

                    if (j < _waypoints.GetLength(1) - 1)
                        adjacentWaypoints.Add(_waypoints[i, j + 1]);

                    break;

                }
            }
        }

        return adjacentWaypoints;
    }


    public List<Transform> getAllWaypointFromDistanceN(Transform waypoint, int chebyshevDist)
    {

        int getChebyshevDist(int x1, int y1, int x2, int y2)
        {
            return Mathf.Max( Mathf.Abs(x1-x2), Mathf.Abs(y1-y2) );
        }


        List<Transform> selected = new List<Transform>();

        int row = -1;
        int col = -1;

        for(int i =0; i < _waypoints.GetLength(0); i++)
            for(int j = 0; j < _waypoints.GetLength(1); j++)          
                if (_waypoints[i,j] == waypoint)
                {
                    row = i; 
                    col = j; 
                    break;
                }


        for (int i = 0; i < _waypoints.GetLength(0); i++)
        {
            for (int j = 0; j < _waypoints.GetLength(1); j++)
            {

                if (getChebyshevDist(row, col, i ,j) == chebyshevDist)
                {
                    selected.Add(_waypoints[i,j]);

                    /*
                    Light l =  _waypoints[i,j].AddComponent<Light>();
                    l.type = LightType.Point;
                    l.color= Color.red;
                    l.intensity = 5;
                    */
                }

            }
        }
          



        return selected;
    }
}



