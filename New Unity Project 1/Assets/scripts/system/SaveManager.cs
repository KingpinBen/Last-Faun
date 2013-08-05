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

    private const string SAVE_FILE_NAME = "./Saves/Save01";

    public static int currentLevel
    {
        get { return _currentLevel; }
    }

    public static bool loadFound
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

        //  Check if the Save folder exists, otherwise we'll need to make one.
        //  It'll throw null on SAVE_FILE_NAME path otherwise.
        if (!Directory.Exists("./Saves"))
        {
            Directory.CreateDirectory("./Saves");
        }

        if (File.Exists(SAVE_FILE_NAME + ".sav"))
        {
            BinaryReader reader = null;

            try
            {
                reader = new BinaryReader(File.OpenRead(SAVE_FILE_NAME + ".sav"));

                _currentLevel = reader.ReadInt32();
                _currentCheckpointIndex = reader.ReadInt32();
            }
            catch
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            if (reader != null)
            {
                reader.Close();
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
        //  Check if the Save folder exists, otherwise we'll need to make one.
        //  It'll throw null on SAVE_FILE_NAME path otherwise.
        if (!Directory.Exists("./Saves"))
        {
            Directory.CreateDirectory("./Saves");
        }

        //  Initialize the writer and make a temp file.
        var writer = new BinaryWriter(File.Create(SAVE_FILE_NAME + ".tmp"));

        //  Save the actual game data
        writer.Write(_currentLevel);
        writer.Write(_currentCheckpointIndex);

        //  Start closing the stream
        writer.Flush();
        writer.Close();

        //  If there is already a save file, remove it
        if (File.Exists(SAVE_FILE_NAME + ".sav"))
        {
            File.Delete(SAVE_FILE_NAME + ".sav");
        }

        //  and make the temp file the new save file.
        File.Move(SAVE_FILE_NAME + ".tmp", SAVE_FILE_NAME + ".sav");

        //  Values should be correct at this point, and on a new game
        //  we need it to display continue on main menu.
        _loadedContent = true;
    }

    public static void ChangeLevel(string newLevel)
    {
        var currentLevelID = Application.loadedLevel;

        Application.LoadLevel(newLevel);

        if (Application.loadedLevel > 0 && Application.loadedLevel != currentLevelID)
        {
            _currentLevel = Application.loadedLevel;
            Save();
        }
        else
        {
            Application.LoadLevel(currentLevelID);
        }
        
    }
}
