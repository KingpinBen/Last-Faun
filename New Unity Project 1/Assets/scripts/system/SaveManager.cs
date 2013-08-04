using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public static class SaveManager
{
    //  Variables to save.  (May want to find more)
    private static int _currentLevel = 1;
    private static int _currentCheckpointIndex = 0;

    //  Object fields
    private static string _savePath;
    private static bool _loadedContent;
    private const string SAVE_FILE_NAME = "";

    public static int currentLevel
    {
        get { return _currentLevel; }
    }

    public static bool loadedContent
    {
        get { return _loadedContent && _currentLevel > 0; }
    }

    private static void CreateDefaultValues()
    {
        _currentLevel = 1;
        _currentCheckpointIndex = 0;
    }

    public static void Load()
    {
        if (_loadedContent) return;

        if (string.IsNullOrEmpty(_savePath))
            _savePath = Path.Combine("./Saves/", SAVE_FILE_NAME + ".save");

        if (File.Exists(_savePath))
        {
            using (var reader = new BinaryReader(File.Open(_savePath, FileMode.Create)))
            {
                _currentLevel = reader.ReadInt32();
                _currentCheckpointIndex = reader.ReadInt32();
            }

            _loadedContent = true;
        }
        else
        {
            CreateDefaultValues();
        }

        
    }

    public static void Save()
    {
        if (string.IsNullOrEmpty(_savePath))
            _savePath = Path.Combine("../Saves/", SAVE_FILE_NAME + ".save");

        using (var writer = new BinaryWriter(File.Create(_savePath)))
        {
            writer.Write(_currentLevel);
            writer.Write(_currentCheckpointIndex);
        }
    }
}
