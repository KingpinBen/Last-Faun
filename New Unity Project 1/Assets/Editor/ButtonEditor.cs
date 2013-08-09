using UnityEngine;
using UnityEditor;

[CustomEditor( typeof ( ButtonScript ) )]
public class ButtonEditor : Editor
{
    private SerializedProperty _isUseable;
    private SerializedProperty _requiresKey;
    private SerializedProperty _requiredKey;

    private void OnEnable()
    {
        _isUseable = serializedObject.FindProperty( "objectActive" );
        _requiredKey = serializedObject.FindProperty( "keyObject" );
        _requiresKey = serializedObject.FindProperty( "requiresKey" );
    }

    public override void OnInspectorGUI()
    {
        _isUseable.boolValue =
            EditorGUILayout.Toggle( "Is Useable", _isUseable.boolValue );
        
        _requiresKey.boolValue = EditorGUILayout.BeginToggleGroup( "Requires a Key", _requiresKey.boolValue );
        
        EditorGUILayout.HelpBox( "Leaving 'Specific Key' empty will allow you to use any key to make this button work",
                                 MessageType.Info );
        _requiredKey.objectReferenceValue =
            EditorGUILayout.ObjectField( "Specific Key", _requiredKey.objectReferenceValue, typeof ( ButtonKeyScript ),
                                         true );
        
        EditorGUILayout.EndToggleGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
