using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SDJK.Renderer.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ImageResource))]
    public class ImageResourceEditor : UnityEditor.Editor
    {
        ImageResource editor;

        void OnEnable() => editor = target as ImageResource;
        
        public override void OnInspectorGUI()
        {
            UseProperty("TextureName");
            UseProperty("SpriteName");
            UseProperty("Color");
            UseProperty("type");
            UseProperty("CustomPath");
            UseProperty("Border");
            
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
