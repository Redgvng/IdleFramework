using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UsefulMethod;

public static class UsefulStatic
{
    public static Type GetOrAddComponent<Type>(this GameObject gameObject)
        where Type : UnityEngine.Component
    {
        return gameObject.GetComponent<Type>() ?? gameObject.AddComponent<Type>();
    }


    public static List<GameObject> GetAll(this GameObject obj)
    {
        List<GameObject> allChildren = new List<GameObject>();
        GetChildren(obj, ref allChildren);
        return allChildren;
    }

    //子要素を取得してリストに追加
    public static void GetChildren(GameObject obj, ref List<GameObject> allChildren)
    {
        Transform children = obj.GetComponentInChildren<Transform>();
        //子要素がいなければ終了
        if (children.childCount == 0)
        {
            return;
        }
        foreach (Transform ob in children)
        {
            allChildren.Add(ob.gameObject);
            GetChildren(ob.gameObject, ref allChildren);
        }
    }


    /// <summary>
    /// １次元配列を２次元配列に変換する関数
    /// </summary>
    public static Type[,] ArrayTo2D<Type>(this Type[] Obj, int First, int Second)
    {
        Type[,] rtnTypes = new Type[First, Second];
        for (int i_f = 0; i_f < First; i_f++)
        {
            for (int i_s = 0; i_s < Second; i_s++)
            {
                rtnTypes[i_f, i_s] = Obj[i_f * Second + i_s];
            }
        }
        return rtnTypes;
    }
}
