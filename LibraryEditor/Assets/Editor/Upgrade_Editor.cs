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
        
        //リソースの種類を指定させます。
        EditorGUILayout.LabelField("コストとして使用するリソースの数と種類を指定してください");
        var resourseNames = serializedObject.FindProperty("resourseNames");
        EditorGUILayout.PropertyField(resourseNames, true);
        if (resourseNames.arraySize == 0)
            return;

        //コストの計算方法を指定させます。
        var costkind = serializedObject.FindProperty("costkind");
        costkind.enumValueIndex = EditorGUILayout.Popup(
            "コストの計算方法",
            costkind.enumValueIndex,
            new string[] { "線形関数", "指数関数" }
            );

        if (costkind.enumValueIndex == (int)CostKind.linear)
        {
            var initialValue = serializedObject.FindProperty("linear_initialValue");
            var steep = serializedObject.FindProperty("linear_steep");
            initialValue.doubleValue = EditorGUILayout.DoubleField("コストの初期値",initialValue.doubleValue);
            steep.doubleValue = EditorGUILayout.DoubleField("コストの傾き",steep.doubleValue);
        }

        EditorGUILayout.Space();

        //効果を設定させます。
        EditorGUILayout.LabelField("効果設定", bold);
        var effectKind = serializedObject.FindProperty("effectKind");
        effectKind.enumValueIndex = EditorGUILayout.Popup(
            "アップグレードの効果",
            effectKind.enumValueIndex,
            new string[] { "Number型を強化します。", "Cal型を強化します。", "クリックを強化します。", "自動生産を強化します。" }
            );

        //強化対象
        //cal以外を選んでいれば
        if(effectKind.enumValueIndex == (int)EffectKind.Cal)
        {
            var targetCal = serializedObject.FindProperty("targetCal");
            EditorGUILayout.PropertyField(targetCal, true);
        }
        else
        {
            var targetNumber = serializedObject.FindProperty("targetNumber");
            EditorGUILayout.PropertyField(targetNumber, true);
        }

        //加算か乗算か？
        var calway = serializedObject.FindProperty("calway");
        calway.enumValueIndex = EditorGUILayout.Popup(
            "加算か乗算か？",
            calway.enumValueIndex,
            new string[] {"加算", "乗算"}
            );


        //　シリアライズオブジェクトのプロパティの変更を更新
        serializedObject.ApplyModifiedProperties();
    }
}