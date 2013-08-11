using UnityEngine;
using UnityEditor;

[CustomEditor( typeof ( BoostPlatformScript ) )]
public class BoostPlatformEditor : Editor
{
    private SerializedProperty _active;

    void OnEnable()
    {
        _active = serializedObject.FindProperty( "objectActive" );
    }

    public override void OnInspectorGUI()
    {
        _active.boolValue = EditorGUILayout.Toggle( "Is Useable", _active.boolValue );
        serializedObject.ApplyModifiedProperties();
    }
}
