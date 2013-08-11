using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OutOfBoundsZone))]
public class OutOfBoundsEditor : Editor
{

    private SerializedProperty _respawnTransform;

    void OnEnable()
    {
        _respawnTransform = serializedObject.FindProperty( "targetRespawnPoint" );
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "If you don't already have an object to use as a respawn position, at the top of Unity go into the 'GameObject' tab and select 'Create Empty' and place at the point you want the player to respawn. That empty GameObject can be reused by multiple OutOfBounds zones!",
            MessageType.Info );
        _respawnTransform.objectReferenceValue = EditorGUILayout.ObjectField( "Respawn Position",
                                                                              _respawnTransform.objectReferenceValue,
                                                                              typeof ( Transform ), true );

        serializedObject.ApplyModifiedProperties();
    }
}
