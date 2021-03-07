using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor (typeof(ToggleButton))]
[CanEditMultipleObjects]
public class ToggleButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ToggleButton toggleButton = target as ToggleButton;

        SerializedProperty positivePro = serializedObject.FindProperty("PositiveEvent");
        SerializedProperty negativePro = serializedObject.FindProperty("NegativeEvent");

        EditorGUILayout.HelpBox("同じトグルのグループに含めたいボタンを全て同じ階層に置き、" +
            "\n全てにToggleButtonコンポーネントをAddしましょう。", MessageType.Info, true);
        
        toggleButton.change = (ToggleButton.Change)EditorGUILayout.EnumPopup("Change Type", toggleButton.change);

        toggleButton.initChecked = EditorGUILayout.Toggle("Initial Check", toggleButton.initChecked);

        if (toggleButton.change == ToggleButton.Change.ActiveObject)
        {
            toggleButton.targetObject = EditorGUILayout.ObjectField("Target Object",
                toggleButton.targetObject, typeof(GameObject), true)as GameObject;
        }else if(toggleButton.change == ToggleButton.Change.ChangeColor)
        {
            toggleButton.selectedColor = EditorGUILayout.ColorField("Selcted Color", toggleButton.selectedColor);
        }else if(toggleButton.change == ToggleButton.Change.ChangeSprite)
        {
            toggleButton.selectedSprite = EditorGUILayout.ObjectField("Selected Color",
                toggleButton.selectedSprite, typeof(Sprite), true) as Sprite;
        }


        serializedObject.Update();
        EditorGUILayout.PropertyField(positivePro);
        EditorGUILayout.PropertyField(negativePro);
        serializedObject.ApplyModifiedProperties();




        EditorUtility.SetDirty(target);
        if (GUI.changed)
        {
            foreach (Object buttonObject in targets)
            {
                ToggleButton subButton = buttonObject as ToggleButton;

                subButton.change = toggleButton.change;
                subButton.selectedColor = toggleButton.selectedColor;
                subButton.selectedSprite = toggleButton.selectedSprite;

                EditorUtility.SetDirty(buttonObject);
            }
        }
    }
}