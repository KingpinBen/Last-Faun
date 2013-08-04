using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DoorScript))]
public class DoorEditorScript : Editor
{
    private SerializedProperty _rotates;
    private SerializedProperty _closeSpeed;
    private SerializedProperty _openSpeed;
    private SerializedProperty _pivot;
    private SerializedProperty _doorTransform;
    private SerializedProperty _distance;


    void OnEnable()
    {
        _rotates = serializedObject.FindProperty("rotates");
        _closeSpeed = serializedObject.FindProperty("closingSpeed");
        _openSpeed = serializedObject.FindProperty("openingSpeed");
        _pivot = serializedObject.FindProperty("pivot");
        _doorTransform = serializedObject.FindProperty("door");
        _distance = serializedObject.FindProperty("distanceToMove");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        _rotates.boolValue = EditorGUILayout.Toggle("Rotating Door", _rotates.boolValue);

        if (_rotates.boolValue)
        {
            _pivot.objectReferenceValue = EditorGUILayout.ObjectField("Pivot Object", _pivot.objectReferenceValue,
                                        typeof(PivotScript), true);
        } 
        else
        {
            _doorTransform.objectReferenceValue = EditorGUILayout.ObjectField("Door Object", _doorTransform.objectReferenceValue,
                                        typeof(Transform), true);

            _closeSpeed.floatValue = EditorGUILayout.Slider("Closing Speed", _closeSpeed.floatValue, 0.1f, 10f);
            _openSpeed.floatValue = EditorGUILayout.Slider("Opening Speed", _openSpeed.floatValue, 0.1f, 10f);
            _distance.floatValue = EditorGUILayout.Slider("Height to Move", _distance.floatValue, -20f, 20f);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
