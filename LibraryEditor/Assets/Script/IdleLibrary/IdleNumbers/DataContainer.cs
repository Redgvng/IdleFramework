using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleLibrary
{
    public interface IDataContainer<T>
    {
        T GetDataByName(Enum name);
        void SetDataByName(T instance, Enum name);
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
}
