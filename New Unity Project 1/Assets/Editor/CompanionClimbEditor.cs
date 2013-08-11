using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CompanionClimbScript))]
public class CompanionClimbEditor : Editor
{

    private SerializedProperty _active;

    void OnEnable()
    {
        _active = serializedObject.FindProperty( "objectActive" );
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "When ticked, the object is visible and climbable. If unticked, the object can't be navigated",
            MessageType.Info );
        _active.boolValue = EditorGUILayout.Toggle( "Is Active", _active.boolValue );
        serializedObject.ApplyModifiedProperties();
    }
}
