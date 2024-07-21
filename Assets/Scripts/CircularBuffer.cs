using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class CircularBuffer<T> 
{
    [SerializeField] int max_size;
    [SerializeField] private T[] list;

    public List<T> List { get { return new List<T>(list); } }
    
    public CircularBuffer(int max_size = 5) 
    {
        this.max_size = max_size;
        list = new T[max_size];
    }

    public void Push(T element)
    {
        for(int i = max_size - 1; i > 0; i--)
        {
            list[i] = list[i-1];
        }

        list[0] = element;
    }


}
