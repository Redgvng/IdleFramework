using System;
using System.Reflection;
using UnityEngine;

namespace IdleLibrary.IntegrationTest
{
    public static class ClassExtension
    {
        public static T DeepCopy<T>(this T self) where T : class
        {
            var ret = Activator.CreateInstance(typeof(T), true) as T;
            var type = self.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
                field.SetValue(ret, field.GetValue(self));

            return ret;
        }


        // クラス複製関数
        public static T CopyNode<T>(this T node) where T : class
        {
            // パラメータのクラスクラスタイプ取得
            Type type = node.GetType();
            // クラス生成する。
            T clone = (T)Activator.CreateInstance(type, true);
            // クラス内部のすべての変数を取得する。
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                // 変数がClassタイプなら再帰方法でクラスを複製する。ただ、Stringはクラスが構造体みたいに使うので例外
                if (field.FieldType.IsClass && field.FieldType != typeof(String) && field.FieldType != typeof(Func<double>))
                {
                    Debug.Log(field.FieldType);
                    // 新しいクラスにデータを格納
                    field.SetValue(clone, CopyNode(field.GetValue(node)));
                    continue;
                }
                // 新しいクラスにデータを格納
                field.SetValue(clone, field.GetValue(node));
            }
            return clone;
        }
    }

}