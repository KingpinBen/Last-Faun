using UnityEngine;
using System.Collections;

public class InGamePause : MonoBehaviour
{
    private static InGamePause _instance;
    public static InGamePause instance
    {
        get { return _instance; }
        private set
        {
            _instance = value;
        }
    }

    public GUISkin guiSkin;

    private Matrix4x4 _guiMatrix;
    private Rect _guiRect = new Rect(0, 0, 500, 300);
    private bool _paused;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        UpdateGui();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            UpdateGameState();

        if (_paused)
            UpdateGui();
    }

    private void UpdateGameState()
    {
        _paused = !_paused;

        Time.timeScale = _paused ? 0 : 1;
        Screen.showCursor = _paused;
    }

    private void UpdateGui()
    {
        var scale = Screen.height*0.001f;
        var offset = new Vector3((Screen.width*.5f) - ((_guiRect.xMax*.5f)*scale),
                                 (Screen.height*.5f) - ((_guiRect.yMax*.5f)*scale), 0);
        _guiMatrix =
            Matrix4x4.TRS(offset, Quaternion.identity, new Vector3(scale, scale, 1));
    }

    private void OnGUI()
    {
        if (!_paused) 
            return;

        if (guiSkin) 
            GUI.skin = guiSkin;

        GUI.matrix = _guiMatrix;

        GUILayout.BeginArea(_guiRect);
        GUILayout.Label("- Paused -", guiSkin.customStyles[1]);
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Resume", guiSkin.customStyles[0]))
            UpdateGameState();

        if (GUILayout.Button("Back to Menu", guiSkin.customStyles[0]))
        {
            UpdateGameState();
            Application.LoadLevel(0);
        }

        GUILayout.EndArea();
    }

    public bool GetIsPaused()
    {
        return _paused;
    }
}
