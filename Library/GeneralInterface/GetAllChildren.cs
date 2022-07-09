using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace IdleLibrary
{
    public static class GetAllChildren
    {
        public static List<GameObject> GetAllByType<Type>(this GameObject obj) where Type : Component
        {
            List<GameObject> allChildren = new List<GameObject>();
            if (obj.HasComponent<Type>())
            {
                allChildren.Add(obj);
            }
            GetChildren(obj, ref allChildren);
            return allChildren;
        }

        public static List<IUpdateText> GetAllUpdateText(this GameObject obj)
        {
            List<GameObject> allChildren = new List<GameObject>();
            GetChildren(obj, ref allChildren);

            List<IUpdateText> unko = new List<IUpdateText>();
            allChildren.ForEach((x) => { if (x.GetComponent<IUpdateText>() != null) unko.Add(x.GetComponent<IUpdateText>()); });
            return unko;
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
    }


    public static class GameObjectExtensions
    {
        /// <summary>
        /// 指定されたコンポーネントがアタッチされているかどうかを返します
        /// </summary>
        public static bool HasComponent<T>(this GameObject self) where T : Component
        {
            if (self != null)
            {
                return self.GetComponent<T>() != null;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 指定されたインターフェイスを実装したコンポーネントを持つオブジェクトを検索します
        /// </summary>
        public static T FindObjectOfInterface<T>() where T : class
        {
            foreach (var n in GameObject.FindObjectsOfType<Component>())
            {
                var component = n as T;
                if (component != null)
                {
                    return component;
                }
            }
            return null;
        }
    }
}