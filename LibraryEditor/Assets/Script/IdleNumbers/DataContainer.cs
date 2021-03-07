using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton
public class DataContainer<T> 
{
    Dictionary<string, T> dictionary;
    static DataContainer<T> instance;
    private DataContainer() { }
    //Public
    public T GetDataByName(string name)
    {
        return dictionary[name];
    }
    public void SetDataByName(T instance, string name)
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
