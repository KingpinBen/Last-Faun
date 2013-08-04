using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraLineController))]
public class CameraLineEditor : Editor
{
    private bool _showTriggers = false;
    private bool _showProperties = true;
    private bool _showCameraNodes = false;

    private SerializedProperty _rotationSpeed;
    private SerializedProperty _snap;
    private SerializedProperty _nodes;
    private SerializedProperty _startTrigger;
    private SerializedProperty _endTrigger;
    private SerializedProperty _cameraProxy;
    private SerializedProperty _cameraTarget;

    void OnEnable()
    {
        _startTrigger = serializedObject.FindProperty("startTrigger");
        _endTrigger = serializedObject.FindProperty("endTrigger");
        _rotationSpeed = serializedObject.FindProperty("rotationSpeed");
        _snap = serializedObject.FindProperty("autoSnapOnTouch");
        _nodes = serializedObject.FindProperty("cameraNodes");
        _cameraProxy = serializedObject.FindProperty("cameraTransformNode");
        _cameraTarget = serializedObject.FindProperty("cameraTarget");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Separator();
        _showProperties = EditorGUILayout.Foldout(_showProperties, "Camera Properties");
        if (_showProperties)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_cameraTarget);
            EditorGUILayout.PropertyField(_rotationSpeed);
            EditorGUILayout.PropertyField(_snap);
        }
        EditorGUILayout.Separator();

        _showTriggers = EditorGUILayout.Foldout(_showTriggers, "Triggers");
        if (_showTriggers)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_startTrigger);
            EditorGUILayout.PropertyField(_endTrigger);
        }

        EditorGUILayout.Separator();
        _showCameraNodes = EditorGUILayout.Foldout(_showCameraNodes, "Camera Nodes");
        if (_showCameraNodes)
        {
            EditorGUILayout.PropertyField(_cameraProxy, new GUIContent("Camera Proxy"));

            for (var i = 0; i < _nodes.arraySize; i++)
            {
                EditorGUILayout.PropertyField(_nodes.GetArrayElementAtIndex(i), new GUIContent("Camera Node " + i));
            }
        }


        serializedObject.ApplyModifiedProperties();
    }
}
