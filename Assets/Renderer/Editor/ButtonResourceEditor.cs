using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SDJK.Renderer.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ButtonResource))]
    public class ButtonResourceEditor : UnityEditor.Editor
    {
        ButtonResource editor;

        void OnEnable() => editor = target as ButtonResource;
        
        public override void OnInspectorGUI()
        {
            UseProperty("TextureName");
            UseProperty("SpriteName");
            UseProperty("SelectedTextureName");
            UseProperty("SelectedSpriteName");
            UseProperty("DisabledTextureName");
            UseProperty("DisabledSpriteName");
            UseProperty("type");
            UseProperty("CustomPath");
            
            if (GUILayout.Button("Rerender"))
                editor.Rerender();

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

        void UseProperty(string propertyName)
        {
            SerializedProperty tps = serializedObject.FindProperty(propertyName);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(tps, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}
