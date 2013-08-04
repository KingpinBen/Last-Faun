using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraPointController))]
public class CameraPointEditor : Editor
{
    private bool _showProperties = true;
    private bool _showCameraNodes = false;

    private SerializedProperty _rotationSpeed;
    private SerializedProperty _snap;
    private SerializedProperty _cameraProxy;
    private SerializedProperty _cameraTarget;

    void OnEnable()
    {
        _rotationSpeed = serializedObject.FindProperty("rotationSpeed");
        _snap = serializedObject.FindProperty("autoSnapOnTouch");
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
        _showCameraNodes = EditorGUILayout.Foldout(_showCameraNodes, "Camera Nodes");
        if (_showCameraNodes)
        {
            EditorGUILayout.PropertyField(_cameraProxy, new GUIContent("Camera Proxy"));
        }


        serializedObject.ApplyModifiedProperties();
    }
}
