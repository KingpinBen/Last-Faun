using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TimerLogicBehaviourScript))]
public class TimedLogicEditor : Editor
{
    private SerializedProperty _active;
    private SerializedProperty _countDown;
    private SerializedProperty _targetObjects;

    void OnEnable()
    {
        _active = serializedObject.FindProperty( "objectActive" );
        _countDown = serializedObject.FindProperty( "countdownToActivate" );
        _targetObjects = serializedObject.FindProperty( "targetObjects" );
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Only flips the state of 'TargetObjects' when this becomes active.\nIf it deactivates before finishing, it cancels the countdown.", MessageType.Info);

        _active.boolValue = EditorGUILayout.Toggle( "Is Active", _active.boolValue );
        _countDown.floatValue = EditorGUILayout.FloatField( "Countdown (seconds) ", _countDown.floatValue );
        EditorGUILayout.PropertyField( _targetObjects, true );

        serializedObject.ApplyModifiedProperties();
    }
}
