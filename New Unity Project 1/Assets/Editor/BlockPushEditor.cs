using UnityEngine;
using UnityEditor;

[CustomEditor( typeof ( BlockPushScript ) )]
public class BlockPushEditor : Editor
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
