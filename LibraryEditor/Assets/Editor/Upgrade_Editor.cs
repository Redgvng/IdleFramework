using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UpgradeLibrary;
using System;

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
        EditorGUILayout.LabelField("コストとして使用するリソースの数を指定してください");
        var resourseNum = serializedObject.FindProperty("resourceNum");
        resourseNum.intValue = EditorGUILayout.IntField("リソースの数", resourseNum.intValue);
        if (resourseNum.intValue == 0)
            return;
        if(resourseNum.intValue > 4)
        {
            EditorGUILayout.LabelField("1 ～ 4の値を指定してください");
            return;
        }

        //配列を取得します。
        var targetObject = (Upgrade_Mono)target;
        var costInfoArray = targetObject.costInfo;
        for (int i = 0; i < resourseNum.intValue; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                int index = i + 1;
                costInfoArray[i].resource = (NumbersName)EditorGUILayout.Popup(
                    index + "つ目のリソース",
                    (int)costInfoArray[i].resource,
                    Enum.GetNames(typeof(NumbersName))
                    );
                costInfoArray[i].costKind = (CostKind)EditorGUILayout.Popup(
                    "コストの計算方法",
                    (int)costInfoArray[i].costKind,
                    new string[] { "線形関数", "指数関数" }
                    );
                //線形か指数かでラベルを変える。
                if(costInfoArray[i].costKind == CostKind.linear)
                {
                    EditorGUILayout.LabelField("初期値");
                    costInfoArray[i].factor1 = EditorGUILayout.DoubleField(costInfoArray[i].factor1);
                    EditorGUILayout.LabelField("傾き");
                    costInfoArray[i].factor2 = EditorGUILayout.DoubleField(costInfoArray[i].factor2);
                }
            }
            EditorGUILayout.EndVertical();
        }

        //コストの計算方法を指定させます。
        /*
        var costkind = serializedObject.FindProperty("costkind");
        costkind.enumValueIndex = EditorGUILayout.Popup(
            "コストの計算方法",
            costkind.enumValueIndex,
            new string[] { "線形関数", "指数関数" }
            );

        if (costkind.enumValueIndex == (int)CostKind.linear)
        {
            var linearInfo = serializedObject.FindProperty("linearInfo"); 
            EditorGUILayout.LabelField("コストの各情報を入力してください");
            linearInfo.arraySize = resourseNum.intValue;
            EditorGUILayout.PropertyField(linearInfo);
        }
        */

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

        //レベル当たりの上昇率を入力してください。
        var valuePerLevel = serializedObject.FindProperty("valuePerLevel");
        valuePerLevel.doubleValue = EditorGUILayout.DoubleField("1レベル当たりの上昇量", valuePerLevel.doubleValue);


        //　シリアライズオブジェクトのプロパティの変更を更新
        serializedObject.ApplyModifiedProperties();
    }
}

