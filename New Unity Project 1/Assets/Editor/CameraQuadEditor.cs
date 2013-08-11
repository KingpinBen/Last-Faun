using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraQuadraticController))]
public class CameraQuadEditor : Editor {

    private SerializedProperty _rotationSpeed;
    private SerializedProperty _snap;
    private SerializedProperty _cameraTarget;

    void OnEnable()
    {
        _rotationSpeed = serializedObject.FindProperty("rotationSpeed");
        _snap = serializedObject.FindProperty("autoSnapOnTouch");
        _cameraTarget = serializedObject.FindProperty("cameraTarget");
    }

    public override void OnInspectorGUI()
    {

        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField( _cameraTarget );
        EditorGUILayout.PropertyField( _rotationSpeed );
        EditorGUILayout.PropertyField( _snap );

        serializedObject.ApplyModifiedProperties();
    }
}
