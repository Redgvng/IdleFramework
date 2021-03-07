using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UpgradeLibrary;

//　カスタマイズするクラスを設定
[CustomEditor(typeof(Upgrade_Mono))]
//　Editorクラスを継承してクラスを作成
public class Upgrade_Editor : Editor
{
    static GUIStyle bold = new GUIStyle();
    public override void OnInspectorGUI()
    {
        bold.fontStyle = FontStyle.Bold;
        bold.normal.textColor = Color.white;
        //　シリアライズオブジェクトの更新
        serializedObject.Update();

        EditorGUILayout.LabelField("コスト設定", bold);
        var costkind = serializedObject.FindProperty("costkind");
        costkind.enumValueIndex = EditorGUILayout.Popup(
            "コストの計算方法",
            costkind.enumValueIndex,
            new string[] { "線形関数", "指数関数" }
            );

        if(costkind.enumValueIndex == (int)CostKind.linear)
        {
            var initialValue = serializedObject.FindProperty("linear_initialValue");
            var steep = serializedObject.FindProperty("linear_steep");
            initialValue.doubleValue = EditorGUILayout.DoubleField("コストの初期値",initialValue.doubleValue);
            steep.doubleValue = EditorGUILayout.DoubleField("コストの傾き",steep.doubleValue);
        }

        //　シリアライズオブジェクトのプロパティの変更を更新
        serializedObject.ApplyModifiedProperties();
    }
}