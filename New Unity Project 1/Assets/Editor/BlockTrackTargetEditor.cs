using UnityEngine;
using UnityEditor;

[CustomEditor( typeof ( BlockTrackTarget ) )]
public class BlockTrackTargetEditor : Editor
{
    private SerializedProperty _block;

    private void OnEnable()
    {
        _block = serializedObject.FindProperty( "occupyingBlock" );
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "If a block is supposed to start ontop of the current green cube, set the 'Occupying Block' to be that block. This stops other blocks from being able to move into it.\n\n Leave empty if the space can be moved to.",
            MessageType.Warning );

        _block.objectReferenceValue = EditorGUILayout.ObjectField( "Occupying Block", _block.objectReferenceValue,
                                                                   typeof ( BlockScript ), true );
        serializedObject.ApplyModifiedProperties();
    }
}
