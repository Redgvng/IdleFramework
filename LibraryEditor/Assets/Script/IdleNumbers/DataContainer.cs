using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Singleton
public class DataContainer<T> 
{
    public static Dictionary<Enum, T> dictionary = new Dictionary<Enum, T>();
    static DataContainer<T> instance;
    private DataContainer() { }
    //Public
    public T GetDataByName(Enum name)
    {
        return dictionary[name];
    }
    public void SetDataByName(T instance, Enum name)
    {
        dictionary[name] = instance;
    }
    public static DataContainer<T> GetInstance()
    {
        if (instance == null)
        {
            return new DataContainer<T>();
        }
        return instance;
    }
}
