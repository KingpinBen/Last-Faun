using UnityEngine;
using UnityEditor;

[CustomEditor( typeof ( LogicBehaviourScript ) )]
public class LogicBehaviourEditor : Editor
{
    private SerializedProperty _active;
    private SerializedProperty _requiredObjects;
    private SerializedProperty _targetObjects;
    private SerializedProperty _shouldLock;

    void OnEnable()
    {
        _active = serializedObject.FindProperty( "objectActive" );
        _requiredObjects = serializedObject.FindProperty( "requiredObjects" );
        _targetObjects = serializedObject.FindProperty( "targetObjects" );
        _shouldLock = serializedObject.FindProperty( "lockAfterUsed" );
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Used as a 'State' object.\n\nAll objects in 'RequiredObjects' must be at their required active state to flip the state of this object (unless forced when this is a target object itself).\n\n" +
                                "When changing active state, all 'TargetObjects' active states are flipped.\n\n" +
                                "If 'Lock After Used' is ticked, once the object is in an active state, it can't be triggered again.", MessageType.Info);

        _active.boolValue = EditorGUILayout.Toggle( "Is Active", _active.boolValue );
        EditorGUILayout.PropertyField( _requiredObjects, true );
        EditorGUILayout.PropertyField( _targetObjects, true );
        _shouldLock.boolValue = EditorGUILayout.Toggle( "Lock once Active", _shouldLock.boolValue );

        serializedObject.ApplyModifiedProperties();
    }
}
