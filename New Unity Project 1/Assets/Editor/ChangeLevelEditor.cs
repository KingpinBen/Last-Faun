using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChangeLevelScript))]
public class ChangeLevelEditor : Editor
{

    private SerializedProperty _nextLevel;

    void OnEnable()
    {
        _nextLevel = serializedObject.FindProperty( "nextLevelName" );
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Object is a requirement for the level and should only be placed once. It handles the fading in and out of levels, and also saves the game on level transition\n\nYou can use the name of the scene you want to go to.", MessageType.Info);
        _nextLevel.stringValue = EditorGUILayout.TextField( "Next Level's Name", _nextLevel.stringValue );
        serializedObject.ApplyModifiedProperties();
    }
}
