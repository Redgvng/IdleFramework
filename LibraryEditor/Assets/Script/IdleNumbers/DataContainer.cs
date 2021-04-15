using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDataContainer<T>
{
    public T GetDataByName(Enum name);
    public void SetDataByName(T instance, Enum name);
}
//Singleton
public class DataContainer<T> : IDataContainer<T>
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
