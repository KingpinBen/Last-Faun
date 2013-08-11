using UnityEngine;
using UnityEditor;

[CustomEditor( typeof ( LeverSwitchScript ) )]
public class LeverEditor : Editor
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
        EditorGUILayout.HelpBox("Levers are _not_ a 'State' object, and instead are only used to force a target object to switch state.\nIf you want to use this as a state type object, set the output object to a new Logic Container that has no input or output.", MessageType.Warning);

        _active.boolValue = EditorGUILayout.Toggle( "Is Active", _active.boolValue );
        _target.objectReferenceValue = EditorGUILayout.ObjectField( "Target Object", _target.objectReferenceValue,
                                                                    typeof ( InteractiveObject ), true );

        serializedObject.ApplyModifiedProperties();
    }
}
