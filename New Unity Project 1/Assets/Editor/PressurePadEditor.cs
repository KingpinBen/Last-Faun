using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PressurePadScript))]
public class PressurePadEditor : Editor
{
    private bool _showHelp;

    private SerializedProperty _requiredObjects;
    private SerializedProperty _targetObjects;
    private SerializedProperty _needToHoldDown;

    void OnEnable()
    {
        _requiredObjects = serializedObject.FindProperty( "requiredObjects" );
        _targetObjects = serializedObject.FindProperty( "targetObjects" );
        _needToHoldDown = serializedObject.FindProperty( "holdSwitch" );
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "Pressure pads are a 'State' object. When newly triggered, it will flip its active state between 'On' and 'Off'.\n\n" +
            "When 'RequiredObjects' is empty, all objects can trigger it. When objects are added, only those listed can trigger it.\n\n" +
            "When the PressurePad switches its state, all the objects in 'TargetObjects' will also have their state flipped. This can be left empty if you solely want it to have its state watched\n\n"+
        "If 'Is Toggle Switch' is ticked, when the switch is triggered it will remain active (pushed) until it's triggered again. If it isn't ticked, when all valid objects have stopped applying pressure, it will reset and turn off.",
            MessageType.Info );
        EditorGUILayout.PropertyField( _requiredObjects, true );
        EditorGUILayout.PropertyField(_targetObjects, true);

        _needToHoldDown.boolValue = !EditorGUILayout.Toggle( "Is Toggle Switch", !_needToHoldDown.boolValue );
        serializedObject.ApplyModifiedProperties();
    }
}
