using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoulderScript))]
public class BoulderEditor : Editor
{
    private SerializedProperty _active;

    void OnEnable()
    {
        _active = serializedObject.FindProperty( "objectActive" );
    }

    public override void OnInspectorGUI()
    {
        _active.boolValue = EditorGUILayout.Toggle( "Is Active", _active.boolValue );

        serializedObject.ApplyModifiedProperties();
    }
}
