using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ButtonKeyScript))]
public class ButtonKeyEditor : Editor {
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Script is used for validating it's a key. Nothing else to do here :[", MessageType.None);
    }
}
