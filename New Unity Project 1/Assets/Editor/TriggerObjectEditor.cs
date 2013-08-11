using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TriggerObjectScript))]
public class TriggerObjectEditor : Editor
{

    private SerializedProperty _active;
    private SerializedProperty _target;

    void OnEnable()
    {
        _active = serializedObject.FindProperty( "objectActive" );
        _target = serializedObject.FindProperty( "targetObject" );
    }

    public override void OnInspectorGUI()
    {
        _active.boolValue = EditorGUILayout.Toggle( "Is Active", _active.boolValue );
        _target.objectReferenceValue = EditorGUILayout.ObjectField( "Target Object", _target.objectReferenceValue,
                                                                    typeof ( InteractiveObject ), true );

        serializedObject.ApplyModifiedProperties();
    }
}
